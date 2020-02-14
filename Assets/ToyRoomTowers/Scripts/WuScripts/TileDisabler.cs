using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileDisabler : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public enum Hoverstate { active, inactive }
    public Hoverstate hoverstate;

    public void Start()
    {
        StartCoroutine(checkState());
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
        hoverstate = Hoverstate.inactive;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer enter");
        hoverstate = Hoverstate.active;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exit");
        hoverstate = Hoverstate.inactive;
    }
}
