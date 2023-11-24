// if you want to save data from a scriptable object its better to have a mirror classes so we can seriazlize the data needed 
// for the save 

namespace DataPersistence.Data
{
    [System.Serializable]
    public class ExampleOfScriptableObject
    {
        private int vitality;
        private int strength;
        private int endurance;
        private int dextery;
        private int intellect;

        public ExampleOfScriptableObject()
        {
            this.intellect = 1;
            this.vitality = 1;
            this.dextery = 1;
            this.strength = 1;
            this.endurance = 1;
        }
    }
}