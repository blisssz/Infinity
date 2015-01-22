using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHP : MonoBehaviour {

	public Text jetpackActivationText;
	public Text outOfAmmoText;
	public SlidingBar slider;
	public GameObject HPbar;

	private float previousHP;
	private float currentHP;
	private float maxHP;
	private PogoBoss pogoBoss;
	private gravityBoss gravitybossinstance;
	private GameObject[] boss1Balls;

 	// Start mag alleen uitgevoerd worden als dit een boss level is
	void Start () {
		if( GameStart.currentBossScene > 0){
			//We're in a boss scene!
			jetpackActivationText.alignment = TextAnchor.LowerCenter;
			outOfAmmoText.alignment = TextAnchor.LowerCenter;
			HPbar.SetActive(true);
			switch(GameStart.currentBossScene){
				case 1:
					//pogostickboss
					maxHP = 150f;
					pogoBoss = GameObject.Find("PogoBossBody").GetComponent<PogoBoss>();
					break;
				case 2:
					//gravityboss
					gravitybossinstance = GameObject.FindWithTag("Enemy").transform.root.GetComponent<gravityBoss>();
					maxHP = 100f;
					break;
				case 3:
					//grapplinghookboss
					boss1Balls = GameObject.FindGameObjectsWithTag("Boss1Balls");
					maxHP = 10f;
					break;
			}
			currentHP = maxHP;
			previousHP = maxHP;
			slider.setValueBossHealth(currentHP, maxHP);
		}
	}

	void Update () {
		switch(GameStart.currentBossScene){
			case 1:
				//pogostickboss
				if(gravitybossinstance == null){
					pogoBoss = GameObject.Find("PogoBossBody").GetComponent<PogoBoss>();
				}else{
					currentHP = 0f;
					for (int i = 0; i < 4; i++){
						currentHP += Mathf.Clamp (pogoBoss.eyeParams[i].eyeHP.getHP(), 0, 30f);
					}
					currentHP += Mathf.Clamp (pogoBoss.eyeBigParam.eyeHP.getHP(), 0, 30f);
				}
				if (currentHP < previousHP){
					slider.setValueBossHealth(currentHP, maxHP);
				}
				previousHP = currentHP;
				break;
			case 2:
				//gravityboss
				if(gravitybossinstance == null){
					GameObject.FindWithTag("Enemy").transform.root.GetComponent<gravityBoss>();
				}else{
					currentHP = gravitybossinstance.lifes;
				}
				if (currentHP < previousHP){
					slider.setValueBossHealth(currentHP, maxHP);
				}
				previousHP = currentHP;
				break;
			case 3:
				//grapplinghookboss
				currentHP = 0f;
				if(boss1Balls.Length == 0){
					boss1Balls = GameObject.FindGameObjectsWithTag("Boss1Balls");
				}else{
					foreach (GameObject ball in boss1Balls){
						if(ball.transform.parent.name.Equals ("BOSS1_prefab")){
							currentHP++;
						}
					}
				}
				if (currentHP < previousHP){
					slider.setValueBossHealth(currentHP, maxHP);
				}
				previousHP = currentHP;
				break;
		}
	}

	//	slider.setValueBossHealth(BOSSCURRENTHEALTH, BOSSMAXHEALTH) 	//Roep deze functie wanneer de boss health geupdate moet worden 
}
