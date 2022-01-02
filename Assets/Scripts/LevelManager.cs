using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private IntVariableSO _currentLevel;
    [SerializeField] private Tilemap[] _levels;

    private void OnEnable() 
    {
        _currentLevel.Value = 0;
    }

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
            _levels[levelIndex].gameObject.SetActive(true);

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
            return _levels[_currentLevel.Value].GetComponent<Level>().CoinReward;
        }
        catch
        {
            Debug.LogError("GetLevelCoinReward: the level does not have a LeverReward component");
            return -1;
        }
    }

    private void DeactivateLevels()
    {
        foreach (Tilemap level in _levels)
        {
            if (level.gameObject.activeSelf)
                level.gameObject.SetActive(false);
        }
    }
}
