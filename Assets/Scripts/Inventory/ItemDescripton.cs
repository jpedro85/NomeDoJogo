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
        Debug.Log(item.name);
        this.itemName.text = item.itemName;
        this.itemDescription.text = item.itemDescription;
        overlay.SetActive(!overlay.activeSelf);
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