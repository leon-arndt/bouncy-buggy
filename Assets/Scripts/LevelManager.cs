using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the saving and loading of different levels
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Level[] levels;
    public GameObject loadedLevel;
    int currentLevelIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int index)
    {
        //unload old geometry
        Destroy(loadedLevel.gameObject);

        //load new geometry
        loadedLevel = Instantiate(levels[index].geometry);

        //update player position
        PlayerController.Instance.transform.position = levels[index].playerStart;

        Goal.Instance = FindObjectOfType<Goal>();
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex < levels.Length)
        {
            currentLevelIndex++;
            LoadLevel(currentLevelIndex);
        }
    }

    public Vector3 GetPlayerStart()
    {
        return levels[currentLevelIndex].playerStart;
    }
}
