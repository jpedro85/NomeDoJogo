using System;
using Inventory;
using Scriptable_Objects.Inventory.Scipts;
using Scriptable_Objects.Items.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace CharacterManager
{
    // This is going to be where you are going to write code relevant to the player actions
    public class CharacterManager : MonoBehaviour
    {
        public InventoryObject inventory;

        public void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<Item>();
            if (item)
            {
                Debug.Log("Its touching");
                bool wasPickup = Inventory.Inventory.instance.add(item);

                if (wasPickup)
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}