using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour {
	private static Quaternion enRotation = Quaternion.identity;
	private static Vector3 enPosition = new Vector3 (0, 3, 0);
	private static float chance;
	private static Vector3 startPosition;
	private static List<string> Names=new List<string>();
	private static List<GameObject> ObjectList=new List<GameObject>();



	public static void checkForEnemy(Vector3 pos, GameObject enemy){
		chance = Random.value;
		if(chance <= 0.1){ Spawn(pos, enemy);}
	}

	public static void Spawn(Vector3 Position, GameObject enemy){
		startPosition = Position + enPosition;
		Instantiate (enemy,Position, enRotation);
	}

	public static void SpawnObject(Vector3 Position, float Chance, string a){
		if(HelpScript.Rand(0,1)<Chance){
		Instantiate (GetObject (a),Position, enRotation);
		}
	}

	public static GameObject SpawnObjectWith(Vector3 Position, string a){
			return Instantiate (GetObject (a),Position, enRotation) as GameObject;
	}

	public static void SpawnObject(Vector3 Position, string a){
		Instantiate (GetObject (a),Position, enRotation);
	}

	public static int GetIndex(string Name){
		for(int i=0; i<Names.Count;i++){
			if(Names[i].Equals (Name)){
				return i;
			}
		}
		//string S=Name + ".prefab";
		//Debug.Log (S);
		//Object C=Resources.Load(S);
		if(Resources.Load( Name)!=null){
			ObjectList.Add ((GameObject)Resources.Load(Name));
				Names.Add (Name);} 

		return ObjectList.Count-1;

	}

	public static GameObject GetObject(string Name){
		if(GetIndex(Name)>-1){
			return ObjectList[GetIndex(Name)];
		} else {
			return null;
		}
	}
}
