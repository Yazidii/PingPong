using UnityEngine;
using System.Collections;
using Assets.Scripts.Engine;

public class BallController : MonoBehaviour {

	Vector3 targetPosition;
	float speed = 0.2f;
    Vector3 originalScale;
    float top;
    float bottom;

    void Awake()
    {
        originalScale = transform.localScale;
    }

	// Use this for initialization
	void Start () {
		targetPosition = GameObject.Find("Bottom/0").GetComponent<Transform>().position;
        top = GameObject.Find("Top").GetComponent<Transform>().GetChild(0).localPosition.y;
        bottom = GameObject.Find("Bottom").GetComponent<Transform>().GetChild(0).localPosition.y;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
        transform.localScale = originalScale * (0.65f + (0.35f * (top - transform.localPosition.y) / (top - bottom)));
	}

	public void MoveBall(Vector3 origin, Vector3 target) {
		transform.position = origin;
		targetPosition = target;
	}
}
