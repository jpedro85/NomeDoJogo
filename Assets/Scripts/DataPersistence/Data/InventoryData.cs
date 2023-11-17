using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class InventoryData
    {
        public List<ItemData> items;

        public InventoryData(List<ItemData> itemsToLoad)
        {
            this.items = itemsToLoad;
        }
        
    }
}