using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RopeMesh{
	
	private Vector3[] Vertices;
	private Vector2[] UV;
	private int[] Triangles;
	
	List<Vector3> verticesList;
	List<Vector3> normalList;
	List<Vector2> uvList;
	List<int> trisList;
	int[] Tris;
	
	Vector2[] uv;
	
	
	public GameObject ropeOB;
//	private GameObject[] ropeArray;

	private GameObject ropeMesh;
	private Vector3 mainOffset;

	
	DynamicRope5 dr5;
	
	// Constructor
	public RopeMesh(){
		ropeMesh = new GameObject();
		ropeMesh.name = "ropeMeshObject";
		//ropeMesh.tag = "ropeMesh";
		ropeMesh.AddComponent<MeshRenderer>();
		ropeMesh.AddComponent<MeshFilter>();


	}
	
	public void UpdateRopeMesh (GameObject[] ropeArray, Vector3 offset, int verts = 6, float radius = 0.1f) {

		mainOffset = offset;

		verticesList = new List<Vector3>();
		normalList = new List<Vector3>();
		uvList = new List<Vector2>();
		
		int v = verts;
		int r = ropeArray.Length; // cylinder rings in the rope
	
		//	createCircle(v, r, 2f);
		Vector3 refPos = ropeArray[ropeArray.Length-1].transform.position;
		createCylinderMapToTransform(v, radius, ropeArray, refPos);
		ropeMesh.transform.position = ropeArray[ropeArray.Length-1].transform.position;
		
		Vertices = verticesList.ToArray();
		uv = uvList.ToArray();
		
		Tris = new int[v*3*(r+r-2)];
		
		
		// create a inside out tube when extruding forward, correct when extruding in y
		int i = 0;
		int k = 0;
		
		for (int j = 0; j < v * (r-1); j++){
			
			if ((j % v)==0 && j != 0){
				k++;
			}
			
			Tris[i] = j % v + k * v;
			Tris[i+1] = (j) % v + (k+1)*v;
			Tris[i+2] = (j+1) % v  + (k+1)*v;
			
			Tris[i+3] = (j+1) % v  + (k+1)*v;
			Tris[i+4] = (j+1) % v + k * v;
			Tris[i+5] = j % v + k * v;
			
			i+= 6;
		}


		Mesh newMesh = new Mesh();
		ropeMesh.GetComponent<MeshFilter>().mesh = newMesh;
		//ropeMesh.GetComponent<MeshRenderer>().material
		newMesh.vertices = Vertices;
		newMesh.triangles = Tris;
		//newMesh.normals = normalList.ToArray();
		newMesh.RecalculateNormals();
		newMesh.uv = uv;
			
		
	}
	
	
	void createCircle(int nVerts, int rings, float radius){
		float rad = Mathf.PI * 2/ ((float) nVerts);
		float currentRad = 0.0f;
		float currentRing = 0.0f;
		
		for (int j = 0; j < rings; j++){
			for (int i = 0; i < nVerts; i++){
				
				Vector3 vpos = radius * new Vector3(-Mathf.Cos(currentRad), Mathf.Sin (currentRad), currentRing);
				Vector3 norm = new Vector3(Mathf.Cos(currentRad), 0f, Mathf.Sin (currentRad));
				currentRad += rad;
				
				verticesList.Add (vpos);
				normalList.Add (norm);
				
				
				uvList.Add (new Vector2(((float) i)/(float)nVerts, (float) j));
			}
			
			currentRing += 5.0f;
			
			
		}
	}
	
	void createCylinderMapToTransform(int nVerts, float radius, GameObject[] obArray, Vector3 refPos){
		float rad = Mathf.PI * 2/ ((float) nVerts);
		float currentRad = 0.0f;
		
		for (int j = obArray.Length-1; j >= 0; j--){
			for (int i = 0; i < nVerts; i++){
				Vector3 vpos;
				if (j == obArray.Length-1){
					vpos = obArray[j-1].transform.right * radius * -Mathf.Cos(currentRad) + obArray[j-1].transform.up * radius * Mathf.Sin(currentRad);
					vpos += obArray[j].transform.position-refPos + mainOffset;
				}
				else if (j == 0){
					vpos = obArray[1].transform.right * radius * -Mathf.Cos(currentRad) + obArray[1].transform.up * radius * Mathf.Sin(currentRad);
					vpos += obArray[j].transform.position-refPos;
				}
				else{
					vpos = obArray[j].transform.right * radius * -Mathf.Cos(currentRad) + obArray[j].transform.up * radius * Mathf.Sin(currentRad);
					vpos += obArray[j].transform.position-refPos;
				}
				Vector3 norm = new Vector3(Mathf.Cos(currentRad), 0f, Mathf.Sin (currentRad));
				currentRad += rad;
				
				verticesList.Add (vpos);
				normalList.Add (norm);
				
				uvList.Add (new Vector2(((float) i)/(float)nVerts, (float) j));
			}
			
			
			
		}
	}
	
	public GameObject getMeshGameObject(){
		return ropeMesh.gameObject;
	}

/*	public void setMeshMaterial(Material mat){
		//ropeMesh.GetComponent<MeshRenderer>().materials[0] = mat;
		ropeMesh.GetComponent<MeshRenderer>().material = mat;

	}*/
	
}
