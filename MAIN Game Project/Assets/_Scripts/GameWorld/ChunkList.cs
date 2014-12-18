﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ChunkList {

	//public static Vector3 SpawnPosition;
	public static Material Fu;
	//private static Vector3 Position;
	//private static Vector3[] lastPositions = new Vector3[3];
	
	//public static GameObject CheckPoint;
	private static List<Chunk> Chunks = new List<Chunk>();
	private static List<Chunk> ToBeUpdated = new List<Chunk>();
	private static List<int[]> ToBeUpdatedPositions = new List<int[]>();
	private static List<int[]> ChunkPositions=new List<int[]>();
	public static int sizeChunk = 16;

	public static float rib=1f;
	
	
	public static GameObject C;
	
	private static Vector3[] Vertices;
	private static int[] Triangles;
	public static Material AA;
	
	
	public static bool CheckChunks(float[][] DataIn){
		List<int[]> ChunkTemp=new List<int[]>();
		int[] CurrentChunk=new int[3];
		bool Already=false;
		
		for(int i=0;i<DataIn.Length;i++){
			int[] x1=new int[4]{(int) (DataIn[i][0]/rib%sizeChunk),(int) (DataIn[i][1]/rib%sizeChunk),(int) (DataIn[i][2]/rib%sizeChunk),(int) (DataIn[i][3])};
			int[] x2=new int[3]{(int) (DataIn[i][0]/(rib*sizeChunk)),(int) (DataIn[i][1]/(rib*sizeChunk)),(int) (DataIn[i][2]/(rib*sizeChunk))};
			bool ChunkNotFound=true;
			for(int j=0;j<3;j++){
				if(x1[j]<0){x1[j]=x1[j]+sizeChunk;
					x2[j]--;}
			}
			
			
			if(CurrentChunk[0]==x2[0]&&CurrentChunk[1]==x2[1]&&CurrentChunk[2]==x2[2]&&ChunkTemp.Count>0){
				ChunkNotFound=false;} else {
				
				for(int u=0;u<ChunkTemp.Count;u++){
					
					if(ChunkTemp[u][0]==x2[0]&&ChunkTemp[u][1]==x2[1]&&ChunkTemp[u][2]==x2[2]){
						CurrentChunk=ChunkTemp[u];
						ChunkNotFound=false;
						break;}
				}
				if(ChunkNotFound){
					ChunkTemp.Add (x2);
					CurrentChunk=x2;
				}
				
			}
			
			
		}
		
		
		
		
		
		for(int i=0;i<ChunkTemp.Count;i++){
			if(ChunkExists (ChunkTemp[i])&&GetChunk (ChunkTemp[i]).updated){
				return true;
			}
		}
		return Already;
	}
	
	public static void UpdateDataChunks (float[][] DataIn)
	{//Debug.Log (DataIn.Length);
		List<List<int[]>> DataTemp=new List<List<int[]>>();
		List<int[]> ChunkTemp=new List<int[]>();
		int[] CurrentChunk=new int[3];
		int ChunkIndex=0;
		
		for(int i=0;i<DataIn.Length;i++){
			int[] x1=new int[4]{(int) (DataIn[i][0]/rib%sizeChunk),(int) (DataIn[i][1]/rib%sizeChunk),(int) (DataIn[i][2]/rib%sizeChunk),(int) (DataIn[i][3])};
			int[] x2=new int[3]{(int) (DataIn[i][0]/(rib*sizeChunk)),(int) (DataIn[i][1]/(rib*sizeChunk)),(int) (DataIn[i][2]/(rib*sizeChunk))};
			bool ChunkNotFound=true;
			for(int j=0;j<3;j++){
				if(x1[j]<0){x1[j]=x1[j]+sizeChunk;
					x2[j]--;}
			}
			
			
			if(CurrentChunk[0]==x2[0]&&CurrentChunk[1]==x2[1]&&CurrentChunk[2]==x2[2]&&ChunkTemp.Count>0){
				ChunkNotFound=false;} else {
				
				for(int u=0;u<ChunkTemp.Count;u++){
					
					if(ChunkTemp[u][0]==x2[0]&&ChunkTemp[u][1]==x2[1]&&ChunkTemp[u][2]==x2[2]){
						CurrentChunk=ChunkTemp[u];
						ChunkNotFound=false; 
						ChunkIndex=u;
						break;}
				}
				if(ChunkNotFound){
					//Debug.Log ("ChunkNotFound");
					ChunkTemp.Add (x2);
					DataTemp.Add (new List<int[]>());
					CurrentChunk=x2;
					ChunkIndex=ChunkTemp.Count-1;
					//Debug.Log(u);
				}
				
			}
			DataTemp[ChunkIndex].Add(x1);
		

		}





		for(int i=0;i<ChunkTemp.Count;i++){
			Chunk xx=GetChunk (ChunkTemp[i]);
			xx.UpdateData(DataTemp[i].ToArray());
			AddChunk (ChunkTemp[i]);
		}

	}

	public static void UpdateSidesChunks(){
		for(int i=0;i<ToBeUpdated.Count;i++){
			ToBeUpdated[i].UpdateSides();
		}
	}

	public static void UpdateTrianglesChunks(){
		for(int i=0;i<ToBeUpdated.Count;i++){
			ToBeUpdated[i].UpdateTriangles();
		}
	}

	public static void UpdateMeshChunks(){
		for(int i=0;i<ToBeUpdated.Count;i++){
			ToBeUpdated[i].UpdateMesh();
		}
	}

	public static void Clear(){
		ToBeUpdated=new List<Chunk>();
	}

	public static void AddChunk(int[] X){
		if(!ToBeUpdatedPositions.Contains(X)){
		Chunk B=GetChunk (X);
		//B.UpdateSides();
		ToBeUpdated.Add (B);
			ToBeUpdatedPositions.Add (X);
		}
	}

	
	
	public static void CreateChunk (int[] Positiona)
	{

		Chunk a=new Chunk(Positiona,AA);
		ChunkPositions.Add (a.GetPosition());
		
		Chunks.Add (a);
		
	}

	public static bool ChunkExists (int[] Positiona)
	{
		for(int i=0;i<ChunkPositions.Count;i++){
			if(Positiona[0]==(ChunkPositions[i][0])&&Positiona[1]==(ChunkPositions[i][1])&&Positiona[2]==(ChunkPositions[i][2])){
				
				//Debug.Log ("ChunkExists");
				return true;
			}
		}
		return false;
	}
	
	public static Chunk GetChunk (int[] Positiona)
	{
		
		for(int i=0;i<ChunkPositions.Count;i++){
			if(Positiona[0]==(ChunkPositions[i][0])&&Positiona[1]==(ChunkPositions[i][1])&&Positiona[2]==(ChunkPositions[i][2])){

				//Debug.Log ("GetChunkSucceeded");
				return Chunks[i];
			}
		}
		
		//Debug.Log ("GetChunkFailed");
		CreateChunk (Positiona);
		return Chunks[ChunkPositions.Count-1];
	}

	public static void Switch ()
	{//

	}
}
