using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerInput : MonoBehaviour {

    private SteamVR_Behaviour_Pose Controller;

    public SteamVR_Action_Boolean BeginAction;
    public SteamVR_Input_Sources handType;

	private MovementTimeControl EffectControl;

	private Transitioner gameControl;

	// Use this for initialization
	void Start () {
		gameControl = FindObjectOfType<Transitioner> ();
		EffectControl = FindObjectOfType<MovementTimeControl> ();

        Controller = GetComponent<SteamVR_Behaviour_Pose>();

        BeginAction.AddOnStateDownListener(TriggerAction, handType);


    }


    public void TriggerAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger");
        if (gameControl.isStarted == false)
        {
            gameControl.StartGame();
        }
    }


    // Update is called once per frame
    void Update () {
        /*
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
			*/
	}



}
