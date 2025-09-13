using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RoleEntry
{
    public String name;        
    public List<Sprite> sprites;     
}

[System.Serializable]
public class RoleCategory
{
    public List<RoleEntry> roles;
}
