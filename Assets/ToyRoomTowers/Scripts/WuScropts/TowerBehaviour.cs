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

                    Debug.Log("Target Acquired");
                    Vector3 Dir = (CurrentTarget.transform.position - transform.position);
                    float angle = Vector3.Angle(Dir, this.transform.forward);

                    if (angle < 15)
                    {
                        turretState = TurretState.firingtarget;
                    }
                }
                else
                {
                    Debug.Log("Searching for target");
                    CurrentTarget = null;
                    SearchTarget();
                }

                break;

            case TurretState.firingtarget:

                Debug.Log("Firing Target");
                FireAtTarget();
                //StartCoroutine(Countdown(1f));
                break;
        }
    }

    public virtual void LookAtTarget()
    {
        //for (int i = 0; i < targets.Count - 1; i++)
        {
            Vector3 targetDir = targets[0].transform.position - transform.position;
            CurrentTarget = targets[0];
            Vector3 newDir = Vector3.RotateTowards(barrel.forward, targetDir, rotateSpeed * Time.deltaTime, 0.0f);
            barrel.rotation = Quaternion.LookRotation(newDir.normalized);
        }
    }


    public virtual void SearchTarget()
    {
        Vector3 newDir = Vector3.RotateTowards(barrel.forward, targetDir, rotateSpeed * Time.deltaTime, 0.0f);

        float angle = Vector3.Angle(targetDir, barrel.forward);

        Debug.Log("angle is " + angle);

        if (angle < 4f || targetDir == Vector3.zero)
        {
            targetDir = (Vector3)Random.insideUnitCircle - barrel.forward;

        }
        Debug.DrawLine(this.transform.position + barrel.forward, this.transform.position + targetDir, Color.red);
        Debug.DrawLine(this.transform.position + barrel.forward, barrel.position, Color.blue);

        barrel.rotation = Quaternion.LookRotation(new Vector3(Mathf.Clamp(newDir.x,-20,20),newDir.y,0));

        //barrel.rotation = newDir;

        //this.transform.rotate)

        //barrel.rotation = Quaternion.Lerp(barrel.rotation, Quaternion.Euler(Mathf.Clamp(targetDir.x, -20, 20), targetDir.y, targetDir.z),10f);
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
                Debug.Log("Time" + totalTime);
                //var integer = (int)totalTime; /* choose how to quantize this */
                /* convert integer to string and assign to text */
                yield return null;
            }
            routineActive = false;
            turretState = TurretState.searching;
        }
    }
}
