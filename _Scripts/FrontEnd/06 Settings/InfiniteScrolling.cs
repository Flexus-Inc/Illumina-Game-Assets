using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScrolling : MonoBehaviour
{
    private Vector3 currentPos, targetPos;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool repeatable = true;
    [SerializeField] private float duration = 10f;
    [SerializeField] private float yPosition = 200f;
    [SerializeField] private float EdgeXAxis = 8000f;
    IEnumerator Start()
    {
        currentPos = transform.localPosition;
        targetPos = new Vector3(EdgeXAxis, yPosition);
        while (repeatable)
        {
            yield return Position(currentPos, targetPos, duration);
        }
    }
    IEnumerator Position(Vector3 currPos, Vector3 updatePos, float time)
    {
        float _start = 0.0f;
        float rate = (1.0f / time) * speed;
        while (_start < 1.0f)
        {
            _start += Time.deltaTime * rate;
            transform.localPosition = Vector3.Lerp(currPos, updatePos, _start);
            yield return null;
        }
    }
}
