using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

    // The type of platform, the amount of platforms in the pool and the list
    public GameObject pooledObject;
    private List<GameObject> pooledObjects;

	// Use this for initialization
	void Start()
    {
        pooledObjects = new List<GameObject>();
	}
	

    // Get an object from this pool in order to relocate it
    public GameObject GetPooledObject()
    {
        // Return the first non-active object in the list
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        // If there are no more non-active objects in the list, create a new one and add it to the list
        GameObject platformObject = Instantiate(pooledObject);
        platformObject.SetActive(false);
        pooledObjects.Add(platformObject);
        return platformObject;
    }
}
