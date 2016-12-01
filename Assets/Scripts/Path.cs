using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Path : MonoBehaviour {
    // https://www.youtube.com/watch?v=Lo5PPqHmsIM
    public Color lineColor;

    private List<Transform> nodes;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;
        nodes = new List<Transform>();
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
            if (pathTransforms[i] != transform)
                nodes.Add(pathTransforms[i]);

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode;

            if (i > 0)
                previousNode = nodes[i - 1].position;
            else
                previousNode = currentNode;

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.2f);
        }

        

    }

}
