﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    public Dictionary<int, Item> ItemDictionary;
    public GameObject Tips;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        UIMove.OnEnter += UIMoveOnEnter;
        UIMove.OnExit += UIMoveOnExit;
    }
    void Start()
    {
        LoadData();
        UITips.instance.HideTips();
    }

    void Update()
    {

    }

    void CreateItem(Item item, Transform parent)
    {
        GameObject newItem = Resources.Load("grid") as GameObject;
        if (newItem.GetComponent<UIItem>() != null)
        {
            newItem.GetComponent<UIItem>().SetName(item.Name);
            newItem.GetComponent<UIItem>().SetCount(item.Count.ToString());
            newItem.GetComponent<UIItem>().SetImage(item.Picture);
        }
        GameObject.Instantiate(newItem, parent);
        ItemData.SaveData(item.Id, item);
    }

    public void StoreItem(int ID)
    {
        if (!ItemDictionary.ContainsKey(ID))
            return;
        Transform parent = GridPanel.instance.GetEmptyGrid();
        if (parent == null)
        {
            Debug.Log("背包装满!");
        }
        else
        {
            Item temp = ItemDictionary[ID];
            if (temp != null)
            {
                if (ItemData.ContainItem(temp))
                {
                    GridPanel.instance.GetExistItem(temp.Name).GetChild(0).GetChild(2).GetComponent<Text>().text = temp.Count.ToString();
                    ItemData.SaveData(ID, temp);
                }
                else
                    CreateItem(temp, parent);
            }
        }
    }

    void LoadData()
    {
        ItemDictionary = new Dictionary<int, Item>();
        Weapon weapon1 = new Weapon(0, "武器1", "武器1武器1武器1武器1武器1", 200
            , 1, "ItemPicture/weapon1", 1, 20);
        Weapon weapon2 = new Weapon(1, "武器2", "武器2武器2武器2武器2武器2", 200
            , 1, "ItemPicture/weapon2", 1, 20);
        Weapon weapon3 = new Weapon(2, "武器3", "武器3武器3武器3武器3武器3", 200
            , 1, "ItemPicture/weapon3", 1, 20);
        ItemDictionary.Add(weapon1.Id, weapon1);
        ItemDictionary.Add(weapon2.Id, weapon2);
        ItemDictionary.Add(weapon3.Id, weapon3);
    }

    void UIMoveOnEnter(Transform transform)
    {
        string name = GridPanel.instance.GetItemFromTransform(transform);
        Debug.Log(name);
        if (name != null)
        {
            //string content = UITips.instance.GetStringText(ItemData.GetItemFromName(name));
            UITips.instance.UpdateText(name);
            UITips.instance.ShowTips();
        }
    }

    void UIMoveOnExit()
    {
        if (UITips.instance.gameObject.activeSelf)
            UITips.instance.HideTips();
    }
}
