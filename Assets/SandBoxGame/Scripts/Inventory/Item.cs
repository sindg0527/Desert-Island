using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType  // 아이템 타입
    {
        Equipment, //장비
        Used,      //소모품
        Ingredient,//재료
        Furniture,//가구
        ETC,      //기타
    }

    public string itemName; // 아이템의 이름
    [TextArea]
    public string itemDesc; // 아이템의 설명
    public ItemType itemType; // 아이템 타입
    public Sprite itemImage; // 아이템의 이미지(인벤 토리 안에서 띄울)
    public Sprite itemSideImage; // 아이템의 옆이미지
    public GameObject itemPrefab;  // 아이템의 프리팹

    public string weaponType;  // 무기 타입
    public int itemValue;
}