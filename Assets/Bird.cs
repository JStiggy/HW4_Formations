using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{


	// Basic movement
	void Update()
	{
		if (Input.GetAxisRaw("Horizontal") == -1)
		{
			this.transform.position += Vector3.left * 5 * Time.deltaTime;
		}

		if (Input.GetAxisRaw("Horizontal") == 1)
		{
			this.transform.position += Vector3.right * 5 * Time.deltaTime;
		}

		if (Input.GetAxisRaw("Vertical") == 1)
		{
			this.transform.position += Vector3.forward * 5 * Time.deltaTime;
		}

		if (Input.GetAxisRaw("Vertical") == -1)
		{
			this.transform.position += Vector3.forward * -5 * Time.deltaTime;
		}
	}

	//If the units enter the black bird, disable the unit
	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Destroyable>())
		{
			Destroy(other.gameObject);
		}
	}
}