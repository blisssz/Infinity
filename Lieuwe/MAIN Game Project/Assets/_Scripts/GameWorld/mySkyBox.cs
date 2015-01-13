using UnityEngine;
using System.Collections;

/// <summary>
/// My sky box >>> Unity skybox;
/// Author: Erik Veldhuis
/// </summary>
public class mySkyBox : MonoBehaviour {

	private Vector3 skyBoxCenter;
	public float skyBoxSize; 			// square box for convinience, this value is no visual box
	//public float skyBoxScaleFac = 1f;

	private Vector3 worldBoxCenter = new Vector3(0, 0, 0); //always origin

	private GameObject skyCam;
	private Camera skyboxCamera;

	public LayerMask cullingMask;
	private Camera mainCamera;

	private Transform skyDomeOrBox;



	// Use this for initialization
	void Start () {

		skyBoxCenter = this.transform.position;

		skyCam = new GameObject();
		skyboxCamera = skyCam.AddComponent<Camera>() as Camera;
		skyCam.name = "mySkyBoxCamera";
		skyCam.tag = "SkyBoxCamera";

		skyboxCamera.depth = -10;
		skyboxCamera.clearFlags = CameraClearFlags.Skybox;
		skyboxCamera.orthographic = false;
		skyboxCamera.cullingMask = cullingMask;
		skyboxCamera.transform.position = skyBoxCenter;
		skyboxCamera.nearClipPlane = 0.001f;
		skyboxCamera.farClipPlane = 3f * skyBoxSize;

		//camCam.cu

		//cam.transform.position = new Vector3(1,1,1);

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Camera.main != null){
			// set parameters of the main camera to the skybox camera
			skyboxCamera.transform.rotation = Camera.main.transform.rotation;

			//float lolSize = skyBoxSize * (1f + Mathf.Sin (Time.time)*0.8f);

			skyboxCamera.transform.position = skyBoxCenter + Camera.main.transform.position/skyBoxSize;

			skyboxCamera.fieldOfView = Camera.main.fieldOfView;
		}
	}
}
