using System.Collections.Generic;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class GameData
    {
        public int stepsTaken;
        public int deathCount;
        public double playerCoins;

        #region PlayerHealthEnergy

        public float playerHealth;
        public float playerHealthToRegen;

        public float playerEnergy;
        public float playerEnergyToRegen;

        #endregion

        #region PlayerCameraPosition

        public float[] currentPlayerPositionOnLvl;
        public float[] currentCameraPositionOnLvl;

        #endregion

        public InventoryData playerInventory;

        public bool isMuted;
        
        // if you want to add scriptable objects to the game data thats going to be save use the mirrored class
        // [SerializeField] private ExampleOfScriptableObject ExampleOfScriptableObject;
        // And add to the constructor as well
        // On the object you are using the scriptable object implement the IDataPersistence as you would normally and 
        // import or export the values of the class that was serialized or deserialized

        // the values defined in this constructor will be the default values
        // the game starts with it when there is no data to load
        public GameData()
        {
            this.stepsTaken = 0;
            this.deathCount = 0;
            this.playerCoins = 0;

            this.playerHealth = 100;
            this.playerHealthToRegen = 100;

            this.playerEnergy = 100;
            this.playerEnergyToRegen = 100;

            this.currentPlayerPositionOnLvl = new[] { 3f, 0f, -2083f };
            this.currentCameraPositionOnLvl = new[] { 3.39f, 6f, -2079f };

            this.playerInventory = new InventoryData(new List<ItemData>());
            this.isMuted=true;
        }

        public GameData(int stepsTaken, int deathCount, double playerCoins, float playerHealth,
            float playerHealthToRegen, float playerEnergy, float playerEnergyToRegen,
            float[] currentPlayerPositionOnLvl, float[] currentCameraPositionOnLvl, InventoryData playerInventory)
        {
            this.stepsTaken = stepsTaken;
            this.deathCount = deathCount;
            this.playerCoins = playerCoins;

            this.playerHealth = playerHealth;
            this.playerHealthToRegen = playerHealthToRegen;

            this.playerEnergy = playerEnergy;
            this.playerEnergyToRegen = playerEnergyToRegen;

            this.currentPlayerPositionOnLvl = currentPlayerPositionOnLvl;
            this.currentCameraPositionOnLvl = currentCameraPositionOnLvl;

            this.playerInventory = new InventoryData(playerInventory.items);
            //TODO ADD TO PARAMETER
            this.isMuted= true;
        }
    }
}