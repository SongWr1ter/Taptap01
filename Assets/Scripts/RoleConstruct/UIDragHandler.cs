using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler
{
    [HideInInspector] public GameObject rolePrefab;
    [HideInInspector] public Sprite dragSprite;

    private RectTransform dragRect;
    private Image dragIcon;

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject icon = new GameObject("dragIcon");
        dragIcon = icon.AddComponent<Image>();
        dragIcon.sprite = dragSprite;
        dragIcon.transform.SetParent(transform.root,false);
        dragRect = dragIcon.GetComponent<RectTransform>();

        dragIcon.raycastTarget = false;                
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null) {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponentInParent<Canvas>().transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,  
                out localPos
            );
            dragRect.localPosition = localPos;
            
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon.gameObject);
            dragIcon = null;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;

        Instantiate(rolePrefab, worldPos,Quaternion.identity);
    }
}
