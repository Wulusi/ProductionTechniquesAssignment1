using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileDisabler : MonoBehaviour
{
    public enum Hoverstate { active, selected, inactive }
    public Hoverstate hoverstate;

    private BuildAreaManager owner;
    public List<GameObject> UIElements = new List<GameObject>();

    bool isSelected;

    public void Start()
    {
        StartCoroutine(checkState());
        owner = GetComponentInParent<BuildAreaManager>();
        isSelected = false;
        GetUIElements();
    }

    private void GetUIElements()
    {
        var UIElementFromArray = GameObject.FindObjectsOfType<UiElementFollow>();

        Debug.Log("UI Found are: " + UIElementFromArray);

        foreach (UiElementFollow uiElement in UIElementFromArray)
        {
            UIElements.Add(uiElement.gameObject);
        }
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

        for (int i = 0; i < UIElements.Count; i++)
        {
            UIElements[i].GetComponent<UiElementFollow>().gameObject.SetActive(true);
            UIElements[i].GetComponent<UiElementFollow>().followMouse();
        }
    }
}
