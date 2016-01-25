﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
    
	public List<CardController> cards = new List<CardController>();
	public GameObject CardPrefab;
	CardController lastAddedCard;
    PlayerHandController playerHand;

	// Use this for initialization
	void Start () {
        playerHand = transform.GetComponentInParent<PlayerHandController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{	
	}

    public CardController DrawCard()
    {
        var newCard = (GameObject)Instantiate(CardPrefab, transform.position, transform.rotation);
        cards.Add(newCard.GetComponent<CardController>());
        lastAddedCard = cards[playerHand.cardsInHand.Count];
        lastAddedCard.deckController = this.transform.GetComponent<Deck>();
        lastAddedCard.CardSpeed = playerHand.cardsInHand.Count;
        lastAddedCard.CardDirectionValue = playerHand.cardsInHand.Count;
        lastAddedCard.CardDirectionIsRight = playerHand.cardsInHand.Count % 2 == 0 ? true : false;
        lastAddedCard.targetPosition = transform.position;
        lastAddedCard.cardFront = ReturnCardFront(lastAddedCard.CardDirectionValue, lastAddedCard.CardDirectionIsRight, lastAddedCard);

        return lastAddedCard;
    }

    public Sprite ReturnCardFront(int value, bool right, CardController lastAddedCard)
    {
        if (value == 0)
        {
            return lastAddedCard.FrontGray;
        }
        else
            return (right ? lastAddedCard.FrontBlue : lastAddedCard.FrontRed);
    }
}
