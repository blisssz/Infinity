using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour {
	public Text user;
	public InputField pass; 
	public string username;
	public string password;

	public void click(){
		username = user.text;
		password = pass.GetComponent<InputField>().text;
		ServerCommunication.username = username;
		ServerCommunication.password = password;
		ServerCommunication.shouldI = true;
	}

	void Update (){
		if (ServerCommunication.loggedIn) {
			Application.LoadLevel("Main Scene");	
		}
	}

	public void Register(){
		username = user.text;
		password = pass.GetComponent<InputField> ().text;
		ServerCommunication.username = username;
		ServerCommunication.password = password;
		ServerCommunication.goRegister = true;
	}

}
