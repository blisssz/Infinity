using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Move {
	private float[][] Points;
	public Vector3 StartPosition;
	public Vector3 EndPosition;
	public Quaternion Direction = new Quaternion ();
	public int Choice;
	public float MinOneDistance;
	public float[][] Update;
	public bool moved;
	public bool clear;
	public Path now;
	public List<int> Cannot=new List<int> ();
	public Quaternion AngleChanger;
    public float[] values;
	public Move (int u, Vector3 Position, float p,Path P)
	{
		Choice = u;
		MinOneDistance = p;
		clear = false;
		StartPosition=Position;
		now=P;
		Direction=P.Direction;

		
	}
	
	public void Choose ()
	{
		Update = new float[0][];
		EndPosition = new Vector3 (0, 0, 0);
		Points= new float[0][];

		
		switch (Choice) {
		case 0:
            values=new float[3];
			float minDistance = 25;
			float maxDistance = 60;
			values[0] =  HelpScript.Rand(minDistance, maxDistance);
            values[1] = HelpScript.Rand (-100, 100);
            values[2] = HelpScript.Rand (-30, 30);
			moved = true;
			clear = true;
			break;
			
		case 1:
            values=new float[1];
			values[0]=12;
			moved = false;
			clear = false;
			break;
			
		case 2:
			values=new float[3];
            minDistance = 15;
            maxDistance = 30;
            values[0] =  HelpScript.Rand(minDistance, maxDistance);
			moved = true;
			clear = true;
			break;
			
		case 3:
            values=new float[3];
			minDistance = 15;
			maxDistance = 60;
			values[0] =  HelpScript.Rand(minDistance, maxDistance);
            values[1] = HelpScript.Rand (-100, 100);
			values[2] = HelpScript.Rand (-30, 30);
			moved = true;
			clear = true;
			break;
			
		case 4:
			values=new float[1];
			minDistance = 15;
			maxDistance = 30;
			values[0] = HelpScript.Rand (minDistance, maxDistance);
			moved = true;
			clear = true;
			break;
			
		case 5:

			break;

		default:
			
			break;
			
			
		}
	}
	
	public bool Check ()
	{   

        bool possible=false;

        switch (Choice) {
        case 0:
            AngleChanger = Quaternion.Euler (0, values[1]  + Direction.eulerAngles.y - 90, values[2]);
            EndPosition = StartPosition + AngleChanger * (new Vector3 (values[0], 0, 0));
			Points=Moves.CheckBox (StartPosition, EndPosition, 6, MinOneDistance);
            possible = PointList.CheckPoints(Points);
            break;
            
        case 1:
			Points=Moves.CheckSphere(StartPosition, values[0], MinOneDistance);
            possible=PointList.CheckPoints(Points);
            break;
            
        case 2:
            EndPosition = StartPosition + (new Vector3 (0, values[0], 0));
			Points=Moves.CheckBox (StartPosition, EndPosition, 6, MinOneDistance);
            possible = PointList.CheckPoints(Points);
            break;
            
        case 3:
            AngleChanger = Quaternion.Euler (0, values[1] + Direction.eulerAngles.y - 90, values[2]);
            EndPosition = StartPosition + AngleChanger * (new Vector3 (values[0], 0, 0));
            Vector3 Diff = (EndPosition - StartPosition).normalized;
            float[][] Points1=Moves.CheckBox (StartPosition, EndPosition, 6, MinOneDistance);
            float[][] Points2=Moves.CheckBox (StartPosition + Diff * 15 - new Vector3 (0, 12, 0), EndPosition - Diff * 15 - new Vector3 (0, 12, 0), 6, 0);
			Points = new float[Points1.Length + Points2.Length/*+NewUpdate3.Length*/][];
			Points1.CopyTo (Points, 0);
			Points2.CopyTo (Points, Points1.Length);
            possible=PointList.CheckPoints (Points);
            break;
            
        case 4:
            EndPosition = StartPosition + (new Vector3 (0, -values[0], 0));
			Points=Moves.CheckBox (StartPosition, EndPosition, 6, MinOneDistance);
            possible = PointList.CheckPoints(Points);
            break;
            
        case 5:
			possible=false;
            break;
            
        default:
            
            break;
        }
		return possible;
	}
	
	public void Execute ()
	{

        moved = false;
		switch (Choice) {
        case 0:   
            AngleChanger = Quaternion.Euler (0, values[1]  + Direction.eulerAngles.y - 90, values[2]);
            EndPosition = StartPosition + AngleChanger * (new Vector3 (values[0], 0, 0));
            Update = (Moves.Box (StartPosition, EndPosition, 6, 0));
            Direction = Quaternion.LookRotation (EndPosition - StartPosition, Vector3.up);
			PointList.AddPoints(Points);
            ChunkList.UpdateDataChunks (Update);
            break;
            
        case 1:
			Update = (Moves.Sphere (StartPosition, values[0], 0 ));
            Cannot.Add(1);
			PointList.AddPoints(Points);
            ChunkList.UpdateDataChunks (Update);
            break;
            
        case 2:
            EndPosition = StartPosition + (new Vector3 (0, values[0], 0));
            Update = Moves.Box (StartPosition, EndPosition, 6, 0);
            Direction = Quaternion.LookRotation (EndPosition - StartPosition, Vector3.up);
            Cannot.Add(3);
            Cannot.Add(2);
            Cannot.Add(4);
			PointList.AddPoints(Points);
            ChunkList.UpdateDataChunks (Update);
            break;
            
        case 3:
            AngleChanger = Quaternion.Euler (0, values[1] + Direction.eulerAngles.y - 90, values[2]);
            EndPosition = StartPosition + AngleChanger * (new Vector3 (values[0], 0, 0));
            Vector3 Diff = (EndPosition - StartPosition).normalized;
            float[][] NewUpdate1 = (Moves.Box (StartPosition, EndPosition, 6, 0));
            float[][] NewUpdate2 = (Moves.Box (StartPosition + Diff * 15 - new Vector3 (0, 12, 0), EndPosition - Diff * 15 - new Vector3 (0, 12, 0), 6, 2));
            //float[][] NewUpdate3 = (Moves.Box (Position+Diff*12+new Vector3(0,12,0), Position2-Diff*12+new Vector3(0,12,0), 6, 1));
            Update = new float[NewUpdate1.Length + NewUpdate2.Length/*+NewUpdate3.Length*/][];
            NewUpdate1.CopyTo (Update, 0);
            NewUpdate2.CopyTo (Update, NewUpdate1.Length);
            //NewUpdate3.CopyTo (NewUpdate,NewUpdate2.Length);
            Direction = Quaternion.LookRotation (EndPosition - StartPosition, Vector3.up);
            Cannot.Add(4);
			PointList.AddPoints(Points);
            ChunkList.UpdateDataChunks (Update);
            break;
            
        case 4:
            EndPosition = StartPosition + (new Vector3 (0, -values[0], 0));
            Update = (Moves.Box (StartPosition, EndPosition, 6, 0));
            Direction = Quaternion.LookRotation (EndPosition - StartPosition, Vector3.up);
            Cannot.Add(2);
			PointList.AddPoints(Points);
            ChunkList.UpdateDataChunks (Update);
            break;
            
        case 5:
            
            break;
            
        default:
            
            break;
            
            
        }

		
		
	}
}