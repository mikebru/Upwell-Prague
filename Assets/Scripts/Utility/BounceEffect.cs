using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour {

	private Vector3 StartPosition;


	// Use this for initialization
	void Start () {
		StartPosition = transform.localPosition;
		StartCoroutine (Bounce ());

	}

	IEnumerator Bounce()
	{
		float t = 0;

		Vector3 CurrentLocation = transform.localPosition;
		Vector3 newLocation = StartPosition + new Vector3 (0, Random.Range(-.1f, .1f), 0);

		while (t < 1f) {
			t += Time.deltaTime;

			transform.localPosition = Vector3.Lerp (CurrentLocation, newLocation, t / 1f);

			yield return new WaitForFixedUpdate ();
		}

		StartCoroutine (Bounce ());

	}
	

}
