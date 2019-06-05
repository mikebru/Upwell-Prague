using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMove : MonoBehaviour {

	private Vector3 StartPosition;
	private Vector3 PressedPosition;

	// Use this for initialization
	void Start () {
		StartPosition = this.transform.localPosition;

		PressedPosition = StartPosition - transform.up * .02f;
	}


	public void Pressed()
	{
		transform.localPosition = PressedPosition;
	}

	public void Released()
	{
		transform.localPosition = StartPosition;
	}

}
