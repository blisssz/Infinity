using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextHighlight : MonoBehaviour {

	protected Text text;
	public Color colorNormal;
	public Color colorHighlighted;

	public AudioClip soundHover;

	// Use this for initialization
	protected virtual void Awake () {
		text = this.GetComponent<Text> ();
		text.color = colorNormal;
	}

	protected virtual void OnDisable () {
		text.color = colorNormal;
	}

	public virtual void color(bool highlighted){
		if(highlighted == true){
			text.color = colorHighlighted;
			playAudio(soundHover);

		}else{
			text.color = colorNormal;
		}
	}

	protected void playAudio (AudioClip clip){
		Time.timeScale = 1f;
		AudioSource.PlayClipAtPoint(clip, new Vector3(0,0,0));
		Time.timeScale = 0f;
	}
	
	protected void playAudio (AudioClip clip, float volume){
		Time.timeScale = 1f;
		AudioSource.PlayClipAtPoint(clip, new Vector3(0,0,0), volume);
		Time.timeScale = 0f;
	}
}
