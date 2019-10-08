using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotation;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, rotation) * Time.deltaTime);
    }
}
