using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour {
    public static SingletonManager instance = null;
    // Start is called before the first frame update
    void Awake() {
        if (instance == null) {
            instance = this;
            GameObject.DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
}