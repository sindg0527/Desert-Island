using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    [Header("���� ��� �������� ����")]
    [SerializeField] private Slot ResultItemSlot;

    [Header("���ۿ� �ʿ��� ��� �������� ��� �����̵� ������ Ʈ������")]
    [SerializeField] private Transform mRecipeContentTransform;

    [Header("���� ��ư")]
    [SerializeField] private Button mCraftingButton;

    [Header("��Ȱ��ȭ ���½� ������ �̹��� ������Ʈ")]
    [SerializeField] private GameObject mDisableImageGo;

    /// <summary>
    /// ���� �ش� ������ ������� ������
    /// </summary>
    [HideInInspector] public CraftingRecipe CurrentRecipe;

    public void Init(CraftingRecipe recipe)
    {
        CurrentRecipe = recipe;

        // ���� Ȱ��ȭ
        gameObject.SetActive(true);

        // ���� ��� �������� ���Կ� ���
        ResultItemSlot.ClearSlot();
        Inventory.instance.AcquireItem(recipe.resultItem.item, recipe.resultItem.count);

        mCraftingButton.GetComponent<Image>().sprite = recipe.buttonSprite;

        // ���� �������� ��� ������ ������ �����ϸ� ������ �°� �ν��Ͻ�
        for (int i = mRecipeContentTransform.childCount; i <= recipe.reqItems.Length; ++i)
            Instantiate(ResultItemSlot, Vector3.zero, Quaternion.identity, mRecipeContentTransform);

        // ��� ��� ������ ������ �ʱ�ȭ
        for (int i = 0; i < mRecipeContentTransform.childCount; ++i)
        {
            // ���� ȹ��
            Slot recipeSlot = mRecipeContentTransform.GetChild(i).GetComponent<Slot>();

            // �������� ��� �������� ���� �ε��� ��ȣ���?
            if (i < recipe.reqItems.Length)
            {
                // ���Կ� �������� ���
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

        // ��� ������ ������ Ȯ���Ͽ� ���� �κ��丮�� �������� ����
        foreach (CraftingItemInfo info in CurrentRecipe.reqItems)
        {
            //Inventory.instance.slots.  (info.item.name, out mainInventoryslot, info.count);
            mainInventoryslot.SetSlotCount(-info.count);
        }

        // ���� �� ��� �������� �κ��丮�� ȹ��
        Inventory.instance.AcquireItem(CurrentRecipe.resultItem.item, CurrentRecipe.resultItem.count);
    }

    private IEnumerator CoCraftItem()
    {
        // ������ ȹ��
        RefreshItems();
        yield return null;
    }

    public void BTN_Craft()
    {
        StartCoroutine(CoCraftItem());
    }
}
