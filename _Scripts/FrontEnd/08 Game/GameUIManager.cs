using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {
    public Text Timer;
    // Start is called before the first frame update

    public static int TimerText { get => int.Parse(GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameUIManager>().Timer.text); set => GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameUIManager>().Timer.text = value.ToString(); }

}