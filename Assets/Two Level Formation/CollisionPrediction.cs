using UnityEngine;
using System.Collections;

public class CollisionPrediction : ObstacleAvoidance {

    //Vector to seek
    public Vector3 seekTarget = Vector3.zero;

    //Values of the actor in the closest collision
    float shortestTime;
    public GameObject firstTarget;
    float firstMinSeperation;
    float firstDistance;
    Vector3 firstRelPostion;
    Vector3 firstRelVelocity;

    //Radius for collision
    float colisionRadius = 1f;

    void Start()
    {
        target = currentNode.gameObject;
    }

    void Update()
    {
        this.rayAvoidance();
        this.predictionAvoidance();

        if (avoidanceTarget != Vector3.zero)
        {
            rayReform = 0f;
            this.Seek( (avoidanceTarget - this.transform.position) * .6f + firstRelPostion *.4f);
        }
        else
        {
            this.Seek( (seekTarget - this.transform.position) * .4f + (firstRelPostion * .6f));
        }
    }

    public void rayAvoidance()
    {
        rayReform += Time.deltaTime / 50;
        rayReform = Mathf.Min(rayReform, .45f);
        Vector3[] raycastArrays = new Vector3[2];
        raycastArrays[0] = (transform.forward * (.55f + rayReform) + transform.right * (.45f - rayReform)).normalized;
        raycastArrays[1] = (transform.forward * (.55f + rayReform) + transform.right * -(.45f - rayReform)).normalized;
        RaycastHit hit;
        avoidanceTarget = Vector3.zero;
        int rayCount = 0;
        for (int i = 0; i < 2; ++i)
        {
            Debug.DrawRay(this.transform.position, raycastArrays[i] * raycastDistance, Color.red);
            if (Physics.Raycast(this.transform.position, raycastArrays[i], raycastDistance, 1 << 8))
            {
                Physics.Raycast(this.transform.position, raycastArrays[i], out hit, raycastDistance, 1 << 8);
                Debug.DrawRay(hit.point, hit.normal * (avoidanceDistance - 1), Color.red);
                avoidanceTarget = hit.point + hit.normal * avoidanceDistance;
                rayCount++;
            }
        }
    }

    public void predictionAvoidance()
    {
        firstTarget = null;

        firstRelPostion = Vector3.zero;
        //Iterate through all collisions find the closest target
        shortestTime = float.MaxValue;
        foreach (Collider obj in Physics.OverlapSphere(this.transform.position, 1.5f, 1 << 9))
        {
            CollisionPrediction cP = obj.GetComponent<CollisionPrediction>();
            Vector3 relPosition = cP.transform.position - this.transform.position;
            Vector3 relVelocity = this.transform.forward * this.linearSpeed -  cP.transform.forward * cP.linearSpeed;
            float relSpeed = relVelocity.magnitude;
            float timeToCollision = Vector3.Dot(relPosition, relVelocity) / (relSpeed * relSpeed);
            float distance = relPosition.magnitude;
            float minSeparation = distance - relSpeed * shortestTime;
            if (minSeparation > 2 * colisionRadius)
            {
                continue;
            }
            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                firstMinSeperation = minSeparation;
                shortestTime = timeToCollision;
                firstTarget = obj.gameObject;
                firstDistance = distance;
                firstRelPostion = relPosition;
                firstRelVelocity = relVelocity;
            }
        }

        //No evasion if no target found
        if (firstTarget == null)
        {
            return;
        }

        //Calculate the evasion vector
        if (firstMinSeperation <= 0 || firstDistance < 2 * colisionRadius)
        {
            firstRelPostion = this.transform.position - firstTarget.transform.position;
        }
        else
        {
            firstRelPostion = firstRelPostion + firstRelVelocity * shortestTime;
        }
    }

    public override void Seek(Vector3 destination)
    {
        Vector3 direction = destination.normalized;
        
        float angularAcceleration = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        Vector3 eulerAngleVelocity = new Vector3(0, angularAcceleration, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, deltaRotation, maxAngularAcceleration* Time.deltaTime);
        transform.eulerAngles.Set(0f, transform.eulerAngles.y, 0f);

        linearSpeed = Mathf.Min(linearSpeed + maxLinearAcceleration, maxLinearSpeed);
        if(Vector3.Distance(this.transform.position, seekTarget) > slowDistance)
        {
            transform.position += transform.forward * linearSpeed * Time.deltaTime;
        }
        else
        {
            linearSpeed = maxLinearSpeed * Vector3.Distance(this.transform.position, seekTarget) / slowDistance;
            transform.position += transform.forward * linearSpeed * Time.deltaTime * maxAngularAcceleration;
        }
    }
}

