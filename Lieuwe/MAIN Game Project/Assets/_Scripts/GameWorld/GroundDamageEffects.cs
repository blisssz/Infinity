using UnityEngine;
using System.Collections;

public class GroundDamageEffects {

	// dmg per second
	public static float lavaDamage = 10f;

	public static void doGroundDamage(GameObject self, GameObject hitobj){

		if (self.GetComponent<HPmanager>()){

			try{
			if (hitobj.renderer.sharedMaterial && hitobj.renderer.sharedMaterial.name.Contains("Lava")){

				self.GetComponent<HPmanager>().doDamage(lavaDamage * Time.deltaTime);
			}
			}
			catch (System.Exception e){
				return;
			}
		}
	}

}
