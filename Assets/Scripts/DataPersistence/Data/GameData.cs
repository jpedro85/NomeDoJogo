using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;
using UnityEditor;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class GameData
    {
        public int stepsTaken;
        public int deathCount;
        public double playerCoins;
        public double currentPlayerPositionOnLvl;
        public InventoryData playerInventory;
        
        // if you want to add scriptable objects to the game data thats going to be save use the mirrored class
        // [SerializeField] private ExampleOfScriptableObject ExampleOfScriptableObject;
        // And add to the constructor as well
        // On the object you are using the scriptable object implement the IDataPersistence as you would normally and 
        // import or export the values of the class that was serialized or deserialized

        // the values defined in this constructor will be the default values
        // the game starts with when there is no data to load
        public GameData()
        {
            this.stepsTaken = 0;
            this.deathCount = 0;
            this.playerCoins = 0;
            this.currentPlayerPositionOnLvl = 0;
            this.playerInventory = new InventoryData(new List<ItemData>());
            // this.playerInventory = new InventoryData(new List<Item>());
            // this.playerInventory = new InventoryData(new Dictionary<string, Item>());
        }

        public GameData(int stepsTaken, int deathCount, double playerCoins, double currentPlayerPositionOnLvl, InventoryData playerInventory)
        {
            this.stepsTaken = stepsTaken;
            this.deathCount = deathCount;
            this.playerCoins = playerCoins;
            this.currentPlayerPositionOnLvl = currentPlayerPositionOnLvl;
            this.playerInventory = new InventoryData(playerInventory.items);
        }
        
    }
}