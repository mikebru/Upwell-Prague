using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaistBand : MonoBehaviour {

	public Transform head;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		PositionBelt ();
	}

	void PositionBelt()
	{
		//position the belt 4/7th of the height of the head
		transform.position = new Vector3 (head.position.x, head.position.y - ((head.position.y / 7) * 4), head.position.z) + (-transform.forward * .1f);
	}





}
