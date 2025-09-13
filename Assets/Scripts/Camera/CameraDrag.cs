using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 5.0f;
    public static bool canDrag = true;
    private Vector3 dragOrigin;
    private bool dragStarted=false;
  

    public Vector2 minBound;
    public Vector2 maxBound;    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canDrag) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0)) {

            dragOrigin = Input.mousePosition;            
            dragStarted = true;
            return;
        }
        if (Input.GetMouseButton(0)) {
            if (dragStarted)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

                transform.Translate(-move, Space.World);
                dragOrigin = Input.mousePosition;
            }
        }
        if (Input.GetMouseButtonUp(0)) { 
            dragStarted = false;
        }
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x,minBound.x,maxBound.x);
        pos.y = Mathf.Clamp(pos.y,minBound.y,maxBound.y);
        transform.position = pos;
    }
}
