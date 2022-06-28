using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public CardScriptable Card;
    [SerializeField] private MeshRenderer DisplayImage;
    [SerializeField] private MeshRenderer TextDisplayImage;
    [SerializeField] private MeshRenderer Name;
    [SerializeField] private MeshRenderer HP;
    [SerializeField] private MeshRenderer Attack;
    [SerializeField] private MeshRenderer Manacost;
 
    void Start()
    {
        DisplayImage.material = Card.CardImage;
        TextDisplayImage.material = Card.TextImage;
    }
}
