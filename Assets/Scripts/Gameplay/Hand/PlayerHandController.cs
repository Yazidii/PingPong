﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandController : MonoBehaviour, IHandController {

	Deck deckController;

    public List<CardController> cardsInHand = new List<CardController>();

    Transform LeftSide;
    Transform RightSide;
    bool IHandController.PlayerHand
    {
        get { return _playerHand; }
    }
    public GameObject CardPrefab;

    // Variable that controls whether player hand can be used
    public bool IsLocked;
    bool _playerHand = true;

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
                DrawCard();
            }
        }

        }

    public int GetDirection()
    {
        int result = 0;
        foreach (CardController card in cardsInHand)
        {
            if (card.isActive)
            {
                result = card.CardDirectionIsRight ? card.CardDirectionValue : -card.CardDirectionValue;
            }
        }
        return result;
    }

    public void DrawCard()
    {
        var newCard = (GameObject)Instantiate(CardPrefab, deckController.transform.position, deckController.transform.rotation);
        cardsInHand.Add(newCard.GetComponent<CardController>());

        var lastAddedCard = cardsInHand[cardsInHand.Count - 1];
        BuildCard(lastAddedCard);

        lastAddedCard.FlipCard();

        UpdateCardPositions();
    }

    void BuildCard(CardController card)
    {
        card.deckController = this.transform.GetComponent<Deck>();
        card.CardSpeed = cardsInHand.Count;
        card.CardDirectionValue = cardsInHand.Count;
        card.CardDirectionIsRight = cardsInHand.Count % 2 == 0 ? true : false;
        card.targetPosition = transform.position;
        card.cardFront = ReturnCardFront(card.CardDirectionValue, card.CardDirectionIsRight, card);
        card.SetHandController(transform.GetComponent<PlayerHandController>());
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

    public void UpdateCardPositions()
    {
        int tempCount2 = 1;
        foreach (CardController card in cardsInHand)
        {
            if (card != null)
            {
                card.targetPosition = new Vector3((LeftSide.position.x + ((RightSide.position.x - LeftSide.position.x) / (cardsInHand.Count + 1)) * tempCount2), RightSide.position.y, RightSide.position.z);
                card.initialPosition = card.targetPosition;
            }
            tempCount2++;
        }
    }

    public void RemoveCard(CardController card)
    {
        cardsInHand.Remove(card);
    }
}
