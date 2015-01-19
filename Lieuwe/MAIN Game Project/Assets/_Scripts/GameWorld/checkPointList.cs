using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class checkPointList  {
	private static List<checkPoint> ObjectList=new List<checkPoint>();
	private static int ActiveID;

	public static int AddCheckPoint(checkPoint NewCheckPoint){
		ObjectList.Add (NewCheckPoint);
		return ObjectList.Count-1;
	}



	public static void Activate(int ID){
		ObjectList[ActiveID].DeActivate();
		ActiveID=ID;
		ObjectList[ActiveID].Activate();
	}
}
