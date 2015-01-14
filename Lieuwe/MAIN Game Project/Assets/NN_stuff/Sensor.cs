using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SensorProximity {

	public Transform transform;
	public float[] sensorValues;
	public float proxValue;
	public float colValue;

	public float proxMaxRange = 3f;
	public float collisionRange = 0.5f;

	private bool invertSensor = false;
	private bool invertProx = false;

	public bool sensorActive = true;

	public float angle;


	public static SensorProximity[] createSensors(Transform mainBody, int amount, float angle, float proxRange = 3f, float colRange = 1f,float offset = 1f){
		List<SensorProximity> sList = new List<SensorProximity>();

		if (amount < 1){
			amount = 1;
		}

		if (angle > 270f){
			angle = 270f;
		}
		else if (angle < 0f){
			angle = 0f;
		}

		float theta  = 0f;

		// always add one sensor in the forward direction
		GameObject sObj = new GameObject();		
		sObj.name = "SensorProximity";		
		sObj.transform.position = mainBody.position;
		sObj.transform.rotation = mainBody.rotation;
		sObj.transform.parent = mainBody;
		sObj.transform.position = mainBody.position + sObj.transform.forward * offset;

		SensorProximity sens = new SensorProximity(proxRange, colRange, sObj.transform);
		sens.angle = theta;
		sList.Add(sens);

		float dAngle = angle / (float) (amount-1);

		// (int division in amount/2 gets floored)
		for (int i = 0; i < amount/2; i++){
			theta += dAngle;
			for (int j = 0; j < 2; j++){

				// positive rotation angle in Transform.Rotate() == clockwise
				float mirror = ( j == 0) ? 1f : -1f;

				sObj = new GameObject();		// 

				sObj.name = "SensorProximity"+i+j;

				sObj.transform.position = mainBody.position;
				sObj.transform.rotation = mainBody.rotation;
				sObj.transform.parent = mainBody;

				sObj.transform.Rotate(mainBody.up, theta * mirror);
				sObj.transform.position = mainBody.position + sObj.transform.forward * offset;

				// add to list
				sens = new SensorProximity(proxRange, colRange, sObj.transform);
				sens.angle = theta*mirror;
				sList.Add(sens);

			}
		}
		return sList.ToArray();
	}



	public SensorProximity(){
		sensorValues = new float[] {0f, 0f};
	}

	public SensorProximity(float proxRange, float colRange){
		proxMaxRange = proxRange;
		collisionRange = colRange;
		sensorValues = new float[] {0f, 0f};
	}

	public SensorProximity(float proxRange, float colRange, Transform t){
		proxMaxRange = proxRange;
		collisionRange = colRange;
		transform = t;
		sensorValues = new float[] {0f, 0f};
	}

	public void setInvertSensor(bool b){
		invertSensor = b;
	}

	public void setInvertProximity(bool b){
		invertProx = b;
	}

	public void setAngle(Transform mainBody){
		//angle = Mathf.Acos(mainBody.forward.Dot)
	}

	/// <summary>
	/// Runs the sensor.
	/// </summary>
	/// <returns>The sensor.</returns>
	public float[] runSensor(){

		sensorValues = new float[] {0f, 0f};

		if (sensorActive){
			activateSensor();
		}

		return sensorValues;
	}

	private void activateSensor(){
		RaycastHit rayHit;
		
		if (Physics.Raycast(transform.position, transform.forward, out rayHit, proxMaxRange)){
			sensorValues[0] = (invertProx) ? rayHit.distance/proxMaxRange : 1 - rayHit.distance/proxMaxRange;
			sensorValues[1] = (rayHit.distance > collisionRange) ? 0f : 1f;			
		}

	}

	public void addProxRange(float deltaRange){
		proxMaxRange += deltaRange;
	}

	public void forgetProxRange(float ffac){
		proxMaxRange *= (1 - ffac);
		if (proxMaxRange < collisionRange){
			proxMaxRange = collisionRange;
		}
	}

	public float[] getValues(){
		return sensorValues;
	}

	public float getProxValue(){
		return sensorValues[0];
	}

	public float getColValue(){
		return sensorValues[1];
	}

	public string ToString(){
		string output = "";

		output += "Sensor: " + transform.name + ", id: "+ transform.GetInstanceID() + " sensors values: <(" + sensorValues[0] + ","  +sensorValues[1]+ ")>";

		return output;
	}





}
