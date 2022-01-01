using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReward : MonoBehaviour
{
    [SerializeField] private IntVariableSO _coinReward;

    public int CoinReward => _coinReward.Value;
}
