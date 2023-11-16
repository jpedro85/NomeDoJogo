using Scriptable_Objects.Items.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public Image icon;
        Item item;

        public void addItem(Item newItem)
        {
            item = newItem;
            icon.sprite = item.icon;
            icon.enabled = true;
        }

        public void clearSlot()
        {
            item = null;

            icon.sprite = null;
            icon.enabled = false;
        }

        public void removeItem()
        {
            Inventory.instance.remove(item);
        }

        public void useItem()
        {
            if (item != null)
            {
                item.use();
            }
        }
    }
}