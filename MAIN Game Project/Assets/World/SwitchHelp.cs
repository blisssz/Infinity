using UnityEngine;
using System.Collections;

public class SwitchHelp : MonoBehaviour {
	
	public static int Switch(float[] chances){
		int currentChance = 0;
		float b = Random.Range (0, 100);
		for (int i=0; i<chances.Length; i++) {
			if(b<chances[i]){currentChance=i; break;}

				}
		return currentChance;

	}

}
