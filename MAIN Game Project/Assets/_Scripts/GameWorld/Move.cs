using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Move {
	public Vector3 StartPosition;
	public Vector3 EndPosition;
	public Quaternion Direction = new Quaternion ();
	public int Choice;
	public GameObject SpikePlane;
	public float[][] Update;
	public bool moved;
	public bool clear;
	public Path now;
	public List<int> Cannot=new List<int> ();
	public Quaternion AngleChanger;
	public Move (int u, Vector3 Position, GameObject p,Path P)
	{
		Choice = u;
		SpikePlane = p;
		clear = false;
		StartPosition=Position;
		now=P;
		
	}
	
	public void Choose ()
	{
		Update = new float[0][];
		EndPosition = new Vector3 (0, 0, 0);
		
		moved = false;
		switch (Choice) {
		case 0:
			float minDistance = 15;
			float maxDistance = 50;
			float Distance = Random.Range (minDistance, maxDistance);
			AngleChanger = Quaternion.Euler (0, Random.Range (-100, 100) + Direction.eulerAngles.y - 90, Random.Range (-30, 30));
			EndPosition = StartPosition + AngleChanger * (new Vector3 (Distance, 0, 0));
			Update = (Moves.Box (StartPosition, EndPosition, 6, 1));
			moved = true;
			clear = true;
			Direction = Quaternion.LookRotation (EndPosition - StartPosition, Vector3.up);
			break;
			
		case 1:
			Update = (Moves.Sphere (StartPosition, 12, 1));
			moved = false;
			clear = false;
			Cannot.Add(1);
			break;
			
		case 2:
			minDistance = 10;
			maxDistance = 25;
			Distance = Random.Range (minDistance, maxDistance);
			EndPosition = StartPosition + (new Vector3 (0, Distance, 0));
			Update = (Moves.Box (StartPosition, EndPosition, 6, 1));
			moved = true;
			clear = true;
			Direction = Quaternion.LookRotation (EndPosition - StartPosition, Vector3.up);
			Cannot.Add(3);
			Cannot.Add(2);
			break;
			
		case 3:
			minDistance = 30;
			maxDistance = 50;
			Distance = Random.Range (minDistance, maxDistance);
			AngleChanger = Quaternion.Euler (0, Random.Range (-100, 100) + Direction.eulerAngles.y - 90, Random.Range (-30, 30));
			EndPosition = StartPosition + AngleChanger * (new Vector3 (Distance, 0, 0));
			Vector3 Diff = (EndPosition - StartPosition).normalized;
			float[][] NewUpdate1 = (Moves.Box (StartPosition, EndPosition, 6, 1));
			float[][] NewUpdate2 = (Moves.Box (StartPosition + Diff * 12 - new Vector3 (0, 12, 0), EndPosition - Diff * 12 - new Vector3 (0, 12, 0), 6, 1));
			//float[][] NewUpdate3 = (Moves.Box (Position+Diff*12+new Vector3(0,12,0), Position2-Diff*12+new Vector3(0,12,0), 6, 1));
			Update = new float[NewUpdate1.Length + NewUpdate2.Length/*+NewUpdate3.Length*/][];
			NewUpdate1.CopyTo (Update, 0);
			NewUpdate2.CopyTo (Update, NewUpdate1.Length);
			//NewUpdate3.CopyTo (NewUpdate,NewUpdate2.Length);
			moved = true;
			clear = true;
			Direction = Quaternion.LookRotation (EndPosition - StartPosition, Vector3.up);
			Cannot.Add(4);
			Cannot.Add(3);
			break;

		case 4:
			minDistance = 10;
			maxDistance = 25;
			Distance = Random.Range (minDistance, maxDistance);
			EndPosition = StartPosition + (new Vector3 (0, -Distance, 0));
			Update = (Moves.Box (StartPosition, EndPosition, 6, 1));
			moved = true;
			clear = true;
			Direction = Quaternion.LookRotation (EndPosition - StartPosition, Vector3.up);
			break;
			
		default:
			
			break;
			
			
		}
	}
	
	public bool Check ()
	{
		bool a = ChunkList.CheckChunks (Update);
		return a;
	}
	
	public void Execute ()
	{
		
		ChunkList.UpdateDataChunks (Update);
		
		switch (Choice) {
		case 3:
			SpikePlane.transform.localScale = new Vector3 (((EndPosition - StartPosition)).magnitude - 6f, 3f, 14f);
			SpikePlane.transform.localRotation = AngleChanger;
			SpikePlane.transform.localPosition = EndPosition / 2 + StartPosition / 2 - new Vector3 (0, 18, 0);
			HelpScript.Instantiate (SpikePlane);
			break;
			
		default:
			break;
			
		}
	}
}