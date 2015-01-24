using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AudioList  {

	public static List<AudioSource> SFXList;
	public static List<AudioSource> BackGroundList;
	public static List<float> SFXVolumes;
	public static List<float> BackGroundVolumes;
	public static MonoBehaviour Dummy;
	public static float VolumeFactorBG;
	public static float VolumeFactorSFX;

	// Use this for initialization
	public static void StartX () {
		GameObject S=new GameObject();
		Dummy=S.AddComponent<Empty>();
		SFXList=new List<AudioSource>();
		BackGroundList=new List<AudioSource>();
		SFXVolumes=new List<float>();
		BackGroundVolumes=new List<float>();
		VolumeFactorBG=1f;
		VolumeFactorSFX=1f;
	}

	public static void VolumeBackground(float factor){
		for(int i=0;i<BackGroundList.Count;i++){
			BackGroundList[i].volume=BackGroundVolumes[i]*factor;
		}
		VolumeFactorBG=factor;
	}

	public static void VolumeSFX(float factor){
		for(int i=0;i<SFXList.Count;i++){
			SFXList[i].volume=SFXVolumes[i]*factor;
		}
		VolumeFactorSFX=factor;
	}

	public static int Add(AudioSource X, bool Background){
		if(Background){
			if(BackGroundList==null){
				StartX ();
			}
			BackGroundList.Add (X);
			BackGroundVolumes.Add (X.volume);
			X.volume=X.volume*VolumeFactorBG;
			return BackGroundList.Count-1;
		}
		SFXList.Add (X);
		SFXVolumes.Add (X.volume);
		X.volume=X.volume*VolumeFactorSFX;
		if(!X.loop){
			//Dummy.StartCoroutine(AudioList.RemoveAfter(SFXList.Count-1));
		}
		return SFXList.Count-1;
	}
	// Update is called once per frame
	static IEnumerator RemoveAfter(int index) {
		yield return new WaitForSeconds(SFXList[index].time);
		Remove (index);
	}

	public static void Remove(int Index){
		SFXList.RemoveAt (Index);
	}

}
