using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generation3 : MonoBehaviour {

	public Vector3 SpawnPosition;
	public float minOneDistance;
	public List<Path> Alfa;
	public List<Vector3> Beta;
	private bool Updating=false;
	private float time;
	private int AlfaIndex;
	private bool MeshingDone;
	private float Tm;
	public Material x;

	Job myJob;
	void Start ()
	{
		ChunkList.C=gameObject;
		time=Time.realtimeSinceStartup;
		//float Tm2 = Time.realtimeSinceStartup;
		Moves.StartX ();
		GenerateCases.GenCases2 ();
//        GenerateCases.SwapCases();
        GenerateCases.WriteToFile();
//		int[][] TempCases = new int[256][];
//		Debug.Log (HelpScript.toStringSpecial (GenerateCases.Cases));
//		GenerateCases.ReadFromFile ();
//		Debug.Log (HelpScript.toStringSpecial (GenerateCases.Cases));
//		GenerateCases.SwapCases ();
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

		var shaderText2 =
			"Shader \"Alpha Additive\" {" +
				"Properties { _Color (\"Main Color\", Color) = (0.66,0.44,0.37,0) }" +
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

		minOneDistance=30;
		ChunkList.AA.Add ( new Material (shaderText));
		ChunkList.AA.Add ( x);
		ChunkList.AA[1].color=Color.red;
		Vector3[] b=new Vector3[4]{new Vector3(0,0,0),new Vector3(0,3,0),new Vector3(3,0,0),new Vector3(0,0,3)};
		//ChunkList.UpdateDataChunks(Moves.Box(b,1));
		Path Important=new Path(SpawnPosition,minOneDistance);
		Important.UpdateList.Add (new Move(5, SpawnPosition,minOneDistance,Important));
		Important.UpdateList[0].Choose ();
		Important.index.Add (0);
		Alfa=new List<Path>();
		Beta=new List<Vector3>();
		Alfa.Add (Important);
		Beta.Add (SpawnPosition);
		AlfaIndex=0;

	}
	
	void Update()
	{	
		if(Input.GetKeyDown(KeyCode.E)){
			Debug.Log ("E pressed");
			float Tm2 = Time.realtimeSinceStartup;
			ChunkList.ToBeUpdatedPositions=ChunkList.ChunkPositions;
			Tm = Time.realtimeSinceStartup;
			//ChunkList.UpdateSurroundingChunks();
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
			Debug.Log("Updating " + Updating);
			if(Updating){ 
				//ChunkList.AA[0].color=Color.red;
			}
			if(!Updating){
				//ChunkList.AA[0].color=Color.blue;
			}
		}


			if (Time.time % 0.1f <= Time.deltaTime){
		Tm=Time.realtimeSinceStartup - Tm;
		if(true){//Tm>0.01){
			//Debug.Log (Tm + " for Frame");
			
		}
		Tm=Time.realtimeSinceStartup;
		if (myJob == null&&Updating&&ChunkList.MeshingDone)
		{myJob = new Job();
			myJob.AlfaIndex=AlfaIndex;
			myJob.Beta=Beta;
			myJob.Alfa=Alfa;
			myJob.MinOneDistance=minOneDistance;
			myJob.b=new System.Random();
				Debug.Log ("Starts Job");
				myJob.Start (); // Don't touch any data in the job class after you called Start until IsDone is true.
		}
		if (myJob != null)
		{
			if (myJob.Update())
			{
				ChunkList.MeshingDone=false;
				AlfaIndex=myJob.AlfaIndex;
				Beta=myJob.Beta;
				Alfa=myJob.Alfa;
				// Alternative to the OnFinished callback
				myJob = null;

			}
		}

			if(myJob==null&&ChunkList.Busy==false&&ChunkList.MeshingDone==false){
				StartMeshing();
			}

		
		Tm=Time.realtimeSinceStartup - Tm;
			if(Tm>0.02){
		//Debug.Log (Tm + " for Frame");

		}
		Tm=Time.realtimeSinceStartup;
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
		if(Stage==3){}
		if(Stage==4){ChunkList.done=true;
			ChunkList.Clear ();
			Debug.Log ("done!");
			Stage=-1;
		}
	}

	public void StartMeshing (){
		ChunkList.Busy=true;
		this.StartCoroutine(ChunkList.UpdateMeshChunksC());

	}
}
	

