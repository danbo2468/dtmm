﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Path : MonoBehaviour {
    public Color lineColor;

    private List<Transform> drawNodes;
    private List<Transform> coreNodes;
    private List<Transform> reachableNodes;
    private List<Transform> pathNodes;

    private Transform firstNode;
    public Transform test;

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

        return coreNodes;
    }

    public int GetLevelID(Transform node)
    {
        List<Transform> temp = GetAllCoreNodes();
        for (int i = 0; i < temp.Count; i++)
            if (temp[i].name == node.name)
                return i + 1;

        throw new UnityException("No level found!");

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

    /// <summary>
    /// Even though most nodes are reachable, this one returns the first uncompleted level.
    /// </summary>
    /// <returns></returns>
    public Transform GetNextUncompletedLevel()
    {
        List<Transform> temp = GetAllCoreNodes();
        for (int i = 0; i < temp.Count; i++)
            if (temp[i].tag == "LevelUncompleted")
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

    public Transform GetNodeOfScene(int number)
    {

        List<Transform> nodeTransforms = GetAllCoreNodes();
        if (nodeTransforms.Count >= number)
            return nodeTransforms[number - 1];

        return null; // No nodes found.
    }

    public Transform GetNextCoreNodeOfPath(Transform coreNode)
    {
        List<Transform> temp = GetAllCoreNodes();
        Transform returnNode = coreNode;

        for (int i = 0; i < temp.Count - 1; i ++) // perhaps Count - 1
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

    public Transform findNodeOnPosition(Vector3 position)
    {
        List<Transform> nodes = GetAllCoreNodes();
        foreach (Transform item in nodes)
            if (item.position == position)
                return item;

        return null;
    }

    public Transform GetLastNode()
    {
        return GetAllCoreNodes()[GetAllCoreNodes().Count - 1];
    }

    /// <summary>
    /// Returns a route based on given parameters, supply a valid startingNode, and a true for next node, false for previous node.
    /// </summary>
    /// <param name="startingNode"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public List<Transform> GetRoute(Transform startingNode, bool direction)
    {   
        List<Transform> route = new List<Transform>();
        List<Transform> allNodes = GetAllCoreNodes();
        if (direction)
        {
            if (allNodes[allNodes.Count - 1] != startingNode)
            {
                Transform nextNode = GetNextCoreNodeOfPath(startingNode);
                if (nextNode.tag != "LevelUnreachable" && startingNode.tag != "LevelUncompleted")
                {
                    List<Transform> temp = GetPathNodesFromCoreNode(nextNode);
                    foreach (Transform node in temp)
                        route.Add(node);
                    route.Add(nextNode);
                }
            }
        }   

        else if (!direction)
        {
            if (allNodes[0] != startingNode)
            {
                Transform previousNode = GetPreviousCoreNodeOfPath(startingNode);
                List<Transform> temp = GetPathNodesFromCoreNode(startingNode);
                foreach (Transform node in temp.AsEnumerable().Reverse())
                    route.Add(node);
                route.Add(previousNode);
            }
        }
        return route;
    }

    public void setLevelCompleted(Transform node)
    {
        if (node.tag != "LevelCompleted")
        {
            node.tag = "LevelCompleted";
            //node.GetComponent<SpriteRenderer>().sprite = Resources.Load("Placeholder - GreenCircle", typeof(Sprite)) as Sprite;
        }
    }

    public void setLevelUncompleted(Transform node)
    {
        if (node.tag != "LevelUncompleted")
        {
            node.tag = "LevelUncompleted";
            //node.GetComponent<SpriteRenderer>().sprite = Resources.Load("Placeholder - RedCircle", typeof(Sprite)) as Sprite;
        }
    }


}


/// OLD METHOD WONT WORK WITH BUTTONS L/R
/// 
/*
 * // This gunna be the boss methods of methods.
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

        if(!noDirection)
            if(!directionNext)
                while(startingNode != targetNode)
                {
                    
                }

        return returnNodes;


    }
*/