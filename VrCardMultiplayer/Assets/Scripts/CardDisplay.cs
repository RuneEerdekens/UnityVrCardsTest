using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public CardScriptable Card;
    [SerializeField] private MeshRenderer DisplayImage;
    [SerializeField] private MeshRenderer TextDisplayImage;
 
    void Start()
    {
        DisplayImage.material = Card.CardImage;
        TextDisplayImage.material = Card.TextImage;
    }
}
