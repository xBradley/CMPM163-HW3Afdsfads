using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
 	Renderer render;
    Color startColor = new Color(1,0,0,1);
	Color endColor = new Color(0,0,1,1);

	// Use this for initialization
	void Start () {
		render = GetComponent<Renderer>();
        render.material.shader = Shader.Find("Custom/ParticleShader");
	}
	
	// Update is called once per frame
	void Update () {
		// this rotates each cube.
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);


		// --------------------------------------------------------
		// ------- animate the cube size based on spectrum data.

		// consolidate spectral data to 8 partitions (1 partition for each rotating cube)
		int numPartitions = 8;
		float[] aveMag = new float[numPartitions];
		float partitionIndx = 0;
		int numDisplayedBins = 512 / 2;

		for (int i = 0; i < numDisplayedBins; i++)  {
			if(i < numDisplayedBins * (partitionIndx + 1) / numPartitions) {
				aveMag[(int)partitionIndx] += AudioPeer.spectrumData [i] / (512/numPartitions);
			}
			else{
				partitionIndx++;
				--i;
			}
		}

		// scale and bound the average magnitude.
		for(int i = 0; i < numPartitions; i++) {
			print(i);
			
			if (i % 3 == 0)
				startColor.r = aveMag[i] * (i * 4000);
			else if (i % 3 == 1)
				startColor.g = aveMag[i] * (i * 2000);
			else if (i % 3 == 2)
				startColor.b = aveMag[i] * (i * 1000);
			
			aveMag[i] = (float)0.5 + aveMag[i]*100;
			
			
			if (aveMag[i] > 100) {
				aveMag[i] = 100;
				startColor.r = aveMag[i];
				startColor.g = aveMag[i];
				startColor.b = aveMag[i];
			}
		}

		transform.localScale = new Vector3 (aveMag[0], aveMag[0], aveMag[0]);
		
		if (Input.GetKeyDown("left")) {
			endColor.r -= 0.1f;
        }
        if (Input.GetKeyDown("right")) {
			endColor.r += 0.1f;
        }
		if (Input.GetKeyDown("up")) {
			endColor.g -= 0.1f;
		}
		if (Input.GetKeyDown("down")) {
			endColor.g += 0.1f;
		}

		//endColor.r = aveMag[0];
		//endColor.g = aveMag[0];
		//endColor.b = aveMag[0];
 
		render.material.SetColor("_StartColor", startColor);
		render.material.SetColor("_EndColor", endColor);

		// --------- End animating cube via spectral data
		// --------------------------------------------------------
	}


}

