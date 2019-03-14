using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource engineSource;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEngineSound(float speed)
    {
        engineSource.pitch = 0.2f + 0.1f * speed;
        engineSource.volume = 0.2f + 0.1f * speed;
    }
}
