using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public static ButtonInfo instance;

    public Item item;
    public Text PriceText;
    Image image;

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
        image = GetComponent<Image>();
        image.sprite = item.itemImage;

        PriceText.text = "Price : " + item.price.ToString();
    }
}