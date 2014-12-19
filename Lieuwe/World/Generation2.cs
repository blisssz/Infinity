using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generation2 :MonoBehaviour
{

	public Vector3 SpawnPosition;
	public GameObject SpikePlane;
	public List<Path> Alfa;
	public List<Vector3> Beta;
	private bool Updating=false;
	private float time;
	private int AlfaIndex;

	


//	void Update ()
//	{
//		if (Input.GetKeyDown (KeyCode.Q)) {
//			//Important.Move1 ();
//			float Tm2 = Time.realtimeSinceStartup;
//			float Tm = Time.realtimeSinceStartup;
//			ChunkList.UpdateSidesChunks ();
//			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateSides");
//			Tm = Time.realtimeSinceStartup;
//			ChunkList.UpdateTrianglesChunks ();
//			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateTriangles");
//			Tm = Time.realtimeSinceStartup;
//			ChunkList.UpdateMeshChunks ();
//			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateMesh");
//			ChunkList.Clear ();
//			Debug.Log (Time.realtimeSinceStartup - Tm2 + " for total");
//		}
//
//		if (Input.GetKeyDown (KeyCode.E)) {
//			float Tm2 = Time.realtimeSinceStartup;
//			float Tm = Time.realtimeSinceStartup;
//			GenerateCases.ReadFromFile ();
//			Debug.Log (Time.realtimeSinceStartup - Tm + " for ReadFromFiles");
//			Debug.Log (Time.realtimeSinceStartup - Tm2 + " for total");
//		}
//
//	}


	

		

		
		
	// Use this for initialization
	void Start ()
	{
		Updating=true;
		ChunkList.C=gameObject;
		time=Time.realtimeSinceStartup;
		float Tm2 = Time.realtimeSinceStartup;
		Moves.StartX ();
		GenerateCases.GenCases ();
		int[][] TempCases = new int[256][];
		Debug.Log (HelpScript.toStringSpecial (GenerateCases.Cases));
		GenerateCases.ReadFromFile ();
		Debug.Log (HelpScript.toStringSpecial (GenerateCases.Cases));
		GenerateCases.SwapCases ();
		Debug.Log (HelpScript.toStringSpecial (GenerateCases.Cases));
		var shaderText =
			"Shader \"Alpha Additive\" {" +
			"Properties { _Color (\"Main Color\", Color) = (0.44,0.66,0.67,0) }" +
			"SubShader {" +
			"	Tags { \"Queue\" = \"Transparent\" }" +
			"	Pass {" +
			"		ColorMask RGB" +
			"		Material { Diffuse [_Color] Ambient [_Color] }" +
			"		Lighting On" +
			"		SetTexture [_Dummy] { combine primary double, primary }" +
			"	}" +
			"}" +
			"}";
		ChunkList.AA = new Material (shaderText);
		Path Important=new Path(SpawnPosition,SpikePlane);
		Important.UpdateList.Add (new Move(1, SpawnPosition,SpikePlane,Important));
		Important.UpdateList[0].Choose ();
		Important.index.Add (3);
		Alfa=new List<Path>();
		Beta=new List<Vector3>();
		Alfa.Add (Important);
		Beta.Add (SpawnPosition);
		AlfaIndex=0;
//		for (int i=0; i<40; i++) {
//
//		}
//		float Tm = 0;
//		ChunkList.UpdateSidesChunks ();
//		Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateSides");
//		Tm = Time.realtimeSinceStartup;
//		ChunkList.UpdateTrianglesChunks ();
//		Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateTriangles");
//		Tm = Time.realtimeSinceStartup;
//		ChunkList.UpdateMeshChunks ();
//		Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateMesh");
//		ChunkList.Clear ();
//		Debug.Log (Time.realtimeSinceStartup - Tm2 + " for total");




		//Moves = AddAllDirections (Moves);
		//for(int i=0;i<22;i++){
		//test2[0]=new float[4]{5,5,5,1};
		//}



	}

	void Update(){
		if(ChunkList.done&&Updating){
			float Tm = Time.realtimeSinceStartup;
				if(Alfa[AlfaIndex].Impossibrah<6){
					Beta[AlfaIndex]=Alfa[AlfaIndex].Move1 ();
					float CH=Random.Range (0,1f);
					if(CH>0.80){
						Debug.Log ("Split");
						Alfa.Add (new Path(Alfa[AlfaIndex].Position,SpikePlane));
						Beta.Add (Beta[AlfaIndex]);
					}
				}
			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateMaze");
			AlfaIndex++;
			if(AlfaIndex==Alfa.Count){AlfaIndex=0;}
		}

		if(Input.GetKeyDown(KeyCode.E)){
			float Tm2 = Time.realtimeSinceStartup;
			float Tm = Time.realtimeSinceStartup;
			ChunkList.UpdateSidesChunks ();
			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateSides");
			Tm = Time.realtimeSinceStartup;
			ChunkList.UpdateTrianglesChunks ();
			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateTriangles");
			Tm = Time.realtimeSinceStartup;
			ChunkList.UpdateMeshChunks ();
			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateMesh");
			Tm = Time.realtimeSinceStartup;
			ChunkList.Clear ();
			Debug.Log (Time.realtimeSinceStartup - Tm + " for Clear");
			Debug.Log (Time.realtimeSinceStartup - Tm2 + " for UpdateTotal");
		}

		if(Input.GetKeyDown(KeyCode.Q)){
			Updating=!Updating;
			Debug.Log("Updating off!");
		}

		if(!ChunkList.done&&!ChunkList.Busy){
			ContinueCouroutine();

		}



	}


	public void ContinueCouroutine ()
	{int Stage=ChunkList.Stage;
		Debug.Log (Time.realtimeSinceStartup-time);
		time=Time.realtimeSinceStartup;
		Debug.Log (Stage);
		ChunkList.Busy=true;
		//IEnumerator a=ChunkList.UpdateDataChunksC();
		if(Stage==0){this.StartCoroutine(ChunkList.UpdateDataChunksC());}
		if(Stage==1){this.StartCoroutine(ChunkList.UpdateSidesChunksC());}
		if(Stage==2){this.StartCoroutine(ChunkList.UpdateTrianglesChunksC());}
		if(Stage==3){this.StartCoroutine(ChunkList.UpdateMeshChunksC());}
		if(Stage==4){ChunkList.done=true;
			ChunkList.Clear ();
			Debug.Log ("done!");
			Stage=-1;
		}
	}
	
		
}

