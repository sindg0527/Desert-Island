using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool invectoryActivated = false;  // 인벤토리 활성화 여부

    [SerializeField]
    private GameObject go_InventoryBase; // Inventory_Base 이미지
    [SerializeField]
    private GameObject go_SlotsParent;  // Slot들의 부모인 Grid Setting 

    private Slot[] slots;  // 슬롯들 배열

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        OnInventory();
    }

    private void OnInventory()  // 인벤토리 on/off
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

    private void OpenInventory() //인벤토리 열기
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory() //인벤토리 닫기
    {
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)  // 아이템 습득
    {
        if (Item.ItemType.Equipment != _item.itemType)  // 장비가 아니라면 갯수 표시, 장비일 경우 갯수 x
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)  // null 이라면 slots[i].item.itemName 할 때 런타임 에러 발생
                {
                    if (slots[i].item.itemName == _item.itemName)  // 이름이 같은 아이템이 인벤토리에 있다면 갯수만 증가
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)  // 이름이 같은 아이템이 인벤토리에 없다면 빈 슬롯에 아이템 추가
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}