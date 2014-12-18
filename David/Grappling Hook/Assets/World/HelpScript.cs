using UnityEngine;
using System.Collections;

public class HelpScript : MonoBehaviour {

	public static int SumArray(int[] toBeSummed)
	{
		int sum = 0;
		
		foreach (int item in toBeSummed)
		{
			sum += item;
		}
		
		return sum;
	}

	public static int SumArray(int[,] toBeSummed)
	{
		int sum = 0;


		foreach (int item in toBeSummed)
		{
			sum += item;
		}
		
		return sum;
	}

	public static int SumArraySpecial(int[,,] toBeSummed)
	{
		int sum = 0;
		
		
		foreach (int item in toBeSummed)
		{
			if(item>0){sum ++;}
		}
		
		return sum;
	}

	public static int SumArraySpecial(int[,] toBeSummed)
	{
		int sum = 0;
		
		
		foreach (int item in toBeSummed)
		{
			if(item>0){sum ++;}
		}
		
		return sum;
	}

	public static float SumArray(float[] toBeSummed)
	{
		float sum = 0;
		
		foreach (float item in toBeSummed)
		{
			sum += item;
		}
		
		return sum;
	}

	public static int Switch(float[] chances){
		float Sum = SumArray (chances);
		if (Sum == 0) {
			Debug.Log ( "chances sum zero!");
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
			currentChance=currentChance+chances[i];
			if(b<currentChance){taken=i; break;}
			
		}
		return taken;
		
	}
}
