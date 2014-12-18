using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthBar : MonoBehaviour {
	public static float playerHealth;
	public GameObject Player;
	public Text Health;
	// Use this for initialization
	void Start () {
		Health = GetComponent<Text>();
		Health.text = "health: 100";
	}
	
	// Update is called once per frame
	void Update () {
		if (Player == null) {
			Player = GameObject.FindWithTag ("Player");		
		}
		playerHealth = Player.GetComponent<HPmanager>().hp;
		Health.text = "health: " + playerHealth;
	}
}
