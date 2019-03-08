using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class used for resettable objects. All children are resettable;
/// </summary>
public abstract class Resettable : MonoBehaviour
{
    public abstract void Reset();
}
