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

    public List<Transform> GetAllCoreNodes()
    {
        coreNodes = new List<Transform>();
        Transform[] nodeTransforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < nodeTransforms.Length; i++)
            if (nodeTransforms[i] != transform && nodeTransforms[i].tag != "Path")
                coreNodes.Add(nodeTransforms[i]);

        Debug.Log("Expected number = 6, actual number: " + coreNodes.Count);
        return coreNodes;
    }

    public List<Transform> GetReachableCoreNodes(List<Transform> coreNodes)
    {
        reachableNodes = new List<Transform>();
        for (int i = 0; i < coreNodes.Count; i++)
            if (coreNodes[i].tag != "LevelUnreachable")
                reachableNodes.Add(coreNodes[i]);

        Debug.Log("Expected number = 3, actual number: " + reachableNodes.Count);
        return reachableNodes;
    }

    public Transform GetNextUnreachableNode()
    {
        List<Transform> temp = GetAllCoreNodes();
        //bool found = false;
        for (int i = 0; i < temp.Count; i++)
            if (temp[i].tag == "LevelUnreachable")
                return temp[i];

        return null;
    }

    public Transform GetFirstNodeOfScene()
    {
        coreNodes = new List<Transform>();
        Transform[] nodeTransforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < nodeTransforms.Length; i++)
            if (nodeTransforms[i] != transform && nodeTransforms[i].tag != "Path")
                return nodeTransforms[i];

        return null; // No nodes found.
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
        Debug.Log(pathNode.transform.parent);
        return pathNode.transform.parent;
    }

}
