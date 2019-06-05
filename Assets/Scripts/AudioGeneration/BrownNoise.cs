using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownNoise : MonoBehaviour {
	[Range(-1f, 1f)]
	public float offset;

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

	void OnAudioFilterRead(float[] data, int channels) {
		for (int i = 0; i < data.Length; i++) {
			float white = (float)(rand.NextDouble() * 2.0 - 1.0 + offset);
			data [i] = (lastOut + (.02f * white)) / 1.02f;
			lastOut = data [i];
			data [i] *= 3.5f;
		}
	}

	void Update() {
		lowPassFilter.cutoffFrequency = engineOn ? cutoffOn : cutoffOff;
	}
}
