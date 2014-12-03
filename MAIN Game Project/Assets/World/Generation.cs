using UnityEngine;
using System.Collections;

public class Generation : MonoBehaviour
{
		public Vector3 SpawnPosition;
		private Vector3 Position;
		private int size = 5;
		private int b = 4;
		public Material F;
		public GameObject C;
		private Vector3[] Vertices;
		private int[] Triangles;
		private float pastdirection = 0;
		private int direction = 2;
		private float up = 1;
	private float jumpDistance=8;

		public void Pyramid (Vector3 Position)
		{
				Vector3[] newVertices = new Vector3[b * b + 1];
				int[] newTriangles = new int[3 * (b - 1) * (2 * (b - 1) + 4)];
				for (int j=0; j<b; j++) {
						for (int k=0; k<b; k++) {
								float c1 = Random.Range (-1F, 1F) * size / (b * 2);
								float c2 = Random.Range (-1F, 1F) * size / (b * 2);
								float c3 = Random.Range (-1F, 1F) * size / (b * 2);
								newVertices [4 * j + k] = new Vector3 ((float)j * size / (b - 1) + c1, 0 + c2, (float)k * size / (b - 1) + c3) + Position;
						}
				}
				newVertices [b * b] = new Vector3 (size / 2, -size, size / 2) + Position;
		
				int u = 0;
				for (int j=0; j<b-1; j++) {
						for (int k=0; k<b-1; k++) {
								newTriangles [u] = 4 * j + k;
								newTriangles [u + 1] = 4 * j + k + 1;
								newTriangles [u + 2] = 4 * (1 + j) + k;
								newTriangles [u + 3] = 4 * (1 + j) + k + 1;
								newTriangles [u + 4] = 4 * (1 + j) + k;
								newTriangles [u + 5] = 4 * j + k + 1;
								u = u + 6;
								if (k == 0) {
										newTriangles [u] = 4 * j;
										newTriangles [u + 1] = 4 * (j + 1);
										newTriangles [u + 2] = b * b;
										newTriangles [u + 5] = 4 * j + b - 1;
										newTriangles [u + 4] = 4 * (j + 1) + b - 1;
										newTriangles [u + 3] = b * b;
										u = u + 6;
								}
				
								if (j == 0) {
										newTriangles [u + 2] = k;
										newTriangles [u + 1] = k + 1;
										newTriangles [u] = b * b;
										newTriangles [u + 3] = 4 * (b - 1) + k;
										newTriangles [u + 4] = 4 * (b - 1) + k + 1;
										newTriangles [u + 5] = b * b;
										u = u + 6;
								}
				
						}
				}
		
//		Vector3[] tempVertices=Vertices;
//		int[] tempTriangles=Triangles;
//		Vertices =new Vector3[b * b + 1+Vertices.Length];
//		Triangles=new int[3 * (b - 1) * (2 * (b - 1) + 4)+Triangles.Length];
//		for (int j=0; j<Vertices.Length; j++) {
//				}
				ObjectCreator.Creator (newVertices, newTriangles, F);
		}

		void moveStandard ()
		{
				float b = Random.Range (0, 100);
				if (b <= 10F && pastdirection <= 0.5F) {
						direction++;
						pastdirection = 1;
				} else if (b <= 60F && pastdirection >= -0.5F) {
						direction--;
						pastdirection = -1;
				} else if (b <= 75F) {
						up = 1 / 2;
						pastdirection = 0;
				} else if (pastdirection >= 0.5) {
						pastdirection = pastdirection - 0.5F;
			
				} else if (pastdirection <= -0.5) {
						pastdirection = pastdirection + 0.5F;
				} 
				if (direction > 3) {
						direction = direction - 4;
				}
				if (direction < 0) {
						direction = direction + 4;
				}
				if (direction == 0) {
			Position.x = Position.x - jumpDistance;
		}
				if (direction == 2) {
			Position.x = Position.x + jumpDistance;
		}
				if (direction == 1) {
			Position.z = Position.z - jumpDistance;
		}
				if (direction == 3) {
			Position.z = Position.z + jumpDistance;
		}
		Position.y = Position.y + jumpDistance * (1 - up);
				up = 1;
				//Instantiate(C, Position, Quaternion.identity);
				Pyramid (Position);
		
		}


// Use this for initialization
		void Start ()
		{
	
	
				Position = SpawnPosition;
				for (int i=0; i<50; i++) {
			moveStandard ();
						
						//new ObjectCreator (Vertices, Triangles, F);
				}
	
				// Update is called once per frame

		}
}
