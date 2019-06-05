using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj; 
	private SteamVR_Controller.Device Controller;

	public int ControllerNumber;
	public bool isFastPulse= false;

	private MovementTimeControl EffectControl;

	public ButtonMove[] Buttons;

	private Transitioner gameControl;

	// Use this for initialization
	void Start () {
		gameControl = FindObjectOfType<Transitioner> ();
		EffectControl = FindObjectOfType<MovementTimeControl> ();

		trackedObj = this.GetComponent<SteamVR_TrackedObject> ();
		Controller = SteamVR_Controller.Input ((int)trackedObj.index);

		//StartCoroutine (SendPulse ());
		//StartCoroutine (IdentifyController ());

		StartCoroutine (IdlePulse ());
	}
	
	// Update is called once per frame
	void Update () {

		if (Controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_Grip)) {

            //click to begin game
            if (gameControl.isStarted == false)
            {
                gameControl.StartGame();
            }
            else
            {
                Debug.Log("Grip Pressed");
               // EffectControl.ColoredLines();
                Buttons[1].Pressed();
            }


		}

		if (Controller.GetPressUp (Valve.VR.EVRButtonId.k_EButton_Grip)) {
			Debug.Log ("Grip Released");
			Buttons [1].Released ();

		}

		if (Controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {

            //click to begin game
            if (gameControl.isStarted == false)
            {
                gameControl.StartGame();
            }
            else
            {
                Debug.Log("Trigger Pressed");
              //  EffectControl.ToggleTimeStop();
                Buttons[0].Pressed();
            }
		}

		if (Controller.GetPressUp (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			Debug.Log ("Trigger Released");
			Buttons [0].Released ();



		}
			
	}


	IEnumerator IdentifyController()
	{
		//yield return new WaitForSeconds (1);

		float t = 0;
		bool touchPadButton = false;
		bool menuButton = false;

		while (t < 1) {

			if (Controller.GetPress (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
				touchPadButton = true;
			}

			if (Controller.GetPress (Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) {
				menuButton = true;
			}
		
			t += Time.deltaTime;

			yield return null;

		}

		if (touchPadButton == false && menuButton == false) {
			ControllerNumber = 0;
		} else if (touchPadButton == true && menuButton == false) {
			ControllerNumber = 1;
		}else if (touchPadButton == false && menuButton == true) {
			ControllerNumber = 2;
		}else if (touchPadButton == true && menuButton == true) {
			ControllerNumber = 3;
		}

		yield return null;

	}


	IEnumerator IdlePulse()
	{
		ushort time = 1500;
		Controller.TriggerHapticPulse (time);
		Debug.Log ("Idle pulse");

		yield return new WaitForSeconds (2);

		if (isFastPulse == false) {
			StartCoroutine (IdlePulse ());
		} else {
			StartCoroutine (FastPulse ());
		}
	}

	IEnumerator FastPulse()
	{
		ushort time = 200;
		Controller.TriggerHapticPulse (time);

		yield return new WaitForSeconds (.25f);
	//	Debug.Log ("fast pulse");

		if (isFastPulse == false) {
			StartCoroutine (IdlePulse ());
		} else {
			StartCoroutine (FastPulse ());
		}

	}

}
