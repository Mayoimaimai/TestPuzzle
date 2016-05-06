using UnityEngine;
using System.Collections;

public class CShuffleButton : MonoBehaviour {

	private bool mIsClicked = false;

	public bool isClicked
	{
		get { return mIsClicked; }
		set { mIsClicked = value; }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick()
	{
		mIsClicked = true;
	}
}
