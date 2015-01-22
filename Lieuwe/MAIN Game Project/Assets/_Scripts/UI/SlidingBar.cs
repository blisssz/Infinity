using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlidingBar : MonoBehaviour {
	
	//	public GameObject percentageText;
	public Text digit;
	private RectTransform rectTransform;
	private Image image;
	private float percentage;
	private Color32 startingColor;
	private float OldValue;
	
	// Use this for initialization
	void Awake () {
		OldValue=100f;
		rectTransform = this.GetComponent<RectTransform>();
		image = this.GetComponent<Image>();
		startingColor = image.color;
	}
	
	// Update is called once per frame
	
	//If you change this name you should also change the name everywhere in HPmanager!
	public void setValueFade(float newValue, float maxValue, bool staticColor){
		percentage = newValue / maxValue;
		rectTransform.anchorMax = new Vector2(percentage, rectTransform.anchorMax.y);
		setIntValue (percentage * 100f);
		if(OldValue>newValue){
		Vignetting.PlayerHit ();
		}
		if (staticColor == false) {
			if(percentage < 0.5f){
				image.color = new Color32(255, (byte)MapValues(newValue, 0f, (maxValue / 2f), 0f, 255f), 0, 255);
			}else{
				image.color = new Color32((byte)MapValues(newValue, (maxValue / 2f), maxValue, 255f, 0f), 255, 0, 255);
			}
		}
		Vignetting.FadeTheAlpha(percentage);
		OldValue=newValue;
	}
	
	public void setValueBossHealth(float newValue, float maxValue){
		percentage = newValue / maxValue;
		rectTransform.anchorMax = new Vector2(percentage, rectTransform.anchorMax.y);
		if(percentage < 0.5f){
			image.color = new Color32(255, (byte)MapValues(newValue, 0f, (maxValue / 2f), 0f, 255f), 0, 255);
		}else{
			image.color = new Color32((byte)MapValues(newValue, (maxValue / 2f), maxValue, 255f, 0f), 255, 0, 255);
		}
	}
	
	public void setValueJetpack(float newValue, float maxValue, bool staticColor){
		percentage = newValue / maxValue;
		rectTransform.anchorMax = new Vector2(percentage, rectTransform.anchorMax.y);
		setIntValue (percentage * 100f);
		if (staticColor == false) {
			if(percentage < (1f/3f)){
				image.color = new Color32(
					(byte)MapValues(newValue, 0f, (maxValue/3f), 255, startingColor.r),	//red
					(byte)MapValues(newValue, 0f, (maxValue/3f), 0, startingColor.g),	//green
					(byte)MapValues(newValue, 0f, (maxValue/3f), 0, startingColor.b),	//blue
					startingColor.a);
			}
		}
	}
	
	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
	
	private void setIntValue(float value){
		char[] values = ((Mathf.RoundToInt (value)).ToString()).ToCharArray();
//		Debug.Log ("char array length: " + values.Length);
		switch(values.Length){
		case 1:
			if(values[0].ToString().Equals ("0")){
				digit.text = "<color=#B4B4B464>" + "000" + "</color>";
			}else{
				digit.text = "<color=#B4B4B464>" + "00" + "</color>" + "<color=#ffffffff>" + values[0].ToString() + "</color>";
			}
			break;
			
		case 2:
			digit.text = "<color=#B4B4B464>" + "0" + "</color>" + "<color=#ffffffff>" + values[0].ToString() + values[1].ToString() + "</color>";
			break;
			
		case 3:
			digit.text = "<color=#ffffffff>" + values[0].ToString() + values[1].ToString() + values[2].ToString() + "</color>";
			break;
			
		default:
			Debug.Log ("Something went wrong if you got here. The string is somehow 0 characters or 4 or more characters in length");
			break;
		}
	}
	
}
