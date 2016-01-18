using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {

	PlayerHandController handController;

	// Use this for initialization
	void Start () {
		handController = GameObject.Find("Hand").GetComponent<PlayerHandController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{	
		handController.DrawCard();
	}
}
