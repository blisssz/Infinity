using UnityEngine;
using System.Collections;


[System.Serializable]
public class EnemySpawnParameters{
	public GameObject enemy;

	public EnemyTypes enemyType = EnemyTypes.undefined;

	public bool usePrefabParams = true;
	//public controlParameters cp;
	public bool flying = false;
	[Range(0f, 1f)]
	public float PgroupRepulsion;

	[Range(0f, 1f)]
	public float createSpawnerChance;

	[Range(0f, 1f)]
	public float spawnChance;

	public float hpmin = 10f;
	public float hpmax = 20f;
}


public class UseSpawner : MonoBehaviour {

	public EnemySpawnParameters[] enemyParams;	

	public int nSpawnerAwayFromPlayer = 1;
	public int nNearestSpawners = 4;

	private static int spawnTimer = 60;
	private static float spawnChance = 0.5f;

	// Use this for initialization
	void Start () {
		Spawner.ResetSpawner();
		Spawner.SetEnemySpawnParameters(enemyParams);
	}
	
	// Update is called once per frame
	void FixedUpdate () {


		if ( Time.frameCount % spawnTimer == 0 && Random.value < spawnChance){

			GameObject player = GameObject.FindGameObjectWithTag("Player");

			if (player != null){

//				int pick = Random.Range (0, enemyParams.Length);

			//	GameObject enemyObj = Spawner.Spawn (enemyParams[pick].enemy, player, 4);
				EnemyTypes etype;
				GameObject enemyObj = Spawner.SpawnType (player, out etype, nNearestSpawners, nSpawnerAwayFromPlayer);

				if (enemyObj != null){

					AgentS agentS = enemyObj.GetComponent<AgentS>();

					if (etype == EnemyTypes.bunny1){
						setBunnyParamaters(agentS, etype);
					}

					else if (etype == EnemyTypes.bunny1){
						setFlyingParamaters(agentS, etype);
					}
				}

			}
		}
	}

	private void setBunnyParamaters(AgentS agentS, EnemyTypes etype){

		if (etype == EnemyTypes.bunny1){
			agentS.groupChance = Random.value;
			agentS.cp.v_max = Random.Range(0.8f, 1f) * agentS.cp.v_max;
			agentS.psoTurnFac = Random.Range(0.5f, 3f);
			
			agentS.globalFitnessFac = Random.value;
			agentS.globalTrackFrames = (int) Random.Range(30, 200);
			
			agentS.cp.L0 = Random.Range (2f, 5f);
			//agentS.cp.dampfac = Random.Range (0.01f, 1f);
			agentS.cp.Lcomp = agentS.cp.L0 * 0.3f;
			
			agentS.groupDistance = Random.Range(10f, 30f);
			agentS.minGroupDistance = Random.Range (5f, 9f);
			
			if (Random.value < 0.3f){
				agentS.groupInvert = true;
			}
		}


	}


	private void setFlyingParamaters(AgentS agentS, EnemyTypes etype){
		
		if (etype == EnemyTypes.flying1){
			agentS.groupChance = Random.value;
			agentS.cp.v_max = Random.Range(0.8f, 1f) * agentS.cp.v_max;
			agentS.psoTurnFac = Random.Range(0.5f, 3f);
			
			agentS.globalFitnessFac = Random.value;
			agentS.globalTrackFrames = (int) Random.Range(30, 200);
			
			agentS.cp.L0 = Random.Range (2f, 5f);
			agentS.cp.dampfac = Random.Range (0.01f, 1f);
			agentS.cp.Lcomp = agentS.cp.L0 * 0.3f;
			
			agentS.groupDistance = Random.Range(10f, 30f);
			agentS.minGroupDistance = Random.Range (5f, 9f);
			
			if (Random.value < 0.5f){
				agentS.groupInvert = true;
			}
		}
		
		
	}



	/// <summary>
	/// Sets the spawn time. In seconds
	/// </summary>
	/// <param name="t">T.</param>
	public static void setSpawnTime(float t){
		spawnTimer = (int)(t * 60f);
	}

	public static void setSpawnChance(float p){
		spawnChance = Mathf.Clamp(p, 0f, 1f);
	}
}
