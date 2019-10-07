using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeInOut : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool repeatable = true;
    [SerializeField] private float duration = 5f;
    public Image img;
    float start;
    
    IEnumerator Start()
    {
        start = Random.Range(0.0f, 1.0f);
        img.canvasRenderer.SetAlpha(start);
        while (repeatable)
        {
            yield return FadeOut(duration);
            yield return FadeIn(duration);
        }
    }
    IEnumerator FadeOut(float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            img.CrossFadeAlpha(0, 2, false);
            yield return null;
        }
    }

    IEnumerator FadeIn(float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while(i < 1.0f)
        {
            i += Time.deltaTime * rate;
            img.CrossFadeAlpha(1, 2, false);
            yield return null;
        }
    }
}
