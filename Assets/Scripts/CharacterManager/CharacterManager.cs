using Scriptable_Objects.Items.Scripts;
using UnityEngine;

namespace CharacterManager
{
    // This is going to be where you are going to write code relevant to the player actions
    public class CharacterManager : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            // TODO needs to be tested to see if gets the right scriptable object instead of the super class Item
            var item = other.GetComponent<Item>();
            if (item)
            {
                Debug.Log("Its touching");
                bool wasPickup = Inventory.Inventory.instance.addToInventory(item);

                if (wasPickup)
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}