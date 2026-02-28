using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TheDropArea : MonoBehaviour, IPointerClickHandler
{
    public InventoryDrag TheDrag;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (TheDrag.IsDraging == true)
            {
                StartCoroutine(TheDrag.DropDownItem());
            }
        }
    }
}
