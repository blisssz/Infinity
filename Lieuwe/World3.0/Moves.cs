using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Moves  {

	//private static int sizeChunk;
	private static float rib;

		
	// Use this for initialization
	public static void StartX () {
		//sizeChunk = ChunkList.sizeChunk;
					rib = ChunkList.rib;
	
	}






	public static float[][] Sphere (Vector3 Position,int radius, int value) {
		int r=(int)(radius/rib);
		List<float[]> lst=new List<float[]>();
		for(int i=-r;i<(r+1);i++){
			for(int j=-r;j<(r+1);j++){
				for(int k=-r;k<(r+1);k++){
					if(i*i+j*j+k*k<r*r){
						lst.Add(new float[4]{i*rib+Position.x,j*rib+Position.y,k*rib+Position.z,value});
					}

					
				}
				
			}

		}
//		Debug.Log (lst.Count);	
//		Debug.Log ("yes");
				return lst.ToArray();
//		Debug.Log (lst.ToString());
	
	}

	public static float[][] Box (Vector3 Position,int sidex,int sidey,int sidez, int value) {
		List<float[]> lst=new List<float[]>();
		sidex=(int)(sidex/rib);
		sidey=(int)(sidey/rib);
		sidez=(int)(sidez/rib);
		for(int i=-sidex/2;i<(sidex/2+1);i++){
			for(int j=-sidey/2;j<(sidey/2+1);j++){
				for(int k=-sidez/2;k<(sidez/2+1);k++){
					lst.Add(new float[4]{i*rib+Position.x,j*rib+Position.y,k*rib+Position.z,value});
					
				}
				
			}
			
		}
//		Debug.Log (lst.Count);	
//		Debug.Log ("yes");
		return lst.ToArray();
//		Debug.Log (lst.ToString());
		
	}

	public static float[][] Box (Vector3 Position1, Vector3 Position2, float minDistance, int value) {
		Vector3 xx=Position2-Position1;
		Vector3 yy=new Vector3(0,10,0);
		Vector3 zz=new Vector3(5,-5,10);
		Vector3.OrthoNormalize (ref xx,ref yy,ref zz);
		Vector3 Difference=Position2-Position1;
		Vector3[] Dat=new Vector3[4];
		xx=xx*minDistance;
		yy=yy*minDistance;
		zz=zz*minDistance;
		Dat[0]=Position1-xx-yy-zz;
		Dat[1]=Position1+xx+Difference-yy-zz;
		Dat[2]=Position1-xx+yy-zz;
		Dat[3]=Position1-xx-yy+zz;
		return Box (Dat,value);

		
	}

	public static float[][] Box (Vector3[] Positions, int value) {
		Vector3[] Positions2=new Vector3[8];
		for(int i=0;i<4;i++){
			Positions2[i]=Positions[i];
			//Debug.Log (Positions[i][0]+ "nah" + Positions[i][1]+ "nah" + Positions[i][2]);
		}
		Vector3[] PositionsDiff=new Vector3[3];
		PositionsDiff[0]=Positions[1]-Positions[0];
		PositionsDiff[1]=Positions[2]-Positions[0];
		PositionsDiff[2]=Positions[3]-Positions[0];
		Positions2[4]=Positions[1]+Positions[2]-Positions[0];
		Positions2[5]=Positions[1]+Positions[3]-Positions[0];
		Positions2[6]=Positions[2]+Positions[3]-Positions[0];
		Positions2[7]=Positions[1]+Positions[2]+Positions[3]-Positions[0]*2;
		int sidex=(int)((PositionsDiff[0]).magnitude/rib)+1;
		int sidey=(int)((PositionsDiff[1]).magnitude/rib)+1;
		int sidez=(int)((PositionsDiff[2]).magnitude/rib)+1;
		float[][] Positions3=HelpScript.Vec3ToFloat(Positions2,1);
		float[][] Positions4=new float[3][];
		Positions4[0]=HelpScript.Vec3ToFloat(PositionsDiff[0],1/(float)sidex);
		Positions4[1]=HelpScript.Vec3ToFloat(PositionsDiff[1],1/(float)sidey);
		Positions4[2]=HelpScript.Vec3ToFloat(PositionsDiff[2],1/(float)sidez);

		List<float[]> lst=new List<float[]>();
		for(int i=0;i<sidex*1.5;i++){
			for(int j=0;j<sidey*1.5;j++){
				for(int k=0;k<sidez*1.5;k++){
					float[] Position5=new float[4];
					Position5[0]=Positions4[0][0]*(i/1.5f)+Positions4[1][0]*(j/1.5f)+Positions4[2][0]*(k/1.5f)+Positions3[0][0];
					Position5[1]=Positions4[0][1]*(i/1.5f)+Positions4[1][1]*(j/1.5f)+Positions4[2][1]*(k/1.5f)+Positions3[0][1];
					Position5[2]=Positions4[0][2]*(i/1.5f)+Positions4[1][2]*(j/1.5f)+Positions4[2][2]*(k/1.5f)+Positions3[0][2];
					Position5[3]=value;
					lst.Add(Position5);
					
				}
				
			}
			
		}
//		Debug.Log (lst.Count);	
//		Debug.Log ("yes");
		return lst.ToArray();
//		Debug.Log (lst.ToString());
		
	}
}
