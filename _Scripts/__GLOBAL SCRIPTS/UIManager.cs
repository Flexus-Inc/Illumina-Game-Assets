using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    // Start is called before the first frame update
    public static GameObject __GameUICanvas;
    void Start() {
        __GameUICanvas = GameObject.Find("__GameUICanvas");
        __GameUICanvas.SetActive(false);
    }
    public static void EnableGUI() {
        __GameUICanvas.SetActive(true);
    }
    public static void DisableGUI() {
        __GameUICanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }
}