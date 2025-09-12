using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleQueueManager : MonoBehaviour
{
    //½ÇÉ«³Ø
    public RoleCategory Attack;
    public RoleCategory Gun;
    public RoleCategory Bomb;

    //²ÛÎ»
    public Image AttackSlot;
    public Image GunSlot;
    public Image BombSlot;

    private Dictionary<Image, RoleCategory> slotToCategory;
    private Dictionary<Image, GameObject> slotToPrefab;

    // Start is called before the first frame update
    void Start()
    {
        slotToCategory = new Dictionary<Image, RoleCategory>()
        {
            {AttackSlot,Attack },
            {GunSlot, Gun },
            {BombSlot, Bomb }
        };
        slotToPrefab = new Dictionary<Image, GameObject>();
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

        slotToPrefab[slot] = rolePrefab;
        slot.sprite = roleCategory.roles[index].sprites[0];

        //ÍÏ×§Âß¼­
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
