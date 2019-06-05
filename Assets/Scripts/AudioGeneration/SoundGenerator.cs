using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGenerator : MonoBehaviour {

	[Range(-1f, 1f)]
	public float offset;

	public float Frequency;

	public float cutoffOn = 800;
	public float cutoffOff = 100;

	public bool engineOn;

	float lastOut = 0.0f;

	System.Random rand = new System.Random();
	AudioLowPassFilter lowPassFilter;

	void Awake() {
		lowPassFilter = GetComponent<AudioLowPassFilter>();
		Update();
	}

	public void SetCutoff(float newCutoff)
	{
		cutoffOff = newCutoff;
	}

	void OnAudioFilterRead(float[] data, int channels) {
		for (int i = 0; i < data.Length; i++) {

			float note = Mathf.Sin (2 * Mathf.PI * Frequency * ((float)i / data.Length));
			//data [i] = (lastOut + (.02f * white)) / 1.02f;
			//lastOut = data [i];
			data [i] = note;
		}
	}

	void Update() {
		lowPassFilter.cutoffFrequency = engineOn ? cutoffOn : cutoffOff;
	}
}
