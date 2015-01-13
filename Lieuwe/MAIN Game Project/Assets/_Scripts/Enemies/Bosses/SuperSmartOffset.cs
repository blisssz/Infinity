using UnityEngine;
using System.Collections;

public class SuperSmartOffset : MonoBehaviour {
	private static int inputN = 3;
	private static int hiddenN = 3;
	private static int outputN = 3;
	private static float learningRate = 0.1f;
	private static float[][] wij;
	private static float[][] wjk;
	private static float[][] deltajk;
	private static float[][] deltaij;
	private static float[] dk;
	private static float[] yk;
	private static float[] ykd;
	private static float ek;
	private static float[] dj;
	private static float[] yj;
	private static float[] thetaj;
	private static float[] thetak;
	private static float[] deltathetaj;
	private static float[] deltathetak;
	private static float ej;
	private static float[] xi;
	private static Vector3 outp;

	// Use this for initialization
	void Start () {
		wij = random (inputN, hiddenN);
		wjk = random (hiddenN, outputN);
		deltaij = zeros (inputN, hiddenN);
		deltajk = zeros (hiddenN, outputN);
		dk = zeros (outputN);
		dj = zeros (hiddenN);
		yk = zeros (outputN);
		yj = zeros (hiddenN);
		thetaj = random (hiddenN);
		thetak = random (outputN);
		deltathetaj = zeros (hiddenN);
		deltathetak = zeros (outputN);
	}

	public static float[] getSmartOffset(Vector3 inp, Vector3 desiredResult){
		xi = vecToRow (inp);
		ykd = vecToRow (desiredResult);
		yj = calculateOutput (xi, wij, thetaj);
		yk = calculateOutput (yj, wjk, thetak);
		jkFeedback ();
		ijFeedback ();
		thetakFeedback ();
		thetajFeedback ();
		return yk;
	}

	static void jkFeedback(){
		for(int j = 0; j < wjk.Length; j++){
			for(int k = 0; k < wjk[j].Length; k++){

				ek = ykd[k] - yk[k];
				dk[k] = yk[k] * (1 - yk[k]) * ek;
				deltajk[j][k] = learningRate * yj[j] * dk[k];
				wjk [j] [k] += deltaij [j] [k];
			}
		}
	}

	static void ijFeedback(){
		for(int i = 0; i < wij.Length; i++){
			for(int j = 0; j < wij[i].Length; j++){
				dj[j] = yj [j] * (1 - yj [j]) * sum (puntproduct (dk, wjk [j]));
				deltaij[i][j] = learningRate * xi [i] * dj[i];
				wij [i] [j] += deltaij[i][j];
			}
		}
	}

	static void thetakFeedback(){
		for (int k = 0; k < thetak.Length; k++) {
			deltathetak[k] = -learningRate * dk[k];
			thetak[k] += deltathetak[k];
		}
	}

	static void thetajFeedback(){
		for (int j = 0; j < thetaj.Length; j++) {
			deltathetaj[j] = -learningRate * dj[j];
			thetaj[j] += deltathetaj[j];
		}
	}

	public static float[] matrixColSearch(float[][] matrix, int k){
		float[] col = new float[matrix [0].Length];
		for (int i = 0; i < matrix[0].Length; i++) {
			col[i] = matrix [i][k];
		}
		return col;
	}

	public static float sum(float[] row){
		float total = 0;
		foreach(float i in row){
			total += i;
		}
		return total;
	}

	public static float[] puntproduct(float[] x, float[] y){
		for(int i = 0; i < x.Length; i++){
			x[i] *= y[i];
		}
		return x;
	}

	public static float[][] puntproduct(float[][] x, float[][] y){
		for (int i = 0; i < x.Length; i++) {
			x[i] = puntproduct (x[i],y[i]);		
		}
		return x;
	}

	static float[] calculateOutput (float[] x, float[][] y, float[] theta){
		float total;
		float[] returnrow = new float[hiddenN];
		for (int j = 0; j < hiddenN; j++){
			float[] ycol = matrixColSearch (y, j);
			total = 0;
			for(int i = 0; i < x.Length; i++){
				total += x[i] * ycol[i];
			}
			returnrow[j] = sigmoid (total - theta[j]);
		}
		return returnrow;
	}

	public static float[][] zeros(int x, int y){
		float[][] returnmatrix = new float[y][];
		for(int i = 0; i < y; i++){
			returnmatrix[i] = zeros (x);
		}
		return returnmatrix;
	}

	public static float[] zeros(int x){
		float[] returnmatrix = new float[x];
		for(int i = 0; i < x; i++){
			returnmatrix[i] = 0;
		}
		return returnmatrix;
	}

	public static float[] random(int x){
		float[] returnrow = new float[x];
		for(int i = 0; i < x; i++){
			returnrow[i] = Random.value;
		}
		return returnrow;
	}

	public static float[][] random(int x, int y){
		float[][] returnmatrix = new float[y][];
		for(int i = 0; i < y; i++){
			returnmatrix[i] = random (x);
		}
		return returnmatrix;
	}

	public static Vector3 rowToVec(float[] row){
		Vector3 vec;
		vec.x = row [0];
		vec.y = row [1];
		vec.z = row [2];
		return vec;
	}

	public static float[] vecToRow(Vector3 vec){
		float[] row = new float[3];
		row [0] = vec.x;
		row [1] = vec.y;
		row [2] = vec.z;
		return row;
	}

	public static float sigmoid(float x){
		return 1 / (1 + Mathf.Exp (-x));
	}

	public static float[] sigmoid(float[] x){
		float[] returnrow = new float[x.Length];
		for(int i = 0; i < x.Length; i++){
			returnrow[i] = sigmoid (x[i]);
		}
		return returnrow;
	}

}
