using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Job : ThreadedJob
{
	public int AlfaIndex;
	public List<Path> Alfa;
	public List<Vector3> Beta;
	public int Stage;
	public float MinOneDistance;
	private float CH;
	public System.Random b;

	
	protected override void ThreadFunction ()
	{
		bool moved=false;
		//Debug.Log ("Starts");
		CH=0;
		if (Alfa [AlfaIndex].Impossibrah < 6) {
			Beta [AlfaIndex] = Alfa [AlfaIndex].Move1 ();
			moved=Alfa[AlfaIndex].moved;
			CH = HelpScript.Rand (0, 1f);
			if (CH > 0.80) {
				Alfa.Add (new Path (Alfa [AlfaIndex].Position, MinOneDistance));
				Alfa[Alfa.Count-1].Cannot=Alfa[AlfaIndex].Cannot;
				Beta.Add (Beta [AlfaIndex]);
			}
		} else if(Alfa [AlfaIndex].Impossibrah==6) {
			//Alfa [AlfaIndex].Finish();
			Alfa [AlfaIndex].Impossibrah=7;
		}
		AlfaIndex++;
		if (AlfaIndex == Alfa.Count) {
			AlfaIndex = 0;
		}
		//if(moved){
		//Debug.Log ("Halfway there");
		ChunkList.UpdateSurroundingChunks ();
		Debug.Log ("Halfway there+1");
		ChunkList.UpdateSidesChunks ();
		//Debug.Log ("Halfway there+2");
		ChunkList.UpdateTrianglesChunks ();
		Debug.Log ("Done");
		//}
	}
	
	protected override void OnFinished ()
	{
		// This is executed by the Unity main thread when the job is finished

	}
}