using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner {

	public static List<GameObject> SpawnedObjects = new List<GameObject>();
	public static List<Vector3> SpawnLocations = new List<Vector3>();

	public static int maxSpawns = 50;
	private static int spawned = 0;

	public static void addSpawnLocation(Vector3 location){
		SpawnLocations.Add (location);
	}

	public static GameObject Spawn(GameObject spawnobj, GameObject nearObj, int nNearest = 1){
		RefreshSpawnList();

		Vector3 location=Vector3.zero;

		if (nNearest == 1){
			location = getNearestPositionTo(nearObj.transform.position);
		}

		else {
			List<Vector3> nLocations = getNearestPositionsTo(nearObj.transform.position, nNearest);
			// pick a random location
			if (nLocations != null&&nLocations.Count>0){
				int element = Random.Range (0, nLocations.Count-1);
				location = nLocations[element];
			}
		}



		if (location!=Vector3.zero&&spawned < maxSpawns && SpawnLocations.Count > 0){
			GameObject spawnedObject = GameObject.Instantiate(spawnobj, location, Quaternion.identity) as GameObject;
			SpawnedObjects.Add (spawnedObject);
			spawned++;
			return spawnedObject;
		}

		return null;
	}

	private static Vector3 getNearestPositionTo(Vector3 pos){

		if (SpawnLocations.Count > 0){
			float nearest = (SpawnLocations[0] - pos).sqrMagnitude;
			Vector3 bestPos = SpawnLocations[0];
			for (int i = 1; i < SpawnLocations.Count; i++){
				float checkNearest = (SpawnLocations[i] - pos).sqrMagnitude;

				if (checkNearest < nearest){
					nearest = checkNearest;
					bestPos = SpawnLocations[i];
				}

			}

			return bestPos;
		}

		return Vector3.zero;

	}

	private static List<Vector3> getNearestPositionsTo(Vector3 pos, int n){

		List<Vector3> spawnLocCopy = new List<Vector3>();
		for (int i = 0; i < SpawnLocations.Count; i++){
			spawnLocCopy.Add(SpawnLocations[i]);
		}
		List<Vector3> bestPositions = new List<Vector3>();


		for (int j = 0; j < n; j++){

			if (spawnLocCopy.Count > 0){

				float nearest = (spawnLocCopy[0] - pos).sqrMagnitude;
				Vector3 bestPos = spawnLocCopy[0];
				int bestIndex = 0;

				for (int i = 1; i < spawnLocCopy.Count; i++){
					float checkNearest = (spawnLocCopy[i] - pos).sqrMagnitude;
					
					if (checkNearest < nearest){
						nearest = checkNearest;
						bestPos = spawnLocCopy[i];
						bestIndex = i;
					}				
				}

				bestPositions.Add(bestPos);				
				spawnLocCopy.RemoveAt(bestIndex);
			}
	

		}
		return bestPositions;
	}


	private static void RefreshSpawnList(){
		for (int i = 0; i < SpawnedObjects.Count; i++){
			if (SpawnedObjects[i] == null){
				SpawnedObjects.RemoveAt(i);
				spawned--;
				i--;
			}
		}
	}

	public static void ResetSpawner(){
		SpawnedObjects.Clear();
		SpawnLocations.Clear();
		spawned = 0;
	}


}
