﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Test {


	public int AlfaIndex;
	public List<Path> Alfa;
	public List<Vector3> Beta;
	public int Stage;
	public float MinOneDistance;
	private float CH;
	public System.Random b;

	// Use this for initialization
	public Test(){}

	public void Execute () {
		bool executed=false;
		//Debug.Log ("Starts");
		CH=0;
		if (Alfa [AlfaIndex].Impossibrah < 12) {
			Beta [AlfaIndex] = Alfa [AlfaIndex].Move1 ();
			executed=Alfa[AlfaIndex].executed;
			CH = HelpScript.Rand (0, 1f);
			if (CH > 0.70) {
				Alfa.Add (new Path (Alfa [AlfaIndex].Position, MinOneDistance));
				Alfa[Alfa.Count-1].Cannot=Alfa[AlfaIndex].Cannot;
				Beta.Add (Beta [AlfaIndex]);
			}
		} else if(Alfa [AlfaIndex].Impossibrah==12) {
			Alfa [AlfaIndex].Finish();
			Alfa [AlfaIndex].Impossibrah=13;
		}
		
		if(executed){
			//Debug.Log ("Halfway there");
			ChunkList.UpdateSurroundingChunks ();
			//Debug.Log ("Halfway there+1");
			ChunkList.UpdateSidesChunks ();
			//Debug.Log ("Halfway there+2");
			ChunkList.UpdateTrianglesChunks ();
			//Debug.Log ("Done");
		}
	}
	

}
