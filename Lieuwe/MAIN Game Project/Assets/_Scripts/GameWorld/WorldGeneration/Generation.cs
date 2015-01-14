using UnityEngine;
using System.Collections;

public class Generation : MonoBehaviour
{
		public Vector3 SpawnPosition;
		public GameObject CheckPoint;
		public GameObject endPoint;
		private Vector3 Position;
		public GameObject enemy;
		public static bool spawnEnemy;
		public GameObject coin;
		private static float minsize;
		private static float maxsize;
		private int b = 15;
		public Material F;
		public GameObject C;
		private Vector3[] Vertices;
		private int[] Triangles;
		private int iteration = 0;
		private static float jumpDistance = 100;
		private Vector3[] lastPositions = new Vector3[3];
		public static Vector3[][] Moves;
	
		public Vector3[][] AddAllDirections (Vector3[][] Movs)
		{
				Vector3[][] TempMovs = new Vector3[Movs.Length * 4][];
				for (int i=0; i<Movs.Length; i++) {
						TempMovs [4 * i] = new Vector3[Movs [i].Length];
						TempMovs [4 * i + 1] = new Vector3[Movs [i].Length];
						TempMovs [4 * i + 2] = new Vector3[Movs [i].Length];
						TempMovs [4 * i + 3] = new Vector3[Movs [i].Length];
			
						for (int j=0; j<Movs[i].Length; j++) {
								TempMovs [4 * i] [j] = Movs [i] [j];

								TempMovs [4 * i + 1] [j].y = Movs [i] [j].y;
								TempMovs [4 * i + 1] [j].x = -Movs [i] [j].x;
								TempMovs [4 * i + 1] [j].z = -Movs [i] [j].z;

								TempMovs [4 * i + 2] [j].y = Movs [i] [j].y;
								TempMovs [4 * i + 2] [j].x = Movs [i] [j].z;
								TempMovs [4 * i + 2] [j].z = -Movs [i] [j].x;

								TempMovs [4 * i + 3] [j].y = Movs [i] [j].y;
								TempMovs [4 * i + 3] [j].x = -Movs [i] [j].z;
								TempMovs [4 * i + 3] [j].z = Movs [i] [j].x;
						}

				}
				return TempMovs;
		}
	

		public GameObject Pyramid (Vector3 Position, float size)
		{
				Vector3[] newVertices = new Vector3[b * b + 1];
				int[] newTriangles = new int[3 * (b - 1) * (2 * (b - 1) + 4)];
				for (int j=0; j<b; j++) {
						for (int k=0; k<b; k++) {
								float c1 = Random.Range (-1F, 1F) * size / (b * 5);
								float c2 = Random.Range (-1F, 1F) * size / (b * 5);
								float c3 = Random.Range (-1F, 1F) * size / (b * 5);
								newVertices [b * j + k] = new Vector3 ((float)j * size / (b - 1) + c1, 0 + c2, (float)k * size / (b - 1) + (float)j * size / (b - 1) / 2f + c3) + Position;
						}
				}
				newVertices [b * b] = new Vector3 (size / 2, -size, size / 2) + Position;
		
				int u = 0;
				for (int j=0; j<b-1; j++) {
						for (int k=0; k<b-1; k++) {
								newTriangles [u] = b * j + k;
								newTriangles [u + 1] = b * j + k + 1;
								newTriangles [u + 2] = b * (1 + j) + k;
								newTriangles [u + 3] = b * (1 + j) + k + 1;
								newTriangles [u + 4] = b * (1 + j) + k;
								newTriangles [u + 5] = b * j + k + 1;
								u = u + 6;
								if (k == 0) {
										newTriangles [u] = b * j;
										newTriangles [u + 1] = b * (j + 1);
										newTriangles [u + 2] = b * b;
										newTriangles [u + 5] = b * j + b - 1;
										newTriangles [u + 4] = b * (j + 1) + b - 1;
										newTriangles [u + 3] = b * b;
										u = u + 6;
								}
				
								if (j == 0) {
										newTriangles [u + 2] = k;
										newTriangles [u + 1] = k + 1;
										newTriangles [u] = b * b;
										newTriangles [u + 3] = b * (b - 1) + k;
										newTriangles [u + 4] = b * (b - 1) + k + 1;
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
				GameObject x = ObjectCreator.Creator (newVertices, newTriangles, F, Position);
				return x;
		}
	
		public GameObject CreatePlane (Vector3 Position, float size)
		{
				int sizex = b;
				int sizey = b;
				int u = 0;
				int[,] Shape = new int [sizex, sizey];
				for (int j=0; j<sizex; j++) {
						for (int k=0; k<sizey; k++) {
								if ((j - k / 2f - sizex * 1f / 4f) * (j - k / 2f - sizex * 1f / 4f) + (k - sizey * 2f / 4f) * (k - sizey * 2f / 4f) < sizex / 3f * sizey / 3f) {
										Shape [j, k] = 1;
								} else {
										Shape [j, k] = -1;
								}
						}
				}

				Vector3[] newVertices = new Vector3[HelpScript.SumArraySpecial (Shape)];
				int[] newTriangles = new int[3 * (b - 1) * (2 * (b - 1) + 4)];
				for (int j=0; j<sizex; j++) {
						for (int k=0; k<sizey; k++) {
								if (Shape [j, k] > -1) {
										float c1 = 0;// Random.Range (-1F, 1F) * size / (b * 2);
										float c2 = 0;// Random.Range (-1F, 1F) * size / (b * 2);
										float c3 = 0;// Random.Range (-1F, 1F) * size / (b * 2);
										newVertices [u] = new Vector3 (((float)j - (float)k / 2f) * size / (b - 1) + c1, 0 + c2, (float)k * size / (b - 1) + c3) + Position;
										Shape [j, k] = u;
										u++;
								}
			
						}
				}

		
				u = 0;
				for (int j=0; j<b-1; j++) {
						for (int k=0; k<b-1; k++) {
								if (Shape [j, k] > -1 && Shape [j + 1, k] > -1 && Shape [j + 1, k + 1] > -1) {
										newTriangles [u] = Shape [j, k];
										newTriangles [u + 2] = Shape [j + 1, k];
										newTriangles [u + 1] = Shape [j + 1, k + 1];
										u = u + 3;
								}

								if (Shape [j, k] > -1 && Shape [j, k + 1] > -1 && Shape [j + 1, k + 1] > -1) {
										newTriangles [u + 1] = Shape [j, k];
										newTriangles [u + 2] = Shape [j, k + 1];
										newTriangles [u] = Shape [j + 1, k + 1];
										u = u + 3;
								}
										
				
						}
				}

//		Vector3[] tempVertices=Vertices;
//		int[] tempTriangles=Triangles;
//		Vertices =new Vector3[b * b + 1+Vertices.Length];
//		Triangles=new int[3 * (b - 1) * (2 * (b - 1) + 4)+Triangles.Length];
//		for (int j=0; j<Vertices.Length; j++) {
//				}
				GameObject x = ObjectCreator.Creator (newVertices, newTriangles, F, Position);
				return x;
		}

		public GameObject CreateCube (Vector3 Position, float size)
		{
				int sizex = 2;
				int sizey = 2;
				int sizez = 2;
				int u = 0;
				int[,,] Shape = new int [sizex, sizey, sizez];
				for (int j=0; j<sizex; j++) {
						for (int k=0; k<sizey; k++) { 
								for (int i=0; i<sizez; i++) { 
										Shape [i, j, k] = 1;
								}
						}
				}
		
				Vector3[] newVertices = new Vector3[HelpScript.SumArraySpecial (Shape)];
				int[] newTriangles = new int[3 * (b - 1) * (2 * (b - 1) + 4)];
				for (int i=0; i<sizez; i++) {
						for (int j=0; j<sizex; j++) {

								for (int k=0; k<sizey; k++) {
										if (Shape [i, j, k] > -1) {
					 
												float c1 = 0+ Random.Range (-1F, 1F) * size / (b * 2);
												float c2 = 0+ Random.Range (-1F, 1F) * size / (b * 2);
												float c3 = 0+ Random.Range (-1F, 1F) * size / (b * 2);
						newVertices [u] = new Vector3 (((float)j - (float)k / 2f) * size / (b - 1) + c1, (float)i * size / (b - 1) + c2- (float)k / 2f * size / (b - 1)- (float)j / 2f * size / (b - 1), (float)k * size / (b - 1) + c3) + Position;
												Shape [i, j, k] = u;
												u++;
										}
								}
				
						}
				}
		
		
				u = 0;
				for (int i=0; i<sizez; i++) {
						for (int j=0; j<sizex; j++) {
								for (int k=0; k<sizey; k++) {
										//j,k
										if (j + 1 < sizex && k + 1 < sizey && Shape [i, j, k] > -1 && Shape [i, j + 1, k] > -1 && Shape [i, j + 1, k + 1] > -1) {
												newTriangles [u] = Shape [i, j, k];
												newTriangles [u + 1] = Shape [i, j + 1, k];
												newTriangles [u + 2] = Shape [i, j + 1, k + 1];
												u = u + 3;
										}
				
										//k,j
										if (j + 1 < sizex && k + 1 < sizey && Shape [i, j, k] > -1 && Shape [i, j, k + 1] > -1 && Shape [i, j + 1, k + 1] > -1) {
												newTriangles [u + 2] = Shape [i, j, k];
												newTriangles [u + 2] = Shape [i, j, k + 1];
												newTriangles [u] = Shape [i, j + 1, k + 1];
												u = u + 3;
										}
										//i,j
										if (j + 1 < sizex && i + 1 < sizez && Shape [i, j, k] > -1 && Shape [i + 1, j, k] > -1 && Shape [i + 1, j + 1, k] > -1) {
												newTriangles [u] = Shape [i, j, k];
												newTriangles [u + 2] = Shape [i + 1, j, k];
												newTriangles [u + 1] = Shape [i + 1, j + 1, k];
												u = u + 3;
										}
										//j,i
										if (j + 1 < sizex && i + 1 < sizez && Shape [i, j, k] > -1 && Shape [i, j + 1, k] > -1 && Shape [i + 1, j + 1, k] > -1) {
												newTriangles [u + 1] = Shape [i, j, k];
												newTriangles [u + 2] = Shape [i, j + 1, k];
												newTriangles [u] = Shape [i + 1, j + 1, k];
												u = u + 3;
										}//i,k
										if (k + 1 < sizey && i + 1 < sizez && Shape [i, j, k] > -1 && Shape [i + 1, j, k] > -1 && Shape [i + 1, j, k + 1] > -1) {
												newTriangles [u] = Shape [i, j, k];
												newTriangles [u + 2] = Shape [i + 1, j, k];
												newTriangles [u + 1] = Shape [i + 1, j, k + 1];
												u = u + 3;
										}
										//k,i
										if (k + 1 < sizey && i + 1 < sizez && Shape [i, j, k] > -1 && Shape [i, j, k + 1] > -1 && Shape [i + 1, j, k + 1] > -1) {
												newTriangles [u + 1] = Shape [i, j, k];
												newTriangles [u + 2] = Shape [i, j, k + 1];
												newTriangles [u] = Shape [i + 1, j, k + 1];
												u = u + 3;
										}
								}
				
				
						}
				}

				//		Vector3[] tempVertices=Vertices;
				//		int[] tempTriangles=Triangles;
				//		Vertices =new Vector3[b * b + 1+Vertices.Length];
				//		Triangles=new int[3 * (b - 1) * (2 * (b - 1) + 4)+Triangles.Length];
				//		for (int j=0; j<Vertices.Length; j++) {
				//				}
				GameObject x = ObjectCreator.Creator (newVertices, newTriangles, F, Position);
				return x;
		}

//		void moveStandard ()
//		{
//				float b = Random.Range (0, 100);
//				if (b <= 10F && pastdirection <= 0.5F) {
//						direction++;
//						pastdirection = 1;
//				} else if (b <= 60F && pastdirection >= -0.5F) {
//						direction--;
//						pastdirection = -1;
//				} else if (pastdirection >= 0.5) {
//						pastdirection = pastdirection - 0.5F;
//			
//				} else if (pastdirection <= -0.5) {
//						pastdirection = pastdirection + 0.5F;
//				} 
//				if (direction > 3) {
//						direction = direction - 4;
//				}
//				if (direction < 0) {
//						direction = direction + 4;
//				}
//				if (direction == 0) {
//			Position.x = Position.x - jumpDistance;
//		}
//				if (direction == 2) {
//			Position.x = Position.x + jumpDistance;
//		}
//				if (direction == 1) {
//			Position.z = Position.z - jumpDistance;
//		}
//				if (direction == 3) {
//			Position.z = Position.z + jumpDistance;
//		}
		//Instantiate(C, Position, Quaternion.identity);
//				Pyramid (Position);
//		
//		}

		void Move (int o)
		{

				for (int i=0; i<Moves[o].Length; i++) {
						float size = Random.Range (minsize, maxsize);
						Vector3 CheckPointOffset = new Vector3 (size / 2f, 5f, size / 1.5f);
						Vector3 SizeOffset = new Vector3 (size / 2f, size / 2f, size / 2f);
						lastPositions [0] = lastPositions [1];
						lastPositions [1] = lastPositions [2];
						lastPositions [2] = Position;
						if (i == 0) {
								Position = Position + SizeOffset + Moves [o] [i];
						} else if (i != 0) {
								Position = Position + SizeOffset + Moves [o] [i] - Moves [o] [i - 1];
						}
						
						if (!lastPositions [0].Equals (Vector3.zero)) {
								Pyramid (lastPositions [0], size);
				onIteration(lastPositions [0] + CheckPointOffset);
								
						}
			Aftermove(Position);
			}

		}

		void moveStandardS ()
		{
				int[] Options = new int[Moves.Length];

				//Instantiate(C, Position, Quaternion.identity);
				for (int i=0; i<Options.Length; i++) {
						Options [i] = 1;
						for (int j=0; j<Moves[i].Length; j++) {
								Collider[] hitColliders = Physics.OverlapSphere (Position + Moves [i] [j], jumpDistance * 3f / 2f);
								if (hitColliders.Length != 0) {
										Options [i] = 0;
								}
				
						}
						
				}

				int Sum = HelpScript.SumArray (Options);
				if (Sum == 0) {
						Debug.Log ("no Options!");
				}
				float[] Chances = new float[Options.Length];
				for (int i=0; i<Options.Length; i++) {
						Chances [i] = Options [i] * 100f / (float)Sum;

				}

				int caseSwitch = HelpScript.Switch (Chances);
				Move (caseSwitch);


		}
	
//	void moveJumpSpecial ()
//	{
//		
//		if (direction == 0) {
//			Position.x = Position.x - jumpDistance*2f;
//		}
//		if (direction == 2) {
//			Position.x = Position.x + jumpDistance*2f;
//		}
//		if (direction == 1) {
//			Position.z = Position.z - jumpDistance*2f;
//		}
//		if (direction == 3) {
//			Position.z = Position.z + jumpDistance*2f;
//		}
//		//Instantiate(C, Position, Quaternion.identity);
//		Position.y = Position.y + jumpDistance*2f;
//		Pyramid (Position);
//		Position.y = Position.y - jumpDistance*2f;
//		if (direction == 0) {
//			Position.x = Position.x - jumpDistance*2f;
//		}
//		if (direction == 2) {
//			Position.x = Position.x + jumpDistance*2f;
//		}
//		if (direction == 1) {
//			Position.z = Position.z - jumpDistance*2f;
//		}
//		if (direction == 3) {
//			Position.z = Position.z + jumpDistance*2f;
//		}
//		Pyramid (Position);
//	}
//	
//	void moveJump ()
//	{
//		float b = Random.Range (0, 100);
//		if (b <= 10F && pastdirection <= 0.5F) {
//			direction++;
//		} else if (b <= 60F && pastdirection >= -0.5F) {
//			direction--;
//		} 
//		
//		pastdirection = 0;
//		if (direction > 3) {
//			direction = direction - 4;
//		}
//		if (direction < 0) {
//			direction = direction + 4;
//		}
//		if (direction == 0) {
//			Position.x = Position.x - jumpDistance/2f;
//		}
//		if (direction == 2) {
//			Position.x = Position.x + jumpDistance/2f;
//		}
//		if (direction == 1) {
//			Position.z = Position.z - jumpDistance/2f;
//		}
//		if (direction == 3) {
//			Position.z = Position.z + jumpDistance/2f;
//		}
//		Position.y = Position.y + jumpDistance/3f;
		//Instantiate(C, Position, Quaternion.identity);
//		Pyramid (Position);
//		
//	}
	
	
		// Use this for initialization
		void Start ()
		{		
				GameController.fallingPossible = true;
				settingSetter ();
				setMoves();
				Vector3 a = new Vector3 (0, 2, 0);
				//GameObject x = CreatePlane (a, minsize * 10);
				Vector3 b = new Vector3 (10, 2, 0);

				Moves = AddAllDirections (Moves);

				a = new Vector3 (0, 20, 0);
				//GameObject x = CreateCube (a, minsize * 10);
		
		
				Position = SpawnPosition;
				for (int i=0; i<300; i++) {
						float[] Chances = new float[3]{80f,0f,0f};
						int caseSwitch = SwitchHelp.Switch (Chances);
						switch (caseSwitch) {
						case 0:
								{
										moveStandardS ();}
								break;
						case 2:
								{
								}
								break;
						default:
								break;
						}

				}
	
				// Update is called once per frame


		}

		void Update ()
		{
				float delta = Time.time / 10f;
				Color E = Color.Lerp (Color.white, Color.black, delta);
				delta = delta % 3f;
				if (0f < delta && delta < 1f) {
						E = Color.Lerp (Color.red, Color.yellow, delta);
				} else if (1f < delta && delta < 2f) {
						E = Color.Lerp (Color.yellow, Color.green, delta - 1);
				} else if (2f < delta && delta < 3f) {
						E = Color.Lerp (Color.green, Color.red, delta - 2);
				}
				F.color = E;
		}

	public static void hookSettings(){
		minsize = 3;
		maxsize = 10;
		jumpDistance = 12;
		spawnEnemy = true; 
	}

	public static void blackHoleSettings(){
		minsize = 2.5f;
		maxsize = 5;
		jumpDistance = 2.5f;
		spawnEnemy = false;
	}

	public static void pogoStickSettings(){
		minsize = 5;
		maxsize = 10;
		jumpDistance = 5;
		spawnEnemy = false;
	}

	public static void gunSettings(){
		minsize = 2;
		maxsize = 3;
		jumpDistance = 1.5f;
		spawnEnemy = true;
	}

	void settingSetter(){
		switch (PlayerManager.useWeaponID) {
		case 1:
			pogoStickSettings ();
			break;
		case 3: 
			hookSettings ();
			break;
		case 4:
		case 5:
			gunSettings ();
			break;
		default:
			break;
		}
	}

	void setMoves(){
		Moves	= new Vector3[4][] {
			new Vector3[1]{new Vector3 (0f, 0f, jumpDistance)},
			new Vector3[1]{new Vector3 (0f, jumpDistance / 3f, jumpDistance / 2f)},
			new Vector3[2] {
				new Vector3 (0f, jumpDistance * 2f, jumpDistance * 2f),
				new Vector3 (0f, 0f, jumpDistance * 4f)
			},
			new Vector3[2] {
				new Vector3 (0f, jumpDistance * 2f, jumpDistance * 2f),
				new Vector3 (jumpDistance * 2f, 0f, jumpDistance * 2f)
			}
		};
	}

	void Aftermove(Vector3 Position){
		float Chance=0.5f;
		if(spawnEnemy){
			ObjectSpawner.SpawnObject(Position, Chance, "Coin");	
		}
		Chance=0.1f;
		if(true){ObjectSpawner.SpawnObject (Position, Chance, "Enemy");
		}
	}

	public void onFinish (Vector3 Position){
		Instantiate(endPoint,Position , Quaternion.identity);
	}

	public void onIteration(Vector3 Position){
		if (iteration%30 == 0) {
			Instantiate (CheckPoint, Position, Quaternion.identity);
		}
		if(iteration%200 == 0){
			onFinish(Position);
		}
		iteration++;
	}
}
