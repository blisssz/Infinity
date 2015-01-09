using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Job : ThreadedJob
{
	public int AlfaIndex;
	public List<Path> Alfa;
	public List<Vector3> Beta;
	public int Stage;
	public GameObject SpikePlane;
	private float CH;
	public System.Random b;

	
	protected override void ThreadFunction ()
	{
		Debug.Log ("Starts");
		Debug.Log (10);
		CH=0;
		if (Alfa [AlfaIndex].Impossibrah < 6) {
			Beta [AlfaIndex] = Alfa [AlfaIndex].Move1 ();
			CH = HelpScript.Rand (0, 1f);
			if (CH > 0.80) {
				Alfa.Add (new Path (Alfa [AlfaIndex].Position, SpikePlane));
				Beta.Add (Beta [AlfaIndex]);
			}
		} else if(Alfa [AlfaIndex].Impossibrah==6) {
			Alfa [AlfaIndex].Finish();
			Alfa [AlfaIndex].Impossibrah++;
		}
		AlfaIndex++;
		if (AlfaIndex == Alfa.Count) {
			AlfaIndex = 0;
		}
		Debug.Log ("Halfway there");
		ChunkList.UpdateSurroundingChunks ();
		Debug.Log ("Halfway there+1");
		ChunkList.UpdateSidesChunks ();
		Debug.Log ("Halfway there+2");
		ChunkList.UpdateTrianglesChunks ();
		Debug.Log ("Done");

	}

	protected override void OnFinished ()
	{
		// This is executed by the Unity main thread when the job is finished

	}
}