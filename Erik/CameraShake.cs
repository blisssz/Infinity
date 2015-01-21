using UnityEngine;

using System.Collections;

public class CameraShake : MonoBehaviour {




	private static Vector3 xycoordsNew;
	private static Vector3 xycoordsOld;

	private Camera[] otherCams;

	private float timer = 0.0f;
	private float shaketime = 0.1f;
	private static float decaytime = 0f;
	private static float decaytimer = 0f;

	public float amplitude = 0.15f;

	public static void shakeMainCamera(float setdecaytime = 1f){
		decaytimer = 0f;
		decaytime = setdecaytime;
	}

	// Use this for initialization
	void Start () {
		otherCams =  this.transform.parent.GetComponentsInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if (timer == 0.0f){
			xycoordsOld = xycoordsNew;
			xycoordsNew = Mathf.Max(1 - decaytimer/decaytime, 0f) * amplitude * new Vector3(Random.value-0.5f, Random.value-0.5f, 0);

		}

		timer += Time.deltaTime;
		decaytimer += Time.deltaTime;

		Vector3 dCo = (xycoordsNew - xycoordsOld);
		Vector3 co = xycoordsOld + dCo * (timer/shaketime); // normalized time

		Vector3 coInWorld = Vector3.zero;

		coInWorld = this.transform.right * co.x + this.transform.up * co.y;

		this.transform.position = this.transform.parent.position + coInWorld;

		for (int i = 0; i < otherCams.Length; i++){
			otherCams[i].transform.position = this.transform.parent.position + 0.5f*coInWorld;
		}

		if (timer > shaketime){
			timer = 0.0f;
		}

		// test shake with key: 2
		//if (KeyManager.key2 == 1){
		//	shakeMainCamera();
		//}
	}
}
