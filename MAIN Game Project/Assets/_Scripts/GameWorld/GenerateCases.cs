using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class GenerateCases {

	private static int found=0;
	private static List<int> FoundPositions=new List<int>();
	private static List<List<int>> VerticePositions=new List<List<int>>();
	private static int[][] NearPositions=new int[8][]{new int[3]{1,2,4},new int[3]{0,3,5},new int[3]{0,3,6},new int[3]{1,2,7},new int[3]{0,5,6},new int[3]{1,4,7},new int[3]{2,4,7},new int[3]{3,5,6}};
	public static int[][] Cases=new int[256][];
	private static int NumberOfTriangles;
	private static int u =0;
	private static List<List<int>> FinalFoundPositions=new List<List<int>>();
	private static List<List<List<int>>> FinalVerticePositions=new List<List<List<int>>>();

	public static void GenCases(){
		int[] a=new int[8];
		
		for(a[0]=0;a[0]<2;a[0]++){
			for(a[1]=0;a[1]<2;a[1]++){
				for(a[2]=0;a[2]<2;a[2]++){
					for(a[3]=0;a[3]<2;a[3]++){
						for(a[4]=0;a[4]<2;a[4]++){
							for(a[5]=0;a[5]<2;a[5]++){
								for(a[6]=0;a[6]<2;a[6]++){
									for(a[7]=0;a[7]<2;a[7]++){
										NumberOfTriangles=-1;
										FoundPositions=new List<int>();
										VerticePositions=new List<List<int>>();
										for(int ii=0;ii<8;ii++){
											bool e=true;
											
											if(a[ii]!=0){
												for(int j=0;j<FoundPositions.Count;j++){
													if(FoundPositions[j]==ii){
														e=false;
														
													} 
												}
												if(e){
													NumberOfTriangles++;
													VerticePositions.Add (new List<int>());
													DurCase(ii,a);
												}
											}
											
											
											if(NumberOfTriangles>8){Debug.Log (99);break;}
										}
										
										VerticeToTriangles();
										FinalFoundPositions.Add(FoundPositions);
										FinalVerticePositions.Add (VerticePositions);
										u++;
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	public static void DurCase(int i,int[] a){
		
		FoundPositions.Add (i);
		found++;
		
		int[] b=NearPositions[i];
		for(int j=0;j<3;j++){
			bool already1=true;
			bool already2=true;
			if(a[b[j]]==0&&VerticePositions[NumberOfTriangles].Count==0){
				
				VerticePositions[NumberOfTriangles].Add (b[j]);
			}else if(a[b[j]]==0){
				for(int k=0;k<VerticePositions[NumberOfTriangles].Count;k++){
					
					if(VerticePositions[NumberOfTriangles][k]==b[j]){already2=false;} 
					
				}
				if(already2){VerticePositions[NumberOfTriangles].Add (b[j]);}
			} else {
				for(int k=0;k<FoundPositions.Count;k++){
					if(FoundPositions[k]==b[j]){already1=false;} 
					
				}
				if(already1){DurCase (b[j],a);}
			}
		}
		
	}
	
	public static void VerticeToTriangles(){
		List<int> Triangle=new List<int>();
		for(int j=0;j<VerticePositions.Count;j++){
			if(VerticePositions[j].Count==3){
				Triangle.Add (VerticePositions[j][0]);
				Triangle.Add (VerticePositions[j][2]);
				Triangle.Add (VerticePositions[j][1]);
			} else if (VerticePositions[j].Count==4){
				int Opposite=-1;
				int[] x=new int[4];
				int[] y=new int[4];
				int[] z=new int[4];
				for(int k=0;k<4;k++){
					x[k]=VerticePositions[j][k]/4;
					y[k]=(VerticePositions[j][k]-x[k]*4)/2;
					z[k]=(VerticePositions[j][k]-x[k]*4-y[k]*2);
				}
				int D=0;
				int[] righ=new int[4];
				for(int k1=0;k1<4;k1++){
					for(int k2=0;k2<4;k2++){
						if(Mathf.Abs(x[k1]-x[k2])+Mathf.Abs(y[k1]-y[k2])+Mathf.Abs(z[k1]-z[k2])>D){
							D=Mathf.Abs(x[k1]-x[k2])+Mathf.Abs(y[k1]-y[k2])+Mathf.Abs(z[k1]-z[k2]);
							righ[0]=k1;righ[1]=k2;
							for(int k3=0;k3<4;k3++){
								if(k3!=k1&&k3!=k2){righ[2]=k3; break;}
							}
							
							for(int k4=0;k4<4;k4++){
								if(k4!=k1&&k4!=k2&&k4!=righ[2]){righ[3]=k4; break;}
							}
						};
					}
				}
				Triangle.Add (VerticePositions[j][righ[2]]);
				Triangle.Add (VerticePositions[j][righ[0]]);
				Triangle.Add (VerticePositions[j][righ[3]]);
				Triangle.Add (VerticePositions[j][righ[3]]);
				Triangle.Add (VerticePositions[j][righ[1]]);
				Triangle.Add (VerticePositions[j][righ[2]]);
				
			}
		}
		Cases[u]=Triangle.ToArray();
	}

	public static void WriteToFile(){
		string x=HelpScript.toStringSpecial(Cases);
		StreamWriter file2 = new StreamWriter(@"Assets\World\filexx.txt");
		file2.WriteLine(x);
		file2.Close();
		Debug.Log ("Succeeded");

	}

	public static void ReadFromFile(){

		StreamReader sr = new StreamReader(@"Assets\World\filex.txt");

		for(int i=0;i<256;i++){

			//Debug.Log (i);
			string[] x=(sr.ReadLine().Split (','));
			Cases[i]=new int[x.Length-2];
			for(int j=1;j<x.Length-1;j++){
				Cases[i][j-1]=int.Parse(x[j]);
				//Debug.Log(i + " " + int.Parse(x[j]));
			}
		}

		Debug.Log(HelpScript.toStringSpecial(Cases));
	}

	public static void SwapCases(){
		for(int i=0;i<Cases.Length;i++){

			for(int j=0;j<Cases[i].Length/3;j++){
				int temp=Cases[i][3*j];
				Cases[i][3*j]=Cases[i][3*j+2];
				Cases[i][3*j+2]=temp;
				
			}

		}

	}

}
