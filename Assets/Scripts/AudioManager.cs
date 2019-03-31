using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource engineSource;
    public AudioSource musicSource;

    private bool musicOverride;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateEngineSound(float speed)
    {
        //dynamic engine sounds
        engineSource.pitch = 0.2f + 0.1f * speed;
        engineSource.volume = 0.2f + 0.1f * speed;

        //dynamic music
        //musicSource.pitch = 0.7f + 0.01f * speed;
        if (!musicOverride)
        {
            musicSource.volume = 0.3f + 0.1f * speed;
        }
    }

    public void ReviveMusic()
    {
        StartCoroutine(MusicRevival(1f));
    }

    IEnumerator MusicRevival(float duration)
    {
        for (float f = 0; f <= duration; f += 0.1f)
        {
            if (f < duration / 2f)
            {
                //slow down first
                musicSource.pitch = duration / 2f - f;
            }
            else
            {
                //speed up again later
                musicSource.pitch = f / duration;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
