using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoleDragHandler : MonoBehaviour
{
    
    public Sprite sprite;
 

    private Image dragIcon;
    private RectTransform dragRect;
    private GameObject dragCharacter;
    private Camera mainCamera;
    private Canvas canvas;
    private static bool isDragging = false;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        canvas = FindAnyObjectByType<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            
            Vector3 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(MouseWorldPos, Vector2.zero);
            if (hit2D.collider != null && hit2D.collider.gameObject.layer == LayerMask.NameToLayer("character"))
            {
                isDragging = true;
                CameraDrag.canDrag = false;
                Debug.Log("hit!");
                dragCharacter = hit2D.collider.gameObject;
                dragCharacter.SetActive(false);
                GameObject icon = new GameObject("dragIcon");
                dragIcon = icon.AddComponent<Image>();
                dragIcon.sprite = sprite;
                dragIcon.transform.SetParent(canvas.transform, false);
                dragRect = dragIcon.GetComponent<RectTransform>();
                dragRect.sizeDelta = new Vector2(100, 100);

                dragIcon.raycastTarget = false;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (dragIcon != null)
            {               
                dragRect.position = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragIcon != null)
                Destroy(dragIcon.gameObject);

            if (dragCharacter != null)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;
                dragCharacter.transform.position = mouseWorldPos;
                dragCharacter.SetActive(true);
                CameraDrag.canDrag = true;
            }
            
            dragCharacter = null;
            dragIcon = null;
            dragRect = null;
            isDragging = false;
        }
    }
}
