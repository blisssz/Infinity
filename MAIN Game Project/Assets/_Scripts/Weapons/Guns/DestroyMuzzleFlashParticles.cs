using UnityEngine;

public class DestroyMuzzleFlashParticles : MonoBehaviour {

	private Gun gun;

	void LateUpdate () {
		if (!particleSystem.IsAlive()){
			gun.DeleteGameObject(gameObject);
			Destroy(this.gameObject);	
		}
	}

	public void setGun(Gun setgun){
		this.gun = setgun;
	}
}
