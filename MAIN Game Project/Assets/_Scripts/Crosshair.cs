using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	
	public bool drawCrosshair = false;
	public bool switcher = false;
	
	public Color crosshairColor = Color.white;   //The crosshair color
	
	public int lineLength = 10; // Length of the crosshair line (in pixels)
	public int lineWidth = 1; // Width of the crosshair line (in pixels)
	public int spread = 4; // Spread of the crosshair lines (in pixels)
	
	private Texture2D tex;
	private GUIStyle lineStyle;
	
	void Awake ()
	{
		tex = new Texture2D(1,1);
		SetTextureColor(tex, crosshairColor); //Set color
		lineStyle = new GUIStyle();
		lineStyle.normal.background = tex;
	}
	
	void OnGUI ()
	{
		Vector2 centerPoint = new Vector2(Screen.width/2,Screen.height/2);
		
		if(drawCrosshair){

			//boven
			GUI.Box(new Rect(centerPoint.x,
			                 centerPoint.y - lineLength - spread +1,
			                 lineWidth,
			                 lineLength), GUIContent.none, lineStyle);

			//onder
			GUI.Box(new Rect(centerPoint.x,
			                 centerPoint.y + spread,
			                 lineWidth,
			                 lineLength),GUIContent.none,lineStyle);

			//rechts
			GUI.Box(new Rect(centerPoint.x + spread,
			                 centerPoint.y,
			                 lineLength,
			                 lineWidth),GUIContent.none,lineStyle);

			//links
			GUI.Box(new Rect(centerPoint.x - spread - lineLength +1,
			                 centerPoint.y,
			                 lineLength,
			                 lineWidth), GUIContent.none, lineStyle);
		}
	}
	
	//Applies color to a texture
	void SetTextureColor(Texture2D texture, Color color){
		for (int y = 0; y < texture.height; y++){
			for (int x = 0; x < texture.width; x++){
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}

	void Update() {
		bool key1 = KeyManager.key1 == 1; // tap key once
		if (key1 == true) {
			if (switcher == false) {
				drawCrosshair = true;
				switcher = true;
			} 
			else {
				drawCrosshair = false;
				switcher = false;
			}
		}
	}
}
