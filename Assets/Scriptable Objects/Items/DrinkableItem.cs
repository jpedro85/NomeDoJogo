using UnityEngine;

namespace Scriptable_Objects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Drinkable Object", menuName = "Item/Create New Drink")]
    public class DrinkableItem : Item
    {

        private static PlayUI playUI = null;

        public override void use()
        {
            if (playUI == null)
                playUI = GameObject.Find("UI_Buttons").GetComponent<PlayUI>();

            playUI.addDeltaEnergy(energy);
            playUI.addDeltaHealth(health);
            Debug.Log("h:" + health + "e:" + energy);
            Inventory.Inventory.instance.removeFromInventory(this);
        }

    }
}