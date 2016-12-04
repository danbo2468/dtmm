using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Path : MonoBehaviour {
    public Color lineColor;

    private List<Transform> drawNodes;
    private List<Transform> coreNodes;
    private List<Transform> reachableNodes;
    private List<Transform> pathNodes;
    private Transform firstNode;
    private Stack<Transform> travelingPath;

    // This is just for unity, drawing the line on the worldmap.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;
        drawNodes = new List<Transform>();
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
            if (pathTransforms[i] != transform)
                drawNodes.Add(pathTransforms[i]);

        for (int i = 0; i < drawNodes.Count; i++)
        {
            Vector3 currentNode = drawNodes[i].position;
            Vector3 previousNode;

            if (i > 0)
                previousNode = drawNodes[i - 1].position;
            else
                previousNode = currentNode;

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.2f);
        }
    }

    // This gunna be the boss methods of methods.
    public List<Transform> CalculateTravelingPath(Transform startingNode, Transform targetNode)
    {
        // First we have to define the direction we have to go. 
        List<Transform> temp = GetAllCoreNodes();
        int startingNodeInt = 0;
        int targetNodeInt = 0;
        bool directionNext = false;
        bool noDirection = false;

        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i] == startingNode)
                startingNodeInt = i;
            if (temp[i] == targetNode)
                targetNodeInt = i;               
        }

        if (startingNodeInt < targetNodeInt)
            directionNext = true;
        else if (startingNodeInt == targetNodeInt)
            noDirection = true;

        // Now we know which way we have to go.
        // directionNext && !noDirection :: Go to next core node
        // !directionNext && !noDirection :: Go to previous core node
        // noDirection :: Player tapped the node that is currently active.

        // get path nodes from core nodes, but when !directionNext, it should get the path nodes of the previous core node in different order!

        List<Transform> returnNodes = new List<Transform>();

        if(!noDirection)
            if(directionNext)
                while(startingNode != targetNode)
                {
                    foreach (Transform node in GetPathNodesFromCoreNode(GetNextCoreNodeOfPath(startingNode)))
                    {
                        returnNodes.Add(node);
                    }
                    returnNodes.Add(GetNextCoreNodeOfPath(startingNode));
                    startingNode = GetNextCoreNodeOfPath(startingNode);
                }

        return returnNodes;
    }

    public List<Transform> GetAllCoreNodes()
    {
        coreNodes = new List<Transform>();
        Transform[] nodeTransforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < nodeTransforms.Length; i++)
            if (nodeTransforms[i] != transform && nodeTransforms[i].tag != "Path")
                coreNodes.Add(nodeTransforms[i]);

        return coreNodes;
    }

    public List<Transform> GetReachableCoreNodes(List<Transform> coreNodes)
    {
        reachableNodes = new List<Transform>();
        for (int i = 0; i < coreNodes.Count; i++)
            if (coreNodes[i].tag != "LevelUnreachable")
                reachableNodes.Add(coreNodes[i]);

        return reachableNodes;
    }

    public Transform GetNextUnreachableNode()
    {
        List<Transform> temp = GetAllCoreNodes();
        for (int i = 0; i < temp.Count; i++)
            if (temp[i].tag == "LevelUnreachable")
                return temp[i];

        return null;
    }

    public List<Transform> GetPathNodesFromCoreNode(Transform coreNode)
    {
        pathNodes = new List<Transform>();
        Transform[] nodeTransforms = coreNode.GetComponentsInChildren<Transform>();
        for (int i = 0; i < nodeTransforms.Length; i++)
            if (nodeTransforms[i].tag == "Path")
                pathNodes.Add(nodeTransforms[i]);
   
        return pathNodes;
    }

    public Transform GetParentNode(Transform pathNode)
    {
        return pathNode.transform.parent;
    }

    public Transform GetFirstNodeOfScene()
    {
        Transform[] nodeTransforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < nodeTransforms.Length; i++)
            if (nodeTransforms[i] != transform && nodeTransforms[i].tag != "Path")
                return nodeTransforms[i];

        return null; // No nodes found.
    }

    public Transform GetNextCoreNodeOfPath(Transform coreNode)
    {
        List<Transform> temp = GetAllCoreNodes();
        Transform returnNode = coreNode;

        for (int i = 0; i < temp.Count-1; i++) // perhaps Count - 1
        {
            if (temp[i] == coreNode)
            {
                returnNode = temp[i + 1];
            }
        }
        return returnNode;
    }

    public Transform GetPreviousCoreNodeOfPath(Transform coreNode)
    {
        List<Transform> temp = GetAllCoreNodes();
        Transform returnNode = coreNode;

        for (int i = 1; i < temp.Count; i++) // perhaps Count - 1
        {
            if (temp[i] == coreNode)
            {
                returnNode = temp[i - 1];
            }
        }
        return returnNode;
    }

    public bool IsPlayerAtCoreNode(Transform node)
    {
        List<Transform> nodes = GetAllCoreNodes();
        foreach(Transform item in nodes)
            if (node == item)
                return true;
        
        return false;
    }

}
