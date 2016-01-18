using UnityEngine;
using System.Collections;

public class BallPath : MonoBehaviour {
	
	public static void DrawTheLine(Vector3 from, Vector3 to)
	{	
		//Debug.Log (bottomPoints.Find (from).position.x);
		Debug.DrawLine (from, to, Color.white);

	}

}
