using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimator : MonoBehaviour
{
    private Vector3 minScale, maxScale;
    [SerializeField] private bool repeatable = true;
    private float duration, speed;
    float sizeX, sizeY;
    IEnumerator Start()
    {
        speed = Random.Range(1f, 1.5f);
        duration = Random.Range(2f, 10f);
        minScale = transform.localScale ;
        sizeX = Random.Range(1.5f, 2f);
        sizeY = sizeX;
        maxScale = new Vector3(sizeX, sizeY);
        while (repeatable)
        {
            yield return Size(minScale, maxScale, duration);
            yield return Size(maxScale, minScale, duration);
        }
    }

    IEnumerator Size(Vector3 currentSize, Vector3 updateSize, float time)
    {
        float _start = 0.0f;
        float rate = (1.0f / time) * speed;
        while (_start < 1.0f)
        {
            _start += Time.deltaTime * rate;
            transform.localScale = Vector3.Slerp(currentSize, updateSize, _start);
            yield return null;
        }
    }
}
