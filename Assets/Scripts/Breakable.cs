using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used for terrain which disappears after a while
/// </summary>
public class Breakable : MonoBehaviour
{
    public float disappearAfter;
    private float disappearSpeed = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        disappearAfter = disappearAfter + Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > disappearAfter)
        {
            transform.Translate(Vector3.down * Time.deltaTime);
        }
    }
}
