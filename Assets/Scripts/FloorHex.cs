using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHex : MonoBehaviour
{
    private Renderer rend;
    private float randomOffset;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            rend = GetComponent<MeshRenderer>();
            rend.material.color = Color.white;
        }

        randomOffset = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (rend != null)
        {
            rend.material.color = Color.Lerp(Color.gray, Color.black, Mathf.PingPong(Time.time + randomOffset, 1));
            transform.Translate(Mathf.Sin(randomOffset * 0.1f * Time.time) * 0.05f *  Vector3.up, Space.World);
        }
    }
}
