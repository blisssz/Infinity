using UnityEngine;
using System.Collections;

[System.Serializable]
public class RopeParams {

	public int iterations = 60;

	// default rope parameters
	public float ropeSegmentLength = 2.0f;
	public float ropeSegmentMass = 6f;
	public float ropeSphereRadius = 1.0f;
	
	public bool useCriticalDamping = true;
	public bool noSpringyRopes = true;
	public float clampTreshhold = 0.10f;
	
	public bool useBoxCollider = true;
	public float boxStretchFac = 1.2f;
	
	public float k_spring = 20000.0f;
	public float c_damp = 1000.0f;

	public int ropeVertices = 6;
	public float ropeRadius = 0.1f;

}
