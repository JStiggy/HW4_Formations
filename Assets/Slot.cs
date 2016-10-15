using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {
	public int[] layersToCollide;

	public EmergentFormation owner;
	// Use this for initialization
	void Start () {
	
	}
	
	public bool ok()
	{
		Collider[] obj = Physics.OverlapSphere(transform.position, .5f);
		for(int i = 0; i < obj.Length; i++)
		{
			for(int j = 0; j < layersToCollide.Length; j++)
			{
				if (obj[i].gameObject.layer == layersToCollide[j])
				{
					return false;
				}
			}
		}
		return true;
	}
	void OnTriggerEnter(Collider col)
	{
		for(int i = 0; i < layersToCollide.Length; i++)
		{
			if(col.gameObject.layer == layersToCollide[i] && col.gameObject != owner.gameObject)
			{
				
				owner.parent.getNode(owner).blocked = Time.deltaTime;
				owner.reposition();
				
			}
		}
	}
}
