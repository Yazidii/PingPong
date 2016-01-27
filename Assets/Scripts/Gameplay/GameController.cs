﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/*
TO DO

    Lots of refactoring
    Create static class with global variables
    Create end game
    Render dotted and animated lines
    Card movement turning animation

*/


public class GameController : MonoBehaviour {

	private bool isTopSide; //false = top
	private int speedScore;
//	private int maxGameScore = 31;
	public static int currentPosition;
	public static int previousPosition;
	public static int deviation;

	//Objects
	private BallController ballController;
	private LineRenderer moveLineRenderer;
	private LineRenderer unalteredLineRenderer;
	private LineRenderer projectedLineRenderer;

	//Temporary
	private Vector3 origin, destination, projection, unaltered;
	private Transform topPoints;
	private Transform bottomPoints;
	private IHandController playerHandController;
    private IHandController aiHandController;

    //Global
    public static bool cardIsActive;
    public static bool playerTurn;
    public static bool applicationQuitting = false;
    public static CardController activeCard;

    private AiHandController aiController;

    private static GameObject cardPreview;

    //Temp
    private int cardsInPlayerHand;
    private int cardsInEnemyHand;

	// Use this for initialization
	void Start () {
		isTopSide = false;
		currentPosition = 0;
		previousPosition = 0;
//		projectedPosition = 0;
//		resultingPosition = 0;
		speedScore = 0;
		deviation = 0;


		ballController = GameObject.Find("Ball").GetComponent<BallController>();
		moveLineRenderer = GameObject.Find("MoveLine").GetComponent<LineRenderer>();
		unalteredLineRenderer = GameObject.Find("UnalteredLine").GetComponent<LineRenderer>();
		projectedLineRenderer = GameObject.Find("ProjectedLine").GetComponent<LineRenderer>();

        projectedLineRenderer.sortingLayerName = "ProjectedLine";


		playerHandController = GameObject.Find("Hand").GetComponent<PlayerHandController>();
        aiHandController = GameObject.Find("EnemyHand").GetComponent<AiHandController>();

        topPoints = GameObject.Find("Top").GetComponent<Transform>();
		bottomPoints = GameObject.Find("Bottom").GetComponent<Transform>();

		//Temp
		UpdateDrawingCoordinates();

        aiController = GameObject.Find("EnemyHand").GetComponent<AiHandController>();

        cardPreview = GameObject.Find("CardPreview");

        playerTurn = true;

        DrawCards();
	}
	
    void Update()
    {

    }

	// Update is called once per frame
	void FixedUpdate () {
        //GL.Clear(true,true,Color.green,0.0f);

        UpdateHelperLines();

		UpdateDrawingCoordinates();
	}

    void UpdateHelperLines()
    {
        if (cardIsActive && activeCard != null && playerTurn)
            projectedLineRenderer.enabled = activeCard.handController.PlayerHand && playerTurn;
        else
            projectedLineRenderer.enabled = false;
    }

	void UpdateDrawingCoordinates()
	{	
		//Debug.Log(devClass.GetOrigin(previousPosition, !isTopSide));

		//Move this to card played event
		origin = GetOrigin(previousPosition, !isTopSide);
		if (currentPosition <= 4 && currentPosition >= -4)
	    destination = GetDestination(currentPosition, !isTopSide);
        
        unaltered = GetDestination(currentPosition + deviation, isTopSide); //throws exception when outside of table
        
        projection = GetDestination(currentPosition + deviation + GetDirection(), isTopSide); //throws exception when outside of table
        
        moveLineRenderer.SetPosition(0, origin + new Vector3(0,0,-0.1f));
        moveLineRenderer.SetPosition(1, destination + new Vector3(0,0,-0.1f));

        unalteredLineRenderer.SetPosition(0, destination + new Vector3(0,0,-0.1f));
        unalteredLineRenderer.SetPosition(1, unaltered + new Vector3(0,0,-0.1f));

        projectedLineRenderer.SetPosition(0, destination + new Vector3(0,0,-0.1f));
        projectedLineRenderer.SetPosition(1, projection + new Vector3(0,0,-0.1f));

        //Debug.Log(origin);
        //Debug.Log(destination);
        //Debug.Log(unaltered);
        //Debug.Log(projection);

	}

	public void CardPlayedEvent(int speed, int direction)
	{
		previousPosition = currentPosition;
		deviation = deviation + direction;
		speedScore += speed;
		currentPosition = previousPosition + deviation;
        //projectedPosition = currentPosition + deviation;

        if (-4 <= currentPosition && currentPosition <= 4)
        {
            origin = GetOrigin(previousPosition, isTopSide);
            destination = GetDestination(currentPosition, isTopSide);

            isTopSide = !isTopSide;

            ballController.MoveBall(origin, destination);
        }
        else {
            ExecuteGameOverEvent();
            Debug.Log("Current Position:" + currentPosition);
        }

        playerTurn = !playerTurn;

        if (!playerTurn)
        {
            aiController.PlayOpponentCard();
        }
	}


	Vector3 GetOrigin(int originNumber, bool topSide)
	{
            if (topSide)
                return topPoints.Find(originNumber.ToString()).position;
            else
                return bottomPoints.Find(originNumber.ToString()).position;
	}

	Vector3 GetDestination(int destinationNumber, bool topSide)
	{
        if (topSide)
        {
            if (destinationNumber > 4)
                return new Vector3((bottomPoints.Find("4").position.x - bottomPoints.Find("3").position.x) * (destinationNumber - 4) + bottomPoints.Find("4").position.x, bottomPoints.Find("4").position.y, bottomPoints.Find("4").position.z);
            if (destinationNumber < -4)
                return new Vector3(bottomPoints.Find("-4").position.x - (bottomPoints.Find("-4").position.x - bottomPoints.Find("-3").position.x) * (destinationNumber + 4), bottomPoints.Find("-4").position.y, bottomPoints.Find("-4").position.z);
            return bottomPoints.Find(destinationNumber.ToString()).position;
        }
        else
        {
            if (destinationNumber > 4)
                return new Vector3((topPoints.Find("4").position.x - topPoints.Find("3").position.x) * (destinationNumber - 4) + topPoints.Find("4").position.x, topPoints.Find("4").position.y, topPoints.Find("4").position.z);
            if (destinationNumber < -4)
                return new Vector3(topPoints.Find("-4").position.x - (topPoints.Find("-4").position.x - topPoints.Find("-3").position.x) * (destinationNumber + 4), topPoints.Find("-4").position.y, topPoints.Find("-4").position.z);
            return topPoints.Find(destinationNumber.ToString()).position;
        }
    }

	int GetDirection() {
			return playerHandController.GetDirection();
	}

    void OnApplicationQuit()
    {
        applicationQuitting = true;
    }
    
    void ExecuteGameOverEvent()
    {
        if (playerTurn)
            Debug.Log("Game Over, You Lose");
        else
            Debug.Log("Game Over, You Win");
        Application.Quit();
    }

    public static void UpdateCardPreview()
    {
        if (cardIsActive && activeCard != null && activeCard.handController.PlayerHand)
        {
            cardPreview.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
            cardPreview.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = activeCard.cardSpriteRenderer.sprite;
            cardPreview.transform.GetChild(3).GetComponent<SpriteRenderer>().sortingLayerName = "ActiveCard";

            cardPreview.transform.GetChild(0).GetComponent<MeshRenderer>().sortingLayerName = "ActiveCardText";
            cardPreview.transform.GetChild(0).GetComponent<TextMesh>().text = activeCard.CardDescriptionText;
            cardPreview.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            cardPreview.transform.GetChild(1).GetComponent<MeshRenderer>().sortingLayerName = "ActiveCardText";
            cardPreview.transform.GetChild(1).GetComponent<TextMesh>().text = activeCard.CardSpeed.ToString();
            cardPreview.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;

            cardPreview.transform.GetChild(2).GetComponent<MeshRenderer>().sortingLayerName = "ActiveCardText";
            cardPreview.transform.GetChild(2).GetComponent<TextMesh>().text = activeCard.CardDirectionValue > 0 ? ((activeCard.CardDirectionIsRight ? "Right: " : "Left: ") + activeCard.CardDirectionValue.ToString()) : "None";
            cardPreview.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
        }

        if (!cardIsActive || activeCard == null)
        {
            cardPreview.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
            cardPreview.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            cardPreview.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            cardPreview.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void DrawCards()
    {
        if (cardsInPlayerHand < 5)
        {
            playerHandController.DrawCard();
            cardsInPlayerHand++;
            StartCoroutine(Wait());
        }
        else
            if (cardsInEnemyHand < 5)
        {
            aiHandController.DrawCard();
            cardsInEnemyHand++;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.4f);
        DrawCards();
    }
}
