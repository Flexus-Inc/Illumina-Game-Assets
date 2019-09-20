using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Illumina.Security;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour {
    // Start is called before the first frame update
    public Text result;
    void Start() {
        var h1 = IlluminaHash.GetHash("Edrian");
        var h2 = IlluminaHash.GetHash("Edrian");
        Debug.Log("Hashed 1 : " + h1);
        Debug.Log("Hashed 2 : " + h2);
        result.text = IlluminaHash.CompareHash(h1, h2).ToString();
        Debug.Log(result.text);
    }   

    // Update is called once per frame
    void Update() {

    }
}