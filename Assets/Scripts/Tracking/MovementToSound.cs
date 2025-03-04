﻿using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class MovementToSound : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
    //private SteamVR_Controller.Device Controller;
    public SteamVR_Behaviour_Pose Controller;

    public SoundGenerator SoundControl;

	// Use this for initialization
	void Start () {

		trackedObj = this.GetComponent<SteamVR_TrackedObject> ();
		//Controller = SteamVR_Controller.Input ((int)trackedObj.index);
	}
	
	// Update is called once per frame
	void Update () {

		SoundControl.SetCutoff (Mathf.Lerp (200, 800, Controller.GetVelocity().magnitude / 4.0f));

	}
}
