using System;
using UnityEngine;

namespace Scriptable_Objects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
    public class FoodObject : ItemObject
    {
        public int restoreHealthValue;

        public void Awake()
        {
            type = ItemType.Food;
        }
    }
}