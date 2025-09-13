using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIDragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler
{
    [HideInInspector] public BattleUnitData battleUnitData;
    [HideInInspector] public Sprite dragSprite;
    [HideInInspector] public int cost;
    public float groundY = -0.4f;
    

    private RectTransform dragRect;
    private Image dragIcon;
    private Sprite forbidSprite;
    private bool canPlace = true;
    private Camera mainCamera;

    void Start()
    {
        forbidSprite = Resources.Load<Sprite>("Sprite/forbidSprite");
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CoinManager.Instance.coinCount < cost)
        {
            Debug.Log("金币不足，无法建造！");
            eventData.pointerDrag = null; // 阻止拖拽事件继续
            return;
        }
        else CoinManager.Instance.SpendCoin(cost);
        CameraDrag.canDrag = false;
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

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float currentHeigthDiff = mouseWorldPos.y - groundY;
          
            if (currentHeigthDiff >= -13f && currentHeigthDiff < -9f)
            {
                
                if (dragIcon != null)
                {

                    if (dragIcon.sprite != dragSprite)
                        dragIcon.sprite = dragSprite;
                }

                canPlace = true;
            }
            else
            {
               
                if (dragIcon != null)
                {

                    if (dragIcon.sprite != forbidSprite)
                        dragIcon.sprite = forbidSprite;
                }
                canPlace = false;
            }
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
        worldPos.y = -12f;
        worldPos.z = 0;
        if (canPlace)
        {
            ObjectPoolRegister.Instance._objectPool.Spawn("Tower", worldPos, Quaternion.identity, battleUnitData);
        }
        else CoinManager.Instance.AddCoin(cost);

            CameraDrag.canDrag = true;
    }
}
