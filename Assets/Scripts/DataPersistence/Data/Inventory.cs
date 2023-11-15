using System.Collections.Generic;
using Scriptable_Objects.Inventory.Scipts;
using Scriptable_Objects.Items.Scripts;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class Inventory
    {
        public List<InventorySlot> container;

        public Inventory()
        {
            this.container = new List<InventorySlot>();
        }


        public Inventory(List<InventorySlot> container)
        {
            this.container = container;
        }
    }
}