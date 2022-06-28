using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "BattleCard", order = 1)]
public class CardScriptable : ScriptableObject
{
    public Material CardImage;
    public Material TextImage;

    public int Health;
    public int AttackPower;
    public int Manacost;

    public bool isTaunt;
    public bool isElisive;

    public string CardName;
}
