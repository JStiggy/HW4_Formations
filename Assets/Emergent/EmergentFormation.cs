using UnityEngine;
using System.Collections;

public class EmergentFormation : MonoBehaviour
{
	public bool isLeader = false;
	public Node[] children;

	public ObstacleAvoidance agent;

	public Slot targetPrefab;

	public Transform target;

	public float shynessRange = 1;

	public static EmergentFormation root;
	public EmergentFormation parent;
	float defaultSpeed;
	public float panicAcceleration;
	public float panicRange = 2;

	[System.Serializable]
	public class Node
	{
		public float blocked = 0;
		//bool blocked;
		public EmergentFormation occupier;
		public Vector3 relativePosition;
	}

	public Node getNode(EmergentFormation obj)
	{
		for (int i = 0; i < children.Length; i++)
		{
			if (children[i].occupier == obj)
			{
				return children[i];
			}
		}
		return null;
	}

	public void remove(EmergentFormation obj)
	{
		for (int i = 0; i < children.Length; i++)
		{
			if (children[i].occupier == obj)
			{
				children[i].blocked = Time.time;
				children[i].occupier = null;
			}
		}
	}

	public bool place(EmergentFormation obj)
	{
		for (int i = 0; i < children.Length; i++)
		{
			
			if (children[i].blocked == 0 && !children[i].occupier)
			{
				children[i].occupier = obj;
				obj.parent = this;
				return true;
			}
		}
		for (int i = 0; i < children.Length; i++)
		{
			if (children[i].occupier && children[i].occupier.place(obj))
			{
				return true;
			}
		}
		if (isLeader)
		{
			refresh();
			isLeader = false;
			if (!place(obj))
			{
				obj.isLeader = true;
				isLeader = true;
			}
			else
			{
				isLeader = true;
				return true;
			}
		}
		return false;
	}
	public void refresh()
	{
		for (int i = 0; i < children.Length; i++)
		{
			children[i].blocked = 0;
			if (children[i].occupier)
			{
				children[i].occupier.refresh();
			}
		}
	}
	void Awake()
	{
		if (isLeader)
		{
			root = this;
		}
	}

	void Start()
	{
		agent = GetComponent<ObstacleAvoidance>();
		if (!isLeader)
		{
			Slot t = Instantiate(targetPrefab);		
			target = t.transform;
			t.owner = this;
			defaultSpeed = agent.maxLinearSpeed;
			agent.target = target.gameObject;
			reposition();
		}
	}

	public void reposition()
	{
		if (isLeader) { return; }
		if (parent)
		{
			parent.remove(this);
		}
		if (root.place(this))
		{
			target.parent = parent.transform;
			target.localPosition = parent.getNode(this).relativePosition;
			if (!target.GetComponent<Slot>().ok())
			{
				reposition();
				return;
			}
			//agent.target = target.gameObject;
		}
		else
		{
			print("ERROR CAN'T BE PLACED");
		}
		for(int i = 0; i < children.Length; i++)
		{
			if (children[i].occupier)
			{
				children[i].occupier.reposition();
			}
		}
	}

	
	public bool catchingUp = true;
	void Update()
	{
		if (isLeader)
		{
			return;
		}
		Debug.DrawLine(transform.position, target.transform.position,Color.cyan);
		if ((transform.position - target.position).magnitude > panicRange)
		{//we're catching up to the spot in our formation.
			agent.maxLinearSpeed = panicAcceleration;
			catchingUp = true;
		}
		else {
			catchingUp = false;
			agent.maxLinearSpeed = defaultSpeed;
			/*Collider[] col = Physics.OverlapSphere(transform.position, shynessRange, (1 << (LayerMask.NameToLayer("Unit")) | (1 << LayerMask.NameToLayer("Obstacle"))));
			//print("Unit: " + (1<<LayerMask.NameToLayer("Unit"))+ " obstacle " + (1<<LayerMask.NameToLayer("Obstacle")) + " total " + (1<<(LayerMask.NameToLayer("Unit")) | (1<<LayerMask.NameToLayer("Obstacle"))));
			if (col.Length > 1)
			{

				print("colliding!");
				for(int i = 0; i < col.Length; i++)
				{
					EmergentFormation other = col[i].GetComponent<EmergentFormation>();
					if (other && col[i].GetComponent<EmergentFormation>().catchingUp)
					{

					}
					else
					{ 
						parent.getNode(this).blocked = Time.time;
						reposition();
					}
					print(col[i]);
				}
			}*/
		}
	}

	void OnDestroy()
	{
		if (parent)
		{
			parent.remove(this);
		}
		if (target)
		{
			Destroy(target.gameObject);
		}
		Slot[] childSlots = GetComponentsInChildren<Slot>();
		for(int i = 0; i < childSlots.Length; i++)
		{
			childSlots[i].transform.parent = null;
		}
		for(int i = 0; i < children.Length; i++)
		{
			if (children[i].occupier&&root)
			{
				print("placing child.");
				root.place(children[i].occupier);
				children[i].occupier.target.parent = children[i].occupier.parent.transform;//yeah this would be refactored if it wasn't the last thing to be added. 
				children[i].occupier.target.localPosition = children[i].occupier.parent.getNode(children[i].occupier).relativePosition;
			}
		}

	}
}
