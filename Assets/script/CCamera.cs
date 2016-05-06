using UnityEngine;
using System.Collections;

public class CCamera : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Camera cam = GetComponent<Camera>();

		float size = cam.orthographicSize;
		Vector3 pos;
		pos.x = size * 10.0f / 16.0f;
		pos.y = -size;
		pos.z = -10.0f;
		cam.transform.position = pos;

		float baseAspect = 16.0f / 10.0f;	// 10:16
		float nowAspect = (float)Screen.height/(float)Screen.width; 
		float changeAspect; 

		if( baseAspect > nowAspect ) 
		{ 
			changeAspect = nowAspect / baseAspect; 
			cam.rect = new Rect( ( 1.0f - changeAspect ) * 0.5f, 0.0f, changeAspect, 1.0f ); 
		} 
		else 
		{ 
			changeAspect = baseAspect / nowAspect; 
			cam.rect = new Rect( 0.0f, ( 1.0f - changeAspect ) * 0.5f, 1.0f, changeAspect ); 
		} 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
