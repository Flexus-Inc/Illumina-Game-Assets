using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueOrbMovement : MonoBehaviour {
    public float amplitude = 1f;
    public float frequency = 0.3f;

    Vector3 posOffset = new Vector2();
    Vector3 tempPos = new Vector2();

    void OnEnable() {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update() {
        tempPos = posOffset;
        tempPos.x = transform.position.x;
        tempPos.y = transform.position.y;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }
}