using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiceValue
{
    public int diceNumber;

    public DiceValue()
    {
        diceNumber = DiceRoll.diceValue;
    }
}
