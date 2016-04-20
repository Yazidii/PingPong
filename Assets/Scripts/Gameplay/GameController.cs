using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using Assets.Scripts.Engine;


public class GameController : MonoBehaviour {

    #region Properties

    private bool isTopSide; //false = top
	private int speedScore = 0;
//	private int maxGameScore = 31;


	//Objects
	private BallController ballController;
	private LineRenderer moveLineRenderer;
	private LineRenderer unalteredLineRenderer;
	private LineRenderer projectedLineRenderer;
    private TextMesh totalSpeedText;

	//Temporary
	private Vector3 origin, destination, projection, unaltered;
	private Transform topPoints;
	private Transform bottomPoints;
	private IHandController playerHandController;
    private IHandController aiHandController;



    private AiHandController aiController;

    private static GameObject cardPreview;

    //Temp
    private int cardsInPlayerHand;
    private int cardsInEnemyHand;

    #endregion Properties

    #region Initialization

    // Use this for initialization
    void Start()
    {
        isTopSide = false;
        GlobalVariables.currentPosition = 0;
        GlobalVariables.previousPosition = 0;
        //		projectedPosition = 0;
        //		resultingPosition = 0;
        speedScore = 0;
        GlobalVariables.deviation = 0;


        ballController = GameObject.Find("Ball").GetComponent<BallController>();
        moveLineRenderer = GameObject.Find("MoveLine").GetComponent<LineRenderer>();
        unalteredLineRenderer = GameObject.Find("UnalteredLine").GetComponent<LineRenderer>();
        projectedLineRenderer = GameObject.Find("ProjectedLine").GetComponent<LineRenderer>();

        projectedLineRenderer.sortingLayerName = "ProjectedLine";


        playerHandController = GameObject.Find("Hand").GetComponent<PlayerHandController>();
        aiHandController = GameObject.Find("EnemyHand").GetComponent<AiHandController>();

        topPoints = GameObject.Find("Top").GetComponent<Transform>();
        bottomPoints = GameObject.Find("Bottom").GetComponent<Transform>();

        totalSpeedText = GameObject.Find("TotalSpeed").GetComponent<TextMesh>();
        GameObject.Find("TotalSpeed").GetComponent<MeshRenderer>().sortingLayerName = "Background";

        //Temp
        UpdateDrawingCoordinates();

        aiController = GameObject.Find("EnemyHand").GetComponent<AiHandController>();

        cardPreview = GameObject.Find("CardPreview");

        GlobalVariables.playerTurn = true;

        DrawCards();
    }

    #endregion Initialization

    #region Events

    public void CardPlayedEvent(int speed, int direction)
    {
        GlobalVariables.previousPosition = GlobalVariables.currentPosition;
        GlobalVariables.deviation = GlobalVariables.deviation + direction;
        speedScore += speed;
        totalSpeedText.text = speedScore + "/21";
        GlobalVariables.currentPosition = GlobalVariables.previousPosition + GlobalVariables.deviation;

        if (-4 <= GlobalVariables.currentPosition && GlobalVariables.currentPosition <= 4 && speedScore <= 21)
        {
            origin = GetOrigin(GlobalVariables.previousPosition, isTopSide);
            destination = GetDestination(GlobalVariables.currentPosition, isTopSide);

            isTopSide = !isTopSide;

            ballController.MoveBall(origin, destination);
        }
        else {
            ExecuteGameOverEvent();
            Debug.Log("Current Position:" + GlobalVariables.currentPosition);
        }

        GlobalVariables.playerTurn = !GlobalVariables.playerTurn;

        if (!GlobalVariables.playerTurn)
        {
            aiController.PlayOpponentCard();
        }
    }

    void ExecuteGameOverEvent()
    {

        origin = GetOrigin(GlobalVariables.previousPosition, isTopSide);
        destination = GetDestination(GlobalVariables.currentPosition, isTopSide);

        isTopSide = !isTopSide;

        ballController.MoveBall(origin, destination);

        moveLineRenderer.enabled = false;
        projectedLineRenderer.enabled = false;
        unalteredLineRenderer.enabled = false;

        if (GlobalVariables.playerTurn)
            Debug.Log("Game Over, You Lose");
        else
            Debug.Log("Game Over, You Win");
        SceneManager.LoadScene("Dev");
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

    void OnApplicationQuit()
    {
        GlobalVariables.applicationQuitting = true;
    }

    #endregion Events

    #region Update Methods

    void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //GL.Clear(true,true,Color.green,0.0f);

        UpdateHelperLines();

        UpdateDrawingCoordinates();
    }

    public static void UpdateCardPreview()
    {
        if (GlobalVariables.cardIsActive && GlobalVariables.activeCard != null && GlobalVariables.activeCard.handController.PlayerHand)
        {
            cardPreview.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
            cardPreview.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = GlobalVariables.activeCard.cardSpriteRenderer.sprite;
            cardPreview.transform.GetChild(3).GetComponent<SpriteRenderer>().sortingLayerName = "ActiveCard";

            cardPreview.transform.GetChild(0).GetComponent<MeshRenderer>().sortingLayerName = "ActiveCardText";
            cardPreview.transform.GetChild(0).GetComponent<TextMesh>().text = GlobalVariables.activeCard.CardDescriptionText;
            cardPreview.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            cardPreview.transform.GetChild(1).GetComponent<MeshRenderer>().sortingLayerName = "ActiveCardText";
            cardPreview.transform.GetChild(1).GetComponent<TextMesh>().text = GlobalVariables.activeCard.CardSpeed.ToString();
            cardPreview.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;

            cardPreview.transform.GetChild(2).GetComponent<MeshRenderer>().sortingLayerName = "ActiveCardText";
            cardPreview.transform.GetChild(2).GetComponent<TextMesh>().text = GlobalVariables.activeCard.CardDirectionValue > 0 ? ((GlobalVariables.activeCard.CardDirectionIsRight ? "Right: " : "Left: ") + GlobalVariables.activeCard.CardDirectionValue.ToString()) : "None";
            cardPreview.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
        }

        if (!GlobalVariables.cardIsActive || GlobalVariables.activeCard == null)
        {
            cardPreview.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
            cardPreview.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            cardPreview.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            cardPreview.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void UpdateHelperLines()
    {
        if (GlobalVariables.cardIsActive && GlobalVariables.activeCard != null && GlobalVariables.playerTurn)
            projectedLineRenderer.enabled = GlobalVariables.activeCard.handController.PlayerHand && GlobalVariables.playerTurn;
        else
            projectedLineRenderer.enabled = false;
    }

    void UpdateDrawingCoordinates()
    {
        //Debug.Log(devClass.GetOrigin(previousPosition, !isTopSide));

        //Move this to card played event
        origin = GetOrigin(GlobalVariables.previousPosition, !isTopSide);
        if (GlobalVariables.currentPosition <= 4 && GlobalVariables.currentPosition >= -4)
            destination = GetDestination(GlobalVariables.currentPosition, !isTopSide);

        unaltered = GetDestination(GlobalVariables.currentPosition + GlobalVariables.deviation, isTopSide); //throws exception when outside of table

        projection = GetDestination(GlobalVariables.currentPosition + GlobalVariables.deviation + GetDirection(), isTopSide); //throws exception when outside of table

        moveLineRenderer.SetPosition(0, origin + new Vector3(0, 0, -0.1f));
        moveLineRenderer.SetPosition(1, destination + new Vector3(0, 0, -0.1f));

        unalteredLineRenderer.SetPosition(0, destination + new Vector3(0, 0, -0.1f));
        unalteredLineRenderer.SetPosition(1, unaltered + new Vector3(0, 0, -0.1f));

        projectedLineRenderer.SetPosition(0, destination + new Vector3(0, 0, -0.1f));
        projectedLineRenderer.SetPosition(1, projection + new Vector3(0, 0, -0.1f));

        //Debug.Log(origin);
        //Debug.Log(destination);
        //Debug.Log(unaltered);
        //Debug.Log(projection);

    }

    #endregion Update Methods

    #region Helper Methods

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

    int GetDirection()
    {
        return playerHandController.GetDirection();
    }

    #endregion Helper Methods

    #region Coroutines

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.4f);
        DrawCards();
    }

    #endregion Coroutines
}
