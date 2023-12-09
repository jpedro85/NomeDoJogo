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
                var inventoryItem = items.First(itemsItem => itemsItem.itemName == item.itemName);

                Debug.LogWarning("before:" + inventoryItem.name + ":" + inventoryItem.amount);
                inventoryItem.amount += item.amount;
                Debug.LogWarning("after:" + inventoryItem.name + ":"  + inventoryItem.amount);
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
            // Going to search the inventory for the item and check whether if used is going to be removed or just decrease its amount
            try
            {
                var inventoryItem = items.First(itemsItem => itemsItem.itemName == item.itemName);

                if (inventoryItem.amount - 1 == 0)
                {
                    items.Remove(item);
                    OnItemChangedCallBack?.Invoke();
                }
                else
                {
                    inventoryItem.amount -= 1;
                    OnItemChangedCallBack?.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Item does not exist and you trying to remove it.\n"+e);
            }
        }

        public void loadData(GameData gameData)
        {
            this.items = gameData.playerInventory.items.Select(itemData =>
            {
                Item item = ScriptableObject.CreateInstance<Item>();
                switch (itemData.itemType)
                {
                    case ItemType.Food:
                        item = ScriptableObject.CreateInstance<FoodItem>();
                        break;
                    case ItemType.Drink:
                        item = ScriptableObject.CreateInstance<DrinkableItem>();
                        break;
                    default:
                        item = ScriptableObject.CreateInstance<Item>();
                        break;
                }

                item.itemName = itemData.itemName;
                item.amount = itemData.amount;
                item.icon = itemData.icon;
                item.itemDescription = itemData.itemDescription;
                item.health = itemData.health;
                item.energy = itemData.energy;

                return item;

            }).ToList();
        }

        public void saveData(GameData gameData)
        {
            gameData.playerInventory.items = this.items.Select(item => new ItemData(item)).ToList();
        }
    }
}