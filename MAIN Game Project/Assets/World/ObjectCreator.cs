using UnityEngine;
using System.Collections;

public class ObjectCreator : MonoBehaviour {

	// Use this for initialization
	public static void Creator(Vector3[] Vertices,int[] Triangles,Material F){
		GameObject plane=GameObject.CreatePrimitive(PrimitiveType.Plane);
		Mesh mesh = new Mesh();
		int L = Triangles.Length;
		int[] newTriangles=new int[L];
		Vector3[] newVertices=new Vector3[L];
		Vector2[] newUV = new Vector2[L];
		for (int i=0; i<L; i++) {
			newVertices[i]=Vertices[Triangles[i]];
			newTriangles[i]=i;
				}

		mesh.vertices = newVertices;
		mesh.uv = newUV;
		F.color = Color.red;
		mesh.triangles = newTriangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
		plane.GetComponent<MeshCollider> ().mesh = mesh;
		plane.GetComponent<MeshFilter>().mesh = mesh;
		Renderer A=plane.GetComponent<MeshRenderer> ();
		A.material = F;
	}

}
