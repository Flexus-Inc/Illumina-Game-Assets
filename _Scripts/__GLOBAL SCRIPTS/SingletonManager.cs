using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour {
    public static SingletonManager instance = null;
    // Start is called before the first frame update
    void Start() {
        if (instance == null) {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        } else {
            Destroy(this);
        }
    }

    // Update is called once per frame
}