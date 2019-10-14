using UnityEngine;
using UnityEngine.UI;

public class ScrollUp : MonoBehaviour
{
    public float upward;
    float rectY = 0;

    public RectTransform rectTransform;

    private void Start()
    {
        rectY = rectTransform.up.y;
    }

    void Update()
    {
        rectY += upward * Time.deltaTime;
    }
}
