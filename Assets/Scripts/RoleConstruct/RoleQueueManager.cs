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
    Dictionary<string, int> roleWeights = new Dictionary<string, int>()
{
    { "Gunner", 70 },   
    { "Bomber", 25 },    
    { "AttackA", 4 },       
    { "AttackB", 1 }      
};


    // Start is called before the first frame update
    void Start()
    {
        slotToCategory = new Dictionary<Image, RoleCategory>()
        {
            {AttackSlot,Attack },
            {GunSlot, Gun },
            {BombSlot, Bomb }
        };
        CoinManager.Instance.AddCoin(100);
        RefreshAll();
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshAll()
    {
        if (CoinManager.Instance.SpendCoin(100))
        {
            foreach (var item in slotToCategory)
            {
                RefreshRole(item.Key, item.Value);
            }
        }
        
    }

    public void RefreshRole(Image slot,RoleCategory roleCategory)
    {
        Debug.Log(roleCategory.roles.Count);
        int totalWeight = 0;
        foreach (var role in roleCategory.roles)
        {
            if (roleWeights.ContainsKey(role.name))
            {
                totalWeight += roleWeights[role.name];
            }
            else
            {
                totalWeight += 1;
            }
        }
        int randomValue = Random.Range(0, totalWeight);

        int cumulative = 0;
        string chosenRoleName = null;
        foreach (var role in roleCategory.roles)
        {
            int weight = roleWeights.ContainsKey(role.name) ? roleWeights[role.name] : 1;
            cumulative += weight;

            if (randomValue < cumulative)
            {
                chosenRoleName = role.name;
                break;
            }
        }

        int index = Random.Range(0,roleCategory.roles.Count);
        System.String roleName = roleCategory.roles[index].name;
        //修改逻辑？此处通过从对象池中拿出一个prefab，再填入对应数据（通过名字？），将其赋值给rolePrefab
        //此处修改为抽取角色名字->生成对应prefab->赋值给rolePrefab
        
        slot.sprite = roleCategory.roles[index].sprites[0];

        //拖拽逻辑
        var drag = slot.GetComponent<UIDragHandler>();
        if (drag == null)
        {
            drag = slot.gameObject.AddComponent<UIDragHandler>();
        }
        drag.battleUnitData = Resources.Load<BattleUnitData>($"Data/{roleName}UnitData");
        drag.dragSprite = roleCategory.roles[index].sprites[1];
        drag.cost = roleCategory.roles[index].cost;
    }
}
