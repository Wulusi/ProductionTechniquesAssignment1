using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{

    public NavMeshAgent navAgent;

    /// <summary>
    /// Health value of this enemy (defaults to 10 and can not be lower than 0)
    /// </summary>
    [Min(0)]
    public float healthValue = 10;

    public Transform DestinationMarker;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown(0.1f));
    }

    /// <summary>
    /// Inflict a variable amount of damage on this enemy
    /// </summary>
    /// <param name="damage">The amount of damage to inflict</param>
    public void LoseHP(float damage)
    {
        healthValue -= damage;

        if (healthValue <= 0)
        {
            KillEnemy();
        }
    }

    /// <summary>
    /// Kills the enemy when they die
    /// </summary>
    private void KillEnemy()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
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
