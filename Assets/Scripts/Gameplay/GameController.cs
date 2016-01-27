﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/*
TO DO

    Lots of refactoring
    Create static class with global variables
    Create end game


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

    //Global
    public static bool cardIsActive;
    public static bool playerTurn;
    public static bool applicationQuitting = false;

    private AiHandController aiController;

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

		topPoints = GameObject.Find("Top").GetComponent<Transform>();
		bottomPoints = GameObject.Find("Bottom").GetComponent<Transform>();

		//Temp
		UpdateDrawingCoordinates();

        aiController = GameObject.Find("EnemyHand").GetComponent<AiHandController>();

        playerTurn = true;
	}
	
	// Update is called once per frame
	void Update () {
		//GL.Clear(true,true,Color.green,0.0f);

		
		UpdateDrawingCoordinates();
	}


	void UpdateDrawingCoordinates()
	{	
		//Debug.Log(devClass.GetOrigin(previousPosition, !isTopSide));

		//Move this to card played event
		origin = GetOrigin(previousPosition, !isTopSide);
		if (currentPosition <= 4 && currentPosition >= -4)
	    destination = GetDestination(currentPosition, !isTopSide);
	    if (currentPosition + deviation <= 4 && currentPosition + deviation >= -4)
        unaltered = GetOrigin(currentPosition + deviation, !isTopSide); //throws exception when outside of table
        if (currentPosition + deviation + GetDirection() <= 4 && currentPosition + deviation + GetDirection() >= -4)
        projection = GetOrigin(currentPosition + deviation + GetDirection(), !isTopSide); //throws exception when outside of table
        

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
			else 
			Application.Quit();

		Debug.Log ("Current Position:" + currentPosition);

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
			return bottomPoints.Find(destinationNumber.ToString()).position;
			else
				return topPoints.Find(destinationNumber.ToString()).position;
    }

	int GetDirection() {
			return playerHandController.GetDirection();
	}

    void OnApplicationQuit()
    {
        applicationQuitting = true;
    }
}
