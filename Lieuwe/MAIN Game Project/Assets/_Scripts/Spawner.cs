using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner {

	public static  List<GameObject> SpawnedObjects;

	public Vector3 spawnLocation{get; set;}
	public GameObject spawnerObject;

	public int maxSpawns = 1;
	public int spawned = 0;


	public Spawner(GameObject sObj){
		spawnerObject = sObj;
		spawnLocation = new Vector3(0.0f, 0.0f, 0.0f);

	}

	public Spawner(GameObject sObj, Vector3 sLoc){
		spawnerObject = sObj;
		spawnLocation = sLoc;
	}

	void Spawn(){
		if (spawned < maxSpawns){
			GameObject.Instantiate(spawnerObject, spawnLocation, Quaternion.identity);
			spawned++;
		}
	}


}
