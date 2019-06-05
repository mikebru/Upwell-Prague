using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StaffController : MonoBehaviour {

    [Range(0, 1)]
    public float ControlSpeed = .1f;
	public int MaxControlledParticles;
	public float EffectRadius;
	public LayerMask EffectedLayer;
	public Transform AttractPoint;

	private SteamVR_TrackedObject trackedObj; 
	private SteamVR_Controller.Device Controller;

	private Color CurrentColor;

	public float CurrentSpeed { get; set;}

	public bool isColored {get; set;}

	// Use this for initialization
	void Start () {

		trackedObj = this.GetComponent<SteamVR_TrackedObject> ();
		Controller = SteamVR_Controller.Input ((int)trackedObj.index);
        isColored = true;
	}
	
	// Update is called once per frame
	void Update () {


		ApplyParticleMotion ();

		CurrentSpeed = Controller.velocity.magnitude;

		if (Controller.velocity.magnitude > 1f) {
			CurrentColor = RotationToColor (Controller.velocity);
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


			//create a list and sort it by distance 
			List<RaycastHit> hitList = new List<RaycastHit>();

			hitList.AddRange (hits);

			hitList = hitList.OrderBy(
				x => Vector2.Distance(AttractPoint.position, x.transform.position)
			).ToList();
				


			for (int i = 0; i < hitList.Count; i++) {
				//limit the amount of particles that can be controlled
				if (i < MaxControlledParticles) {

					if (Controller.velocity.magnitude > ControlSpeed && hitList [i].transform.gameObject.GetComponent<ParticleMovement> ().enabled == true) {
						hitList [i].transform.gameObject.GetComponent<ParticleMovement> ().AttractAngularVelocity (AttractPoint.position, Controller.velocity.magnitude);
					}

					//hits [i].transform.gameObject.GetComponent<ParticleMovement> ().AddVelocity (AttractPoint.forward.normalized , Controller.velocity);

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


		float R = (CurrentVector.x * Controller.velocity.x/10) + .1f;
		float G = (CurrentVector.y * Controller.velocity.y/10) + .1f;
		float B = (CurrentVector.z * Controller.velocity.z/10) + .1f;

		Color DirectionColor = new Color (R, G, B, 1);


		return DirectionColor;

	}

}
