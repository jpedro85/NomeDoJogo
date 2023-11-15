using System;
using Inventory;
using Scriptable_Objects.Inventory.Scipts;
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
            var item = other.GetComponent<GameItem>();
            if (item)
            {
                Debug.Log("Its touching");
                inventory.addItem(item.item, 1);
                Destroy(other.gameObject);
            }
        }
    }
}