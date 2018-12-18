using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterTornadoScript : MonoBehaviour {

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Unpullable")
        {
            //other.gameObject.layer = 0 << 2;
            other.gameObject.layer = LayerMask.NameToLayer("Default");
            other.gameObject.tag = "Untagged";
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
