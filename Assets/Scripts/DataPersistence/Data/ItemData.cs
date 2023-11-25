using UnityEngine;
using Scriptable_Objects.Items.Scripts;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public int amount;
        public ItemType itemType;
        public Sprite icon;
        public string itemDescription;
        

        public ItemData(Item item)
        {
            this.itemName = item.itemName;
            this.amount = item.amount;
            this.itemType = item.itemType;
            this.icon = Resources.Load<Sprite>($"Items/{item.itemName}");
            this.itemDescription = item.itemDescription;
        }
    }
}