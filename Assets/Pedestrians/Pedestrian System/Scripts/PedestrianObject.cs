using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;

[ExecuteInEditMode]
public class PedestrianObject : MonoBehaviour 
{
	public  PedestrianSystem.ObjectFrequency    m_assetFrequency      = PedestrianSystem.ObjectFrequency.HIGH;

	public  PedestrianNode                      m_currentNode         = null;                 // the current node that the pedestrian object will travel to 
    public  float                               m_rotationSpeed       = 3.5f;                 // the speed at which we will rotaion
	public  bool                                m_onlyRotYAxis        = true;                 // set to true this means the object will stand upright always
	public  float                               m_nodeThreshold       = 5.0f;                 // how close the pedestrian object needs to get to the m_currentNode before it moves on to another node

    private float                               max_walking_speed     = 6.0f;
    private float                               min_walking_speed     = 4.0f;
    private float                               walking_speed;
    private float                               running_speed;
    

    public int                                  m_nodeVisitThreshold  = 2;                    // this is the amount of nodes to remember we have visited so we don't visit the same node again within this threshold
	private PedestrianNode[]                    m_prevPedestrianNodes;
	private int                                 m_nodeVisitIndex      = 0;

    private NavMeshAgent                        m_navMeshAgent;
    private Animator                            m_animator;
    public GameObject                           disaster; 
    private float                               disaster_size_unsafe_area = 300;
    private bool                                has_seen_disaster = false;

	public  PathingStatus                       m_pathingStatus       = PathingStatus.RANDOM; // this determines how the pedestrian object will traverse through the pathing nodes
	public  int                                 m_pathingIndex        = 0;                    // if the m_pathingStatus is set to INDEX, then the pedestrian object will try to use this index position for the m_nodes associated in the PedestrianNode.cs object 
	public  Vector3                             m_offsetPosVal        = Vector3.zero;         // the amount to offset the position of this object
	public  bool                                m_lookAtNode          = true;                 // set this to true if you want the objects forward direction to face the node it is currently going towards

	protected float                             m_lanePosXVariation   = 0.0f;
	protected float                             m_lanePosZVariation   = 0.0f;

	private bool                                ThresholdReached     { get; set; }

	public enum PathingStatus
	{
		RANDOM = 0
	}

	void Awake () 
	{
		m_prevPedestrianNodes = new PedestrianNode[m_nodeVisitThreshold];
		for(int nIndex = 0; nIndex < m_prevPedestrianNodes.Length; nIndex++)
			m_prevPedestrianNodes[nIndex] = null;

		if(PedestrianSystem.Instance)
			PedestrianSystem.Instance.RegisterObject( this );
	}
	
	IEnumerator Start () 
	{
        disaster = GameObject.FindGameObjectWithTag("disaster");
        m_animator = gameObject.GetComponent<Animator>();
        m_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        walking_speed = UnityEngine.Random.Range(min_walking_speed, max_walking_speed);
        running_speed = walking_speed * 2;

		if(PedestrianSystem.Instance)
		{
			m_lanePosXVariation = UnityEngine.Random.Range(-PedestrianSystem.Instance.m_globalLanePosVariation, PedestrianSystem.Instance.m_globalLanePosVariation);
			m_lanePosZVariation = UnityEngine.Random.Range(-PedestrianSystem.Instance.m_globalLanePosVariation, PedestrianSystem.Instance.m_globalLanePosVariation);
		}

		yield return null;
	}

	void Update ()
	{
        Vector3 disaster_to_pedestrian_vector = (gameObject.transform.position - disaster.transform.position);
        float disaster_size = disaster.GetComponent<Renderer>().bounds.size.magnitude;

        if (disaster_to_pedestrian_vector.magnitude < disaster_size + disaster_size_unsafe_area)
        {
            has_seen_disaster = true;
            m_animator.SetInteger("Mode", 2);  // running animation
            m_navMeshAgent.SetDestination(gameObject.transform.position + disaster_to_pedestrian_vector);
            m_navMeshAgent.acceleration = 1000;
            m_navMeshAgent.speed = running_speed;
        }
        else
        {
            followPedestrianPaths();
            /* m_animator.SetInteger("Mode", 1);
            m_navMeshAgent.SetDestination(gameObject.transform.position);
            m_navMeshAgent.speed = 0.0f;
            */
        }
       
	}

    private void followPedestrianPaths()
    {
        if (m_navMeshAgent.speed <= 0.0f)
            m_animator.SetInteger("Mode", 0); // idle animation
        else
            m_animator.SetInteger("Mode", 1); // walking animation

        Vector3 dir = m_currentNode.transform.position;
        dir.x += m_lanePosXVariation;
        dir.z += m_lanePosZVariation;
        dir = dir - (transform.position + m_offsetPosVal); // find the direction to the next node

        if (ThresholdReached)
        {
                m_prevPedestrianNodes[m_nodeVisitIndex] = m_currentNode;
                m_nodeVisitIndex++;
                if (m_nodeVisitIndex >= m_prevPedestrianNodes.Length)
                    m_nodeVisitIndex = 0;

                m_currentNode = m_currentNode.NextNode(this);  // find another node or do something else
                ThresholdReached = false;
        }
        else if (dir.magnitude > 5.0f)
        {
            m_navMeshAgent.SetDestination(m_currentNode.transform.position);  // move us by the determined speed
            m_navMeshAgent.acceleration = 1;
            m_navMeshAgent.speed = walking_speed;

            if (m_lookAtNode)
                transform.forward = Vector3.Slerp(transform.forward, dir.normalized, m_rotationSpeed * Time.deltaTime);   // rotate our forward directoin over time to face the node we are moving towards

            if (m_onlyRotYAxis)
                transform.rotation = Quaternion.Euler(new Vector3(0.0f, transform.eulerAngles.y, 0.0f)); // only rotate around the Y axis.
        }
        else
            ThresholdReached = true;
        
    }

	void Destroy()
	{
		if(PedestrianSystem.Instance)
			PedestrianSystem.Instance.UnRegisterObject( this );
	}

	public void Spawn( Vector3 a_pos, PedestrianNode a_startNode )
	{
		transform.position = a_pos - m_offsetPosVal;
		     m_currentNode = a_startNode;
	}

	public bool HasVisitedNode( PedestrianNode a_node )
	{
		for(int nIndex = 0; nIndex < m_prevPedestrianNodes.Length; nIndex++)
		{
			if(m_prevPedestrianNodes[nIndex] == a_node)
				return true;
		}

		return false;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube((transform.position + m_offsetPosVal), Vector3.one * 0.25f);
	}
}
