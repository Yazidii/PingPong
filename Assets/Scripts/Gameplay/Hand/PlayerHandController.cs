using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandController : MonoBehaviour {


	List<Card> cards = new List<Card>();
	Deck deckController;

	public GameObject CardPrefab;



	// Use this for initialization
	void Start () {
		//deckController = transform.Find("Deck").GetComponent<Deck>();
	}
	
	// Update is called once per frame
	void Update () {
		
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
