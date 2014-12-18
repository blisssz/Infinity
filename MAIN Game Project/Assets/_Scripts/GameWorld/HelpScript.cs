using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpScript : MonoBehaviour
{



		public static Vector3 IntToVec3 (int[] ToVec,int factor)
		{
				Vector3 sum;

				sum.x = ToVec [0]*factor;
				sum.y = ToVec [1]*factor;
				sum.z = ToVec [2]*factor;

				return sum;
		}

	public static float[] Vec3ToFloat (Vector3 ToFloat,float factor)
	{
		float[] sum=new float[3];
		
		sum[0] = ToFloat [0]*factor;
		sum[1] = ToFloat [1]*factor;
		sum[2] = ToFloat [2]*factor;
		
		return sum;
	}

	public static float[][] Vec3ToFloat (Vector3[] ToFloat,float factor)
	{
		float[][] sum=new float[ToFloat.Length][];
		for(int i=0;i<ToFloat.Length;i++){
			sum[i]=Vec3ToFloat(ToFloat[i],factor);
		}
		
		return sum;
	}

		public static int SumArray (int[] toBeSummed)
		{
				int sum = 0;
		
				foreach (int item in toBeSummed) {
						sum += item;
				}
		
				return sum;
		}

		public static void DESTROYY (GameObject X)
		{
				Destroy (X);
		}

		public static int SumArray (int[,] toBeSummed)
		{
				int sum = 0;


				foreach (int item in toBeSummed) {
						sum += item;
				}
		
				return sum;
		}

		public static int SumArraySpecial (int[,,] toBeSummed)
		{
				int sum = 0;
		
		
				foreach (int item in toBeSummed) {
						if (item > 0) {
								sum ++;
						}
				}
		
				return sum;
		}

	public static string toStringSpecial (int[][] toString)
	{
		string sum="";
		
		
		for (int i=0;i<toString.Length;i++) {
			sum+="["+i+"]{";
			for (int j=0;j<toString[i].Length;j++) {
				sum+=","+ toString[i][j];
				
				
			}
			sum+=",}"+System.Environment.NewLine;
		}
		
		return sum;
	}

		public static int SumArraySpecial (int[,] toBeSummed)
		{
				int sum = 0;
		
		
				foreach (int item in toBeSummed) {
						if (item > 0) {
								sum ++;
						}
				}
		
				return sum;
		}

		public static float SumArray (float[] toBeSummed)
		{
				float sum = 0;
		
				foreach (float item in toBeSummed) {
						sum += item;
				}
		
				return sum;
		}

		public static int Switch (float[] chances)
		{
				float Sum = SumArray (chances);
				if (Sum == 0) {
						Debug.Log ("chances sum zero!");
				}
				if (Sum != 100) {
						for (int i =0; i<chances.Length; i++) {
								chances [i] = chances [i] * 100F / Sum;
						}
				}
				float currentChance = 0;
				float b = Random.Range (0, 100);
				int taken = 0;
				for (int i=0; i<chances.Length; i++) {
						currentChance = currentChance + chances [i];
						if (b < currentChance) {
								taken = i;
								break;
						}
			
				}
				return taken;
		
		}
}
