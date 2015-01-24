using UnityEngine;
using System.Collections;

public class AddAudio : MonoBehaviour {

	public bool BackGround;

	// Use this for initialization
	void Start () {
		AudioSource[] List=this.GetComponents<AudioSource>();
		for(int i=0;i<List.Length;i++){
			AudioList.Add (List[i],BackGround);
				}
	
	}

}
