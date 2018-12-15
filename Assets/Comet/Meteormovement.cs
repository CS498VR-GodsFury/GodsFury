using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteormovement : MonoBehaviour {
    public GameObject comet;
    public ParticleSystem expObject;
    private ParticleSystem cometTrail;

    //private Vector3 currentPosition;
    private Vector3 targetPosition;
    private float velocity = 10.0f;
    private Vector3 directionVector;

    private const float MAX_POWER = 60000000.0f;
    private const float IMPACT_RADIUS = 75.0f;

    private bool endOfLifeCycle;
    public bool shoot;
    public GameObject laser;
	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(10, 10, 10);
        endOfLifeCycle = false;
        laser = GameObject.Find("Laser Pointer");
        LazerInitialization lazerInfo = laser.GetComponent<LazerInitialization>();
        Vector3 targetPosition = lazerInfo.hitPoint;
       
        directionVector = targetPosition - this.transform.position;
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
            try
            {
                objectBody.AddExplosionForce(power, impactPos, IMPACT_RADIUS, -5.0f, ForceMode.Impulse);
            }
            catch
            { }
        }
    }

    private void despawnComet() {
        comet.GetComponent<MeshRenderer>().enabled = false;
        comet.GetComponent<SphereCollider>().enabled = false;
        comet.GetComponent<Rigidbody>().isKinematic = true;
        ParticleSystem.MainModule ps = cometTrail.main;
        ps.loop = false;
        Destroy(comet, 2.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (endOfLifeCycle)
            return;

        if (other.transform.root.GetComponent<Disaster>() != null) return;

        endOfLifeCycle = true;
        Vector3 impactPosition = transform.position;
        Explode(impactPosition);

        repelAllBuildings(impactPosition);

        despawnComet();
    }

}
