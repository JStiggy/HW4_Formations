using UnityEngine;
using System.Collections;

public class FormationChange : MonoBehaviour {
	public int newWidth = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider obj)
	{
		print("entering");
		ConnorFormation temp = obj.GetComponent<ConnorFormation>();
		if (temp)
		{
			temp.changeFormation(newWidth);
		}
	}
}
