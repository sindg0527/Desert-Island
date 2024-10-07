using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject shopPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        shopPanel.SetActive(false); // 시작 시 패널 비활성화
    }

    public void ShowShop()
    {
        shopPanel.SetActive(true);
        PlayerManager.Instance.playerPause = true;
    }
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        PlayerManager.Instance.playerPause = false;
    }

    public void Buy()
    {
        if (GameManager.Instance.playerCoin >= ButtonInfo.instance.item.price)
        {
            GameManager.Instance.playerCoin -= ButtonInfo.instance.item.price;
            ToolManager.instance.theInventory.AcquireItem(ButtonInfo.instance.item);
        }
        else
        {
            Debug.Log("금액이 부족합니다.");
        }
    }
}