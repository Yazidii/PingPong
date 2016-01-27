using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	Vector3 targetPosition;
	float speed = 0.2f;
	// Use this for initialization
	void Start () {
		targetPosition = GameObject.Find("Bottom/0").GetComponent<Transform>().position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
	}

	public void MoveBall(Vector3 origin, Vector3 target) {
		transform.position = origin;
		targetPosition = target;
	}
}
