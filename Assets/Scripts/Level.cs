using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private IntVariableSO _coinReward;

    public int CoinReward => _coinReward.Value;
}
