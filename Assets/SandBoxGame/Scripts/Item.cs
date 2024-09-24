using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType  // ������ Ÿ��
    {
        Equipment,
        Used,
        Ingredient,
        ETC,
    }

    public string itemName; // �������� �̸�
    [TextArea]
    public string itemDesc; // �������� ����
    public ItemType itemType; // ������ Ÿ��
    public Sprite itemImage; // �������� �̹���(�κ� �丮 �ȿ��� ���)
    public GameObject itemPrefab;  // �������� ������

    public string weaponType;  // ���� Ÿ��
}