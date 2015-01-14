using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class GenerateCases
{
	public static bool done=false;
    private static int found = 0;
    private static List<int> FoundPositions = new List<int>();
    private static List<List<int>> VerticePositions = new List<List<int>>();
	private static string Path=Application.dataPath;
    private static int[][] NearPositions = new int[8][] {
                new int[3]{1,4,2},
                new int[3]{3,5,0},
                new int[3]{0,6,3},                new int[3] {
                        1,
                        2,
                        7
                },
                new int[3] {
                        0,
                        5,
                        6
                },
                new int[3] {
                        7,
                        4,
                        1
                },
                new int[3] {
                        2,
                        4,
                        7
                },
                new int[3] {
                        6,
                        5,
                        3
                }
        };
		private static int[][] PointsPerPlane = new int[6][] {
                new int[4]{0,1,3,2},
                new int[4] {
                        0,
                        1,
                        5,
                        4
                },
                new int[4] {
                        0,
                        2,
                        6,
                        4
                },
                new int[4] {
                        1,
                        3,
                        7,
                        5
                },
                new int[4] {
                        2,
                        3,
                        7,
                        6
                },
                new int[4] {
                        4,
                        5,
                        7,
                        6
                }
        };
    public static int[][] Cases = new int[256][];
    private static int NumberOfPoints;
    private static int u = 0;
    private static List<List<int>> FinalFoundPositions = new List<List<int>>();
    private static List<List<List<int>>> FinalVerticePositions = new List<List<List<int>>>();

    public static int getRealValue(int value, int edge)
    {
        int answer = (value % edge + 10 * edge) % edge;
        return answer % edge;
    }

    public static int[] FindPlane(int Corner1, int Corner2)
    {
        int[] planes = new int[2]{-1,-1};
        int index = 0;
        for (int i=0; i<6; i++)
        {
            bool a1 = false;
            bool a2 = false;

            for (int j=0; j<PointsPerPlane[i].Length; j++)
            {
                if (Corner1 == PointsPerPlane [i] [j])
                {
                    a1 = true;
                }
                if (Corner2 == PointsPerPlane [i] [j])
                {
                    a2 = true;
                }
            }
            if (a1 && a2)
            {
                planes [index] = i;
                index++;
            }
        }
        return planes;
    }

    public static int FindPlane(int Corner1, int Corner2, int Corner3)
    {
        int planes = -1;
        for (int i=0; i<6; i++)
        {
            bool a1 = false;
            bool a2 = false;
            bool a3 = false;

            for (int j=0; j<PointsPerPlane[i].Length; j++)
            {
                if (Corner1 == PointsPerPlane [i] [j])
                {
                    a1 = true;
                }
                if (Corner2 == PointsPerPlane [i] [j])
                {
                    a2 = true;
                }
                if (Corner3 == PointsPerPlane [i] [j])
                {
                    a3 = true;
                }
            }
            if (a1 && a2 && a3)
            {
                planes = i;
                return planes;
            }
        }
        return planes;
    }

    public static int GetIndexInPlane(int plane, int Corner)
    {
        for (int j=0; j<PointsPerPlane[plane].Length; j++)
        {
            if (Corner == PointsPerPlane [plane] [j])
            {
                return j;
            }
        }
        return -1;
    }

    public static int FindinBetween(int Corner1, int Corner2)
    {
        int answer = -1;
        if (Corner1 == 0 && Corner2 == 1 || Corner2 == 0 && Corner1 == 1)
        {
            return 0;
        }
        if (Corner1 == 0 && Corner2 == 2 || Corner2 == 0 && Corner1 == 2)
        {
            return 1;
        }
        if (Corner1 == 0 && Corner2 == 4 || Corner2 == 0 && Corner1 == 4)
        {
            return 2;
        }
        if (Corner1 == 1 && Corner2 == 3 || Corner2 == 1 && Corner1 == 3)
        {
            return 3;
        }
        if (Corner1 == 1 && Corner2 == 5 || Corner2 == 1 && Corner1 == 5)
        {
            return 4;
        }
        if (Corner1 == 2 && Corner2 == 3 || Corner2 == 2 && Corner1 == 3)
        {
            return 5;
        }
        if (Corner1 == 2 && Corner2 == 6 || Corner2 == 2 && Corner1 == 6)
        {
            return 6;
        }
        if (Corner1 == 3 && Corner2 == 7 || Corner2 == 3 && Corner1 == 7)
        {
            return 7;
        }
        if (Corner1 == 4 && Corner2 == 5 || Corner2 == 4 && Corner1 == 5)
        {
            return 8;
        }
        if (Corner1 == 4 && Corner2 == 6 || Corner2 == 4 && Corner1 == 6)
        {
            return 9;
        }
        if (Corner1 == 5 && Corner2 == 7 || Corner2 == 5 && Corner1 == 7)
        {
            return 10;
        }
        if (Corner1 == 6 && Corner2 == 7 || Corner2 == 6 && Corner1 == 7)
        {
            return 11;
        }
        return -1;
    }

    public static void GenCases2()
    {
		u = 0;
        int[] a = new int[8];

        for (a[0]=0; a[0]<2; a[0]++)
        {
            for (a[1]=0; a[1]<2; a[1]++)
            {
                for (a[2]=0; a[2]<2; a[2]++)
                {
                    for (a[3]=0; a[3]<2; a[3]++)
                    {
                        for (a[4]=0; a[4]<2; a[4]++)
                        {
                            for (a[5]=0; a[5]<2; a[5]++)
                            {
                                for (a[6]=0; a[6]<2; a[6]++)
                                {
                                    for (a[7]=0; a[7]<2; a[7]++)
                                    {
                                        NumberOfPoints = -1;
                                        FoundPositions = new List<int>();
                                        VerticePositions = new List<List<int>>();
                                        for (int CornerPosition=0; CornerPosition<8; CornerPosition++)
                                        {
                                            bool e = true;

                                            if (a [CornerPosition] != 0)
                                            {
                                                for (int j=0; j<FoundPositions.Count; j++)
                                                {
                                                    if (FoundPositions [j] == CornerPosition)
                                                    {
                                                        e = false;

                                                    }
                                                }
                                                if (e)
                                                {

                                                    int[] b = NearPositions [CornerPosition];
                                                    for (int j=0; j<3; j++)
                                                    {
                                                        if (a [b [j]] == 0)
                                                        {
                                                            NumberOfPoints++;
                                                            VerticePositions.Add(new List<int>());
                                                            StartLine(CornerPosition, a, b [j], -1);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }


                                            if (NumberOfPoints > 8)
                                            {
                                                Debug.Log(99);
                                                break;
                                            }
                                        }

                                        VerticeToTriangles();
                                        FinalFoundPositions.Add(FoundPositions);
                                        FinalVerticePositions.Add(VerticePositions);
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

    public static void StartLine(int cornerPosition, int[] a, int OtherCorner, int lastplane)
    {

        int otherIndex = -1;

        for (int i=0; i<3; i++)
        {
            if (NearPositions [cornerPosition] [i] == OtherCorner)
            {
                otherIndex = i;
                break;
            }
            ;
        }
        FoundPositions.Add(cornerPosition);
        found++;
        int newPlane = -1;
        bool bigger = false;
        if (VerticePositions [VerticePositions.Count - 1].Count > 1)
        {
            bigger = true;
        }
        if (!bigger || (VerticePositions [VerticePositions.Count - 1] [0] != FindinBetween(cornerPosition, OtherCorner)))
        {
            VerticePositions [VerticePositions.Count - 1].Add(FindinBetween(cornerPosition, OtherCorner));
            if (lastplane != FindPlane(cornerPosition, OtherCorner, NearPositions [cornerPosition] [(otherIndex + 1) % 3]))
            {
                newPlane = FindPlane(cornerPosition, OtherCorner, NearPositions [cornerPosition] [(otherIndex + 1) % 3]);
            } else if (lastplane != FindPlane(cornerPosition, OtherCorner) [0])
            {
                newPlane = FindPlane(cornerPosition, OtherCorner) [0];
            } else if (lastplane != FindPlane(cornerPosition, OtherCorner) [1])
            {
                newPlane = FindPlane(cornerPosition, OtherCorner) [1];
            }
            int[] b = PointsPerPlane [newPlane];
            int index = GetIndexInPlane(newPlane, cornerPosition);
            int Direction = 0;
            if (getRealValue(index - 1, 4) == GetIndexInPlane(newPlane, OtherCorner))
            {
                Direction = 1;
            }
            if (getRealValue(index + 1, 4) == GetIndexInPlane(newPlane, OtherCorner))
            {
                Direction = -1;
            }
            for (int i=index; i<index+4&&i>index-4; i+=Direction)
            {
                int index2 = getRealValue(i, 4);
                if (a [b [index2]] == 0)
                {
                    int a1 = getRealValue(index2 - Direction, 4);
                    int a2 = getRealValue(index2, 4);
                    StartLine(b [getRealValue(index2 - Direction, 4)], a, b [index2], newPlane);
                    break;
                }
            }
        }

    }

    public static void GenCases()
    {
        int[] a = new int[8];

        for (a[0]=0; a[0]<2; a[0]++)
        {
            for (a[1]=0; a[1]<2; a[1]++)
            {
                for (a[2]=0; a[2]<2; a[2]++)
                {
                    for (a[3]=0; a[3]<2; a[3]++)
                    {
                        for (a[4]=0; a[4]<2; a[4]++)
                        {
                            for (a[5]=0; a[5]<2; a[5]++)
                            {
                                for (a[6]=0; a[6]<2; a[6]++)
                                {
                                    for (a[7]=0; a[7]<2; a[7]++)
                                    {
                                        NumberOfPoints = -1;
                                        FoundPositions = new List<int>();
                                        VerticePositions = new List<List<int>>();
                                        for (int CornerPosition=0; CornerPosition<8; CornerPosition++)
                                        {
                                            bool e = true;

                                            if (a [CornerPosition] != 0)
                                            {
                                                for (int j=0; j<FoundPositions.Count; j++)
                                                {
                                                    if (FoundPositions [j] == CornerPosition)
                                                    {
                                                        e = false;

                                                    }
                                                }
                                                if (e)
                                                {
                                                    NumberOfPoints++;
                                                    VerticePositions.Add(new List<int>());
                                                    DurCase(CornerPosition, a);
                                                }
                                            }


                                            if (NumberOfPoints > 8)
                                            {
                                                Debug.Log(99);
                                                break;
                                            }
                                        }

                                        VerticeToTriangles();
                                        FinalFoundPositions.Add(FoundPositions);
                                        FinalVerticePositions.Add(VerticePositions);
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

    public static void DurCase(int cornerPosition, int[] a)
    {

        FoundPositions.Add(cornerPosition);
        found++;

        int[] b = NearPositions [cornerPosition];
        for (int j=0; j<3; j++)
        {
            bool already1 = true;
            bool already2 = true;
            if (a [b [j]] == 0 && VerticePositions [NumberOfPoints].Count == 0)
            {

                VerticePositions [NumberOfPoints].Add(FindinBetween(cornerPosition, b [j]));
            } else if (a [b [j]] == 0)
            {
                for (int k=0; k<VerticePositions[NumberOfPoints].Count; k++)
                {

                    if (VerticePositions [NumberOfPoints] [k] == FindinBetween(cornerPosition, b [j]))
                    {
                        already2 = false;
                    }

                }
                if (already2)
                {
                    VerticePositions [NumberOfPoints].Add(FindinBetween(cornerPosition, b [j]));
                }
            } else
            {
                for (int k=0; k<FoundPositions.Count; k++)
                {
                    if (FoundPositions [k] == b [j])
                    {
                        already1 = false;
                    }

                }
                if (already1)
                {
                    DurCase(b [j], a);
                }
            }
        }

    }

    public static void VerticeToTriangles()
    {
        List<int> Triangle = new List<int>();
        for (int j=0; j<VerticePositions.Count; j++)
        {
            List<int> Positions = VerticePositions [j];
            if (Positions.Count == 3)
            {
                Triangle.Add(Positions [0]);
                Triangle.Add(Positions [1]);
                Triangle.Add(Positions [2]);
            } else if (Positions.Count > 3)
            {int mini=1;
              int maxi=0;

				for(int i=0;i<Positions.Count-2;i++){
                    List<int> Tri=new List<int>();
                    Tri.Add(mini);
                    Tri.Add(maxi);
                    if(i%2==0){ mini++;Tri.Add(mini);}
                    if(i%2==1){ maxi--;if(maxi==-1){maxi=Positions.Count-1;}Tri.Add (maxi);};
                    Tri.Sort ();
                    Triangle.Add(Positions [Tri[0]]);
                    Triangle.Add(Positions [Tri[1]]);
                    Triangle.Add(Positions [Tri[2]]);
				}

            }
        }
        Cases [u] = Triangle.ToArray();
    }

    public static void WriteToFile()
    {
        string x = HelpScript.toStringSpecial(Cases);
        StreamWriter file2 = new StreamWriter(@"C:\Users\LieuweLocht\Documents\Minor1\Assets\World\Data.txt");
        file2.WriteLine(x);
        file2.Close();
        Debug.Log("Succeeded");

    }

    public static void ReadFromFile()
    {

        StreamReader sr = new StreamReader(@"C:\Users\LieuweLocht\Documents\Minor1\Assets\World\filex.txt");

        for (int i=0; i<256; i++)
        {

            //Debug.Log (i);
            string[] x = (sr.ReadLine().Split(','));
            Cases [i] = new int[x.Length - 2];
            for (int j=1; j<x.Length-1; j++)
            {
                Cases [i] [j - 1] = int.Parse(x [j]);
                //Debug.Log(i + " " + int.Parse(x[j]));
            }
        }

        Debug.Log(HelpScript.toStringSpecial(Cases));
    }

    public static void SwapCases()
    {
        for (int i=0; i<Cases.Length; i++)
        {

            for (int j=0; j<Cases[i].Length/3; j++)
            {
                int temp = Cases [i] [3 * j];
                Cases [i] [3 * j] = Cases [i] [3 * j + 2];
                Cases [i] [3 * j + 2] = temp;

            }

        }

    }

}
