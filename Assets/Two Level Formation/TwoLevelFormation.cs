using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TwoLevelFormation : MonoBehaviour
{
    //Current target for the invisible leader
    public Node currentNode;
    public GameObject target;

    //Values used to control the movement of the leader
    public float linearSpeed;
    public float maxLinearSpeed;
    public float maxLinearAcceleration;
    public float maxAngularAcceleration;
    public float slowDistance;

    //The current formation being deployed by the followers
    public int formationNumber = 1;

    //Values used to determine the positions of all units
    public List<CollisionPrediction> followerUnits;
    public List<Vector3> formPositions;

    //Used as offset values for each formation
    float linearFormSeperation = 3f;
    float twoFormationSeperation = 2f;

    void Start()
    {
        //Seek the next node
        target = currentNode.gameObject;

        //Get all units in the formation and add them to the list
        followerUnits = new List<CollisionPrediction>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Unit"))
        {
            CollisionPrediction c = go.GetComponent<CollisionPrediction>();
            c.formation = this;
            followerUnits.Add(c);
        }

        //Set up a list of positions for the formation based on the current position of the leader
        formPositions = new List<Vector3>();
        formPositions.Clear();
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
        //Determine which units correspond to which slots
        delegatePositions();
    }

    // Constantly update the slots in the formation
    void Update()
    {
        this.CheckPath();
        this.Seek(target.transform.position);
        formPositions.Clear();
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
        updatePositions();
    }

    //Update the transforms for all the follower units
    private void updatePositions()
    {
        for(int i = 0 ; i < formPositions.Count; ++i)
        {
            followerUnits[i].seekTarget = formPositions[i];
        }
    }

    //When a formation is created, iterate through all positions and add the closest unit to the slot
    public void delegatePositions()
    {
        int i = 0;
        float distance = 10000;
        //If no followers exist don't delegate positions
        if (0 == followerUnits.Count)
        {
            return;
        }
        CollisionPrediction closest = followerUnits[0];
        List<CollisionPrediction> gO = new List<CollisionPrediction>( followerUnits);

        //Itreate through all positions and set the closest unit to the slot
        foreach (Vector3 p in formPositions)
        {

            foreach (CollisionPrediction g in gO)
            {
                if(g == null)
                {
                    continue;
                }
                float curDist = Vector3.Distance(p, g.transform.position);
                if (curDist < distance)
                {
                    distance = curDist;
                    closest = g;
                }
            }
            //Remove the unit from the list
            gO.Remove(closest);
            closest.seekTarget = p;
            distance = 10000;
            /*
              Set the unit in the equivalent location in the list
              This allows for the unit to be more likely to be selected for this position
              the next time delegate positon is called
            */
            followerUnits[i] = closest;
            i++;
        }
    }

    //Determine the positions of the units
    private void linearFormation()
    {
        int i = 1;
        foreach (CollisionPrediction t in followerUnits)
        {
            if (t.gameObject.activeInHierarchy)
            {
                formPositions.Add(this.transform.position - this.transform.forward * (i - 5.5f) * linearFormSeperation);
                ++i;
            }
        }
    }

    //Determine the ppositions of the units
    private void twoFormation()
    {
        int i = 1;
        foreach (CollisionPrediction t in followerUnits)
        {
            if (t.gameObject.activeInHierarchy)
            {
                if (i % 2 == 0)
                {
                    formPositions.Add(this.transform.position - (this.transform.forward * (i - 5.5f - 1) * twoFormationSeperation) - this.transform.right);
                    ++i;
                }
                else if (i % 2 == 1)
                {
                    formPositions.Add(this.transform.position - (this.transform.forward * (i - 5.5f) * twoFormationSeperation) + this.transform.right);
                    ++i;
                }
            }
        }
    }

    //Used to determine if the current node has changed
    public void CheckPath()
    {
        if (currentNode != null)
        {
            if (Vector3.Distance(this.transform.position, currentNode.transform.position) <= slowDistance)
            {
                currentNode = currentNode.GetNextNode();
                if (currentNode != null)
                {
                    target = currentNode.gameObject;
                }
            }
        }
    }

    //Allows for the leader follow the path
    public void Seek(Vector3 destination)
    {
        Vector3 direction = (destination - this.transform.position).normalized;

        float angularAcceleration = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        Vector3 eulerAngleVelocity = new Vector3(0, angularAcceleration, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, deltaRotation, Time.deltaTime);
        transform.eulerAngles.Set(0f, transform.eulerAngles.y, 0f);

        linearSpeed = Mathf.Min(linearSpeed + maxLinearAcceleration, maxLinearSpeed);

        transform.position += transform.forward * linearSpeed * Time.deltaTime;

    }

}