using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    public static UiController Instance;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    TextMeshProUGUI distanceText;

    [SerializeField]
    Animation fadeAnimation;

    [SerializeField]
    AnimationClip fadeOutClip, fadeInClip;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        canvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDistanceText(float srqDistance)
    {
        distanceText.text = Mathf.Round(srqDistance / 30f).ToString() + "m";
    }

    public void FadeToBlackThenClear()
    {
        StartCoroutine(FadeOutThenIn());
    }

    IEnumerator FadeOutThenIn()
    {
        fadeAnimation.Play(fadeOutClip.name);
        yield return new WaitForSeconds(fadeOutClip.length);
        fadeAnimation.Play(fadeInClip.name);
    }
}
