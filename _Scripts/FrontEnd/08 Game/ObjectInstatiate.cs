using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInstatiate : MonoBehaviour
{
    public GameObject canvas;
    public Image img;
    [SerializeField] private int NumOfObjects = 5;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < NumOfObjects; i++)
        {
            var C = Instantiate(img, new Vector3(Random.Range(-400f, 400f), Random.Range(-300f, 300f)), Quaternion.identity) as Image;
            C.transform.SetParent(canvas.transform, false);
        }
    }

}
