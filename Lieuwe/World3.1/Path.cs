﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path  {
	
	public Vector3 Position;
	public float[] Chances;
	public List<Move> UpdateList = new List<Move> ();
	public List<int> index = new List<int> ();
	public Quaternion Direction = new Quaternion ();
	public List<int> Cannot = new List<int>();
	public float MinOneDistance;
	public int Impossibrah=0;
	public bool moved;


	
	public Path(Vector3 StartPosition, float Min){
		Position=StartPosition;
		MinOneDistance=Min;
		moved=false;
	}
	
	public Vector3 Move1 ()
	{
		moved=false;
		float[] DefChances = new float[5]{0.8f,0.1f,0.2f,0.6f,0.2f};
		Chances =new float[DefChances.Length];
		for(int i=0;i<DefChances.Length;i++){
			if(Cannot.Contains (i)){
				Chances[i]=0;
			} else {
				Chances[i]=DefChances[i];
			}
			
		}
		int u = 0;

		u = HelpScript.Switch (Chances);

		Move Ch=new Move(u,Position,MinOneDistance,this);
		Ch.Choose();
		//Debug.Log ("StartCheck");
		bool can=Ch.Check ();
		//Debug.Log ("EndCheck");

		if (can&&Ch.moved&&index.Count>0&& index [0] == 0) {
//			List<Move> TempUpdateList = new List<Move> ();
//			List<int> TempIndex = new List<int> ();
			UpdateList.Add (Ch);
//
			for (int i=0; i<UpdateList.Count; i++) {
					UpdateList [i].Execute();
			}
			UpdateList =new List<Move>();
//			UpdateList = TempUpdateList;
//			index = TempIndex;
//			index.Add (0);
			Direction = Ch.Direction;
			Position = Ch.EndPosition;
			//Debug.Log ("Not Impossibrauh1");
			Impossibrah=0;
			
		} else if (Ch.moved && can &&(index.Count==0|| index [0] != 0)) {
			for (int i=0; i<index.Count; i++) {
				index [i] = index [i] - 1;
			}
			index.Add (0);
			UpdateList.Add (Ch);
			Direction = Ch.Direction;
			Position = Ch.EndPosition;
			//Debug.Log ("Not Impossibrauh2");
			Impossibrah=0;
			
		} else if (!Ch.moved && can) {
			UpdateList.Add (Ch);
			index.Add (0);
			//Debug.Log ("Not Impossibrauh3");
			Impossibrah=0;
			
		} else if (!can) {
			//Debug.Log ("Impossibrauh");
			Impossibrah++;
		}
		if(Ch.clear){
			Cannot=new List<int>();
		}
		Cannot.AddRange(Ch.Cannot.ToArray());
		moved=Ch.moved;
		return Position;
	}
	public void Finish(){
		for(int i=0;i<UpdateList.Count;i++){
			UpdateList[i].Execute ();
		}

	}

	public void AddCannot(int u){
		if(!Cannot.Contains(u)){
			Cannot.Add (u);
		}
	}
}
