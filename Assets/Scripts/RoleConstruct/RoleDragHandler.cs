using MemoFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoleDragHandler : MonoBehaviour
{
    
    
    public float recycleRadius = 10f;
    public float groundY = -0.4f;
    

    private Image dragIcon;
    private Sprite forbidSprite;
    private RectTransform dragRect;
    private GameObject dragCharacter;
    private Camera mainCamera;
    private Canvas canvas;
    private static bool isDragging = false;
    private bool canPlace = true;
    private Transform recycleArea;
    private Vector3 originPos;
    private Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        canvas = FindAnyObjectByType<Canvas>();
        recycleArea = canvas.transform.Find("OptionUI/Recycle").GetComponent<RectTransform>();
        forbidSprite = Resources.Load<Sprite>("Sprite/forbidSprite");
        sprite = Resources.Load<Sprite>("Sprite/permitSprite");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            
            Vector3 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            originPos = MouseWorldPos;

            RaycastHit2D hit2D = Physics2D.Raycast(MouseWorldPos, Vector2.zero);
            if (hit2D.collider != null && hit2D.collider.gameObject.layer == LayerMask.NameToLayer("character"))
            {
                isDragging = true;
                CameraDrag.canDrag = false;

                Debug.Log("hit!");
                Debug.Log(hit2D.collider.gameObject.layer);
                dragCharacter = hit2D.collider.gameObject.transform.parent.gameObject;
                dragCharacter.SetActive(false);
                GameObject icon = new GameObject("dragIcon");
                dragIcon = icon.AddComponent<Image>();
                dragIcon.sprite = sprite;
                dragIcon.transform.SetParent(canvas.transform, false);
                dragRect = dragIcon.GetComponent<RectTransform>();
                dragRect.sizeDelta = new Vector2(100, 150);

                dragIcon.raycastTarget = false;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (dragIcon != null)
            {               
                dragRect.position = Input.mousePosition;
            }
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float currentHeigthDiff = mouseWorldPos.y - groundY;
            
            if(currentHeigthDiff >= -13f  & currentHeigthDiff < -9f)
            {
               
                if (dragIcon != null)
                {

                    if (dragIcon.sprite != sprite)
                        dragIcon.sprite = sprite;
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

        if (Input.GetMouseButtonUp(0))
        {

            if (dragCharacter != null)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.y = -12f;
                mouseWorldPos.z = 0f;
                dragCharacter.transform.position = mouseWorldPos;

                if (canPlace)
                {                 
                    dragCharacter.SetActive(true);
                }
                else
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(
                    recycleArea as RectTransform,
                    Input.mousePosition,
                    null))
                    {
                        
                        CoinManager.Instance.AddCoin(500);
                        ObjectPoolRegister.Instance._objectPool.Despawn(dragCharacter.GetComponent<IObject>());
                    }
                    else 
                    {
                        Vector3 tmp = new Vector3(originPos.x, -12f,0);
                        dragCharacter.transform.position = tmp;    
                        dragCharacter.SetActive(true);
                    }
                    
                }
               
                CameraDrag.canDrag = true;
            }
            if (dragIcon != null)
                Destroy(dragIcon.gameObject);
            dragCharacter = null;
            dragIcon = null;
            dragRect = null;
            isDragging = false;
        }
    }
}
