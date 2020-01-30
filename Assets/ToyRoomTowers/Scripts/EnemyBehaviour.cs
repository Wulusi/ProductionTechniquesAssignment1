using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{

    public NavMeshAgent navAgent;
    public List<GameObject> WayPoints = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GoToSpot();
    }

    void GoToSpot()
    {
        navAgent.SetDestination(WayPoints[0].transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
