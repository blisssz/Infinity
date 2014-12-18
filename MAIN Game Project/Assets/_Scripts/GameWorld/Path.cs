using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path  {
	
	public Vector3 Position;
	public float[] Chances;
	public List<Move> UpdateList = new List<Move> ();
	public List<int> index = new List<int> ();
	private Quaternion Direction = new Quaternion ();
	private List<int> Cannot = new List<int>();
	public GameObject SpikePlane;
	public int Impossibrah=0;
	
	public Path(Vector3 StartPosition, GameObject Spikeplane){
		Position=StartPosition;
		SpikePlane=Spikeplane;
	}
	
	public Vector3 Move1 ()
	{
		
		float[] DefChances = new float[5]{0.8f,0.1f,0.2f,0.6f,0.2f};
		Chances =new float[DefChances.Length];
		int n=0;
		for(int i=0;i<DefChances.Length;i++){
			if(Cannot.Contains (i)){
				Chances[i]=0;
			} else {
				Chances[i]=DefChances[i];
			}
			
		}
		Vector3 Position2 = new Vector3 (0, 0, 0);
		int u = 0;

		bool clear = false;
		u = HelpScript.Switch (Chances);
		Debug.Log(u);

		Move Ch=new Move(u,Position,SpikePlane,this);
		Ch.Choose();
		bool can=!Ch.Check ();

		if (can&&Ch.moved&& index [0] == 0) {
			List<Move> TempUpdateList = new List<Move> ();
			List<int> TempIndex = new List<int> ();
			for (int i=0; i<index.Count; i++) {
				if (index [i] == 0) {
					UpdateList [i].Execute();
				} else {
					TempIndex.Add (index [i] - 1);
					TempUpdateList.Add (UpdateList [i]);
				}
			}
			UpdateList = TempUpdateList;
			UpdateList.Add (Ch);
			index = TempIndex;
			index.Add (3);
			Direction = Ch.Direction;
			Position = Ch.EndPosition;
			//Debug.Log ("Not Impossibrauh1");
			Impossibrah=0;
			
		} else if (Ch.moved && can && index [0] != 0) {
			for (int i=0; i<index.Count; i++) {
				index [i] = index [i] - 1;
			}
			index.Add (3);
			UpdateList.Add (Ch);
			Direction = Ch.Direction;
			Position = Ch.EndPosition;
			//Debug.Log ("Not Impossibrauh2");
			Impossibrah=0;
			
		} else if (!Ch.moved && can) {
			UpdateList.Add (Ch);
			index.Add (3);
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

		return Position;
	}

	public void AddCannot(int u){
		if(!Cannot.Contains(u)){
			Cannot.Add (u);
		}
	}
}
