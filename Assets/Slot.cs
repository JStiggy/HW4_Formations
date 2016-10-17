using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {
	public int[] layersToCollide;
	public int overlapTolerance = 5;
	public int overlapTimes = 0;
	public EmergentFormation owner;
	// Use this for initialization
	void Start () {
	
	}
	void Update()
	{
		if(Random.value < .05)
		{
			int hit = 0;
			Collider[] obj = Physics.OverlapSphere(transform.position, .1f);
			for(int i = 0; i < obj.Length; i++)
			{
				if (obj[i].GetComponent<Slot>())
				{
					hit++;
					if (overlapTimes < overlapTolerance)
					{
						overlapTimes += 1;
					}
					else
					{
						block();
					}
				}
			}
			if (hit <= 1)
			{
				overlapTimes-=2;
			}
		}
		overlapTimes = Mathf.Clamp(overlapTimes, 0, overlapTolerance + 1);
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
				block();
			}
		}
	}
	void block()
	{
		owner.parent.getNode(owner).blocked = Time.deltaTime;
		owner.reposition();
	}
}
