using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool invectoryActivated = false;  // �κ��丮 Ȱ��ȭ ����

    [SerializeField]
    private GameObject go_InventoryBase; // Inventory_Base �̹���
    [SerializeField]
    private GameObject go_SlotsParent;  // Slot���� �θ��� Grid Setting 

    private Slot[] slots;  // ���Ե� �迭

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        OnInventory();
    }

    private void OnInventory()  // �κ��丮 on/off
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            invectoryActivated = !invectoryActivated;

            if (invectoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory() //�κ��丮 ����
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory() //�κ��丮 �ݱ�
    {
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)  // ������ ����
    {
        if (Item.ItemType.Equipment != _item.itemType)  // ��� �ƴ϶�� ���� ǥ��, ����� ��� ���� x
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)  // null �̶�� slots[i].item.itemName �� �� ��Ÿ�� ���� �߻�
                {
                    if (slots[i].item.itemName == _item.itemName)  // �̸��� ���� �������� �κ��丮�� �ִٸ� ������ ����
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)  // �̸��� ���� �������� �κ��丮�� ���ٸ� �� ���Կ� ������ �߰�
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}