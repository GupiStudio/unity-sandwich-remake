using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private IntVariableSO _currentLevel;
    [SerializeField] private GameObject[] _levels;

    private void Start()
    {
        DeactivateLevels();
        LoadLevel((uint)_currentLevel.Value);
    }

    public void LoadNextLevel()
    {
        DeactivateLevels();
        _currentLevel.Value++;
        LoadLevel((uint)_currentLevel.Value);
    }

    public bool LoadLevel(uint levelIndex)
    {
        try
        {
            DeactivateLevels();
            _levels[levelIndex].SetActive(true);

            return true;
        }
        catch
        {
            if (levelIndex >= _levels.Length)
                Debug.LogError("LoadLevel: levelIndex is out of range");

            return false;
        }
    }

    public int GetLevelCoinReward()
    {
        try
        {
            return _levels[_currentLevel.Value].GetComponent<LevelReward>().CoinReward;
        }
        catch
        {
            Debug.LogError("GetLevelCoinReward: the level does not have a LeverReward component");
            return -1;
        }
    }

    private void DeactivateLevels()
    {
        foreach (GameObject level in _levels)
        {
            if (level.activeSelf)
                level.SetActive(false);
        }
    }
}
