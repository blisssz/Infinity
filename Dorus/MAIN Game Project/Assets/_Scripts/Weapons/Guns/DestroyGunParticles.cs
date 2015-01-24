using UnityEngine;

public class DestroyGunParticles : MonoBehaviour {
	
	void LateUpdate () {
		if (!particleSystem.IsAlive())
			Destroy(this.gameObject);	
	}
}
