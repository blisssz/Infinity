using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {


	public Vector2 translate1amount = new Vector2 (0.3f, 0.3f);
	public Vector2 translate2amount = new Vector2 (-0.3f, 0.3f);
	private Vector2 translated1;
	private Vector2 translated2;

	public float translateFac = 1f;

	public Vector2 mixFactorRange = new Vector2(1,6);
	public float mixFreq = 2f;

	// Use this for initialization
	void Start () {
		translated1 = new Vector2(0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		translateLava();
		applyMixFactor();
	
	}

	private void translateLava(){
		translated1 += translate1amount * translateFac;
		this.renderer.sharedMaterial.SetFloat("_tr1x", translated1.x);
		this.renderer.sharedMaterial.SetFloat("_tr1y", translated1.y);

		translated2 += translate2amount*translateFac;
		this.renderer.sharedMaterial.SetFloat("_tr2x", translated2.x);
		this.renderer.sharedMaterial.SetFloat("_tr2y", translated2.y);
	}

	private void applyMixFactor(){
		float mix = mixFactorRange.x+(mixFactorRange.y - mixFactorRange.x)/2f + (mixFactorRange.y - mixFactorRange.x)/2f * Mathf.Sin(mixFreq * Time.time);
		this.renderer.sharedMaterial.SetFloat("_fac",  mix);
	}

}
