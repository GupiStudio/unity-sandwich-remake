using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform ParentIngredients;

    [SerializeField] private IntVariableSO _coinCount;
    [SerializeField] private FloatVariableSO _flipDuration;

    [HideInInspector] public bool IngredientIsAnimating;

    private Stack[] _allStackInScene;
    private Transform _moreHeight;

    private UserInterfaceController _uiController;
    private LevelManager _levelManager;

    public float FlipDuration => _flipDuration.Value;

    private List<FlipCommand> _flipHistory = new List<FlipCommand>();

    private void Awake()
    {
        Instance = this;

        _uiController = FindObjectOfType<UserInterfaceController>();
        _levelManager = GetComponent<LevelManager>();
    }

    private void OnDisable()
    {
        Instance = null;
    }

    private void Start()
    {
        _moreHeight = transform;
        _moreHeight.position = Vector3.zero;
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
            
            ClearFlipHistory();
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

    public void ClearFlipHistory()
    {
        _flipHistory.Clear();
    }

    public void RecordFlip(FlipCommand flip)
    {
        _flipHistory.Add(flip);
    }

    public void UndoFlip()
    {
        var lastFlip = _flipHistory[_flipHistory.Count - 1];

        StartCoroutine(lastFlip.Undo());

        _flipHistory.Remove(lastFlip);
    }
}
