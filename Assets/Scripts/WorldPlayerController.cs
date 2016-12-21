using UnityEngine;
using System.Collections;

public class WorldPlayerController : MonoBehaviour {

    private Vector3 previousLocation;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    void Update()
    {
        Vector3 curVel = (transform.position - previousLocation) / Time.deltaTime;
        animator.SetFloat("y", curVel.y);
        previousLocation = transform.position;
    }
}
