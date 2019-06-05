using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewShift : MonoBehaviour {


	public GameObject AudienceCamera;
	public GameObject DancerCamera;

	private Vector3 StartPositon;
	private Vector3 StartRotation;

	private Transform StartParent;

	private bool isFirstPerson;

	// Use this for initialization
	void Start () {
		StartPositon = AudienceCamera.transform.position;
		StartRotation = AudienceCamera.transform.forward;

		StartParent = AudienceCamera.transform.parent;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Alpha3) && isFirstPerson == false) {
			StartCoroutine (GoToHeadPosition ());
		} else if (Input.GetKeyDown (KeyCode.Alpha3) && isFirstPerson == true) {
			StartCoroutine (ReturnToAudience ());
		}

	}



	public IEnumerator GoToHeadPosition()
	{
        if (isFirstPerson == false)
        {
            float t = 0;

            AudienceCamera.transform.parent = null;
            Vector3 CurrentPosition = AudienceCamera.transform.position;
            Vector3 CurrentRotation = AudienceCamera.transform.forward;

            while (t < 5)
            {
                t += Time.fixedUnscaledDeltaTime;

                AudienceCamera.transform.position = Vector3.Lerp(CurrentPosition, DancerCamera.transform.position, t / 5);
                AudienceCamera.transform.forward = (Vector3.Lerp(CurrentRotation, DancerCamera.transform.forward, t / 5));

                yield return new WaitForFixedUpdate();
            }

            isFirstPerson = true;
            AudienceCamera.transform.parent = DancerCamera.transform;
        }

        yield return null;
	}

    public IEnumerator ReturnToAudience()
	{
        if (isFirstPerson == true)
        {
            float t = 0;

            AudienceCamera.transform.parent = null;

            while (t < 5)
            {
                t += Time.fixedUnscaledDeltaTime;

                AudienceCamera.transform.position = Vector3.Lerp(DancerCamera.transform.position, StartPositon, t / 5);
                AudienceCamera.transform.forward = (Vector3.Lerp(DancerCamera.transform.forward, StartRotation, t / 5));

                yield return new WaitForFixedUpdate();
            }

            isFirstPerson = false;

            AudienceCamera.transform.parent = StartParent;
        }

        yield return null;
	}

}
