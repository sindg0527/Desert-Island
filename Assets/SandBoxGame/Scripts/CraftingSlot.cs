using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    [Header("제작 결과 아이템의 슬롯")]
    [SerializeField] private Slot ResultItemSlot;

    [Header("제작에 필요한 재료 아이템을 담는 슬라이드 콘텐츠 트랜스폼")]
    [SerializeField] private Transform mRecipeContentTransform;

    [Header("제작 버튼")]
    [SerializeField] private Button mCraftingButton;

    [Header("비활성화 상태시 보여줄 이미지 오브젝트")]
    [SerializeField] private GameObject mDisableImageGo;

    /// <summary>
    /// 현재 해당 슬롯이 사용중인 레시피
    /// </summary>
    [HideInInspector] public CraftingRecipe CurrentRecipe;

    public void Init(CraftingRecipe recipe)
    {
        CurrentRecipe = recipe;

        // 슬롯 활성화
        gameObject.SetActive(true);

        // 제작 결과 아이템을 슬롯에 등록
        ResultItemSlot.ClearSlot();
        Inventory.instance.AcquireItem(recipe.resultItem.item, recipe.resultItem.count);

        mCraftingButton.GetComponent<Image>().sprite = recipe.buttonSprite;

        // 제작 레시피의 재료 아이템 슬롯이 부족하면 개수에 맞게 인스턴스
        for (int i = mRecipeContentTransform.childCount; i <= recipe.reqItems.Length; ++i)
            Instantiate(ResultItemSlot, Vector3.zero, Quaternion.identity, mRecipeContentTransform);

        // 모든 재료 아이템 슬롯을 초기화
        for (int i = 0; i < mRecipeContentTransform.childCount; ++i)
        {
            // 슬롯 획득
            Slot recipeSlot = mRecipeContentTransform.GetChild(i).GetComponent<Slot>();

            // 레시피의 재료 개수보다 작은 인덱스 번호라면?
            if (i < recipe.reqItems.Length)
            {
                // 슬롯에 아이템을 등록
                recipeSlot.ClearSlot();
                Inventory.instance.AcquireItem(recipe.reqItems[i].item, recipe.reqItems[i].count);
                recipeSlot.gameObject.SetActive(true);
            }
            else
            {
                recipeSlot.gameObject.SetActive(false);
            }
        }
    }

    public void ToggleSlotState(bool isCraftable)
    {
        mDisableImageGo.SetActive(!isCraftable);
        mCraftingButton.interactable = isCraftable;
    }

    private void RefreshItems()
    {
        Slot mainInventoryslot = null;

        // 재료 아이템 정보를 확인하여 메인 인벤토리의 아이템을 제거
        foreach (CraftingItemInfo info in CurrentRecipe.reqItems)
        {
            //Inventory.instance.slots.  (info.item.name, out mainInventoryslot, info.count);
            mainInventoryslot.SetSlotCount(-info.count);
        }

        // 제작 후 결과 아이템을 인벤토리에 획득
        Inventory.instance.AcquireItem(CurrentRecipe.resultItem.item, CurrentRecipe.resultItem.count);
    }

    private IEnumerator CoCraftItem()
    {
        // 아이템 획득
        RefreshItems();
        yield return null;
    }

    public void BTN_Craft()
    {
        StartCoroutine(CoCraftItem());
    }
}
