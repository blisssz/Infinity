using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk
{
		private int[,,] Data;
		private int[] Position2;
		private Vector3[,,] Vertices;
		private int sizeChunk;
		private float rib;
		private GameObject zero;
		private List<Vector3> VerticesNeeded;
		private int[] Triangles;
		private Material Fu;
		private int[] temp;
		public bool updated = false;

		public Chunk (int[] Position1, Material F1)
		{
				temp = new int[3];
				Position2 = new int[3];
				temp [0] = Position1 [0];
				temp [1] = Position1 [1];
				temp [2] = Position1 [2];
				Position2 [0] = Position1 [0];
				Position2 [1] = Position1 [1];
				Position2 [2] = Position1 [2];
//				if (Position2 [0] == 0 && Position2 [1] == 1 && Position2 [2] == 2) {
//						Debug.Log ("yus");
//				}
				sizeChunk = ChunkList.sizeChunk;
				rib = ChunkList.rib;
				Fu = F1;
				Data = new int [sizeChunk + 1, sizeChunk + 1, sizeChunk + 1];
				Vertices = new Vector3[sizeChunk + 1, sizeChunk + 1, sizeChunk + 1];
				
				for (int i=0; i<sizeChunk; i++) {
						for (int j=0; j<sizeChunk; j++) {
								for (int k=0; k<sizeChunk; k++) {

										Data [i, j, k] = 0;
					float c1 = Position2 [0] * sizeChunk * rib;//+ Random.Range (-1F, 1F) * rib / (3);
					float c2 = Position2 [1] * sizeChunk * rib;//+ Random.Range (-1F, 1F) * rib / (3);
					float c3 = Position2 [2] * sizeChunk * rib;//+ Random.Range (-1F, 1F) * rib / (3);
										Vertices [i, j, k] = new Vector3 (i * rib + c1, j * rib + c2, k * rib + c3);
								}
				
						}
				}

		}

		public int[] GetPosition ()
		{
				return Position2;
		}

		public void UpdateData (int[][] DataPositions)
		{
				for (int i=0; i<DataPositions.Length; i++) {
						Data [DataPositions [i] [0], DataPositions [i] [1], DataPositions [i] [2]] = DataPositions [i] [3];
				}
				updated = true;
		//Debug.Log (11);
		}

//		public bool testPossible (int i, int j, int k, int l)
//		{
//				for (int u=0; u<3; u++) {
//						if (Cases [l] [u] [0] + i < 0 || Cases [l] [u] [0] + i > sizeChunk  || Cases [l] [u] [1] + j < 0 || Cases [l] [u] [1] + j > sizeChunk  || Cases [l] [u] [2] + k < 0 || Cases [l] [u] [2] + k > sizeChunk ) {
//								return false;
//						} 
//				}
//
//				return true;
//
//		}

		public void UpdateSides ()
		{
//				if (Position2 [0] == 0 && Position2 [1] == 1 && Position2 [2] == 2) {
//						Debug.Log ("yus");
//				}
				int[] Position3 = new int[3];
				if (updated) {
						for (int i1=0; i1<2; i1++) {
								for (int j1=0; j1<2; j1++) {
										for (int k1=0; k1<2; k1++) {
												Position3 [0] = i1 + Position2 [0];
												Position3 [1] = j1 + Position2 [1];
												Position3 [2] = k1 + Position2 [2];
												if (!ChunkList.ChunkExists (Position3)) {
														ChunkList.AddChunk (Position3);
												}
										}
								}
						}
				}


				for (int i1=-1; i1<1; i1++) {
						for (int j1=-1; j1<1; j1++) {
								for (int k1=-1; k1<1; k1++) {
										if (i1 + j1 + k1 == 0) {
												break;
										}
										Position3 [0] = i1 + Position2 [0];
										Position3 [1] = j1 + Position2 [1];
										Position3 [2] = k1 + Position2 [2];
//										if (Position3 [0] == 0 && Position3 [1] == 1 && Position3 [2] == 2) {
//												Debug.Log (1000);
//												Debug.Log (Position2 [0] + " " + Position2 [1] + " " + Position2 [2]);
//										}
										;
										if (ChunkList.ChunkExists (Position3)) {
												Chunk au = ChunkList.GetChunk (Position3);
												for (int i=(sizeChunk*(-i1)); i<(sizeChunk)-i1; i++) {
														for (int j=(sizeChunk*(-j1)); j<(sizeChunk)-j1; j++) {
																for (int k=(sizeChunk*(-k1)); k<(sizeChunk)-k1; k++) {
																		au.Data [i, j, k] = this.Data [i + i1 * sizeChunk, j + j1 * sizeChunk, k + k1 * sizeChunk];
																		au.Vertices [i, j, k] = this.Vertices [i + i1 * sizeChunk, j + j1 * sizeChunk, k + k1 * sizeChunk];
																}
														}
												}



										} else if (updated) {
												ChunkList.AddChunk (Position3);
												Chunk au = ChunkList.GetChunk (Position3);
												for (int i=(sizeChunk*(-i1)); i<(sizeChunk)-i1; i++) {
														for (int j=(sizeChunk*(-j1)); j<(sizeChunk)-j1; j++) {
																for (int k=(sizeChunk*(-k1)); k<(sizeChunk)-k1; k++) {
																		au.Data [i, j, k] = this.Data [i + i1 * sizeChunk, j + j1 * sizeChunk, k + k1 * sizeChunk];
																		au.Vertices [i, j, k] = this.Vertices [i + i1 * sizeChunk, j + j1 * sizeChunk, k + k1 * sizeChunk];
																}
														}
												}
										}
								}
						}
				}


		}

		public int sumCube (int bin1, int bin2, int bin3)
		{
				int sum = 0;
				int factor = 128;
				for (int i=0; i<2; i++) {
						for (int j=0; j<2; j++) {
								for (int k=0; k<2; k++) {
										sum = sum + (Data [i + bin1, j + bin2, k + bin3]-1)*-1* factor;
										factor = factor / 2;
								}
						}
				}
				return sum;
		}

		public void UpdateTriangles ()
		{
//				if (Position2 [0] == 0 && Position2 [1] == 1 && Position2 [2] == 2) {
//						Debug.Log ("yus");
//				}

				VerticesNeeded = new List<Vector3> ();
				//Debug.Log (testPossible (3, 4, 5, 5));
				//	Debug.Log (Cases [5] [1] [0] + Cases [5] [1] [1] + Cases [5] [1] [2]);
				for (int i=0; i<sizeChunk; i++) {
						for (int j=0; j<sizeChunk; j++) {
								for (int k=0; k<sizeChunk; k++) {
										int index = sumCube (i, j, k);
										if (index > 0 && index < 255) {
												int[] Tri = GenerateCases.Cases [index];
												int[] x = new int[Tri.Length];
												int[] y = new int[Tri.Length];
												int[] z = new int[Tri.Length];
												for (int uu=0; uu<Tri.Length; uu++) {
														x [uu] = Tri [uu] / 4;
														y [uu] = (Tri [uu] - x [uu] * 4) / 2;
														z [uu] = (Tri [uu] - x [uu] * 4 - y [uu] * 2);
														if (x [uu] > 1 || y [uu] > 1 || z [uu] > 1) {
																Debug.Log (1000);
														}
														VerticesNeeded.Add (Vertices [i + x [uu], j + y [uu], k + z [uu]]);
//														if (VerticesNeeded [uu] == new Vector3 (0, 0, 0) && (i == 0 && j == 0 && k + z [uu] < 15)) {
//																Debug.Log ("lol");
//														}
														;
												}
										}

								}
						}
				}
				Triangles = new int[VerticesNeeded.Count];
				for (int i=0; i<Triangles.Length; i++) {
						Triangles [i] = i;
				}

				
		}

		public void UpdateMesh ()
		{

				HelpScript.Destroy (zero);
				zero = ObjectCreator.Creator (VerticesNeeded.ToArray (), Triangles, Fu, HelpScript.IntToVec3 (Position2, sizeChunk));
		}
}
