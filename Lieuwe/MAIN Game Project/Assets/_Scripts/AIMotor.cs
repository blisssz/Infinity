using UnityEngine;
using System.Collections;


[System.Serializable]
public class controlParameters{
	public float KpBaseVal = 75.0f;
	//public float Kp = ;
	public float Kd = 0f;
	public float Ki = 10.0f;
	public float KiClamp = 35.0f;

	public float v_max = 10f;
	public float walkForceMax = 2500.0f;
	
	public float Ldesired = 2f;
	public float Loffset = 0f;	// used for stickyness to surface;
	public float L0 = 2f;
	public float Lcomp = 0.3f;
	public float dampfac = 1f;

	public LayerMask ignoreLayer;
}


public class AIMotor {

	public float Kp;
	public float Kd;
	public float Ki;
	public float E;
	private float KiClamp;
	public float walkForceMax = 2500.0f;
	public float v_max;

	public float L0;
	public float Lcomp;
	public float Ldesired;
	public float Loffset;
	public float dampfac;
	public LayerMask ignoreLayer;

	private bool AIMotorEnabled = true;

	private Vector3 E_old = new Vector3(0.0f, 0.0f, 0.0f);
	private Vector3 E_integral = new Vector3(0.0f, 0.0f, 0.0f);

	private GameObject mainGameObject;

	public AIMotor(GameObject Gobj, controlParameters cp){
		if (!Gobj.transform.root.gameObject.GetComponent<Rigidbody>()){
			Gobj.transform.root.gameObject.AddComponent<Rigidbody>();
		}

		Kp = cp.KpBaseVal * Gobj.rigidbody.mass;
		Kd = 2.0f * Mathf.Sqrt (Gobj.rigidbody.mass*Kp);
		Ki = cp.Ki;
		walkForceMax = cp.walkForceMax;
		v_max = cp.v_max;
		KiClamp = cp.KiClamp;
		mainGameObject = Gobj.transform.root.gameObject;

		Ldesired = cp.Ldesired;
		L0 = cp.Ldesired + cp.Lcomp;
		Loffset = cp.Loffset;
		Lcomp = cp.Lcomp;
		dampfac = cp.dampfac;
		ignoreLayer = cp.ignoreLayer;
	}

	/// <summary>
	/// Runs the AI motor. Applies force over its transform.forward;
	/// </summary>
	/// <param name="vel">Vel.</param>
	public void runAIMotor(float vel, bool UseSpringUpForce = true){
		// control linear velocity

		if (AIMotorEnabled){
			Vector3 springUpF = Vector3.zero;
			if (UseSpringUpForce){
				springUpF = springUpForce();
			}
			Vector3 V_ref = mainGameObject.transform.localToWorldMatrix.MultiplyVector (new Vector3(0, 0, Mathf.Min(vel, v_max)));
			Vector3 E = V_ref - mainGameObject.rigidbody.velocity;
			
			E_integral += E;
			E_integral.y = 0.0f;
			E_integral = Vector3.ClampMagnitude (E_integral, KiClamp);

			Vector3 U = new Vector3(0, 0, 0);
			U = E * Kp + (E - E_old) * Kd + E_integral;
			U.y = 0.0f; // ignore applying velocity in direction of gravity
			// clamp input force
			U = Vector3.ClampMagnitude(U, walkForceMax);
			
			E_old = E;

			mainGameObject.rigidbody.AddForce(U + springUpF, ForceMode.Force);
		}
	}

	public void runAIMotor(){
	}

	private Vector3 springUpForce(){

		RaycastHit rayHit;
		if (Physics.Raycast(mainGameObject.transform.position, -mainGameObject.transform.up, out rayHit, L0+Loffset, ~ignoreLayer)){

			float k = mainGameObject.rigidbody.mass * Physics.gravity.magnitude / Lcomp;
			float cdamp = 2f*dampfac * Mathf.Sqrt(k*mainGameObject.rigidbody.mass);
			float L_compression = L0 - rayHit.distance;
			Vector3 forceUpOut = mainGameObject.transform.up * (cdamp * Vector3.Dot (mainGameObject.rigidbody.velocity, -mainGameObject.transform.up)
					+ L_compression * k);

			try {
				GroundDamageEffects gde = mainGameObject.GetComponent<GroundDamageEffects>();
				if (gde.minHeight > rayHit.distance){
					gde.groundStatus = GroundDamageEffects.doGroundDamage(mainGameObject, rayHit.transform.gameObject);
				}
			}
			catch (System.Exception e){
			}

			return forceUpOut;

		}

		else{
			return Vector3.zero;
		}
	}

	public void setDampFac(controlParameters cp){
		dampfac = cp.dampfac;
	}
	public void setDampFac(float v){
		dampfac = v;
	}
	
	public void setSpringUpParams(controlParameters cp){
		L0 = cp.L0;
		L0 = cp.Ldesired + cp.Lcomp;
		Lcomp = cp.Lcomp;
		dampfac = cp.dampfac;
	}

	public void setLdesired(float l){
		Ldesired = l;
		L0 = l + Lcomp;
	}

	public void setLcomp(float l){
		Lcomp = l;
	}

	public void floatingEffect(float baseheight, float freq, float amp){
		setLdesired(baseheight + amp * Mathf.Sin ( Time.timeSinceLevelLoad * freq));
	}

	public void enableMovement(bool b){
		AIMotorEnabled = b;
	}





}
