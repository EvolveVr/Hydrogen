using UnityEngine;
using System;
using System.Collections;

public class SpectrumAnalyzer : MonoBehaviour 
{
	public GameObject cube;
	private float previousAverage;

	void Update()
	{
		float[] samples = new float[256];
		AudioListener.GetSpectrumData(samples, 0, FFTWindow.Blackman);

		float average = 0f;
		foreach (float num in samples) {
			average += num;
		}

		Mathf.Lerp (previousAverage, average, 0.01f);

		cube.transform.localScale = new Vector3(0.5f, average + 0.4f, 0.5f);
		previousAverage = average;
	}
}
