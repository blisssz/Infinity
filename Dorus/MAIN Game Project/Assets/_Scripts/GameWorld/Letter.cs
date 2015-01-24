using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour  {
	void Update() {
		transform.Translate (Vector3.up * Time.deltaTime * 2, Space.World);
	}
}