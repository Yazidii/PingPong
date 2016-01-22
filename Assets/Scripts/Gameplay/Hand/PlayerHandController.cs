using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandController : MonoBehaviour {

	Deck deckController;

    public List<CardController> cardsInHand = new List<CardController>();

    Transform LeftSide;
    Transform RightSide;
    public bool PlayerHand;

	// Use this for initialization
	void Start () {
        //deckController = transform.Find("Deck").GetComponent<Deck>();
        LeftSide = transform.GetChild(1);
        RightSide = transform.GetChild(2);
        deckController = transform.GetChild(0).GetComponent<Deck>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.D))
        {
            if (cardsInHand.Count < 5)
            {
                cardsInHand.Add(deckController.DrawCard());
                int tempCount = 1;
                foreach (CardController card in cardsInHand)
                {
                    card.targetPosition = new Vector3((LeftSide.position.x + ((RightSide.position.x - LeftSide.position.x) / (cardsInHand.Count + 1)) * tempCount++), RightSide.position.y, RightSide.position.z);
                    card.initialPosition = card.targetPosition;
                    if (PlayerHand && !card.front)
                    {
                        card.FlipCard();
                    }
                    if (!PlayerHand)
                    {
                        card.transform.localScale = new Vector3(-1, -1, 1);
                    }
                    card.playerHandController = this.transform.GetComponent<PlayerHandController>();
                }
                
            }
        }

        int tempCount2 = 1;
        foreach (CardController card in cardsInHand)
        {
            if (!card.mouseEntered)
            {
                card.targetPosition = new Vector3((LeftSide.position.x + ((RightSide.position.x - LeftSide.position.x) / (cardsInHand.Count + 1)) * tempCount2), RightSide.position.y, RightSide.position.z);
                card.initialPosition = card.targetPosition;
            }
            tempCount2++;
        }

        }

	public int GetDirection()
	{/*
		foreach (CardController card in cards)
			if (card.hover)
				return card.GetDirection();*/
		return 0;
	}

	public void DrawCard()
	{
		Debug.Log("Drawing Card");
	}

}
