﻿using UnityEngine;
using System.Collections;

public class ObstacleAvoidance : MonoBehaviour {

    public Node currentNode;
    public GameObject target;

    public Vector3 avoidanceTarget;

    public float linearSpeed;
    public float maxLinearSpeed;
    public float maxLinearAcceleration;
    public float maxAngularAcceleration;
    public float raycastDistance = 7f;
    public float avoidanceDistance = 6f;
    public float rayReform = 0f;

    public float slowDistance;
    public bool isEvading = false;

    public Vector3 appliedTorque;
    public Vector3 appliedForce;

    void Start()
    {
        target = currentNode.gameObject;
    }

    void Update()
    {
        this.CheckPath();
        rayReform += Time.deltaTime / 100;
        rayReform = Mathf.Min(rayReform, .2f);
        Vector3[] raycastArrays = new Vector3[2];
        raycastArrays[0] = (transform.forward * (.8f + rayReform) + transform.right * (.2f - rayReform) ).normalized;
        raycastArrays[1] = (transform.forward * (.8f + rayReform) + transform.right * -(.2f - rayReform) ).normalized;
        RaycastHit hit;
        avoidanceTarget = Vector3.zero;
        int rayCount = 0;
        for (int i = 0; i < 2; ++i)
        {
            Debug.DrawRay(this.transform.position, raycastArrays[i]* raycastDistance, Color.red);
            if (Physics.Raycast(this.transform.position, raycastArrays[i], raycastDistance, 1 << 8))
            {
                Physics.Raycast(this.transform.position, raycastArrays[i], out hit, raycastDistance, 1 << 8);
                Debug.DrawRay(hit.point, hit.normal * (avoidanceDistance - 1), Color.red);
                avoidanceTarget = hit.point + hit.normal * avoidanceDistance;
                rayCount++;
            }
        }

        if(avoidanceTarget != Vector3.zero)
        {
            rayReform = 0f;
            //Debug.Log("Avoid " + avoidanceTarget);
            this.Seek(avoidanceTarget);
        }
        else 
        {
            //Debug.Log("Node " + target.transform.position);
            this.Seek(target.transform.position);
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

    public virtual void Seek(Vector3 destination)
    {
        Vector3 direction = (destination - this.transform.position).normalized;

        float angularAcceleration = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        Vector3 eulerAngleVelocity = new Vector3(0, angularAcceleration, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, deltaRotation, Time.deltaTime);
        transform.eulerAngles.Set (0f, transform.eulerAngles.y, 0f);

        linearSpeed = Mathf.Min(linearSpeed+maxLinearAcceleration, maxLinearSpeed);

        transform.position += transform.forward * linearSpeed * Time.deltaTime;

    }
}
