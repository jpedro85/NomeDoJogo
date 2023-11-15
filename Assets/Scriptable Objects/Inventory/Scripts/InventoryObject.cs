using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;
using UnityEngine;

namespace Scriptable_Objects.Inventory.Scipts
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public List<InventorySlot> container = new List<InventorySlot>();

        public void addItem(ItemObject _item, int _amount)
        {
            for (int i = 0; i < container.Count; i++)
            {
                if (container[i].item == _item)
                {
                    container[i].addAmount(_amount);
                    return;
                }
            }
            container.Add(new InventorySlot(_item, _amount));
        }
    }

    [System.Serializable]
    public class InventorySlot
    {
        public ItemObject item;
        public int amount;

        public InventorySlot(ItemObject _item, int _amount)
        {
            item = _item;
            amount = _amount;
        }

        public void addAmount(int value)
        {
            this.amount += value;
        }
    }
}