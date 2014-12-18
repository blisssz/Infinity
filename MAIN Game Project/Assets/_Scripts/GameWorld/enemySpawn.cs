using UnityEngine;
using System.Collections;

public class enemySpawn : MonoBehaviour {
	private static Quaternion enRotation = Quaternion.identity;
	private static Vector3 enPosition = new Vector3 (0, 3, 0);
	private static float chance;
	private static Vector3 startPosition;

	public static void checkForEnemy(Vector3 pos, GameObject enemy){
		chance = Random.value;
		if(chance <= 0.1){ Spawn(pos, enemy);}
	}

	public static void Spawn(Vector3 pos, GameObject enemy){
		startPosition = pos + enPosition;
		Instantiate (enemy,startPosition, enRotation);
	}
}
