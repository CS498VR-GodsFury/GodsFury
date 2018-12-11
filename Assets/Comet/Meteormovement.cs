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

    private const float MAX_POWER = 60000000.0f;
    private const float IMPACT_RADIUS = 50.0f;

    private bool endOfLifeCycle;
    public bool shoot;
    public GameObject laser;
	// Use this for initialization
	void Start () {
        laser = GameObject.Find("Laser Pointer");
        reset();
	}

    private void reset()
    {
        shoot = false;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        //transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.position = new Vector3(-97, 1024, -840);
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        ParticleSystem exp = expObject.GetComponent<ParticleSystem>();
        exp.Stop();
    }

    private void Update()
    {
        OVRInput.Update();
        LazerInitialization lazerInfo = laser.GetComponent<LazerInitialization>();
        if(OVRInput.Get(OVRInput.RawButton.Y)&&(!shoot))
        {
            if(lazerInfo.isHit)
            {
                transform.localScale = new Vector3(20, 20, 20);
                shoot = true;
                init(lazerInfo.hitPoint);
            }
            
        }
    }

    private void init(Vector3 targetPosition)
    {
        endOfLifeCycle = false;
        currentPosition = transform.position;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        comet.GetComponent<Rigidbody>().isKinematic = false;
        /* 
         * TODO When imported in final project, the way that the comet gets its target position needs to be implemented.
         * Either it can read from the target laser object and see where it is pointing OR,
         * the target marker can spawn an empty object (or a visible marker) and the comet can read its position when it is created
         */


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
            try
            {
                objectBody.AddExplosionForce(power, impactPos, IMPACT_RADIUS, 0.0f, ForceMode.Impulse);
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
        Destroy(comet, 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (endOfLifeCycle)
            return;
        comet.GetComponent<Rigidbody>().isKinematic = true;
        transform.localScale = new Vector3(0, 0, 0);
        endOfLifeCycle = true;
        Vector3 impactPosition = transform.position;
        Explode(impactPosition);

        repelAllBuildings(impactPosition);

        //despawnComet();
        Invoke("reset", 5);
    }

}
