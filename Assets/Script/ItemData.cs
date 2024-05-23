using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")] // CreateAssetMenu : 커스텀 메뉴를 생성하는 속성
public class ItemData : ScriptableObject
{ // ScriptableObject : 다양한 데이터를 저장하는 에셋
    public enum ItemType { Melee, Range, Glove, Shoe, Heal};

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount; // 근접 : 갯수, 원거리 : 관통
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;
}
