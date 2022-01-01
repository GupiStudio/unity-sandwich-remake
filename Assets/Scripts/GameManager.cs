using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public Transform ParentIngredients;

    [HideInInspector] public bool IngredientIsAnimating;

    private Stack[] _allStackInScene;
    private Transform _moreHeight;

    // Replay Vars
    private List<ReplaySample> _replaySamples = new List<ReplaySample>();
    private bool _sampleTheTransform;
    private int _replayIndex;
    private bool _doFirstReplayFrame;
    private Transform _recordingTarget;

    private bool _doOneRetry;

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _moreHeight = transform;
        _moreHeight.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ReplaySampler();
        RewindReplayPlayer();
    }

    public void FindStack()
    {
        _allStackInScene = FindObjectsOfType<Stack>();

        if (_allStackInScene.Length == 1)
        {
            Debug.Log("Finished");
            CheckWin(_allStackInScene[0]);
        }
    }

    public void CheckWin(Stack _stack)
    {
        for (int i = 0; i < _stack.Childrens.Length; i++)
        {
            if(_stack.Childrens[i].position.y >= _moreHeight.position.y)
            {
                _moreHeight = _stack.Childrens[i];
            }
        }
        if (_moreHeight.name.Contains("Bread") && _stack.Childrens[0].name.Contains("Bread"))
        {
            Debug.Log("You win");
        }
        else
        {
            Debug.Log("You Lose");
        }
    }

    private void OnDisable()
    {
        Singleton = null;
    }

    public void StartReplaySampling(Transform targetIngredient)
    {
        _recordingTarget = targetIngredient;
        _sampleTheTransform = true;
        //StartCoroutine(ReplaySampler(targetIngredient));
    }

    public void StopReplaySamples()
    {
        _sampleTheTransform = false;
    }

    public void ResetReplaySamples()
    {
        _replaySamples.Clear();
    }
    
    public void RewindReplay()
    {
        if (!_doOneRetry)
        {
            //StartCoroutine(RewindReplayPlayer());
            _doOneRetry = true;
        }
    }

    void ReplaySampler ()
    {
        if (_sampleTheTransform)
        {
            _replaySamples.Add(new ReplaySample(_recordingTarget));
        }
    }

    void RewindReplayPlayer()
    {
        if (_doOneRetry)
        {
            if (!_doFirstReplayFrame)
            {
                _replayIndex = _replaySamples.Count - 1;
                _doFirstReplayFrame = true;
            }
            if (_replayIndex >= 0)
            {
                Stack tempStack = _replaySamples[_replayIndex].TargetIngredient.GetComponent<Stack>();
                if (tempStack != null)
                {
                    Destroy(tempStack);
                }

                _replaySamples[_replayIndex].TargetIngredient.parent = ParentIngredients;

                _replaySamples[_replayIndex].SetIngredientToAllSampledProperties();

                _replayIndex--;
            }
            else
            {
                ResetReplaySamples();

                //Resetting the stacks
                GameObject[] allIngredients = GameObject.FindGameObjectsWithTag("Ingredients");
                
                for (int i = 0; i < allIngredients.Length; i++)
                {
                    allIngredients[i].AddComponent<Stack>();
                }
                _doOneRetry = false;
                _doFirstReplayFrame = false;
            }
        }
    }
}
