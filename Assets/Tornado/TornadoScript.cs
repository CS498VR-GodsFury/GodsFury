using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoScript : MonoBehaviour {

    public float speed;
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Pullable") {
            GameObject pulledObj = other.gameObject;

            pulledObj.transform.position = Vector3.MoveTowards(pulledObj.transform.position, this.transform.position, speed * Time.deltaTime);
        }
    }
}
