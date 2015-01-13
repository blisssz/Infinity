using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemySpawn : MonoBehaviour {
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

	public static void Spawn(Vector3 pos, GameObject enemy){
		startPosition = pos + enPosition;
		Instantiate (enemy,startPosition, enRotation);
	}

	public static void SpawnObject(Vector3 pos, float Chance, string a){
//		if(HelpScript.Rand(0,1)<Chance){
//		Instantiate (GetObject (a),startPosition, enRotation);
//		}
	}

	public static int GetIndex(string Name){
		for(int i=0; i<Names.Count;i++){
			if(Names[i].Equals (Name)){
				return i;
			}
		}

		ObjectList.Add ((GameObject)Resources.Load(Name, typeof(GameObject)));
		Names.Add (Name);
		return ObjectList.Count-1;

	}

	public static GameObject GetObject(string Name){
		return ObjectList[GetIndex(Name)];
	}
}
