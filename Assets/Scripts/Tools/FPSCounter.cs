using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour {

	public int frameRange = 60;
	public int FPS { get; private set; }
	public int AverageFPS { get; private set; }
	// Use this for initialization
	void Start () {
		
	}
	
	int[] fpsBuffer;
	int fpsBufferIndex;
	void InitializeBuffer () {
		if (frameRange <= 0) {
			frameRange = 1;
		}
		fpsBuffer = new int[frameRange];
		fpsBufferIndex = 0;
	}	
	// Update is called once per frame
	void Update () {
		// FPS = (int)(1f / Time.deltaTime);
		FPS = (int)(1f / Time.unscaledDeltaTime);

		if (fpsBuffer == null || fpsBuffer.Length != frameRange) {
			InitializeBuffer();
		}
		UpdateBuffer();
		CalculateFPS();		
	}

	void UpdateBuffer () {
		fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
		if (fpsBufferIndex >= frameRange) {
			fpsBufferIndex = 0;
		}
	}
	void CalculateFPS () {
		int sum = 0;
		for (int i = 0; i < frameRange; i++) {
			sum += fpsBuffer[i];
		}
		AverageFPS = sum / frameRange;
	}
}
