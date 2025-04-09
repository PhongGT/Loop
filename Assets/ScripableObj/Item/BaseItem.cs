using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BaseItem", menuName = "BaseItem")]
public class BaseItem : ScriptableObject
{
    [SerializeField] public List<Sprite> itemImage;
    public enum ItemName
    {
        Sword,
        Armor,
        Bow,
        Staff,
        Book,

    }
    public ItemName itemName;
}
