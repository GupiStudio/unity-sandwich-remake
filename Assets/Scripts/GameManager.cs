using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public Transform ParentIngredients;

    [SerializeField] private IntVariableSO _coinCount;

    [HideInInspector] public bool IngredientIsAnimating;

    private Stack[] _allStackInScene;
    private Transform _moreHeight;

    private UserInterfaceController _uiController;
    private LevelManager _levelManager;

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

        _uiController = FindObjectOfType<UserInterfaceController>();
        _levelManager = GetComponent<LevelManager>();
    }

    private void OnDisable()
    {
        Singleton = null;
    }

    private void Start()
    {
        _moreHeight = transform;
        _moreHeight.position = Vector3.zero;
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

            _coinCount.Value += _levelManager.GetLevelCoinReward();
            _uiController.ShowWinScreenUI();
        }
        else
        {
            // note: in the game, there is no lose screen. player just need to retry
            Debug.Log("You Lose");
        }
    }

    public void LoadNextLevel()
    {
        _levelManager.LoadNextLevel();
        _uiController.ShowGameplayUI();
    }

    public void SkipLevel()
    {
        _coinCount.Value -= 50;
        LoadNextLevel();
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

    private void ReplaySampler()
    {
        if (_sampleTheTransform)
        {
            _replaySamples.Add(new ReplaySample(_recordingTarget));
        }
    }

    private void RewindReplayPlayer()
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
                GameObject[] allIngredients = GameObject.FindGameObjectsWithTag("Ingredient");
                
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
