using UnityEngine;
using System.Collections;

public class ScalableFormation : MonoBehaviour {

    public int formationNumber = 1;
    public Transform[] unitTransforms;

    public float linearFormSeperation = 2f;

	// Use this for initialization
	void Start () {

	}
	
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

}
