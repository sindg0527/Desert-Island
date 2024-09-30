using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage;  // 아이템의 이미지

    [SerializeField]
    private Text text_Count;  // 아이템 카운트
    [SerializeField]
    private GameObject go_CountImage;

    Inventory inventory;

    // 아이템 이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 인벤토리에 새로운 아이템 슬롯 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment) //획득한 아이템이 장비일 때, 중첩 불가
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }

    // 해당 슬롯의 아이템 갯수 업데이트
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    // 해당 슬롯 하나 삭제
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                // 소모품일 경우 사용
                if(item.itemType == Item.ItemType.Used)
                {
                    Debug.Log(item.itemName + " 을 사용했습니다.");
                    SetSlotCount(-1);
                }
                else if (item.itemType == Item.ItemType.Equipment)
                {
                    Debug.Log(item.itemName + " 을 장착했습니다.");
                    if(ToolManager.instance.getItem == null)
                    {
                        ToolManager.instance.getItem = item.itemPrefab;
                        ToolManager.instance.ToolSprite();
                        ClearSlot();
                    }
                    else
                    {
                        inventory = gameObject.GetComponent<Inventory>();
                        inventory.AcquireItem(item);
                        ToolManager.instance.getItem = item.itemPrefab;
                        ToolManager.instance.ToolSprite();
                    }
                }
            }
        }
    }

    // 마우스 드래그가 시작 됐을 때 발생하는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    // 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop호출됨");
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    // 슬롯을 드래그 드롭하여 서로 자리를 바꾸기 위한 함수
    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }

        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}