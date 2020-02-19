using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{

    public NavMeshAgent navAgent;
    //TO DO: make modular destination selection system
    public Transform DestinationMarker;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown(0.1f));
    }

    void GoToSpot()
    {
        navAgent.SetDestination(DestinationMarker.position);
    }

    private IEnumerator Countdown(float duration)
    {
        float totalTime = 0;
        while (totalTime <= duration)
        {
            //countdownImage.fillAmount = totalTime / duration;
            totalTime += Time.deltaTime;
            yield return null;
        }
        GoToSpot();
    }
}
