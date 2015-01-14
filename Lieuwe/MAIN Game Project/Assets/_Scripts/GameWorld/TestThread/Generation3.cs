using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generation3 : MonoBehaviour {

	public Vector3 SpawnPosition;
	public GameObject CheckPoint;
	public GameObject endPoint;
	public float minOneDistance;
	private List<Path> Alfa;
	private List<Vector3> Beta;
	private bool Updating=true;
	private float time;
	private int AlfaIndex;
	private bool MeshingDone;
	private float Tm;
	public Material x;
	private int FinishNumber=100;
	private int iteration=1;

	Job myJob;
	void Start ()
	{
		settingSetter ();
		GameController.fallingPossible = false;
		ChunkList.positionsReset ();
		GameController.fallingPossible = false;
		ChunkList.C=gameObject;
		time=Time.realtimeSinceStartup;
		//float Tm2 = Time.realtimeSinceStartup;
		Moves.StartX ();
		if(GenerateCases.done==false){
		GenerateCases.GenCases2 ();
//        GenerateCases.SwapCases();
        GenerateCases.WriteToFile();
			GenerateCases.done=true;
		}
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



		ChunkList.AA.Add ( new Material (shaderText));
		ChunkList.AA.Add ( x);
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
//		if(Input.GetKeyDown(KeyCode.E)){
//			Debug.Log ("E pressed");
//			float Tm2 = Time.realtimeSinceStartup;
//			ChunkList.ToBeUpdatedPositions=ChunkList.ChunkPositions;
//			Tm = Time.realtimeSinceStartup;
//			//ChunkList.UpdateSurroundingChunks();
//			ChunkList.UpdateSidesChunks ();
//			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateSides");
//			Tm = Time.realtimeSinceStartup;
//			ChunkList.UpdateTrianglesChunks ();
//			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateTriangles");
//			Tm = Time.realtimeSinceStartup;
//			ChunkList.UpdateMeshChunks ();
//			Debug.Log (Time.realtimeSinceStartup - Tm + " for UpdateMesh");
//			Tm = Time.realtimeSinceStartup;
//			ChunkList.Clear ();
//			Debug.Log (Time.realtimeSinceStartup - Tm + " for Clear");
//			Debug.Log (Time.realtimeSinceStartup - Tm2 + " for UpdateTotal");
//		}
//		
//		if(Input.GetKeyDown(KeyCode.Q)){
//			Updating=!Updating;
//			Debug.Log("Updating " + Updating);
//			if(Updating){ 
//				//ChunkList.AA[0].color=Color.red;
//			}
//			if(!Updating){
				//ChunkList.AA[0].color=Color.blue;
//			}
//		}


			if (Time.time % 0.1f <= Time.deltaTime){
		Tm=Time.realtimeSinceStartup - Tm;
		if(true){//Tm>0.01){
			//Debug.Log (Tm + " for Frame");
			
		}
		if(ChunkList.UpdateNumber==FinishNumber){
				Updating=false;
				AlfaIndex--;
				if(AlfaIndex==-1){
					AlfaIndex=Alfa.Count-1;
				}
				onFinish(Beta[AlfaIndex]);
				ChunkList.UpdateNumber++;
			}
		Tm=Time.realtimeSinceStartup;
		if (myJob == null&&Updating&&ChunkList.MeshingDone)
		{myJob = new Job();
			myJob.AlfaIndex=AlfaIndex;
			myJob.Beta=Beta;
			myJob.Alfa=Alfa;
			myJob.MinOneDistance=minOneDistance;
			myJob.b=new System.Random();
				//Debug.Log ("Starts Job");
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
				if(Alfa[AlfaIndex].executed){
					AfterMove(Beta[AlfaIndex]);
					onIteration(Beta[AlfaIndex]);
					}
					AlfaIndex++;
					myJob = null;
					if (AlfaIndex == Alfa.Count) {
						AlfaIndex = 0;
					}

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

	public void AfterMove (Vector3 Position){
		int Choice=HelpScript.Switch(new float[5]{0.7f,0.05f,0.05f,0.1f,0.1f});
		switch(Choice){
		case 0:
			ObjectSpawner.SpawnObject(Position,"Coin");
			break;
		case 1:
			ObjectSpawner.SpawnObject(Position,"AmmoPack");
			break;
		case 2:
			ObjectSpawner.SpawnObject(Position,"MedPack");
			break;
		case 3:
			Spawner.addSpawnLocation(Position);
			break;
		case 4:
			Spawner.addSpawnLocation (Position);
			break;
		default:
			break;
		}

	}

	public void onIteration(Vector3 Position){
		if(iteration<ChunkList.UpdateNumber){
		if (iteration%30 == 0) {
			Instantiate (CheckPoint, Position, Quaternion.identity);
		}
		if(iteration%FinishNumber == 0){
			//onFinish(Position);
		}
		iteration=ChunkList.UpdateNumber;
		}
	}

	void settingSetter(){
		switch (PlayerManager.useWeaponID) {
		case 1:
			pogoStickSettings ();
			break;
		case 2:
			blackHoleSettings();
			break;
		case 3: 
			hookSettings ();
			break;
		case 4:
			gunSettings ();
			break;
		case 5:
			gunSettings ();
			break;
		case 6:
			gunSettings ();
			break;
		default:
			break;
		}
	}

	public static void hookSettings(){
		UseSpawner.setSpawnTime(3);
		UseSpawner.setSpawnChance(1);
		Spawner.maxSpawns = 20;
	}
	
	public static void blackHoleSettings(){
		UseSpawner.setSpawnTime(3);
		UseSpawner.setSpawnChance(1);
		Spawner.maxSpawns = 20;
	}


	public static void pogoStickSettings (){
		UseSpawner.setSpawnTime(3);
		UseSpawner.setSpawnChance(1);
		Spawner.maxSpawns = 20;
	}

	public static void gunSettings(){
		UseSpawner.setSpawnTime(1);
		UseSpawner.setSpawnChance(0.5f);
		Spawner.maxSpawns = 70;
	}

	public void onFinish (Vector3 Position){
		GameObject Finish=Instantiate(endPoint,Position , Quaternion.identity) as GameObject;
		Finish.GetComponent<endPoint>().isBossLevel=false;
	}
}
	

