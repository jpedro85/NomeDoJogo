using Scriptable_Objects.Items.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public TMP_Text amountText;
        public Image icon;
        Item item;

        public ItemDescripton itemDescriptonOverlay;
        
        public void addItem(Item newItem)
        {
            item = newItem;
            icon.sprite = item.icon;
            icon.enabled = true;
            amountText.enabled = true;
            amountText.text = item.amount.ToString("n0");
        }
        
        public void clearSlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            amountText.enabled = false;
        }

        public void removeItem()
        {
            Inventory.instance.removeFromInventory(item);
        }

        public void displayItemInfo()
        {
            if (item==null)
            {
                return;
            }

            itemDescriptonOverlay.displayItemInfo(item);
        }
    }
}