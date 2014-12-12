using UnityEngine;
using System.Collections;

public class PlatformMotion : MonoBehaviour {

	public enum PlatformType {Translation = 0, Rotation = 1, TransAndRotation = 3, Goniometric = 4};

	public PlatformType pltf = PlatformType.Translation;
	public float Amplitude = 10.0f;

	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//if (GetComponent<Rigidbody>()){
		//	rigidbody.velocity = Amplitude * Mathf.Sin(Time.time) *  new Vector3(1.0f, 0.0f, 0.0f);
		//	//srigidbody.angularVelocity = new Vector3(0.0f, 1.0f, 0.0f);
		//}
		//else{
			transform.position = initialPosition + Amplitude * Mathf.Sin(Time.time) * new Vector3(1.0f, 0.0f, 0.0f);//initialPosition +  Amplitude * (Time.time) * new Vector3(1.0f, 0.0f, 0.0f);
			//transform.Rotate (new Vector3(0.0f, 1.0f, 0.0f));
		//}
	}
}
