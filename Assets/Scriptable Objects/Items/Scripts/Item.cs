using UnityEngine;

namespace Scriptable_Objects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public int amount;
        public Sprite icon;

        public virtual void use()
        {
            // Use the item
            // Something might happen 
            
            Debug.Log("Using "+ itemName);
        }
    }
}