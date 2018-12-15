using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMovementScript : MonoBehaviour {
    private GameObject laser;
    private float movementSpeed = 70.0f;
    // Use this for initialization
    void Start () {
        laser = GameObject.Find("Laser Pointer");
    }
	
	// Update is called once per frame
	void Update () {
        LazerInitialization lazerInfo = laser.GetComponent<LazerInitialization>();
        if (lazerInfo.isHit)
        {
            Vector3 lazerPos = lazerInfo.hitPoint;
            Vector3 currentPos = this.transform.position;
            this.transform.position = Vector3.MoveTowards(this.transform.position, lazerPos, movementSpeed * Time.deltaTime);
        }
    }
}
