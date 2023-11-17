using System;
using System.Collections.Generic;
using System.Linq;
using DataPersistence;
using DataPersistence.Data;
using Scriptable_Objects.Items.Scripts;
using UnityEngine;


namespace Inventory
{
    public class Inventory : MonoBehaviour, IDataPersistence
    {
        #region Singleton

        public static Inventory instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("Theres more than one instance");
                return;
            }

            instance = this;
        }

        #endregion

        public delegate void onItemChanged();

        public onItemChanged OnItemChangedCallBack;

        public List<Item> items = new List<Item>();
        public int SPACE = 20;

        public bool addToInventory(Item item)
        {
            if (items.Count >= SPACE)
            {
                Debug.Log("Not Enough Space!");
                return false;
            }

            // Searches the inventory and gets the first item on it that matches the name of the item picked up
            // if it doesnt it adds to the inventory with its amount
            try
            {
                Item inventoryItem = items.First(itemsItem => itemsItem.itemName == item.itemName);

                Debug.Log(inventoryItem);
                inventoryItem.amount += item.amount;
                OnItemChangedCallBack?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(item);
                items.Add(item);
                OnItemChangedCallBack?.Invoke();
                return true;
            }
        }

        public void removeFromInventory(Item item)
        {
            items.Remove(item);
            OnItemChangedCallBack?.Invoke();
        }

        public void loadData(GameData gameData)
        {
            foreach (var item in gameData.playerInventory.items)
            {
                items.Add(item.Value);
            }
            // this.items = gameData.playerInventory.items;
            // this.items = gameData.playerInventory.items.Select(itemData => 
            // {
            //     Item newItem = ScriptableObject.CreateInstance<Item>();
            //     newItem.itemName = itemData.itemName;
            //     newItem.amount = itemData.amount;
            //     // newItem.icon = ItemData.ConvertBytesToSprite(itemData.icon);
            //     newItem.icon = AssetDatabase.LoadAssetAtPath<Sprite>(itemData.iconPath);
            //     newItem.itemDescription = itemData.itemDescription;
            //     return newItem;
            // }).ToList();
        }

        public void saveData(GameData gameData)
        {
            Dictionary<string, Item> savedInventory = new Dictionary<string, Item>();

            foreach (var item in this.items)
            {
                savedInventory.Add(item.itemName,item);
            }

            gameData.playerInventory.items = savedInventory;
            // gameData.playerInventory.items = this.items;
            // gameData.playerInventory.items = this.items.Select(item => new ItemData(item)).ToList();
        }
    }
}