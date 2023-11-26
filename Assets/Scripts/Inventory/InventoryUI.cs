using System;
using UnityEngine;

namespace Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public Transform itemsParent;
        public GameObject inventoryUI;

        Inventory inventory;

        InventorySlot[] slots;

        private void Start()
        {
            inventory = Inventory.instance;
            inventory.OnItemChangedCallBack += updateUI;

            slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        }

        public void updateUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < inventory.items.Count)
                {
                    slots[i].addItem(inventory.items[i]);
                }
                else
                {
                    slots[i].clearSlot();
                }
            }
        }
    }
}