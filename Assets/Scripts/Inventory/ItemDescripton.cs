using Inventory;
using Scriptable_Objects.Items.Scripts;
using TMPro;
using UnityEngine;

public class ItemDescripton : MonoBehaviour
{
    #region Overlay

    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public GameObject overlay;
    private Item item;

    public void displayItemInfo(Item item)
    {
        // Check if the overlay is already active, if it is it just updates the contents else it activates the popup
        if (!overlay.activeSelf)
        {
            overlay.SetActive(!overlay.activeSelf);
        }
        
        this.itemName.text = item.itemName;
        this.itemDescription.text = item.itemDescription;
        this.item = item;
    }

    #endregion

    public void useItem()
    {
        if (item == null)
        {
            Debug.Log("Item Null");
        }
        else if (item.amount - 1 == 0)
        {
            item.use();
            overlay.SetActive(!overlay.activeSelf);
            item = null;
        }
        else
        {
            item.use();
            overlay.SetActive(!overlay.activeSelf);
            item = null;
        }
    }
}