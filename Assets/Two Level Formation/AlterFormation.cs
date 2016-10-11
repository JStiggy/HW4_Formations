using UnityEngine;
using System.Collections;

public class AlterFormation : MonoBehaviour {

    public int formationType = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Leader")
        {
            other.GetComponent<TwoLevelFormation>().formationNumber = formationType;
        }
    }

}
