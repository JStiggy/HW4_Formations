using UnityEngine;
using System.Collections;

public class StaticFollow : MonoBehaviour {

	public Vector3 start;
	public Vector3 end;
	public float speed;

	bool moving;


	float dist;
	float progress;
	Transform t;

	void Awake()
	{

		t = transform;
	}
	public void moveTo(Vector3 dest)
	{
		progress = 0;
		end = dest; 
		moving = true;
		start = t.localPosition;
		dist = (start - end).magnitude;
		if (dist == 0)
		{
			moving = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
		{
			progress += speed * Time.deltaTime;
			t.localPosition = Vector3.Lerp(start, end, progress / dist);
			if(progress/dist > 1)
			{
				moving = false;
			}
		}
	}

	void OnDestroy()
	{
		if (GetComponentInParent<ScalableFormation>())
		{
			GetComponentInParent<ScalableFormation>().Recalculate();
		}
	}
	
}
