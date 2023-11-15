using UnityEngine;

namespace Scriptable_Objects.Items.Scripts
{
    public enum ItemType
    {
        Food,
        Drink,
    }
    public abstract class ItemObject : ScriptableObject
    {
        public GameObject prefab;
        public ItemType type;
        [TextArea(15,20)]
        public string descpription;
    }
}