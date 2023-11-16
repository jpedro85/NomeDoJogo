using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class InventoryData
    {
        public List<Item> items = new List<Item>();

        public InventoryData(List<Item> Items)
        {
            this.items = new List<Item>();
        }
    }
}