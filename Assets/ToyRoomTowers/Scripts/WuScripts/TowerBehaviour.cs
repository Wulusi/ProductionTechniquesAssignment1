using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBehaviour : MonoBehaviour
{
    public enum TurretState { searching, firingtarget };
    public TurretState turretState;

    public GameObject VisionRadius, CurrentTarget;
    public List<GameObject> targets = new List<GameObject>();

    public Transform barrel;

    public float rotateSpeed;
    public bool routineActive;
    public Vector3 targetDir;

    // Start is called before the first frame update
    public virtual void Start()
    {
        StartCoroutine(ActivateTurret());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ActivateTurret()
    {
        while (true)
        {
            TurretStates();
            CheckTarget();
            yield return null;
        }

    }

    public virtual void TurretStates()
    {
        switch (turretState)
        {
            case TurretState.searching:

                if (targets.Count != 0)
                {
                    LookAtTarget();

                    //Debug.Log("Target Acquired");
                    Vector3 Dir = (CurrentTarget.transform.position - transform.position);
                    float angle = Vector3.Angle(Dir, this.transform.forward);

                    if (angle < 15)
                    {
                        turretState = TurretState.firingtarget;
                    }
                }
                else
                {
                    //Debug.Log("Searching for target");
                    CurrentTarget = null;
                    SearchTarget();
                }

                break;

            case TurretState.firingtarget:

                //Debug.Log("Firing Target");
                FireAtTarget();
                //StartCoroutine(Countdown(1f));
                break;
        }
    }

    public void CheckTarget()
    {
        if (CurrentTarget == null)
        {
            targets.Remove(CurrentTarget);

            if (targets.Count > 0)
            {
                CurrentTarget = targets[0];
            }
        }
        else if (!CurrentTarget.activeSelf)
        {
            targets.Remove(CurrentTarget);
        }
    }

    public virtual void LookAtTarget()
    {
        Vector3 targetDir = targets[0].transform.position - transform.position;
        CurrentTarget = targets[0];
        Vector3 newDir = Vector3.RotateTowards(barrel.forward, targetDir, rotateSpeed * Time.deltaTime, 0.0f);
        barrel.rotation = Quaternion.LookRotation(newDir.normalized);
    }


    public virtual void SearchTarget()
    {
        Vector3 newDir = Vector3.RotateTowards(barrel.forward, targetDir, Mathf.Lerp(0, rotateSpeed, Time.deltaTime), 0.0f);

        float angle = Vector3.Angle(barrel.forward, new Vector3(targetDir.x, 0, targetDir.z));

        //Debug.Log("angle is " + angle);

        if (angle < 0.1f || targetDir == Vector3.zero)
        {
            targetDir = (Vector3)Random.insideUnitCircle - barrel.forward;
        }
        barrel.rotation = Quaternion.LookRotation(new Vector3(newDir.normalized.x, 0, newDir.normalized.z));
    }

    public virtual void FireAtTarget()
    {
        //Fire bullet code here

        StartCoroutine(Countdown(1f));
    }

    public IEnumerator Countdown(float duration)
    {
        if (!routineActive)
        {
            routineActive = true;
            float totalTime = 0;
            while (totalTime <= duration)
            {
                //countdownImage.fillAmount = totalTime / duration;
                totalTime += Time.deltaTime;
                //Debug.Log("Time" + totalTime);
                //var integer = (int)totalTime; /* choose how to quantize this */
                /* convert integer to string and assign to text */
                yield return null;
            }
            routineActive = false;
            turretState = TurretState.searching;
        }
    }
}
