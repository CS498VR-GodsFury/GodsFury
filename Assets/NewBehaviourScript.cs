using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public int pressed;
    
	// Use this for initialization
	void Start () {
        pressed = 1;
        ParticleSystem system  = GetComponent<ParticleSystem>();
        var em = system.emission;
        em.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {     
        if (Input.GetKeyDown("space"))
        {
            pressed = 1 - pressed;
        }
        ParticleSystem system = GetComponent<ParticleSystem>();
        var em = system.emission;
        if (pressed == 0 ) em.enabled= false;
        else em.enabled = true;
        //else system.Play();


    }
}
