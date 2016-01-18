using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	public Sprite FrontBlue;
	public Sprite FrontRed;
	public Sprite FrontGray;
	public Sprite Back;
	private struct Values 
	{
		private int speed;
		private int direction;
		private bool right;
	}

	private float targetRotation = 0;
	private float startRotation;
	private float rotationRate = 9;
	public bool front = false;


	//Card Properties and Components
	SpriteRenderer cardSpriteRenderer;
	MeshRenderer speedText;
	MeshRenderer directionText;
	MeshRenderer descriptionText;
	Sprite cardFront;

	public bool hover = false;




	// Use this for initialization
	void Start () {
		cardSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		speedText = transform.Find("Speed").GetComponent<MeshRenderer>();
		directionText = transform.Find("Direction").GetComponent<MeshRenderer>();
		descriptionText = transform.Find("Description").GetComponent<MeshRenderer>();
		speedText.sortingLayerName = "CardText";
		directionText.sortingLayerName = "CardText";
		descriptionText.sortingLayerName = "CardText";

		cardFront = FrontGray;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateCardPosition();
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
		if (transform.rotation.eulerAngles.y != targetRotation)
		{
			 Vector3 to = new Vector3(0, targetRotation, 0);
	         if (Vector3.Distance(transform.eulerAngles, to) > 0.01f)
	         {
	             transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to,rotationRate * Time.deltaTime);
	         }
	         else
	         {
	             transform.eulerAngles = to;
	         }
     	}
	}

	void UpdateCardGraphics()
	{
	    if (transform.rotation.eulerAngles.y > 90 && transform.rotation.eulerAngles.y < 270)
 		{
 			transform.GetChild(0).localScale = new Vector3(-1, 1, 1);		
 			front = true;
 			cardSpriteRenderer.sprite = cardFront;
 			speedText.GetComponent<Renderer>().enabled = true;
 			directionText.GetComponent<Renderer>().enabled = true;
 			descriptionText.GetComponent<Renderer>().enabled = true;

 		}
 		else
 		{
 			transform.GetChild(0).localScale = new Vector3(1, 1, 1);
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

	void OnMouseHover()
	{
		hover = true;
	}
}
