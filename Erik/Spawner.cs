using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyTypes{undefined, flying1, bunny1, flying2}

public class SpawnInfo{
	public Vector3 spawnLocation;
	public EnemyTypes enemyType;
}

public class Spawner {

	public static EnemySpawnParameters[] enemySpawnParams;	

	public static List<GameObject> SpawnedObjects = new List<GameObject>();
	public static List<Vector3> SpawnLocations = new List<Vector3>();
	public static List<EnemyTypes> SpawnTypeList = new List<EnemyTypes>();

	public static List<SpawnInfo> SpawnInfoList = new List<SpawnInfo>();

	public static int maxSpawns = 50;
	private static int spawned = 0;

	public static void addSpawnLocation(Vector3 location){
		SpawnLocations.Add (location);
		
		SpawnInfo sp = new SpawnInfo();
		sp.spawnLocation = location;

		EnemyTypes enemytype = getEnemyTypeByChances();
		SpawnTypeList.Add(enemytype);
		sp.enemyType = enemytype;
		
		
		SpawnInfoList.Add(sp);
	}


	public static void addSpawnLocation(Vector3 location, EnemyTypes enemytype){
		SpawnLocations.Add (location);

		SpawnInfo sp = new SpawnInfo();
		sp.spawnLocation = location;

		if (enemytype != EnemyTypes.undefined){
			SpawnTypeList.Add(enemytype);
			sp.enemyType = enemytype;
		}
		else{
			enemytype = getEnemyTypeByChances();
			SpawnTypeList.Add(enemytype);
			sp.enemyType = enemytype;
		}

		SpawnInfoList.Add(sp);
	}


	public static GameObject SpawnType(GameObject nearObj, out EnemyTypes etypeOut, int nNearest = 1, int nthNearest = 0){
		RefreshSpawnList();
		
		Vector3 location=Vector3.zero;
		
		if (nNearest == 1){
			location = getNearestPositionTo(nearObj.transform.position);
		}
		
		else {
			List<Vector3> nLocations = getNearestPositionsTo(nearObj.transform.position, nNearest, nthNearest);
			// pick a random location
			if (nLocations != null&&nLocations.Count>0){
				int element = Random.Range (0, nLocations.Count-1);
				location = nLocations[element];
			}
		}	
		
		if (location!=Vector3.zero && spawned < maxSpawns && SpawnLocations.Count > 0){

			EnemyTypes etype = EnemyTypes.undefined;

			for (int i = 0; i < SpawnLocations.Count; i++){
				if (SpawnLocations[i].Equals(location)){
					etype = SpawnTypeList[i];
				}
			}

			for (int i = 0; i < enemySpawnParams.Length; i++){
				if (etype == enemySpawnParams[i].enemyType){
					GameObject spawnedObject = GameObject.Instantiate(enemySpawnParams[i].enemy, location, Quaternion.identity) as GameObject;

					float hp = Random.Range(enemySpawnParams[i].hpmin, enemySpawnParams[i].hpmax);

					HPmanager hpman = spawnedObject.GetComponent<HPmanager>();

					if (hpman){
						hpman.GetComponent<HPmanager>().setHP(hp);
						hpman.GetComponent<HPmanager>().setMaxHP(hp);
					}
					else{
						hpman = spawnedObject.AddComponent<HPmanager>();
						hpman.GetComponent<HPmanager>().setHP(hp);
						hpman.GetComponent<HPmanager>().setMaxHP(hp);
					}
					SpawnedObjects.Add (spawnedObject);
					spawned++;
					etypeOut = etype;
					return spawnedObject;
				}
			}


		}
		etypeOut = EnemyTypes.undefined;
		return null;
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

	private static List<Vector3> getNearestPositionsTo(Vector3 pos, int n, int nthNearest = 0){

		List<Vector3> spawnLocCopy = new List<Vector3>();
		for (int i = 0; i < SpawnLocations.Count; i++){
			spawnLocCopy.Add(SpawnLocations[i]);
		}
		List<Vector3> bestPositions = new List<Vector3>();

		int nthNearestDone = 0;


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
				if (nthNearestDone == nthNearest){
					bestPositions.Add(bestPos);				
					spawnLocCopy.RemoveAt(bestIndex);
				}
				else{
					spawnLocCopy.RemoveAt(bestIndex);
					nthNearestDone++;
				}
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
		SpawnTypeList.Clear();
		spawned = 0;
	}

	public static void SetEnemySpawnParameters(EnemySpawnParameters[] esp){
		enemySpawnParams = esp;
	}

	private static EnemyTypes getEnemyTypeByChances(){

		List<float> chanceList = new List<float>();
		chanceList.Add (0f);
		float accumulatedChance = 0f;

		for (int i = 0; i < enemySpawnParams.Length; i++){
			chanceList.Add(enemySpawnParams[i].createSpawnerChance + accumulatedChance);
			accumulatedChance += enemySpawnParams[i].createSpawnerChance;

		}

		float rnd = Random.Range(0f, accumulatedChance);

		int lastIndex = 0;
		float max = 0;
		if (chanceList.Count != 0){
			max = chanceList[0];
			
			for (int i = 1; i < chanceList.Count; i++){
				if (rnd > max){
					max = chanceList[i];
					lastIndex = i-1;
				}
			}
		}
		return enemySpawnParams[lastIndex].enemyType;

	}

	private static float getMaxListValue(List<float> L){
		float max = 0;
		if (L.Count != 0){
			max = L[0];

			for (int i = 1; i < L.Count; i++){
				if (max < L[i]){
					max = L[i];
				}
			}
		}

		return max;
	}

}
