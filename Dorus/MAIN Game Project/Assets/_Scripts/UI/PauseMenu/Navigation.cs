using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Navigation : MonoBehaviour {

	public Text displayText;

	public int currentSetting;
	public int maxSetting;
	
	public AudioClip soundClick;

	public void setText (string text){
		displayText.text = text;	
	}

	public virtual void changeSetting (bool up){
		if(up == true){
			if(currentSetting != maxSetting){
				playAudio (soundClick, 0.5f);
			}
			currentSetting++;
		}
		else{
			if(currentSetting != 0){
				playAudio (soundClick, 0.5f);
			}
			currentSetting--;
		}
		currentSetting = Mathf.Clamp (currentSetting, 0, maxSetting);
	}

	private void playAudio (AudioClip clip){
		Time.timeScale = 1f;
		AudioSource.PlayClipAtPoint(clip, new Vector3(0,0,0));
		Time.timeScale = 0f;
	}

	private void playAudio (AudioClip clip, float volume){
		Time.timeScale = 1f;
		AudioSource.PlayClipAtPoint(clip, new Vector3(0,0,0), volume);
		Time.timeScale = 0f;
	}
}
