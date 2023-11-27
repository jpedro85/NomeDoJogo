using UnityEngine;

namespace Scriptable_Objects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Food Object", menuName = "Item/Create New Food")]
    public class FoodItem : Item
    {
        public float health;
        public float energy;

        public override void use()
        {
            // TODO implement the actual functionality for the use of the food like displaying the item description on touch
            Debug.Log("Regenerated");
            Inventory.Inventory.instance.removeFromInventory(this);
        }
    }
}