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

//	private Vector3 worldBoxCenter = new Vector3(0, 0, 0); //always origin

	private GameObject skyCam;
	private Camera skyboxCamera;
	private GameObject skyCam2;
	private Camera skyboxCameraSniper;
	private Camera sniperCamera;
	private bool currentWeaponSniper = false;

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
		if (PlayerManager.useWeaponID == 6) {
			currentWeaponSniper = true;
			skyCam2 = new GameObject();
			skyboxCameraSniper = skyCam2.AddComponent<Camera>() as Camera;
			skyCam2.name = "mySniperSkyBoxCamera";
			skyCam2.tag = "SkyBoxCamera";
			skyboxCameraSniper.depth = -0.5f;
			skyboxCameraSniper.clearFlags = CameraClearFlags.Skybox;
			skyboxCameraSniper.orthographic = false;
			skyboxCameraSniper.cullingMask = cullingMask;
			skyboxCameraSniper.transform.position = skyBoxCenter;
			skyboxCameraSniper.nearClipPlane = 0.001f;
			skyboxCameraSniper.farClipPlane = 3f * skyBoxSize;
			skyboxCameraSniper.rect = new Rect(0.36f, 0.36f, 0.24f, 0.28f); //X,Y,W,H
			skyboxCameraSniper.enabled = false;
			if(GameObject.Find("Sniper Camera") != null){
				sniperCamera = GameObject.Find("Sniper Camera").GetComponent<Camera>();
			}

		}

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Camera.main != null){
			// set parameters of the main camera to the skybox camera
			skyboxCamera.transform.rotation = Camera.main.transform.rotation;

			//float lolSize = skyBoxSize * (1f + Mathf.Sin (Time.time)*0.8f);

			skyboxCamera.transform.position = skyBoxCenter + Camera.main.transform.position/skyBoxSize;

			skyboxCamera.fieldOfView = Camera.main.fieldOfView;

			if(currentWeaponSniper == true && sniperCamera != null){
				if(sniperCamera.enabled == true){
					skyboxCameraSniper.enabled = true;
					skyboxCameraSniper.transform.rotation = sniperCamera.transform.rotation;
					skyboxCameraSniper.transform.position = skyBoxCenter + sniperCamera.transform.position/skyBoxSize;
					skyboxCameraSniper.fieldOfView = sniperCamera.fieldOfView;
					if(Sniper.shooting == true){
						skyboxCameraSniper.rect = new Rect(0.33f, 0.36f, 0.27f, 0.28f);
						sniperCamera.rect = new Rect(0.33f, 0.36f, 0.27f, 0.28f);
					} else{
						skyboxCameraSniper.rect = new Rect(0.36f, 0.36f, 0.24f, 0.28f);
						sniperCamera.rect = new Rect(0.36f, 0.36f, 0.24f, 0.28f);
					}
				}
				else{
					skyboxCameraSniper.enabled = false;
				}
			}
			else if(currentWeaponSniper == true && sniperCamera == null){
				sniperCamera = GameObject.Find("Sniper Camera").GetComponent<Camera>();
				skyboxCameraSniper.enabled = false;
			}
		}
	}
}
