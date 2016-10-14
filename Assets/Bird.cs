using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {
	/// <summary>
	/// Follows the mouse and destroys any object with a destroyable component. 
	/// </summary>
	
	// Update is called once per frame
	void Update () {
		RaycastHit info;
		Physics.Raycast(Camera.current.ScreenPointToRay(Input.mousePosition), out info);
		//print(Camera.current + "  " + Input.mousePosition + "  " + info);
		transform.position = info.point;

	}

	void OnTriggerEnter(Collider col)
	{
		print("overlapping" + col.gameObject);
		if (col.GetComponent<Destroyable>())
		{
			Destroy(col.gameObject);
		}
	}
}
