using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RoleEntry
{
    public GameObject prefab;        
    public List<Sprite> sprites;     
}

[System.Serializable]
public class RoleCategory
{
    public List<RoleEntry> roles;
}
