using UnityEngine;
using System.Collections;

public class GroundDamageEffects {

	// dmg per second
	public static float lavaDamage = 10f;

	public static void doGroundDamage(GameObject self, GameObject hitobj){

		if (self.GetComponent<HPmanager>()){
			if (hitobj.renderer && hitobj.renderer.material.name.Contains("Lava")){
				self.GetComponent<HPmanager>().doDamage(lavaDamage * Time.deltaTime);
			}
		}
	}

}
