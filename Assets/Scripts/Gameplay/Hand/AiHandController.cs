﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiHandController : MonoBehaviour, IHandController {

    Deck deckController;

    public List<CardController> cardsInHand = new List<CardController>();

    Transform LeftSide;
    Transform RightSide;
    public GameObject CardPrefab;
    bool IHandController.PlayerHand
    {
        get { return _playerHand; }
    }

    // Variable that controls whether player hand can be used
    public bool IsLocked = true;

    bool _playerHand = false;

    // Use this for initialization
    void Start()
    {
        //deckController = transform.Find("Deck").GetComponent<Deck>();
        LeftSide = transform.GetChild(1);
        RightSide = transform.GetChild(2);
        deckController = transform.GetChild(0).GetComponent<Deck>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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

        lastAddedCard.transform.localScale = new Vector3(-1, -1, 1);
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
        card.SetHandController(transform.GetComponent<AiHandController>());
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

    public void PlayOpponentCard()
    {
        StartCoroutine(Think());
    }

    private void PlayCardResponse()
    {
        foreach (CardController card in cardsInHand)
        {
            int result = 2 * GameController.currentPosition - GameController.previousPosition + (card.CardDirectionIsRight ? card.CardDirectionValue : -card.CardDirectionValue);
            if (-4 <= result && result <= 4)
            {
                card.PlayCard();
                return;
            }
        }
        cardsInHand[0].PlayCard();
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(2);
        PlayCardResponse();
    }
}
