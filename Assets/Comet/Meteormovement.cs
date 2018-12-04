using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteormovement : MonoBehaviour {
    public GameObject comet;
    public ParticleSystem expObject;
    private ParticleSystem cometTrail;

    private Vector3 currentPosition;
    private Vector3 targetPosition;
    private float velocity = 10.0f;
    private Vector3 directionVector;

    private const float MAX_POWER = 100000000.0f;
    private const float IMPACT_RADIUS = 20.0f;

    private bool endOfLifeCycle;

	// Use this for initialization
	void Start () {
        endOfLifeCycle = false;
        currentPosition = transform.position;
        /* 
         * TODO When imported in final project, the way that the comet gets its target position needs to be implemented.
         * Either it can read from the target laser object and see where it is pointing OR,
         * the target marker can spawn an empty object (or a visible marker) and the comet can read its position when it is created
         */
        targetPosition = new Vector3(-831.0f, 378.0f, -1136.0f);

        directionVector = targetPosition - currentPosition;
        this.GetComponent<Rigidbody>().AddForce(directionVector * velocity);
        cometTrail = transform.Find("CometTrail").GetComponent<ParticleSystem>();
	}
	
    private void Explode(Vector3 impactPos) {
        expObject.transform.position = impactPos;
        ParticleSystem exp = expObject.GetComponent<ParticleSystem>();
        exp.Play();
    }

    private void repelAllBuildings(Vector3 impactPos) {

        Collider[] hitColliders = Physics.OverlapSphere(impactPos, IMPACT_RADIUS);
        Debug.Log(hitColliders.Length);
        foreach (Collider col in hitColliders) {
            Rigidbody objectBody = col.gameObject.GetComponent<Rigidbody>();

            float power = MAX_POWER;
            objectBody.AddExplosionForce(power, impactPos, IMPACT_RADIUS, 0.0f, ForceMode.Impulse);
        }
    }

    private void despawnComet() {
        comet.GetComponent<MeshRenderer>().enabled = false;
        comet.GetComponent<SphereCollider>().enabled = false;
        comet.GetComponent<Rigidbody>().isKinematic = true;
        ParticleSystem.MainModule ps = cometTrail.main;
        ps.loop = false;
        Destroy(comet, 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (endOfLifeCycle)
            return;

        endOfLifeCycle = true;
        Vector3 impactPosition = transform.position;
        Explode(impactPosition);

        repelAllBuildings(impactPosition);

        despawnComet();
    }

}
