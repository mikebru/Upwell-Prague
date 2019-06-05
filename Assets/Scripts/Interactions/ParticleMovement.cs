using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovement : MonoBehaviour {

	//public Material[] ParticleMaterials;
	public bool inZone;
	public bool isMoveUp = true;

	// Use this for initialization
	void Start () {
		
	}

	public void AddVelocity(Vector3 heading, Vector3 newVelocity)
	{
		//Debug.Log (Mathf.Abs (newVelocity.magnitude));
		if (GetComponent<Rigidbody> ().velocity.magnitude < 1f && inZone == true) {
			GetComponent<Rigidbody> ().velocity = heading * newVelocity.magnitude;
		}
	}

	//move to the staff
	public void AttractAngularVelocity(Vector3 newDirection, float speed)
	{
		if (inZone == false) {
			GetComponent<Particle_Color> ().ActiveState ();
			StartCoroutine (ControlCheck ());

		}


		inZone = true;

		Vector3 newHeading = newDirection - this.transform.position;
		newHeading = newHeading.normalized;

		GetComponent<Rigidbody> ().velocity = newHeading * speed;

	}


	IEnumerator ControlCheck()
	{
		yield return new WaitForSeconds (1);

		inZone = false;

		yield return new WaitForSeconds (1);

		if (inZone == false) {
			GetComponent<Particle_Color> ().InactiveState ();
			inZone = false;

			if (isMoveUp == true) {
				StartCoroutine (MoveUp ());
			} else {
				StartCoroutine (MoveDown ());
			}

		}

	}

	//final move
	public IEnumerator FlyAway()
	{

		while (transform.position.y < 5f) {
			//give the object an upward movement
			GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 2f, GetComponent<Rigidbody>().velocity.z);
			yield return new WaitForFixedUpdate ();
		}

		Destroy (this.gameObject);

	}

	public IEnumerator MoveUp()
	{

		while (transform.position.y < 3.0f && this != null) {
			//give the object an upward movement
			GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, .5f, GetComponent<Rigidbody>().velocity.z);
			yield return new WaitForFixedUpdate ();
		}
			
	}

	public IEnumerator MoveDown()
	{

		while (transform.position.y > .5f) {
			//give the object an upward movement
			GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, -.25f, GetComponent<Rigidbody>().velocity.z);
			yield return new WaitForFixedUpdate ();
		}

	}
		

}
