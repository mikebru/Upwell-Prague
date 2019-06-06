using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTimeControl : MonoBehaviour {

	public StaffController[] Controllers;
	private bool isActive;
	public bool StopTime {get; set;}

	public AudioSource[] A_Sources;
	public float[] A_StartPitch;

	private Transitioner GameTransitioner;
	private bool isRetracting = false;
	private int CurrentParticle = 0;


	// Use this for initialization
	void Start () {
		//A_Sources = GetComponentsInChildren<AudioSource> ();
		GameTransitioner = GetComponent<Transitioner>();


		A_StartPitch = new float[A_Sources.Length];

		for (int i = 0; i < A_Sources.Length; i++) {

			A_StartPitch [i] = A_Sources [i].pitch;
		}


		//Controllers = FindObjectsOfType<StaffController> ();

	}
	
	// Update is called once per frame
	void Update () {

		//toggle time control
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			ToggleTimeStop (!isActive);
		} 
			

		if (Input.GetKeyDown (KeyCode.Alpha4) && StopTime == false) {
			Time.timeScale = 0;
			StopTime = true;
		} else if (Input.GetKeyDown (KeyCode.Alpha4) && StopTime == true) {
			Time.timeScale = 1;
			StopTime = false;
		} 

		//activate and deactivate particle retraction 
		if (Input.GetKeyDown (KeyCode.Alpha5) && GameTransitioner.ParticlesCreated == true) {
			if (isRetracting == true) {
				StartCoroutine (ReleaseParticles ());
				isRetracting = false;
			} else {
				StartCoroutine (RetractParticles ());
				isRetracting = true;
			}
		}

		//increment and decrement how many particles can be controlled at once 
		if (Input.GetKeyDown (KeyCode.UpArrow) && Controllers [0].MaxControlledParticles < 100) {

			for (int i = 0; i < Controllers.Length; i++) {
				Controllers [i].MaxControlledParticles += 20;
			}
		} else if (Input.GetKeyDown (KeyCode.DownArrow) && Controllers [0].MaxControlledParticles > 40) {
			for (int i = 0; i < Controllers.Length; i++) {
				Controllers [i].MaxControlledParticles -= 20;
			}
		}


		//toggle colored lines
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			ColoredLines ();
		} 
			

		if (isActive == true) {
			Time.timeScale = .1f + ((Controllers [0].CurrentSpeed + Controllers [1].CurrentSpeed) / 2);

			//Debug.Log (Time.timeScale);

            /*
			for (int i = 0; i < A_Sources.Length; i++) {
				A_Sources [i].pitch = A_StartPitch [i] * Time.timeScale;
			}
            */
		} 
		
	}

	//retract the particles 
	IEnumerator RetractParticles()
	{

		for (int i = CurrentParticle; i < GameTransitioner.particles.Length; i++) {

			if (isRetracting == true) {
				//switch between where the particle return to 
				if (GameTransitioner.particles [i].layer == 8) {
					StartCoroutine (MoveParticleToOrgin (Controllers [0].gameObject, i));
				} else {
					StartCoroutine (MoveParticleToOrgin (Controllers [1].gameObject, i));
				}

				CurrentParticle = i;
			}

			yield return new WaitForSeconds (.05f);
		}
	}

	//release the particles
	IEnumerator ReleaseParticles()
	{
		for (int i = CurrentParticle; i > 0; i--) {

			if (isRetracting == false) {
				
				GameTransitioner.particles [i].gameObject.SetActive (true);
				GameTransitioner.particles[i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
				GameTransitioner.particles [i].GetComponent<ParticleMovement> ().enabled = false;

				StartCoroutine (DelayActivateMotion (i));

				CurrentParticle = i;
			}

			yield return new WaitForSeconds (.05f);
		}
			
		yield return new WaitForFixedUpdate ();
	}


	IEnumerator DelayActivateMotion(int particleIndex)
	{
		yield return new WaitForSeconds (.5f);
		GameTransitioner.particles [particleIndex].GetComponent<ParticleMovement> ().enabled = true;
	}


	IEnumerator MoveParticleToOrgin(GameObject Controller, int particleIndex)
	{
		while(Vector3.Distance(Controller.transform.position, GameTransitioner.particles[particleIndex].transform.position) > .3f)
		{
		Vector3 newHeading = Controller.transform.position - GameTransitioner.particles[particleIndex].transform.position;
		newHeading = newHeading.normalized;

		GameTransitioner.particles[particleIndex].GetComponent<Rigidbody> ().velocity = newHeading * 2;

		yield return new WaitForFixedUpdate ();
		}

		GameTransitioner.particles [particleIndex].SetActive (false);
	}

    public void ToggleTimeStop(bool timetoggle)
    {
        isActive = timetoggle;


        if (isActive == false)
        {
            Time.timeScale = .66f;


            /*
            for (int i = 0; i < A_Sources.Length; i++)
            {
                A_Sources[i].pitch = A_StartPitch[i] * Time.timeScale;
            }
            */
        }

    }

	public void ColoredLines()
	{
		Controllers [0].isColored = !Controllers [0].isColored;
		Controllers [1].isColored = !Controllers [1].isColored;
	}

}
