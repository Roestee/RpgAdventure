using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Defualt")]
public class DefualtObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Defult;
    }
}
