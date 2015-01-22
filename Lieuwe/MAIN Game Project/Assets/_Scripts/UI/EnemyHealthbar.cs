using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour {

	public RectTransform canvas;
//	private HPmanager hp;

	// Use this for initialization
	void Start () {
		canvas = this.GetComponent<RectTransform> ();
//		hp = this.GetComponent<HPmanager> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		activate ();
	}

	public void activate(){
		canvas.LookAt (Camera.main.transform.position);
	}
}
