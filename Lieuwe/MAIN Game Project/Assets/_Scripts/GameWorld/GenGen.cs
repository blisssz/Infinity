using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenGen : MonoBehaviour {

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
	
	// Use this for initialization
	void Start () {
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
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Q)){
			Test1 ();
		}
	
	}

	void Test1 () {
		Test myJob=new Test();
		myJob.AlfaIndex=AlfaIndex;
		myJob.Beta=Beta;
		myJob.Alfa=Alfa;
		myJob.MinOneDistance=minOneDistance;
		myJob.b=new System.Random();
		myJob.Execute();
		ChunkList.MeshingDone=false;
		AlfaIndex=myJob.AlfaIndex;
		Beta=myJob.Beta;
		Alfa=myJob.Alfa;
		// Alternative to the OnFinished callback
		myJob = null;
		StartMeshing();
	}

	public void StartMeshing (){
		ChunkList.Busy=true;
		this.StartCoroutine(ChunkList.UpdateMeshChunksC());
		
	}
}
