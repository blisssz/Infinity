using UnityEngine;
using System.Collections;

public class SwitchHelp : MonoBehaviour {
	
	public static int Switch(float[] chances){
		float currentChance = 0;
		float b = Random.Range (0, 100);
		int taken = 0;
		for (int i=0; i<chances.Length; i++) {
			currentChance=currentChance+chances[i];
			if(b<currentChance){taken=i; break;}

				}
		return taken;

	}

}
