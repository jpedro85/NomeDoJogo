using System;
using UnityEngine;

namespace Scriptable_Objects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
    public class FoodObject : Item
    {
        public int restoreHealthValue;

        public override void use()
        {
            Debug.Log("Regenerated");
        }
        
    }
}