﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Custom vision detection for towers
public class VisionDetection : MonoBehaviour, PooledObjInterface
{
    int layer_mask;
    public TowerBehaviour owner;

    [SerializeField]
    sObj_tower_Params tower_Params;

    public void OnEnable()
    {
        owner = GetComponentInParent<TowerBehaviour>();
    }

    public void OnPooledObjSpawn()
    {
        tower_Params = owner.towerParams;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Obtain a reference to the tower params in order to caculate the detection radius
        OnPooledObjSpawn();
        //Get a reference to the interger representing the layermask with enemies
        layer_mask = (1 << LayerMask.NameToLayer("Enemies"));
        //Launch coroutine loop for detection of targets
        StartCoroutine(DetectTargets());
    }

    //Main Coroutine While Loop, continously detects for targets
    private IEnumerator DetectTargets()
    {
        while (true)
        {
            DetectionRange();
            yield return null;
        }
    }

    void DetectionRange()
    {
        //Using Overlap Sphere as a detection radius on the target layermask 
        //with enemies to detect if there are enemies around the tower
        //this returns an array of game Objects colliders marked as enemies
        Collider[] colliders = Physics.OverlapSphere(owner.transform.position, tower_Params._detectionRadius, layer_mask);

        //Reference to the owner Towerbehaviour script
        var ownerList = owner.targets;

        //Add all elements detected 
        foreach (Collider detected in colliders)
        {
            if (!ownerList.Contains(detected.gameObject))
            {
                ownerList.Add(detected.gameObject);
            }
        }

        for (int i = 0; i <= colliders.Length; i++)
        {
            if (colliders.Length != ownerList.Count)
            {
                if (ownerList.Count == 1)
                {
                    owner.GetComponent<TowerBehaviour>().CurrentTarget = null;
                    ownerList.RemoveAt(i);
                }
                else
                {
                    ownerList.RemoveAt(i);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        owner.targets.Remove(other.gameObject);
    }
}
