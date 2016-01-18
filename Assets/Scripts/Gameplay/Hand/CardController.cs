using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour {



	void Start() 
	{

	}

	void Update()
	{

	}

	void MoveTo(Vector3 target)
	{

	}

	//Deprecated

	/*
	Vector3 targetPosition;
	Vector3 initialPosition;
	float speed = 0.2f;
	int speedValue;
	int directionValue;
	public bool hover;
	MeshRenderer textOnCard;
	bool mouseDown;

	GameController gameController;

	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
		targetPosition = transform.position;
		speedValue = Random.Range(0,10);
		directionValue = Random.Range (-2,2);
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		textOnCard = transform.GetComponentInChildren<MeshRenderer>();
		textOnCard.sortingLayerName = "CardText";
		transform.GetComponentInChildren<TextMesh>().text = "Spd:"  + speedValue + " Dir: " + directionValue;
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseDown) 
		{
		targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,10));
		}
	transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
	}

	void OnMouseOver() {
		//Debug.Log(transform.name);
		//Debug.Log(speedValue);
		//Debug.Log(directionValue);
	}

	void OnMouseEnter() {
		targetPosition = initialPosition + new Vector3(0, 2, 0);
		hover = true;
	}

	void OnMouseExit() {
		targetPosition = initialPosition;
		hover = false;
	}

	void OnMouseDown()
	{	
		mouseDown = true;
		//gameController.CardPlayedEvent(speedValue, directionValue);
		
	}


	void OnMouseUp()
	{	
		mouseDown = false;
		if (targetPosition.y - initialPosition.y > 3)
			{gameController.CardPlayedEvent(speedValue, directionValue);
			speedValue = Random.Range(0,10);
			directionValue = Random.Range (-2,2);
			transform.GetComponentInChildren<TextMesh>().text = "Spd:"  + speedValue + " Dir: " + directionValue;}
			targetPosition = initialPosition;
	}

	public int GetDirection(){
		return directionValue;
	}
	*/
}
