using System.Collections.Generic;

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