using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles resetting breakables and saws to their initial position
/// </summary>
public class ResetManager : MonoBehaviour
{
    public static ResetManager Instance;

    public List<Resettable> resetList = new List<Resettable>();

    private void Start()
    {
        Instance = this;
    }

    public void ResetAll()
    {
        foreach (var item in resetList)
        {
            item.Reset();
        }
    }

    public void AddObjectToResetList(Resettable go)
    {
        resetList.Add(go);
    }

    public void RemoveObjectFromResetList(Resettable go)
    {
        resetList.Remove(go);
    }
}
