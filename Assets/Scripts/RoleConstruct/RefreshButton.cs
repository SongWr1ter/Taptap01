using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshButton : MonoBehaviour
{
    public RoleQueueManager RoleQueueManager;
    // Start is called before the first frame update
    public void OnClickRefresh()
    {
        RoleQueueManager.RefreshAll();
    }
}
