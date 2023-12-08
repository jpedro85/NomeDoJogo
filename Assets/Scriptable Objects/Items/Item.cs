using UnityEngine;

namespace Scriptable_Objects.Items.Scripts
{
    public enum ItemType
    {
        Food,
        Drink
    }
    
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public int amount;
        public Sprite icon;
        public ItemType itemType;
        public float health;
        public float energy;

        [TextArea(15, 20)] public string itemDescription;

        public virtual void use()
        {
            // Use the item
            // Something might happen 
            Debug.Log("Using " + itemName);
        }
    }
}