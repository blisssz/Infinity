using UnityEngine;
using System.Collections;

public class TextCreator  {

	public Vector3 StartPosition;
	public Vector3 OffSet=new Vector3(-0.2f,0,0);
	public Vector3 EnterOffSet=new Vector3(0,-2,0);
	private Vector3 Position;
	private int enters=0;

	public TextCreator(Vector3 Start){
		StartPosition=Start;
	}

	public void CreateText(string S){
		Position=StartPosition;
		float SizeLetter=0f;
		for(int i=0;i<S.Length;i++){
			char Char=S[i];
			if(Char==' '){
				Position+=OffSet+new Vector3(-SizeLetter,0,0);
			} else if(Char==';'){
				enters++;
				Position=StartPosition+EnterOffSet*enters;
			} else{
				GameObject Letter=ObjectSpawner.SpawnObjectWith(Position,"Letters/Text" + Char);
			Letter.AddComponent<Letter>();
				Letter.AddComponent("Rigidbody");
				Letter.rigidbody.useGravity = false;
				Letter.AddComponent("MeshCollider");
			SizeLetter=Letter.renderer.bounds.size.x;
			Position+=OffSet+new Vector3(-SizeLetter,0,0);
			}
			}
	}
}
