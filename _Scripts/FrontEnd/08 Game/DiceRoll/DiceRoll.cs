using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;

public class DiceRoll : MonoBehaviour {
    int previousRoll = 0;

    [HideInInspector]
    public static int diceValue = 0;

    public float delay;
    public int repetitions;
<<<<<<< HEAD
    public int soundIndex;
=======
    bool roll_enabled = true;
    public static List<int> hexpos = new List<int>();
>>>>>>> 2be8f8ee494afa3d23a6f439b559598f8fac4934

    public Collider2D diceCollider;
    public Animator[] animator;
    public SpriteRenderer[] renderHex;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && roll_enabled) {
            StartCoroutine(DiceRoller());
        }

    }

    IEnumerator DiceRoller() {
        diceCollider.enabled = false;
        hexpos.Clear();
        for (int i = 0; i <= repetitions; i++) {
            diceValue = Random.Range(1, 7);
<<<<<<< HEAD
            GameObject.Find("__SoundManager").GetComponent<SoundManager>().PlaySound(soundIndex);
            if (diceValue == 1)
            {
                RollOne(); previousRoll = 1;
            }
            else if(diceValue == 2)
            {
                RollTwo(); previousRoll = 2;
            }
            else if (diceValue == 3)
            {
                RollThree(); previousRoll = 3;
            }
            else if (diceValue == 4)
            {
                RollFour(); previousRoll = 4;
            }
            else if (diceValue == 5)
            {
                RollFive(); previousRoll = 5;
            }
            else if (diceValue == 6)
            {
                RollSix(); previousRoll = 6;
=======

            if (diceValue == 1) {
                RollOne();
                previousRoll = 1;
            } else if (diceValue == 2) {
                RollTwo();
                previousRoll = 2;
            } else if (diceValue == 3) {
                RollThree();
                previousRoll = 3;
            } else if (diceValue == 4) {
                RollFour();
                previousRoll = 4;
            } else if (diceValue == 5) {
                RollFive();
                previousRoll = 5;
            } else if (diceValue == 6) {
                RollSix();
                previousRoll = 6;
>>>>>>> 2be8f8ee494afa3d23a6f439b559598f8fac4934
            }

            yield return new WaitForSeconds(delay);
        }
        GamePlayManager.TurnMaxMoves = diceValue;
        Debug.Log("Roll: " + diceValue);
        hexpos.Reverse();
        roll_enabled = false;

        //When the player finishes his roll
        yield return new WaitForSeconds(2f);
        animator[0].SetBool("Shrink", true);
        animator[1].SetBool("Shrink", true);
        animator[2].SetBool("Shrink", true);
        GamePlayManager.GamePaused = false;

    }

    public static void RemoveHexPos() {
        if (hexpos.Count != 0) {
            GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<DiceRoll>().DisableRenderHexPos(hexpos[0]);
            hexpos.RemoveAt(0);
        }
    }

    public void DisableRenderHexPos(int pos) {
        renderHex[pos].enabled = false;
    }
    void RollOne() {
        switch (previousRoll) {
            case 1:
                DisableOne();
                break;
            case 2:
                DisableTwo();
                break;
            case 3:
                DisableThree();
                break;
            case 4:
                DisableFour();
                break;
            case 5:
                DisableFive();
                break;
            case 6:
                DisableSix();
                break;
        }

        renderHex[0].enabled = true;
        hexpos.Add(0);

    }

    void RollTwo() {
        switch (previousRoll) {
            case 1:
                DisableOne();
                break;
            case 2:
                DisableTwo();
                break;
            case 3:
                DisableThree();
                break;
            case 4:
                DisableFour();
                break;
            case 5:
                DisableFive();
                break;
            case 6:
                DisableSix();
                break;
        }

        renderHex[1].enabled = true;
        renderHex[4].enabled = true;
        hexpos.Add(1);
        hexpos.Add(4);
    }

    void RollThree() {
        switch (previousRoll) {
            case 1:
                DisableOne();
                break;
            case 2:
                DisableTwo();
                break;
            case 3:
                DisableThree();
                break;
            case 4:
                DisableFour();
                break;
            case 5:
                DisableFive();
                break;
            case 6:
                DisableSix();
                break;
        }

        renderHex[1].enabled = true;
        renderHex[4].enabled = true;
        renderHex[0].enabled = true;
        hexpos.Add(0);
        hexpos.Add(1);
        hexpos.Add(4);
    }

    void RollFour() {
        switch (previousRoll) {
            case 1:
                DisableOne();
                break;
            case 2:
                DisableTwo();
                break;
            case 3:
                DisableThree();
                break;
            case 4:
                DisableFour();
                break;
            case 5:
                DisableFive();
                break;
            case 6:
                DisableSix();
                break;
        }

        renderHex[0].enabled = true;
        renderHex[1].enabled = true;
        renderHex[3].enabled = true;
        renderHex[5].enabled = true;
        hexpos.Add(0);
        hexpos.Add(1);
        hexpos.Add(3);
        hexpos.Add(5);
    }

    void RollFive() {
        switch (previousRoll) {
            case 1:
                DisableOne();
                break;
            case 2:
                DisableTwo();
                break;
            case 3:
                DisableThree();
                break;
            case 4:
                DisableFour();
                break;
            case 5:
                DisableFive();
                break;
            case 6:
                DisableSix();
                break;
        }

        renderHex[0].enabled = true;
        renderHex[1].enabled = true;
        renderHex[3].enabled = true;
        renderHex[4].enabled = true;
        renderHex[6].enabled = true;
        hexpos.Add(0);
        hexpos.Add(1);
        hexpos.Add(3);
        hexpos.Add(4);
        hexpos.Add(6);
    }

    void RollSix() {
        switch (previousRoll) {
            case 1:
                DisableOne();
                break;
            case 2:
                DisableTwo();
                break;
            case 3:
                DisableThree();
                break;
            case 4:
                DisableFour();
                break;
            case 5:
                DisableFive();
                break;
            case 6:
                DisableSix();
                break;
        }

        renderHex[1].enabled = true;
        renderHex[2].enabled = true;
        renderHex[3].enabled = true;
        renderHex[4].enabled = true;
        renderHex[5].enabled = true;
        renderHex[6].enabled = true;
        hexpos.Add(1);
        hexpos.Add(2);
        hexpos.Add(3);
        hexpos.Add(4);
        hexpos.Add(5);
        hexpos.Add(6);

    }

    void DisableOne() {
        renderHex[0].enabled = false;
    }

    void DisableTwo() {
        renderHex[1].enabled = false;
        renderHex[4].enabled = false;
    }

    void DisableThree() {
        renderHex[1].enabled = false;
        renderHex[4].enabled = false;
        renderHex[0].enabled = false;
    }

    void DisableFour() {
        renderHex[0].enabled = false;
        renderHex[1].enabled = false;
        renderHex[3].enabled = false;
        renderHex[5].enabled = false;
    }

    void DisableFive() {
        renderHex[0].enabled = false;
        renderHex[1].enabled = false;
        renderHex[3].enabled = false;
        renderHex[4].enabled = false;
        renderHex[6].enabled = false;
    }

    void DisableSix() {
        renderHex[1].enabled = false;
        renderHex[2].enabled = false;
        renderHex[3].enabled = false;
        renderHex[4].enabled = false;
        renderHex[5].enabled = false;
        renderHex[6].enabled = false;
    }
}