using UnityEngine;
using System.Collections;
using Proyecto26;

public class DiceRoll : MonoBehaviour
{
    int previousRoll = 0;

    [HideInInspector]
    public static int diceValue = 0;

    public float delay;
    public int repetitions;

    public Collider2D diceCollider;
    public Animator[] animator;
    public SpriteRenderer[] renderHex;
    

    private void OnMouseDown()
    {
        StartCoroutine(DiceRoller());
    }

    IEnumerator DiceRoller()
    {
        diceCollider.enabled = false;

        for (int i = 0; i <= repetitions; i++)
        {
            diceValue = Random.Range(1, 7);

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
            }

            yield return new WaitForSeconds(delay);
        }

        Debug.Log("Roll: " + diceValue);

        DiceValue diceNumber = new DiceValue();
        RestClient.Put("https://dice-database-test.firebaseio.com/.json", diceNumber);

        //When the player finishes his roll
        yield return new WaitForSeconds(2f);
        animator[0].SetBool("Shrink", true);
        animator[1].SetBool("Shrink", true);

        //If it is the player's turn
        //yield return new WaitForSeconds(10f);
        diceCollider.enabled = true;
        /*animator[0].SetBool("Shrink", false);
        animator[1].SetBool("Shrink", false);*/
    }

    void RollOne()
    {
        switch (previousRoll)
        {
            case 1: DisableOne(); break;
            case 2: DisableTwo(); break;
            case 3: DisableThree(); break;
            case 4: DisableFour(); break;
            case 5: DisableFive(); break;
            case 6: DisableSix(); break;
        }

        renderHex[0].enabled = true;
    }

    void RollTwo()
    {
        switch (previousRoll)
        {
            case 1: DisableOne(); break;
            case 2: DisableTwo(); break;
            case 3: DisableThree(); break;
            case 4: DisableFour(); break;
            case 5: DisableFive(); break;
            case 6: DisableSix(); break;
        }

        renderHex[1].enabled = true;
        renderHex[4].enabled = true;
    }

    void RollThree()
    {
        switch (previousRoll)
        {
            case 1: DisableOne(); break;
            case 2: DisableTwo(); break;
            case 3: DisableThree(); break;
            case 4: DisableFour(); break;
            case 5: DisableFive(); break;
            case 6: DisableSix(); break;
        }

        renderHex[1].enabled = true;
        renderHex[4].enabled = true;
        renderHex[0].enabled = true;
    }

    void RollFour()
    {
        switch (previousRoll)
        {
            case 1: DisableOne(); break;
            case 2: DisableTwo(); break;
            case 3: DisableThree(); break;
            case 4: DisableFour(); break;
            case 5: DisableFive(); break;
            case 6: DisableSix(); break;
        }

        renderHex[0].enabled = true;
        renderHex[1].enabled = true;
        renderHex[3].enabled = true;
        renderHex[5].enabled = true;
    }

    void RollFive()
    {
        switch (previousRoll)
        {
            case 1: DisableOne(); break;
            case 2: DisableTwo(); break;
            case 3: DisableThree(); break;
            case 4: DisableFour(); break;
            case 5: DisableFive(); break;
            case 6: DisableSix(); break;
        }

        renderHex[0].enabled = true;
        renderHex[1].enabled = true;
        renderHex[3].enabled = true;
        renderHex[4].enabled = true;
        renderHex[6].enabled = true;
    }

    void RollSix()
    {
        switch (previousRoll)
        {
            case 1: DisableOne(); break;
            case 2: DisableTwo(); break;
            case 3: DisableThree(); break;
            case 4: DisableFour(); break;
            case 5: DisableFive(); break;
            case 6: DisableSix(); break;
        }

        renderHex[1].enabled = true;
        renderHex[2].enabled = true;
        renderHex[3].enabled = true;
        renderHex[4].enabled = true;
        renderHex[5].enabled = true;
        renderHex[6].enabled = true;
    }

    void DisableOne()
    {
        renderHex[0].enabled = false;
    }

    void DisableTwo()
    {
        renderHex[1].enabled = false;
        renderHex[4].enabled = false;
    }

    void DisableThree()
    {
        renderHex[1].enabled = false;
        renderHex[4].enabled = false;
        renderHex[0].enabled = false;
    }

    void DisableFour()
    {
        renderHex[0].enabled = false;
        renderHex[1].enabled = false;
        renderHex[3].enabled = false;
        renderHex[5].enabled = false;
    }

    void DisableFive()
    {
        renderHex[0].enabled = false;
        renderHex[1].enabled = false;
        renderHex[3].enabled = false;
        renderHex[4].enabled = false;
        renderHex[6].enabled = false;
    }

    void DisableSix()
    {
        renderHex[1].enabled = false;
        renderHex[2].enabled = false;
        renderHex[3].enabled = false;
        renderHex[4].enabled = false;
        renderHex[5].enabled = false;
        renderHex[6].enabled = false;
    }
}
