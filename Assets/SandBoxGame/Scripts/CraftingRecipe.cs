using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "New Recipe/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("제작에 필요한 재료 아이템들")]
    [SerializeField] public CraftingItemInfo[] reqItems;

    [Header("제작 결과물 아이템")]
    [SerializeField] public CraftingItemInfo resultItem;

    [Header("아이콘을 표시할 이미지")]
    [SerializeField] public Sprite buttonSprite;
}

[System.Serializable]
public struct CraftingItemInfo
{
    [SerializeField] public Item item;
    [SerializeField] public int count;
}