using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Shop[] shops;

    public Button refreshBtn;

    private void OnEnable()
    {
        refreshBtn.interactable = true;
    }

    public void OnRefresh()
    {
        refreshBtn.interactable = false;
        foreach (Shop shop in shops)
        {
            Shop.ShopType shopType = shop.shopType;

            switch (shopType)
            {
                case Shop.ShopType.Heal:
                    shop.count = 1;
                    break;
                case Shop.ShopType.Upgrade:
                    shop.count = 2;
                    break;
                case Shop.ShopType.Scroll:
                    shop.count = 0;
                    break;
                case Shop.ShopType.Awake:
                    shop.count = 0;
                    break;
            }
        }
    }
}
