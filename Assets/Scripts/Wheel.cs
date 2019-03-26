using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public enum WheelType
    {
        Left,
        Right
    }

    public WheelType wheelType;

    float spinSpeed;
    private const float DIFFERENTIAL_FACTOR = 5f;

    // Update is called once per frame
    void Update()
    {
        spinSpeed = PlayerController.Instance.speed;

        //outside wheel needs to turn faster
        if (wheelType == WheelType.Left)
        {
            //left wheel needs to spin faster when turning right
            spinSpeed += DIFFERENTIAL_FACTOR * Mathf.Max(0, PlayerController.Instance.horizontalInput);
        }
        else
        if (wheelType == WheelType.Right)
        {
            //right wheel needs to spin faster when turning left
            spinSpeed += DIFFERENTIAL_FACTOR * Mathf.Max(0, -PlayerController.Instance.horizontalInput);
        }
        transform.Rotate(spinSpeed * Vector3.down, Space.Self);
    }
}
