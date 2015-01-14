using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk
{
		private int[,,] Data;
		private int[] Position2;
		private Vector3[,,] Vertices;
		private Vector3[,,,] Vertices2;
		private int sizeChunk;
		private float rib;
		private List<GameObject> zero;
		private List<List<Vector3>> VerticesNeeded;
		private List<int> MaterialList;
		private List<int[]> Triangles;
		private int[] temp;
		public int updated = -1;
		public int updated2 = -1;

		public Chunk (int[] Position1)
		{
		zero=new List<GameObject>();
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
				Data = new int [sizeChunk + 2, sizeChunk + 2, sizeChunk + 2];
				Vertices = new Vector3[sizeChunk + 2, sizeChunk + 2, sizeChunk + 2];
		float[,,,] C=HelpScript.Rand (-1*rib/3,1*rib/3,sizeChunk+2,3);
				
				
				for (int i=0; i<sizeChunk+1; i++) {
						for (int j=0; j<sizeChunk+1; j++) {
								for (int k=0; k<sizeChunk+1; k++) {

										Data [i, j, k] = 1;
										Vertices [i, j, k] = new Vector3 (i * rib +Position2[0]*sizeChunk+ C[i,j,k,0], j * rib +Position2[1]*sizeChunk+ C[i,j,k,1], k * rib +Position2[2]*sizeChunk+ C[i,j,k,2]);
								}
				
						}
				}
				

		}

		public void UpdateVertices ()
		{


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
		updated = ChunkList.UpdateNumber;
				//Debug.Log (11);
		}

		public void AddSurrounds ()
		{
				
				if (updated==ChunkList.UpdateNumber) {
						for (int i1=-1; i1<2; i1++) {
								for (int j1=-1; j1<2; j1++) {
										for (int k1=-1; k1<2; k1++) {
												int[] Position3 = new int[3];
												Position3 [0] = i1 + Position2 [0];
												Position3 [1] = j1 + Position2 [1];
												Position3 [2] = k1 + Position2 [2];
												ChunkList.AddChunk (Position3);
										}
								}
						}
				}

		}

		public void UpdateSides ()
		{
//				if (Position2 [0] == 0 && Position2 [1] == 1 && Position2 [2] == 2) {
//						Debug.Log ("yus");
//				}
				int[] Position3 = new int[3];


				for (int i1=0; i1<2; i1++) {
						for (int j1=0; j1<2; j1++) {
							for (int k1=0; k1<2; k1++) {
										if (i1 + j1 + k1 == 0) {
											k1++;
										}
										Position3 [0] = i1 + Position2 [0];
										Position3 [1] = j1 + Position2 [1];
										Position3 [2] = k1 + Position2 [2];
										
												Chunk au = ChunkList.GetChunk (Position3);
												for (int i=(sizeChunk*(i1)); i<(sizeChunk)+i1; i++) {
														for (int j=(sizeChunk*(j1)); j<(sizeChunk)+j1; j++) {
																for (int k=(sizeChunk*(k1)); k<(sizeChunk)+k1; k++) {
																		this.Data [i, j, k] = au.Data [i - i1 * sizeChunk, j - j1 * sizeChunk, k - k1 * sizeChunk];
																		this.Vertices [i, j, k] = au.Vertices [i - i1 * sizeChunk, j - j1 * sizeChunk, k - k1 * sizeChunk];
//																		if(i>0){this.Vertices2 [i-i1, j, k, 0] = this.Vertices[i-i1,j,k]/2+this.Vertices[i,j,k]/2;}
//																	    if(j>0){this.Vertices2 [i, j-j1, k, 1] = this.Vertices[i,j-j1,k]/2+this.Vertices[i,j,k]/2;}
//																		if(k>0){this.Vertices2 [i, j, k-k1, 2] = this.Vertices[i,j,k-k1]/2+this.Vertices[i,j,k]/2;}
//																		
//																		
																}
														}
												}


								}
						}
				}

		Vertices2 = new Vector3[sizeChunk+1, sizeChunk+1, sizeChunk+1, 3];
		for (int i=0; i<sizeChunk+1; i++) {
			for (int j=0; j<sizeChunk+1; j++) {
				for (int k=0; k<sizeChunk+1; k++) {
					
					Vertices2 [i, j, k, 0] = Vertices [i, j, k] / 2 + Vertices [i + 1, j, k] / 2;
					Vertices2 [i, j, k, 1] = Vertices [i, j, k] / 2 + Vertices [i, j + 1, k] / 2;
					Vertices2 [i, j, k, 2] = Vertices [i, j, k] / 2 + Vertices [i, j, k + 1] / 2;
				}
				
			}
		}
		
		
	}
	
	public int[] Coordinates(int Rib){
		int[] value=new int[4];
		if(Rib==0){
			value[0]=0;			value[1]=0;			value[2]=0;			value[3]=0;
		} else if(Rib==1){
			value[0]=0;			value[1]=0;			value[2]=0;			value[3]=1;
		} else if(Rib==2){
			value[0]=0;			value[1]=0;			value[2]=0;			value[3]=2;

		} else if(Rib==3){
			value[0]=1;			value[1]=0;			value[2]=0;			value[3]=1;
		} else if(Rib==4){
			value[0]=1;			value[1]=0;			value[2]=0;			value[3]=2;

		} else if(Rib==5){
			value[0]=0;			value[1]=1;			value[2]=0;			value[3]=0;
		} else if(Rib==6){
			value[0]=0;			value[1]=1;			value[2]=0;			value[3]=2;

		} else if(Rib==7){
			value[0]=1;			value[1]=1;			value[2]=0;			value[3]=2;

		} else if(Rib==8){
			value[0]=0;			value[1]=0;			value[2]=1;			value[3]=0;
		} else if(Rib==9){
			value[0]=0;			value[1]=0;			value[2]=1;			value[3]=1;
		} else if(Rib==10){
			value[0]=1;			value[1]=0;			value[2]=1;			value[3]=1;
		} else if(Rib==11){
			value[0]=0;			value[1]=1;			value[2]=1;			value[3]=0;
		} 
		return value;
	}

		public int sumCube (int bin1, int bin2, int bin3)
		{
				int sum = 0;
				int factor = 128;
				
			for (int k=0; k<2; k++) {
						for (int j=0; j<2; j++) {
				for (int i=0; i<2; i++) {
					int value=0;
					if(Data [i + bin1, j + bin2, k + bin3]>0){value=1;}
										sum = sum + ( value- 1) * -1 * factor;
										factor = factor / 2;
								}
						}
				}
				return sum;
		}

	public int sumCube2 (int bin1, int bin2, int bin3)
	{
		int sum = 0;
		int number=0;
		List<int> Values=new List<int>();
		List<int> Frequency=new List<int>();
		
		for (int k=0; k<2; k++) {
			for (int j=0; j<2; j++) {
				for (int i=0; i<2; i++) {
					if(!Values.Contains (Data [i + bin1, j + bin2, k + bin3])&&0!=Data [i + bin1, j + bin2, k + bin3]){
						Values.Add (Data [i + bin1, j + bin2, k + bin3]);
						Frequency.Add (1);}
					else if(0!=Data [i + bin1, j + bin2, k + bin3]){Frequency[Values.IndexOf(Data [i + bin1, j + bin2, k + bin3])]++;}


				}
			}
		}
		int Max=HelpScript.max (Frequency);

		int index=Frequency.IndexOf(Max);
		return Values[index];
	}

	public int GetVerticeList(int MaterialNumber){
		for(int i=0;i<MaterialList.Count;i++){
			if(MaterialNumber==MaterialList[i]){ return i;}
		}

		MaterialList.Add (MaterialNumber);
		VerticesNeeded.Add (new List<Vector3>());
		return (MaterialList.Count-1);

	}

		public void UpdateTriangles ()
		{
//				if (Position2 [0] == 0 && Position2 [1] == 1 && Position2 [2] == 2) {
//						Debug.Log ("yus");
//				}

				VerticesNeeded = new List<List<Vector3>> ();
				MaterialList = new List<int>();
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
												int[] d = new int[Tri.Length];
												int MaterialNumber=sumCube2(i,j,k);
												for (int uu=0; uu<Tri.Length; uu++) {
													int[] value= Coordinates(Tri [uu] );
													x [uu] = value[0];
													y [uu] = value[1];
													z [uu] = value[2];
													d [uu] = value[3];
														if (x [uu] > 1 || y [uu] > 1 || z [uu] > 1) {
																Debug.Log (1000);
														}
														int VerticeListNumber=GetVerticeList(MaterialNumber);
														VerticesNeeded[VerticeListNumber].Add (Vertices2 [i + x [uu], j + y [uu], k + z [uu],d[uu]]);
//														if (VerticesNeeded [uu] == new Vector3 (0, 0, 0) && (i == 0 && j == 0 && k + z [uu] < 15)) {
//																Debug.Log ("lol");
//														}
												}
										}

								}
						}
				}
				Triangles = new List<int[]>();
		for(int j=0;j<MaterialList.Count;j++){
			Triangles.Add (new int[VerticesNeeded[j].Count]);
				for (int i=0; i<Triangles[j].Length; i++) {
						Triangles [j][i] = i;
				}
		}

				
		}

		public void UpdateMesh ()
		{
		for(int i=0;i<zero.Count;i++){
				HelpScript.Destroy (zero[i]);
		}
		zero=new List<GameObject>();
		for(int i=0; i<VerticesNeeded.Count;i++){
				zero.Add (ObjectCreator.Creator (VerticesNeeded[i].ToArray (), Triangles[i], ChunkList.AA[MaterialList[i]-1], HelpScript.IntToVec3 (Position2, sizeChunk)));
		}
		}
}
