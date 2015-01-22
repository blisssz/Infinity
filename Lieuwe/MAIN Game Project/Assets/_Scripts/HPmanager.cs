using UnityEngine;
using System.Collections;

public class HPmanager : MonoBehaviour {

	public float hp = 100f;
	public float maxhp = 100f;
	public float dmgModifier = 1.0f;
	private GameObject sliderFill;

	private float velMinTreshold = 0f;
	private float velMaxTreshold = 100f;
	private float TimeOfLastHit;
	public AudioClip X;
	public AudioSource Sound;


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
		float DeltaHp=0;
		if (all){
			DeltaHp=- dmg * dmgModifier;
			if (vel.magnitude > min){
				DeltaHp -= Mathf.Min(vel.magnitude, max) * dmgModifier;
			}
		}

		else{
			if (!velBasedDmg){
				DeltaHp =- dmg * dmgModifier;
			}
			else if (vel.magnitude > min){
				DeltaHp =- Mathf.Min(vel.magnitude, max) * dmgModifier;
			}

		}
		hp+=DeltaHp;
		if(hp>maxhp){hp=maxhp;}
		UpdateSlider (DeltaHp);

	}

	/// <summary>
	/// Do the dmg based on this(min max parameters)
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	/// <param name="vel">Vel.</param>
	/// <param name="all">If set to <c>true</c> ignores dmg on velocity OR dmg function</param>
	public void doDamage(float dmg, Vector3 vel, bool all){
		float DeltaHp=0;
		if (all){
			DeltaHp =- dmg * dmgModifier;

		}
		else{
			if (!velBasedDmg){
				DeltaHp =- dmg * dmgModifier;
			}
			else{
				doDamage(vel, velMinTreshold, velMaxTreshold);
			}
		}
		if(hp>maxhp){hp=maxhp;}
		UpdateSlider (DeltaHp);
	}


	public void doDamage(float dmg){
		float DeltaHp=0;
		if (!velBasedDmg){
			DeltaHp = -dmg * dmgModifier;
			hp +=DeltaHp;
		}
		if(hp>maxhp){hp=maxhp;}
		UpdateSlider (DeltaHp);
	}

	/// <summary>
	/// Velocity based dmg
	/// </summary>
	/// <param name="vel">Vel.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	public void doDamage(Vector3 vel, float min, float max){
		float DeltaHp=0;
		if (vel.magnitude > min){
			DeltaHp=-Mathf.Min(vel.magnitude, max) * dmgModifier;
			hp +=DeltaHp;
		}
		if(hp>maxhp){hp=maxhp;}
		UpdateSlider (DeltaHp);
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

	public void setMaxHP(float newHP){
		maxhp = newHP;
	} 


	// Use this for initialization
	void Start () {
		if(hp>maxhp){hp=maxhp;}
		UpdateSlider (0);
		Sound=this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	public void UpdateSlider (float DeltaHp) {
		if(this.tagS.Equals ("Player")){
			if(sliderFill==null){
				sliderFill = GameObject.FindWithTag ("Health");}
			sliderFill.GetComponent<SlidingBar> ().setValueFade (hp, 100f, false);
			if(TimeOfLastHit+1<Time.realtimeSinceStartup&&DeltaHp<0){
				TimeOfLastHit=Time.realtimeSinceStartup;
				GameObject Player=this.gameObject;

				Player.GetComponent<PlayerManager>().Sound.pitch=HelpScript.Rand(0.8f,1.2f);
				Player.GetComponent<PlayerManager>().Sound.Play();

			}
		}

		if(this.tagS.Equals ("Enemy")||this.tagS.Equals ("EnemyHead")&&DeltaHp<0){
			if(Sound==null){
				Sound=this.GetComponent<AudioSource>();
			}
			Sound.pitch=HelpScript.Rand(0.8f,1.2f);
			Sound.Play();
		}
	}
}
