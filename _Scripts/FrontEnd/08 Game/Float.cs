using UnityEngine;
public class Float : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    Vector2 posOffset = new Vector2();
    Vector2 tempPos = new Vector2();

    private void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tempPos = posOffset;
        tempPos.x = 0.0f;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }
}
