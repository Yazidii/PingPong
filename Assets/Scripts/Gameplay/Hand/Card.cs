using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	public Sprite FrontBlue;
	public Sprite FrontRed;
	public Sprite FrontGray;
	public Sprite Back;

	private float targetRotation = 0;
	private float startRotation;
	private float rotationRate = 9;
	public bool front = false;

	//Card Properties and Components
	public int CardSpeed;
	public int CardDirectionValue;
	public bool CardDirectionIsRight;

	SpriteRenderer cardSpriteRenderer;

	MeshRenderer speedTextRenderer;
	MeshRenderer directionTextRenderer;
	MeshRenderer descriptionTextRenderer;

	TextMesh speedText;
	TextMesh directionText;
	TextMesh descriptionText;
	Sprite cardFront;
    Collider collider;

    Transform cardGraphics;
	Transform cardSpriteObject;

	public bool hover = false;




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
		directionText.text = CardDirectionIsRight ? "Right: " : "Left: " + CardDirectionValue.ToString();


		speedTextRenderer.sortingLayerName = "CardText";
		directionTextRenderer.sortingLayerName = "CardText";
		descriptionTextRenderer.sortingLayerName = "CardText";

		cardFront = FrontGray;

	}
	
	// Update is called once per frame
	void Update () {


		//Call to rotate or move card
		UpdateCardPosition();

		//Call to update card graphics to what they should currently be
		UpdateCardGraphics();

		if (Input.GetKeyDown("space"))
		{
			FlipCard();
		}
	}

	void FlipCard()
	{
			targetRotation = Mathf.Abs(targetRotation - 180);
			Debug.Log(targetRotation);
	}

	void UpdateCardPosition()
	{				
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
		FlipCard();
	}

	void OnMouseExit()
	{
		FlipCard();
	}

	void OnMouseDown()
	{
		FlipCard();
	}
}
