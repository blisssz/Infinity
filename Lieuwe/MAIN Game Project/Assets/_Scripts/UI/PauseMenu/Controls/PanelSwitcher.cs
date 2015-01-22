using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PanelSwitcher : MonoBehaviour {

	public AudioClip soundClick;

	public GameObject generalPanel;
	public GameObject pogostickPanel;
	public GameObject blackHoleGunPanel;
	public GameObject GrapplingHookPanel;
	public GameObject HandGunPanel;
	public GameObject SMGPanel;
	public GameObject SniperPanel;

	public Text dropdown1;
	public Text dropdown2;
	public Text dropdown3;
	public Text dropdown4;
	public Text dropdown5;

	private int panelId;
	private Dictionary<int, GameObject> panels;
	private Dictionary<int, string> panelNames;
	private int currentWeaponId;
	private int[] otherWeaponIds = new int[5];

	private bool weaponsSetCorrectly = false;

	// Use this for initialization
	void Start () {
		panelId = 0;
		panels = new Dictionary<int, GameObject>();
		panels.Add (0, generalPanel);
		panels.Add (1, pogostickPanel);
		panels.Add (2, blackHoleGunPanel);
		panels.Add (3, GrapplingHookPanel);
		panels.Add (4, HandGunPanel);
		panels.Add (5, SMGPanel);
		panels.Add (6, SniperPanel);
		panels [0].SetActive (true);
		panelNames = new Dictionary<int, string> ();
		panelNames.Add (1, "POGOSTICK");
		panelNames.Add (2, "BLACK HOLE GUN");
		panelNames.Add (3, "GRAPPLING HOOK");
		panelNames.Add (4, "HANDGUN");
		panelNames.Add (5, "SMG");
		panelNames.Add (6, "SNIPER RIFLE");
		if(PlayerManager.useWeaponID != 0){
			currentWeaponId = PlayerManager.useWeaponID;
			updateOtherWeaponsPanel();
		}
	}

	public void updateOtherWeaponsPanel(){
		if (weaponsSetCorrectly == false) {
			if(PlayerManager.useWeaponID != 0){
				currentWeaponId = PlayerManager.useWeaponID;
			}

			int currentWeaponIdCopy;
			if(currentWeaponId != 0){
				currentWeaponIdCopy = currentWeaponId;
				weaponsSetCorrectly = true;
			}else{
				currentWeaponIdCopy = 1;
			}

			int currentWeaponFound = 0;
			for(int i = 0; i < 6; i++){
				if((i + 1) != currentWeaponIdCopy){
					otherWeaponIds[i - currentWeaponFound] = i + 1;
				}else{
					currentWeaponFound = 1;
				}
			}

			dropdown1.text = panelNames[otherWeaponIds[0]];
			dropdown2.text = panelNames[otherWeaponIds[1]];
			dropdown3.text = panelNames[otherWeaponIds[2]];
			dropdown4.text = panelNames[otherWeaponIds[3]];
			dropdown5.text = panelNames[otherWeaponIds[4]];
		}
	}

	public void setCurrentWeaponPanel(){
		if(currentWeaponId ==0){
			if(PlayerManager.useWeaponID != 0){
				currentWeaponId = PlayerManager.useWeaponID;
				setPanel (currentWeaponId);
			}else{
				setPanel (1);
			}
		}else{
			setPanel (currentWeaponId);
		}
	}

	public void setOtherWeaponPanel(int dropdownId){
		setPanel (otherWeaponIds [dropdownId]);
	}
	
	public void setPanel(int id) {
		playAudio (soundClick, 0.5f);
		int previousPanelId = panelId;
		panelId = id;
		panels [previousPanelId].SetActive (false);
		panels [id].SetActive (true);
	}

	private void playAudio (AudioClip clip, float volume){
		Time.timeScale = 1f;
		AudioSource.PlayClipAtPoint(clip, new Vector3(0,0,0), volume);
		Time.timeScale = 0f;
	}
}
