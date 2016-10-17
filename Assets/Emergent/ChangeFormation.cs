using UnityEngine;
using System.Collections;

public class ChangeFormation : MonoBehaviour
{

    public int formationType = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Leader")
        {
            EmergentFormation form = other.GetComponent<EmergentFormation>();
            form.children = changeChildren();
        }
    }

    EmergentFormation.Node[] changeChildren()
    {
        EmergentFormation.Node[] n;
        switch (formationType)
        {
            case 0:
                n = new EmergentFormation.Node[1];
                n[0] = new EmergentFormation.Node();
                n[0].relativePosition = this.transform.forward * -2;
                break;
            default:
                n = new EmergentFormation.Node[3];
                n[0] = new EmergentFormation.Node();
                n[0].relativePosition =  new Vector3(2, 0, -2);

                n[1] = new EmergentFormation.Node();
                n[1].relativePosition = new Vector3(0, 0, -2);

                n[2] = new EmergentFormation.Node();
                n[2].relativePosition = new Vector3(-2, 0, -2);
                break;
        }
        return n;
    }

}
