using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleQueueManager : MonoBehaviour
{
    //角色池
    public RoleCategory Attack;
    public RoleCategory Gun;
    public RoleCategory Bomb;

    //槽位
    public Image AttackSlot;
    public Image GunSlot;
    public Image BombSlot;

    private Dictionary<Image, RoleCategory> slotToCategory;
   

    // Start is called before the first frame update
    void Start()
    {
        slotToCategory = new Dictionary<Image, RoleCategory>()
        {
            {AttackSlot,Attack },
            {GunSlot, Gun },
            {BombSlot, Bomb }
        };
        
        RefreshAll();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshAll()
    {
        foreach (var item in slotToCategory)
        {
            RefreshRole(item.Key, item.Value);  
        }
    }

    public void RefreshRole(Image slot,RoleCategory roleCategory)
    {
        Debug.Log(roleCategory.roles.Count);
        int index = Random.Range(0,roleCategory.roles.Count);
        GameObject rolePrefab = roleCategory.roles[index].prefab;
        //修改逻辑？此处通过从对象池中拿出一个prefab，再填入对应数据（通过名字？），将其赋值给rolePrefab
        //此处修改为抽取角色名字->生成对应prefab->赋值给rolePrefab
        
        slot.sprite = roleCategory.roles[index].sprites[0];

        //拖拽逻辑
        var drag = slot.GetComponent<UIDragHandler>();
        if (drag == null)
        {
            drag = slot.gameObject.AddComponent<UIDragHandler>();
        }
        drag.rolePrefab = rolePrefab;
        Debug.Log(roleCategory.roles[index].sprites.Count);
        
        drag.dragSprite = roleCategory.roles[index].sprites[1];
    }
}
