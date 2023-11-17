using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class InventoryData
    {
        // public List<ItemData> items;
        // public List<Item> items;
        public List<string> keys = new List<string>();
        public List<Item> values = new List<Item>();
        public Dictionary<string, Item> items
        {
            get { return this.ToDictionary(); }
            set { this.FromDictionary(value); }
        }

        // public InventoryData(List<ItemData> itemsToLoad)
        // public InventoryData(List<Item> itemsToLoad)
        public InventoryData(Dictionary<string, Item> itemsToLoad)
        {
            this.items = itemsToLoad;
        }
        
        public Dictionary<string, Item> ToDictionary()
        {
            Dictionary<string, Item> dictionary = new Dictionary<string, Item>();

            for (int i = 0; i < keys.Count; i++)
            {
                dictionary[keys[i]] = values[i];
            }

            return dictionary;
        }

        public void FromDictionary(Dictionary<string, Item> dictionary)
        {
            keys.Clear();
            values.Clear();

            foreach (var kvp in dictionary)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }
        
    }
}