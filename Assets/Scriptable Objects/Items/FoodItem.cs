using UnityEngine;

namespace Scriptable_Objects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Food Object", menuName = "Item/Create New Food")]
    public class FoodItem : Item
    {
        private PlayUI playUI = null;

        public override void use()
        {
            if(playUI == null)
                playUI = GameObject.Find("UI_Buttons").GetComponent<PlayUI>();

            Inventory.Inventory.instance.removeFromInventory(this);
            playUI.addDeltaEnergy(energy);
            playUI.addDeltaHealth(health);
        }
    }
}