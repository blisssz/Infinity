using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SoundObjectParameters{
	public GameObject soundObject;
	public AudioClip audioClip;
	public bool mute = false;
	public bool playOnAwake = true;
	public bool loop = false;

	[Range(0,1)]
	public float volume = 1f;
	
	[Range(-3,3)]
	public float maxpitch = 1f;
	[Range(-3,3)]
	public float minpitch = 1f;

	public AudioRolloffMode rolloff = AudioRolloffMode.Logarithmic;
	public float minDistance = 1f;
	public float maxDistance = 500f;

	[Range(0, 5)]
	public float dopplerLevel = 1f;

	public string soundTag = "tag";

	public  SoundObjectParameters(){
		maxpitch = 1f;
		minpitch = 1f;
		minDistance = 1f;
		volume = 1f;
		playOnAwake = true;

		minDistance = 1f;
		maxDistance = 500f;
		dopplerLevel = 1f;


	}

}


public class SoundObjectSpawner : MonoBehaviour {

	public SoundObjectParameters[] audioParams;

	public Dictionary<string, int> idDict = new Dictionary<string, int>();


	// Use this for initialization
	void Start () {
		for (int i =0; i < audioParams.Length; i++){
			if (idDict.ContainsKey(audioParams[i].soundTag)){
				idDict.Add (audioParams[i].soundTag+i, i);
				audioParams[i].soundTag = audioParams[i].soundTag+i;
			}
			else{
				idDict.Add (audioParams[i].soundTag, i);
			}
		}
	}


	public void InstantiateAllSound(Vector3 pos, Quaternion q, float t = 1f){

		//GameObject g = GameObject.Instantiate(SoundGameObjects[0], pos, q) as GameObject;
		//Destroy (g, t);
		for (int i = 0; i < audioParams.Length; i++){
			GameObject g = createSoundObject(audioParams[i], pos);
			Destroy (g, g.audio.clip.length);		
		}

	}
	public void InstantiateRandomSound(Vector3 pos, Quaternion q, float t = 1f, float minpitch = 1f, float maxpitch = 1f){
	
		int index = Random.Range(0, audioParams.Length-1);
		GameObject g = createSoundObject(audioParams[index], pos);
		Destroy (g, g.audio.clip.length);		
	}

	public void InstantiateSound(Vector3 pos, string soundTag, float t = 1f){

		try{
			GameObject g = createSoundObject(audioParams[idDict[soundTag]], pos);
			Destroy (g, g.audio.clip.length);
		}
		catch (System.Exception e){
			Debug.Log ("tag: <|" + soundTag + "|> does not exist");
			Debug.Log (e);
		}
	}
	
	public void oneSound(){
		audio.PlayOneShot(audio.clip);
	}

	public void setAudioParams(SoundObjectParameters soundParams){

	}

	private GameObject createSoundObject(SoundObjectParameters audioParams, Vector3 pos){
		GameObject g;
		//SoundObjectParameters audioParamsSingle = audioParams;
		
		
		if (audioParams.soundObject == null){
			g = new GameObject();
			g.transform.position = pos;
			g.AddComponent<AudioSource>();
		}
		else{
			g = GameObject.Instantiate(audioParams.soundObject, pos, Quaternion.identity) as GameObject;
		}
		
		if (audioParams.audioClip != null){
			g.audio.clip = audioParams.audioClip;
		}
		
		g.audio.playOnAwake = audioParams.playOnAwake;
		g.audio.loop = audioParams.loop;
		g.audio.pitch = Random.Range (audioParams.minpitch, audioParams.maxpitch);
		g.audio.rolloffMode = audioParams.rolloff;
		g.audio.volume = audioParams.volume;
		g.audio.minDistance = audioParams.minDistance;
		g.audio.maxDistance = audioParams.maxDistance;
		g.audio.dopplerLevel = audioParams.dopplerLevel;
		if (audioParams.playOnAwake){
			g.audio.Play();
		}
		return g;
	}



}
