using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditDragging : EventTrigger
{
    private bool isDragging = false;
    private Vector2 startClickPos, currentClickPos;
    private Rigidbody2D CreditImg;
    [SerializeField] private float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        CreditImg = GetComponent<Rigidbody2D>();
        CreditImg.AddForce(transform.up * Time.deltaTime * 200, ForceMode2D.Impulse);
        //CreditImg.velocity = new Vector2(0, speed);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        startClickPos.y = Input.mousePosition.y;
        isDragging = true;
        CreditImg.velocity = new Vector2(0, 0);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        CreditImg.velocity = new Vector2(0, speed);
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y <= -2160 || transform.localPosition.y >= 2160)
            transform.localPosition = new Vector2(transform.localPosition.x, 0);

        if (isDragging)
        {
            currentClickPos.y = Input.mousePosition.y;
            if ((currentClickPos.y - startClickPos.y) > 0)
            {
                CreditImg.AddForce(transform.up * Time.deltaTime * 75, ForceMode2D.Impulse);
            }

            if ((currentClickPos.y - startClickPos.y) < 0)
            {
                CreditImg.AddForce(transform.up * Time.deltaTime * -75, ForceMode2D.Impulse);
            }

        }
    }
}
