using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class TwoLevelFormation : MonoBehaviour
{

    public Node currentNode;
    public GameObject target;

    public float linearSpeed;
    public float maxLinearSpeed;
    public float maxLinearAcceleration;
    public float maxAngularAcceleration;
    public float slowDistance;

    public int formationNumber = 1;

    public CollisionPrediction[] followerUnits;
    private List<Vector3> formPositions;

    float linearFormSeperation = 3f;
    float twoFormationSeperation = 2f;

    void Start()
    {
        target = currentNode.gameObject;
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
        delegatePositions();
    }

    // Update is called once per frame
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

    private void updatePositions()
    {
        for(int i = 0 ; i < formPositions.Count; ++i)
        {
            followerUnits[i].seekTarget = formPositions[i];
        }
    }

    private void delegatePositions()
    {
        int i = 0;
        float distance = 10000;
        CollisionPrediction closest = followerUnits[0];
        List<CollisionPrediction> gO = followerUnits.ToList();
        foreach (Vector3 p in formPositions)
        {
            foreach (CollisionPrediction g in gO)
            {
                float curDist = Vector3.Distance(p, g.transform.position);
                if (curDist < distance)
                {
                    distance = curDist;
                    closest = g;
                }
            }
            gO.Remove(closest);
            closest.seekTarget = p;
            distance = 10000;
            followerUnits[i] = closest;
            i++;
        }
    }

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