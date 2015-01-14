using UnityEngine;
using System.Collections;

public class HPmanager : MonoBehaviour {

	public float hp = 100f;
	public float maxhp = 100f;
	public float dmgModifier = 1.0f;
	private GameObject sliderFill;

	private float velMinTreshold = 0f;
	private float velMaxTreshold = 100f;


	public bool velBasedDmg = false;

	/// <summary>
	/// Do damage based on dmg function or on velocity or force to use both dmg types!
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	/// <param name="vel">Vel.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	/// <param name="all">If set to <c>true</c> all.</param>
	public void doDamage(float dmg, Vector3 vel, float min, float max, bool all){
		if (all){
			hp -= dmg * dmgModifier;
			if (vel.magnitude > min){
				hp -= Mathf.Min(vel.magnitude, max) * dmgModifier;
			}
		}

		else{
			if (!velBasedDmg){
				hp -= dmg * dmgModifier;
			}
			else if (vel.magnitude > min){
				hp -= Mathf.Min(vel.magnitude, max) * dmgModifier;
			}

		}
		if(hp>maxhp){hp=maxhp;}
		if(this.tag.Equals ("Player")){
			if(sliderFill==null){
				sliderFill = GameObject.FindWithTag ("Health");}
			sliderFill.GetComponent<SlidingBar> ().setValueFade (hp, 100f, false);
		}

	}

	/// <summary>
	/// Do the dmg based on this(min max parameters)
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	/// <param name="vel">Vel.</param>
	/// <param name="all">If set to <c>true</c> ignores dmg on velocity OR dmg function</param>
	public void doDamage(float dmg, Vector3 vel, bool all){
		if (all){
			hp -= dmg * dmgModifier;

		}
		else{
			if (!velBasedDmg){
				hp -= dmg * dmgModifier;
			}
			else{
				doDamage(vel, velMinTreshold, velMaxTreshold);
			}
		}
		if(hp>maxhp){hp=maxhp;}
		if(this.tag.Equals ("Player")){
			if(sliderFill==null){
				sliderFill = GameObject.FindWithTag ("Health");}
			sliderFill.GetComponent<SlidingBar> ().setValueFade (hp, 100f, false);
		}
	}


	public void doDamage(float dmg){
		if (!velBasedDmg){
			hp -= dmg * dmgModifier;
		}
		if(hp>maxhp){hp=maxhp;}
		if(this.tag.Equals ("Player")){
			if(sliderFill==null){
				sliderFill = GameObject.FindWithTag ("Health");}
			sliderFill.GetComponent<SlidingBar> ().setValueFade (hp, 100f, false);
		}
	}

	/// <summary>
	/// Velocity based dmg
	/// </summary>
	/// <param name="vel">Vel.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	public void doDamage(Vector3 vel, float min, float max){
		if (vel.magnitude > min){
			hp -= Mathf.Min(vel.magnitude, max) * dmgModifier;
		}
		if(hp>maxhp){hp=maxhp;}
		if(this.tag.Equals ("Player")){
			if(sliderFill==null){
				sliderFill = GameObject.FindWithTag ("Health");}
			sliderFill.GetComponent<SlidingBar> ().setValueFade (hp, 100f, false);
		}
	}


	public float getHP(){
		return hp;
	}

	public void setHP(float newHP){
		hp = newHP;
	}

	public void setMinMaxVel(float min, float max){
		velMinTreshold = min;
		velMaxTreshold = max;
	}


	// Use this for initialization
	void Start () {
		if(hp>maxhp){hp=maxhp;}
		if(this.tag.Equals ("Player")){
			if(sliderFill==null){
				sliderFill = GameObject.FindWithTag ("Health");}
			sliderFill.GetComponent<SlidingBar> ().setValueFade (hp, 100f, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
