using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "New Recipe/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("���ۿ� �ʿ��� ��� �����۵�")]
    [SerializeField] public CraftingItemInfo[] reqItems;

    [Header("���� ����� ������")]
    [SerializeField] public CraftingItemInfo resultItem;

    [Header("�������� ǥ���� �̹���")]
    [SerializeField] public Sprite buttonSprite;
}

[System.Serializable]
public struct CraftingItemInfo
{
    [SerializeField] public Item item;
    [SerializeField] public int count;
}