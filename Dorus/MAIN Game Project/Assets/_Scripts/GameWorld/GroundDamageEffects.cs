using UnityEngine;
using System.Collections;


public enum GroundFXstatus  {KilledBy, Damaging, Nothing};

public class GroundDamageEffects : MonoBehaviour {
	
	//parameters for attached component
	public GroundFXstatus groundStatus = GroundFXstatus.Nothing;
	public float minHeight = 1f;
	
	
	// Global parameters
	// dmg per second
	public static float lavaDamage = 10f;
	
	
	//public enum GroundFXstatus  {KilledBy, Damaging, Nothing};
	
	public static GroundFXstatus doGroundDamage(GameObject self, GameObject hitobj){
		
		if (self.GetComponent<HPmanager>()){
			if (hitobj.GetComponent<Renderer>() && hitobj.renderer.sharedMaterial.name.Contains("Lava")){
				self.GetComponent<HPmanager>().doDamage(lavaDamage * Time.deltaTime);
				if (self.GetComponent<HPmanager>().hp <= 0){
					return GroundFXstatus.KilledBy;
				}
				else{
					return GroundFXstatus.Damaging;
				}
			}
		}
		
		return GroundFXstatus.Nothing;
	}
	
	public static void doGroundDamage(GameObject self, GameObject hitobj, Vector3 hitLoc){
		
		if (self.GetComponent<HPmanager>()){
			if (hitobj.renderer && hitobj.renderer.material.name.Contains("Lava")){
				self.GetComponent<HPmanager>().doDamage(lavaDamage * Time.deltaTime);
			}
		}
	}
	
}
