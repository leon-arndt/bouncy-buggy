﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used for terrain which disappears after a while
/// </summary>
public class Breakable : Resettable
{
    private const float disappearDelay = 1.5f;
    private float disappearAfter = 0f;
    private float disappearRate = 2f;

    private Renderer rend;
    private Vector3 startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetManager.Instance.AddObjectToResetList(this);

        startPosition = transform.position;

        disappearRate += Random.Range(0, 1);
        disappearAfter = disappearDelay + 0.1f * Vector3.Distance(transform.position, PlayerController.Instance.transform.position);

        if (GetComponent<MeshRenderer>() != null)
        {
            rend = GetComponent<MeshRenderer>();
            rend.material.color = Color.white;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rend != null)
        {
            rend.material.color = Color.Lerp(Color.white, Color.black, Time.timeSinceLevelLoad / disappearAfter);
        }

        if (Time.timeSinceLevelLoad > disappearAfter)
        {
            transform.Translate(Vector3.down * disappearRate * Time.deltaTime);
        }
    }

    public override void Reset()
    {
        transform.position = startPosition;
    }

    private void OnDestroy()
    {
        ResetManager.Instance.RemoveObjectFromResetList(this);
    }
}
