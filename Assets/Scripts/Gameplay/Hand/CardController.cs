using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour {

	public Sprite FrontBlue;
	public Sprite FrontRed;
	public Sprite FrontGray;
	public Sprite Back;

	private float targetRotation = 0;
	private float rotationRate = 9;
	public bool front = false;

    public bool mouseDown = false;
    public bool mouseEntered = false;

    //Card Properties and Components
    public bool isActive = false;

    public int CardSpeed;
	public int CardDirectionValue;
	public bool CardDirectionIsRight;

    public Vector3 targetPosition;
    float cardMovingSpeed = 0.2f;

    public SpriteRenderer cardSpriteRenderer;

	MeshRenderer speedTextRenderer;
	MeshRenderer directionTextRenderer;
	MeshRenderer descriptionTextRenderer;

	TextMesh speedText;
	TextMesh directionText;
	TextMesh descriptionText;
	public Sprite cardFront;
    //Collider collider;

    Transform cardGraphics;
	Transform cardSpriteObject;

	public bool hover = false;

    public Vector3 initialPosition;

    public IHandController handController;
    public Deck deckController;

    GameController gameController;


    // Use this for initialization
    void Start () {
		cardGraphics = transform.Find("Graphics");
		cardSpriteObject = cardGraphics.Find("Sprite");

		cardSpriteRenderer = cardSpriteObject.GetComponent<SpriteRenderer>();
		speedText = cardGraphics.Find("Speed").GetComponent<TextMesh>();
		directionText = cardGraphics.Find("Direction").GetComponent<TextMesh>();
		descriptionText = cardGraphics.Find("Description").GetComponent<TextMesh>();

		speedTextRenderer = cardGraphics.Find("Speed").GetComponent<MeshRenderer>();
		directionTextRenderer = cardGraphics.Find("Direction").GetComponent<MeshRenderer>();
		descriptionTextRenderer = cardGraphics.Find("Description").GetComponent<MeshRenderer>();

		speedText.text = CardSpeed.ToString();
        if (CardDirectionValue != 0)
            directionText.text = (CardDirectionIsRight ? "Right: " : "Left: ") + CardDirectionValue;
        else
            directionText.text = "None";


		speedTextRenderer.sortingLayerName = "CardText";
		directionTextRenderer.sortingLayerName = "CardText";
		descriptionTextRenderer.sortingLayerName = "CardText";

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
		//Call to rotate or move card
		UpdateCardPosition();

		//Call to update card graphics to what they should currently be
		UpdateCardGraphics();

		if (Input.GetKeyDown("space"))
		{
			FlipCard();
		}
	}

	public void FlipCard()
	{
			targetRotation = Mathf.Abs(targetRotation - 180);
	}

	void UpdateCardPosition()
	{
        if (mouseDown)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, cardMovingSpeed);

        if (cardGraphics.rotation.eulerAngles.y != targetRotation)
		{
			 Vector3 to = new Vector3(0, targetRotation, 0);
	         if (Vector3.Distance(cardGraphics.eulerAngles, to) > 0.01f)
	         {
	             cardGraphics.eulerAngles = Vector3.Lerp(cardGraphics.rotation.eulerAngles, to,rotationRate * Time.deltaTime);
	         }
	         else
	         {
	             cardGraphics.eulerAngles = to;
	         }
     	}

        speedTextRenderer.sortingLayerName = isActive ? "ActiveCardText" : "CardText";
        directionTextRenderer.sortingLayerName = isActive ? "ActiveCardText" : "CardText";
        descriptionTextRenderer.sortingLayerName = isActive ? "ActiveCardText" : "CardText";
        cardSpriteRenderer.sortingLayerName = isActive ? "ActiveCard" : "Card";

    }

	void UpdateCardGraphics()
	{
	    if (cardGraphics.rotation.eulerAngles.y > 90 && cardGraphics.rotation.eulerAngles.y < 270)
 		{
 			cardSpriteObject.localScale = new Vector3(-1, 1, 1);		
 			front = true;
 			cardSpriteRenderer.sprite = cardFront;
 			speedText.GetComponent<Renderer>().enabled = true;
 			directionText.GetComponent<Renderer>().enabled = true;
 			descriptionText.GetComponent<Renderer>().enabled = true;

 		}
 		else
 		{
 			cardSpriteObject.localScale = new Vector3(1, 1, 1);
 			front = false;
			cardSpriteRenderer.sprite = Back;
 			speedText.GetComponent<Renderer>().enabled = false;		
 			directionText.GetComponent<Renderer>().enabled = false;		
 			descriptionText.GetComponent<Renderer>().enabled = false;			
 		}
	}

	void UpdateCardLocation()
	{

	}

	void OnMouseEnter()
	{
        if (!GameController.cardIsActive)
        {
            if (GameController.playerTurn)
            {
                isActive = true;
                GameController.activeCard = this;
            }
            GameController.cardIsActive = true;
        }
        mouseEntered = true;
        targetPosition = initialPosition + new Vector3(0, transform.localScale.y, 0);
	}

	void OnMouseExit()
	{
        if (!mouseDown && isActive)
        {
            isActive = false;
            GameController.cardIsActive = false;
        }
        mouseEntered = false;
        targetPosition = initialPosition;
	}

    public void PlayCard()
    {
        GameController.cardIsActive = false;
        if (gameController != null)
            gameController.CardPlayedEvent(CardSpeed, CardDirectionIsRight ? CardDirectionValue : -CardDirectionValue);
        Destroy(this.gameObject);
    }

	void OnMouseDown()
	{
        if (GameController.playerTurn)
        mouseDown = true;
	}

    void OnMouseUp()
    {
        if (mouseDown)
        {
            mouseDown = false;
            targetPosition = initialPosition;
            if (transform.position.y > initialPosition.y + 8)
            {
                PlayCard();
            }
        }
    }

    void OnDestroy()
    {
        if (!GameController.applicationQuitting)
        {
            handController.RemoveCard(transform.GetComponent<CardController>()); 
            handController.UpdateCardPositions();
        }
    }

    public void SetHandController(IHandController handController)
    {
        this.handController = handController;
    }
}
