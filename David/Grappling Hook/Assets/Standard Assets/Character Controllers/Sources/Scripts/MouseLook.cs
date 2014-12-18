using UnityEngine;
using System.Collections;

/* Mouse look class
 * 
 * Attach this script to the Main Parent.
 * - The parent must have a child object "Head".
 * 
 * 
 */

public class MouseLook : MonoBehaviour {

	public float sensitivityX = 2.5f;
	public float sensitivityY = 2.5f;

	public float minimumY = -60.0f;
	public float maximumY = 60.0f;

	private GameObject Head = null;		// Camera, Hands/Arms, others are parented to this

	private float accum_rot_y = 0.0f;

	float rotationY = 0F;

	void Update ()
	{	
		
		// Mouse Look
		// Note that rotations are in degrees and NOT Radians!

		Screen.lockCursor = true;
		
		// yaw
		transform.Rotate( new Vector3(0.0f, Input.GetAxis("Mouse X") *sensitivityX , 0.0f) );
		
		// pitch, note that rot_y is positive when looking down
		float rot_y = -Input.GetAxis("Mouse Y") * sensitivityY;
		
		// clamp pitch
		if ((accum_rot_y + rot_y) > ( maximumY)){
			rot_y = maximumY - accum_rot_y;
		}
		else if ((accum_rot_y + rot_y) < ( minimumY)){
			rot_y =  minimumY - accum_rot_y;
		}
		
		accum_rot_y += rot_y;	// add to accumlated rotation
		
		// apply pitch to the "head" object
		Head.transform.Rotate( new Vector3( rot_y, 0.0f , 0.0f) );

	}
	
	void Start ()
	{
		Screen.lockCursor = true;

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;

		// check if the player object has a head;
		if (Head == null){
			foreach (Transform child in transform ){
				if (child.name.Equals("Player_Head")){
					Head =  child.gameObject;
				}
				else{
					print ("No Head Object Found or The Object name is not: >>Player_Head<<");
				}
			}
		}
		else{
			print ("No Head Object Found");
		}
	}
	
}	
