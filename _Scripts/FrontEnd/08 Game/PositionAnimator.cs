using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionAnimator : MonoBehaviour
{
    public Sprite[] baybayinImg;
    public Image img;
    private Vector3 currentPos, targetPos;
    [SerializeField] private bool repeatable = true;
    [SerializeField] private float LimitXAxis = 1920f;
    [SerializeField] private float LimitYAxis = 1080f;
    private float speed; 
    private float duration;  
    float x, y;
    int randomImg;
    IEnumerator Start()
    {
        randomImg = Random.Range(0, baybayinImg.Length);
        img.sprite = baybayinImg[randomImg];
        Debug.Log(img.sprite);
        speed = Random.Range(1f, 1.5f);
        duration = Random.Range(10f, 20f);
        currentPos = transform.localPosition;
        img.canvasRenderer.SetAlpha(Random.Range(0.1f, 1f));
        while (repeatable)
        {
            
            x = Random.Range(-(LimitXAxis), (LimitXAxis));
            y = Random.Range(-(LimitYAxis), (LimitYAxis));
            targetPos = new Vector3(x, y);
            
            yield return Position(currentPos, targetPos, duration);
            //yield return Position(targetPos, currentPos, duration);
        }
    }

    IEnumerator Position(Vector3 currPos, Vector3 updatePos, float time)
    {
        float _start = 0.0f;
        float rate = (1.0f / time) * speed;
        
        while (_start < 1f)
        {
            transform.localPosition = Vector3.Slerp(currPos, updatePos, _start);
            if (_start <= 0.6f)
                img.CrossFadeAlpha(1, 0.6f, false);
            else
                img.CrossFadeAlpha(0, 0.40f, false);
            _start += Time.deltaTime * rate;
            yield return null;
        }
    }
}
