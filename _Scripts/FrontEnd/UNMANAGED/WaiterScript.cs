using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaiterScript : MonoBehaviour {
    // Start is called before the first frame update
    public float delay = 0;
    Button WaiterButton;
    void Awake() {
        WaiterButton = GetComponent<Button>();
        StartCoroutine(ExecuteWaitingEvents());
    }

    // Update is called once per frame
    IEnumerator ExecuteWaitingEvents() {
        yield return new WaitForSeconds(delay);
        WaiterButton.onClick.Invoke();
    }
}