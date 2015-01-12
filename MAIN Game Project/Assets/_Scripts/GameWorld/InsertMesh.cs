using UnityEngine;
using System.Collections;

public class InsertMesh : MonoBehaviour {


	// Use this for initialization
	void Start () {
		GameObject X=this.gameObject;
		Mesh mesh1 = X.GetComponent<MeshFilter>().mesh;
		int[] Triangles = mesh1.triangles;
		int[] newTriangles = mesh1.triangles;
		for (int i=0; i<Triangles.Length/3; i++) {
			Triangles[3*i]=newTriangles[3*i+2];
			Triangles[3*i+2]=newTriangles[3*i];
				}
		mesh1.triangles = Triangles;
		X.GetComponent<MeshFilter>().mesh = mesh1;
		X.collider.isTrigger = true;
	}
	

}
