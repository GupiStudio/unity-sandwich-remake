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

    private void OnEnable() // temporary
    {
        _coinCount.Value = 0;
    }

    private void Start()
    {
        ShowGameplayUI();
    }

    public void ShowGameplayUI()
    {
        _winScreenCanvas.gameObject.SetActive(false);
        _gameplayCanvas.gameObject.SetActive(true);

        _levelCountText.text = $"Level {_levelCount.Value.ToString()}";
    }

    public void ShowWinScreenUI()
    {
        _gameplayCanvas.gameObject.SetActive(false);
        _winScreenCanvas.gameObject.SetActive(true);

        _coinCountText.text = $"Coin: {_coinCount.Value.ToString()}";
    }
}
