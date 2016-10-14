using UnityEngine;
using System.Collections;

public class ScalableFormation : ConnorFormation
{


	public int formationNumber = 1;
	public StaticFollow[] units;

	public float spacing = .5f;

	public ObstacleAvoidance agent;
	float maxAV;
	public float whiskers = 1;
	public float avoidSpeed = 1;


	public override void changeFormation(int columns)
	{
		//-z is backwards, x is sideways.
		formationNumber = columns;
		float formationWidth = spacing * columns - spacing;

		for (int i = 0; i < units.Length; i++)
		{
			float x = formationWidth / 2 - ((i % columns) * spacing);
			//print(x);
			float z = -(spacing + (i / columns) * spacing);
			units[i].moveTo(new Vector3(x, 0, z));
		}
	}

	// Use this for initialization
	void Start()
	{
		agent = GetComponent<ObstacleAvoidance>();
		GameObject[] obj = GameObject.FindGameObjectsWithTag("follower");
		units = new StaticFollow[obj.Length];
		for (int i = 0; i < obj.Length; i++)
		{
			units[i] = obj[i].GetComponent<StaticFollow>();
			units[i].transform.parent = transform;
			units[i].transform.localEulerAngles = new Vector3(0, 0, 0);
		}
		changeFormation(formationNumber);
	}

	void Update()
	{
		if (proximityAlert())
		{
			print("proximityAlert!");
			agent.canRotate = false;
		}
		else
		{
			agent.canRotate = true;
		}
	}


	bool proximityAlert()
	{
		RaycastHit info;
		Transform leftAgent = units[((units.Length - 1) / formationNumber) * formationNumber].transform;
		Debug.DrawLine(leftAgent.position, leftAgent.position+leftAgent.right*whiskers);
		if (Physics.Raycast(leftAgent.position, leftAgent.right, out info, whiskers, LayerMask.GetMask("Obstacle")))
		{
			if (!info.transform.GetComponent<ScalableFormation>())
			{
				transform.Translate(-agent.maxLinearSpeed * Time.deltaTime * avoidSpeed, 0, 0);
				return true;
			}
		}

		Transform rightAgent = units[(units.Length - 1)].transform;
		Debug.DrawLine(rightAgent.position, rightAgent.position - rightAgent.right * whiskers);
		if (Physics.Raycast(rightAgent.position, -rightAgent.right, out info, whiskers, LayerMask.GetMask("Obstacle"))) 
		{
			if (!info.transform.GetComponent<ScalableFormation>())
			{
				transform.Translate(agent.maxLinearSpeed * Time.deltaTime * avoidSpeed, 0, 0);
				return true;
			}
		}
		return false;
	}
}