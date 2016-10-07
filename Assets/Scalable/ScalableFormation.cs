using UnityEngine;
using System.Collections;

public class ScalableFormation : MonoBehaviour {
	

    public int formationNumber = 1;
    public StaticFollow[] units;

    public float spacing = .5f;


	public void changeFormation(int columns)
	{
		//-z is backwards, x is sideways.
		formationNumber = columns;
		float formationWidth = spacing * columns;
		
		for(int i = 0; i < units.Length; i++)
		{
			float x = formationWidth / 2 + ((i % columns) * spacing);
			float z = -(spacing + (i / columns) * formationWidth);
			units[i].moveTo(new Vector3(x, 0, z));
		}
	}

	// Use this for initialization
	void Start () {

		GameObject[] obj = GameObject.FindGameObjectsWithTag("follower");
		units = new StaticFollow[obj.Length];
		for (int i = 0; i < obj.Length; i++)
		{
			units[i] = obj[i].GetComponent<StaticFollow>();
			units[i].transform.parent = transform;
		}
		changeFormation(formationNumber);
	}
	/*
	// Update is called once per frame
	void Update () {
        switch (formationNumber)
        {
            case 0:
                linearFormation();
                break;
            case 1:
                twoFormation();
                break;
            default:
                linearFormation();
                break;
        }
	}

    private void linearFormation()
    {
        int i = 1;
        foreach(Transform t in unitTransforms)
        {
            if(t.gameObject.activeInHierarchy)
            {
               
                t.position = Vector3.Lerp(t.position, this.transform.position - this.transform.forward * i * linearFormSeperation, Time.deltaTime);
                ++i;
            }
            
        }

    }

    private void twoFormation()
    {
        int i = 1;
        foreach (Transform t in unitTransforms)
        {
            if (t.gameObject.activeInHierarchy)
            {
                if (i % 2 == 0)
                {
                    t.position = Vector3.Lerp(t.position, this.transform.position - (this.transform.forward * (i-1) * linearFormSeperation) + this.transform.right, Time.deltaTime);
                    ++i;
                } else if (i % 2 == 1)
                {
                    t.position = Vector3.Lerp(t.position, this.transform.position - (this.transform.forward * i * linearFormSeperation) - this.transform.right, Time.deltaTime);
                    ++i;
                }
            }

        }

    }
	*/
}
