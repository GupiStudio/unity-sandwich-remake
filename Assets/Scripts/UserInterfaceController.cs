using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private Canvas _gameplayCanvas;
    [SerializeField] private Canvas _winScreenCanvas;
    
    [Header("Text")]
    [SerializeField] private Text _levelCountText;
    [SerializeField] private Text _coinCountText;
    [SerializeField] private Text _coinRewardText;

    [Header("Values SO")]
    [SerializeField] private IntVariableSO _levelCount;
    [SerializeField] private IntVariableSO _coinCount;

    private void Start()
    {
        ShowGameplayUI();
    }

    public void ShowGameplayUI()
    {
        _winScreenCanvas.enabled = false;
        _gameplayCanvas.enabled = true;

        _levelCountText.text = $"Level {_levelCount.Value.ToString()}";
    }

    public void ShowWinScreenUI()
    {
        _gameplayCanvas.enabled = false;
        _winScreenCanvas.enabled = true;

        _coinCountText.text = $"Coin: {_coinCount.Value.ToString()}";
    }
}
