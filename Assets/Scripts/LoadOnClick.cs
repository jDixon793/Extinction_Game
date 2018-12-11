using UnityEngine;
using System.Collections;
using UnityEngine.UI;
	
public class LoadOnClick : MonoBehaviour {

	void Awake () 
	{

	}
	public void LoadScene(int level)
	{

		Application.LoadLevel(level);
	}
}