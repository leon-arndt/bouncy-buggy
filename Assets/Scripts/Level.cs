using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "BouncyBuggy/Level", order = 1)]
public class Level : ScriptableObject
{
    public GameObject geometry;
    public Vector3 playerStart;
}
