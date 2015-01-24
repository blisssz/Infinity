using UnityEngine;
using System.Collections;

/// <summary>
/// Big eye FX. Specific class for the spawn effect
/// </summary>
public class BigEyeFX : MonoBehaviour {


	public float mainSize = 1.0f;
//	private float fac = 0.0f;

	private float time = 0f;
	public float totTime = 5f;

	public AnimationCurve scaleFac; // default of x (0,1) and y (0, 1)

	public bool activateFX = false;

	// Use this for initialization
	void Start () {
		foreach (Transform t in this.GetComponentsInChildren<Transform>()){
			if (t.GetInstanceID() != this.transform.GetInstanceID()){
				t.localScale = new Vector3(0, 0, 0);	
			}
		}
	}

	void Update () {
		if (activateFX){
			float scale = scaleFac.Evaluate(time/totTime)  * mainSize;
			foreach (Transform t in this.GetComponentsInChildren<Transform>()){
				if (t.GetInstanceID() != this.transform.GetInstanceID()){
					t.localScale = new Vector3(1, 1, scale);	
				}
			}

			time += Time.deltaTime;

			if (time > totTime){
				time = 0f;
				activateFX = false;
				foreach (Transform t in this.GetComponentsInChildren<Transform>()){
					if (t.GetInstanceID() != this.transform.GetInstanceID()){
						t.localScale = new Vector3(0, 0, 0);	
					}
				}
			}
		}

	
	}
}
