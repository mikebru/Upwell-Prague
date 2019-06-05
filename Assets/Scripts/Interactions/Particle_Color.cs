using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Color : MonoBehaviour {

	private Material ActiveMaterial;
	public Material[] InactiveMaterials;
	private int RandomIndex;
	// Use this for initialization
	void Start () {
		
	}

	public void InactiveState()
	{
		RandomIndex = Random.Range (0, InactiveMaterials.Length);
        //GetComponent<MeshRenderer> ().material = InactiveMaterials [RandomIndex];
        GetComponent<MeshRenderer>().material.SetFloat("_ActiveState", 0);
        GetComponent<TrailRenderer> ().enabled = false;

	}

	public void ActiveState()
	{
        //GetComponent<MeshRenderer> ().material = ActiveMaterial;
        GetComponent<MeshRenderer>().material.SetFloat("_ActiveState", 1);
        GetComponent<TrailRenderer> ().enabled = true;
	}

    /*
	public IEnumerator StartFade()
	{
		ActiveMaterial = GetComponent<MeshRenderer> ().material;
		RandomIndex = Random.Range (0, InactiveMaterials.Length);

		Color StartColor = GetComponent<MeshRenderer> ().material.GetColor ("_EmissionColor");
		Color EndColor = InactiveMaterials[RandomIndex].GetColor ("_EmissionColor");

		float t = 0;

		while (t < 2) {
			t += Time.deltaTime;

			GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", Color.Lerp(StartColor, EndColor, t/2));
			yield return new WaitForFixedUpdate ();
		}
			
		ActiveMaterial.SetColor ("_TintColor", StartColor);
		GetComponent<MeshRenderer> ().material = InactiveMaterials [RandomIndex];
	}


	public IEnumerator FadeOut()
	{
		ActiveMaterial = GetComponent<MeshRenderer> ().material;

		Color StartColor = GetComponent<MeshRenderer> ().material.GetColor ("_EmissionColor");
		Color EndColor = Color.black;

		float t = 0;

		while (t < 10) {
			t += Time.deltaTime;

			GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", Color.Lerp(StartColor, EndColor, t/5));
			yield return new WaitForFixedUpdate ();
		}

		ActiveMaterial.SetColor ("_EmissionColor", StartColor);
		Destroy (this.gameObject);
	}
    */
}
