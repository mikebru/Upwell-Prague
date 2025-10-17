using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR;

//using Valve.VR;

public class StaffController : MonoBehaviour {

    [Range(0, 1)]
    public float ControlSpeed = .1f;
	public int MaxControlledParticles;
	public float EffectRadius;
	public LayerMask EffectedLayer;
	public Transform AttractPoint;


	public OVRInput.Controller controller;
	//  private SteamVR_Behaviour_Pose Controller;

	private Color CurrentColor;

	public float CurrentSpeed { get; set;}


    public bool useAttractPoint; 

	public bool isColored {get; set;}

	public Vector3 currentVelocity { get; set; }

	// Use this for initialization
	void Start () {


        isColored = true;
	}
	
	// Update is called once per frame
	void Update () {


		//need to update - with new velocity data
		CurrentSpeed = OVRInput.GetLocalControllerVelocity(controller).magnitude;
		currentVelocity = OVRInput.GetLocalControllerVelocity(controller);

		ApplyParticleMotion ();



		if (CurrentSpeed > 1f) {
			CurrentColor = RotationToColor (OVRInput.GetLocalControllerVelocity(controller));
		}
	}

	void ApplyParticleMotion()
	{
		//scaled area of effect based on velocity
		//float areaOfEffect = Mathf.Lerp (EffectRadius * 2, EffectRadius * 4, Controller.velocity.magnitude/4);

		RaycastHit[] hits;

		hits = Physics.SphereCastAll (AttractPoint.position, EffectRadius, transform.forward, 20f, EffectedLayer);
        
		//Debug.Log (Controller.angularVelocity.magnitude);

		//if we actually hit something
		if (hits.Length > 0) {

            float speed = CurrentSpeed;

            if (useAttractPoint == true)
            {
                speed = AttractPoint.GetComponent<Rigidbody>().linearVelocity.magnitude;
            }

            //create a list and sort it by distance 
            List<RaycastHit> hitList = new List<RaycastHit>();

			hitList.AddRange (hits);

			hitList = hitList.OrderBy(
				x => Vector2.Distance(AttractPoint.position, x.transform.position)
			).ToList();
				


			for (int i = 0; i < hitList.Count; i++) {
				//limit the amount of particles that can be controlled
				if (i < MaxControlledParticles) {



					if (speed > ControlSpeed && hitList [i].transform.gameObject.GetComponent<ParticleMovement> ().enabled == true) {
						hitList [i].transform.gameObject.GetComponent<ParticleMovement> ().AttractAngularVelocity (AttractPoint.position, CurrentSpeed);
					}

					hits [i].transform.gameObject.GetComponent<ParticleMovement> ().AddVelocity (AttractPoint.forward.normalized , currentVelocity);

					if (isColored == true) {
						hitList [i].transform.gameObject.GetComponent<TrailRenderer> ().startColor = CurrentColor;
						hitList [i].transform.gameObject.GetComponent<TrailRenderer> ().endColor = CurrentColor * .5f;
					} else {
						hitList [i].transform.gameObject.GetComponent<TrailRenderer> ().startColor = Color.white;
						hitList [i].transform.gameObject.GetComponent<TrailRenderer> ().endColor = Color.white * .5f;
					}

				}
			}

		}

	}


	Color RotationToColor(Vector3 CurrentVector)
	{

		float R = (CurrentVector.x * CurrentVector.x / 10) + .1f;
		float G = (CurrentVector.y * CurrentVector.y/ 10) + .1f;
		float B = (CurrentVector.z * CurrentVector.z/ 10) + .1f;

		Color DirectionColor = new Color (R, G, B, 1);


		return DirectionColor;

	}

}
