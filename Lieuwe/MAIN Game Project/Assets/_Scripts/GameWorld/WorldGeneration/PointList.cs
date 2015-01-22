using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PointList
{
	private static List<float[]> PointsLst=new List<float[]>();
    public static float Delta=6f;
	

	public static void AddPoints (float[][] Points)
	{
		PointsLst.AddRange (Points);
//		for(int i=0;i<Points.Length;i++){
//			if(Points[i][4]==0){
//				HelpScript.createSphere(new Vector3(Points[i][0],Points[i][1],Points[i][2]));}
//			if(Points[i][4]==1){
//				HelpScript.createCube(new Vector3(Points[i][0],Points[i][1],Points[i][2]));}
//		}
//		
		
	}

	public static void Reset(){
		PointsLst=new List<float[]>();
	}
	
	public static bool CheckPoints(float[][] Points){
//		bool IsPossible=true;
		for(int i=0; i<Points.Length;i++){
			for(int j=0; j<PointsLst.Count;j++){
				if(Points[i][4]==0){
						if(!CheckDistance(Points[i],PointsLst[j])){
						return false;
					}
				} else	if(Points[i][4]==1&&PointsLst[j][4]==0){
					if(!CheckDistance(Points[i],PointsLst[j])){
						return false;
					}
				} 
			}
		} 
        return true;


	}

		public static bool CheckDistance(float[] Point1, float[] Point2){
		float Distance=(Point1[0]-Point2[0])*(Point1[0]-Point2[0])+(Point1[1]-Point2[1])*(Point1[1]-Point2[1])+(Point1[2]-Point2[2])*(Point1[2]-Point2[2]);
		if(Distance<(Point1[3]+Point2[3])*(Point1[3]+Point2[3])){
			return false;} 
		else {
			return true;}
		}


				
}

