using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterTornadoScript : MonoBehaviour {

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Unpullable")
        {
            other.gameObject.tag = "Untagged";
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
