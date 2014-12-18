using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthBar : MonoBehaviour {
	public static int playerHealth;
	public Text Health;
	// Use this for initialization
	void Start () {
		Health = GetComponent<Text>();
		playerHealth = 100;
		Health.text = "health: 100";
	}
	
	// Update is called once per frame
	void Update () {
		Health.text = "health: " + playerHealth;
	}
}
