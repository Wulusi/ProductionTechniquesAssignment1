using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileDisabler : MonoBehaviour
{
    public enum Hoverstate { active, inactive }
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

        foreach(UiElementFollow uiElement in UIElementFromArray)
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
            hoverstate = Hoverstate.active;
        }
    }

    private void OnMouseDown()
    {
        isSelected = true;
        OnSelected();
    }

    private void OnSelected()
    {
        for (int i = 0; i < UIElements.Count; i++)
        {
            UIElements[i].GetComponent<UiElementFollow>().followMouse();
        }
        isSelected = false;
    }
}
