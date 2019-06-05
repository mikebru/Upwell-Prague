using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShape : MonoBehaviour {

	public Transform ParentObject;
	public GameObject ShapeToSpawn;
	public Transform SpawnPoint;

	public float ChangeTime;

	public float StartRate;
	public float EndRate;
	private float CurrentRate;

	public Color SlowColor;
	public Color FastColor;

	private Color CurrentColor;

	private SteamVR_TrackedObject trackedObj; 
	private SteamVR_Controller.Device Controller;

	public bool stopSpawning { get; set;}

	public int SpawnCount { get; set; }

	private Transitioner TransitionControl;

	public bool FadeOut {get; set;}

	// Use this for initialization
	void Start () {
		TransitionControl = FindObjectOfType<Transitioner> ();

		CurrentRate = StartRate;

		trackedObj = this.GetComponent<SteamVR_TrackedObject> ();
		Controller = SteamVR_Controller.Input ((int)trackedObj.index);


	}

	public void StartSpawning()
	{
        stopSpawning = false;
        SpawnCount = 0;
		CurrentRate = StartRate;
		
		StartCoroutine (SpawnTimer ());
		StartCoroutine (RateChange ());
	}
		

	IEnumerator RateChange()
	{
		float t = 0;

		while (t < ChangeTime) {
			t += Time.unscaledDeltaTime;

			CurrentRate = Mathf.Lerp (StartRate, EndRate, t / ChangeTime);

			yield return new WaitForFixedUpdate ();
		}
	}


	IEnumerator SpawnTimer()
	{

		yield return new WaitForSecondsRealtime (CurrentRate);

		GameObject SpawnedShape = Instantiate (ShapeToSpawn, ParentObject);
		SpawnedShape.transform.position = SpawnPoint.transform.position;

		//give the object an upward movement
		SpawnedShape.GetComponent<Rigidbody>().AddForce(Vector3.up * 10);

		//CurrentColor = Color.Lerp (SlowColor, FastColor, Controller.velocity.magnitude/3);
	//	if (Controller.velocity.magnitude > 1.5f) {
	//		SpawnedShape.GetComponent<MeshRenderer> ().sharedMaterial = FastColor;
	//	} else {
	//		SpawnedShape.GetComponent<MeshRenderer> ().sharedMaterial = SlowColor;
	//	}

        if(FadeOut == false)
        {
            SpawnedShape.GetComponent<Particle_Color>().InactiveState();
        }


		SpawnedShape.GetComponent<MeshRenderer> ().material.SetColor("_EmissionColor", Color.Lerp(SlowColor, FastColor, Controller.velocity.magnitude/3));

		SpawnedShape.transform.localScale = Vector3.Lerp (new Vector3 (.02f, .02f, .02f), new Vector3 (.2f, .2f, .2f), Controller.velocity.magnitude/3);
		//SpawnedShape.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", CurrentColor);

		//switch what layers the particles are on 
		//SpawnCount < 200 && 
		//if (FadeOut == false) {
		if (SpawnCount % 2 == 0) {
			SpawnedShape.layer = 8;
		} else {
			SpawnedShape.layer = 9;
		}
		//} 
		//else {
		//	SpawnedShape.layer = 0;
		//}

		//make the particle fade out or to a new color
		//if (FadeOut == true) {
		//	StartCoroutine (SpawnedShape.GetComponent<Particle_Color> ().FadeOut ());
		//} else {
		//	StartCoroutine (SpawnedShape.GetComponent<Particle_Color> ().StartFade ());
		//}

		StartCoroutine (SpawnedShape.GetComponent<ParticleMovement> ().MoveUp ());

		
		SpawnCount += 1;
		

		StartCoroutine (AddCollisions (SpawnedShape));

		if (stopSpawning == false) {
			StartCoroutine (SpawnTimer ());
		}
	}

	IEnumerator AddCollisions(GameObject SpawnParticle)
	{

		yield return new WaitForSeconds (1);
		SpawnParticle.GetComponent<SphereCollider> ().enabled = true;
	}


}
