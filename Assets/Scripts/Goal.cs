using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static Goal Instance;
    Vector3 turnDirection = Vector3.zero;
    float turnSpeed = 5f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Turn());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(turnSpeed * turnDirection);
        transform.localScale = Vector3.one * (0.5f + Mathf.PingPong(Time.time, 1.5f));
    }

    IEnumerator Turn()
    {
        turnDirection = Random.insideUnitSphere;
        yield return new WaitForSeconds(1f);
        StartCoroutine(Turn());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}
