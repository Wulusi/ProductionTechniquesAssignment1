﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileDisabler : MonoBehaviour
{
    public enum Hoverstate { active, selected, inactive }
    public Hoverstate hoverstate;

    private BuildAreaManager owner;
    bool isSelected;

    public void Start()
    {
        StartCoroutine(checkState());
        owner = GetComponentInParent<BuildAreaManager>();
        isSelected = false;
    }

    public IEnumerator checkState()
    {
        while (true)
        {
            ActivateTile();
            yield return null;
        }
    }

    public void ActivateTile()
    {
        switch (hoverstate)
        {
            case Hoverstate.active:

                GetComponent<MeshRenderer>().enabled = true;
                break;

            case Hoverstate.selected:

                GetComponent<MeshRenderer>().enabled = true;
                break;

            case Hoverstate.inactive:

                GetComponent<MeshRenderer>().enabled = false;
                break;
        }
    }

    private void OnMouseExit()
    {
        if (!isSelected)
        {
            hoverstate = Hoverstate.inactive;
        }
        else
        {
            if (owner.selectedTile == null)
            {
                Debug.Log("Activation Mesh");
                hoverstate = Hoverstate.active;
            }
        }
    }

    private void OnMouseDown()
    {
        if (owner.selectedTile == null)
        {
            isSelected = true;
            hoverstate = Hoverstate.selected;
            OnSelected();
        }
    }

    private void OnSelected()
    {
        owner.selectedTile = this.gameObject;
        owner.noSelection = false;
        //Activate this on BuildAreaManager
        owner.ActivateUI();
    }
}
