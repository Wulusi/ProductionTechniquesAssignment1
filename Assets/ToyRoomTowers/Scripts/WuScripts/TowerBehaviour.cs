using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnityCustomEvent : UnityEngine.Events.UnityEvent
{

}

public abstract class TowerBehaviour : MonoBehaviour
{
    public enum TurretState { searching, firingtarget };
    public TurretState turretState;

    [SerializeField]
    private GameObject projectilePrefab;

    public GameObject VisionRadius, CurrentTarget;
    public List<GameObject> targets = new List<GameObject>();

    public delegate void CustomEvent();
    CustomEvent customEvent;

    public UnityCustomEvent fireAtTarget;

    public Transform barrel;
    private ObjectPooler objectPooler;

    public float rotateSpeed;
    public bool routineActive;
    public Vector3 targetDir;

    // Start is called before the first frame update
    public virtual void Start()
    {
        StartCoroutine(ActivateTurret());
        objectPooler = ObjectPooler.Instance;
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
                    Vector3 Dir = (CurrentTarget.transform.position - barrel.transform.position);

                    //Vector3 TargetDir = (barrel.transform.forward - this.transform.forward);
                    float angle = Vector3.Angle(Dir, barrel.transform.forward);

                    Debug.Log("angle is " + angle);
                    if (angle <= 5)
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
        CheckTarget();
        if (targets[0])
        {
            Vector3 targetDir = targets[0].transform.position - barrel.transform.position;

            CurrentTarget = targets[0];
            Vector3 newDir = Vector3.RotateTowards(barrel.forward, targetDir, rotateSpeed * Time.deltaTime, 0.0f);
            barrel.rotation = Quaternion.LookRotation(newDir.normalized);
        }
    }

    public virtual void SearchTarget()
    {
        Vector3 newDir = Vector3.RotateTowards(barrel.forward, targetDir, Mathf.Lerp(0, rotateSpeed, Time.deltaTime), 0.0f);

        float angle = Vector3.Angle(barrel.forward, new Vector3(targetDir.x, 0, targetDir.z));

        if (angle < 0.1f || targetDir == Vector3.zero)
        {
            targetDir = (Vector3)Random.insideUnitCircle - barrel.forward;
        }
        barrel.rotation = Quaternion.LookRotation(new Vector3(newDir.normalized.x, 0, newDir.normalized.z));
    }

    public virtual void FireAtTarget()
    {
        //if (projectilePrefab && !IsInvoking("TestFire"))
        //{
        //    Invoke("TestFire", 0.2f);
        //}
        //TestFire();
        //StartCoroutine(Countdown(1f));
        LookAtTarget();
        CoolDown(0.75f, fireAtTarget);
    }

    public void TestFire()
    {
        CheckTarget();
        if (CurrentTarget)
        {
            objectPooler.SpawnFromPool(projectilePrefab.name, barrel.transform.position, barrel.transform.rotation);
            //Instead of instantiating a projectile, spawn it from a pool of previously spawned objects 
            //Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(CurrentTarget.transform.position - transform.position));
        }
    }

    public void CoolDown(float duration, UnityCustomEvent eventToInvoke)
    {
        StartCoroutine(Countdown(duration, eventToInvoke));
    }

    public IEnumerator Countdown(float duration, UnityCustomEvent eventToInvoke)
    {
        if (!routineActive)
        {
            routineActive = true;
            float totalTime = 0;
            while (totalTime <= duration)
            {
                totalTime += Time.deltaTime;
                yield return null;
            }
            routineActive = false;
            eventToInvoke?.Invoke();
        }
    }
}
