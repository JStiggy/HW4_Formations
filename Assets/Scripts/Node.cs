using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour
{

    public Node nextNode = null;

    public Node GetNextNode()
    {
        return nextNode;
    }
}
