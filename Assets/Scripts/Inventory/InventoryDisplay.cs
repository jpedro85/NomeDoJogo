using System;
using System.Collections.Generic;
using DataPersistence;
using DataPersistence.Data;
using Scriptable_Objects.Inventory.Scipts;
using TMPro;
using UnityEngine;

namespace Inventory
{
    public class InventoryDisplay : MonoBehaviour, IDataPersistence
    {
        public InventoryObject inventory;
        public int X_START;
        public int Y_START;
        public int NUMBER_OF_COLUMNS;
        private Dictionary<InventorySlot, GameObject> itemsDisplay = new Dictionary<InventorySlot, GameObject>();

        private void Start()
        {
            createDisplay();
        }

        private void Update()
        {
            updateDisplay();
        }

        public void createDisplay()
        {
            for (int itemIndex = 0; itemIndex < inventory.container.Count; itemIndex++)
            {
                var obj = Instantiate(inventory.container[itemIndex].item.prefab, Vector3.zero, Quaternion.identity, transform);
                RectTransform rectTransform = inventory.container[itemIndex].item.prefab.GetComponent<RectTransform>();
                float itemWidth = rectTransform.rect.width * rectTransform.localScale.x;
                float itemHeight = (rectTransform.rect.height + 20) * rectTransform.localScale.y;
                obj.GetComponent<RectTransform>().localPosition = getPosition(itemIndex, itemWidth, itemHeight);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[itemIndex].amount.ToString("n0");
                itemsDisplay.Add(inventory.container[itemIndex], obj);
            }
        }

        public Vector3 getPosition(int itemIndex, float itemWidth, float itemHeight)
        {
            return new Vector3(X_START + (itemWidth * (itemIndex % NUMBER_OF_COLUMNS)),Y_START + (-itemHeight * (itemIndex / NUMBER_OF_COLUMNS)), 0f);
        }

        public void updateDisplay()
        {
            for (int itemIndex = 0; itemIndex < inventory.container.Count; itemIndex++)
            {
                if (itemsDisplay.ContainsKey(inventory.container[itemIndex]))
                {
                    itemsDisplay[inventory.container[itemIndex]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[itemIndex].amount.ToString("n0");
                }
                else
                {
                    var obj = Instantiate(inventory.container[itemIndex].item.prefab, Vector3.zero, Quaternion.identity,transform);
                    RectTransform rectTransform = inventory.container[itemIndex].item.prefab.GetComponent<RectTransform>();
                    
                    float itemWidth = rectTransform.rect.width * rectTransform.localScale.x;
                    float itemHeight = (rectTransform.rect.height + 20) * rectTransform.localScale.y;
                    
                    obj.GetComponent<RectTransform>().localPosition = getPosition(itemIndex, itemWidth, itemHeight);
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[itemIndex].amount.ToString("n0");
                    
                    itemsDisplay.Add(inventory.container[itemIndex], obj);
                }
            }
        }

        public void loadData(GameData gameData)
        {
            this.inventory.container = gameData.playerInventory.container;
        }

        public void saveData(GameData gameData)
        {
            gameData.playerInventory.container = this.inventory.container;
        }
    }
}