using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : Singleton<OpenShop>
{
    private GameObject shop;

    void Start()
    {
        shop = GameObject.Find("ShopPanel");
        if (shop != null)
        {
            shop.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shop != null && other.gameObject.GetComponent<PlayerController>())
        {
            shop.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (shop != null && other.gameObject.GetComponent<PlayerController>())
        {
            shop.SetActive(false);
        }
    }

}
