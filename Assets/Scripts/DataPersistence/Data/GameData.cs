using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class GameData
    {
        public int stepsTaken;
        public int deathCount;
        public double playerCoins;
        public double currentPlayerPositionOnLvl;


        // the values defined in this constructor will be the default values
        // the game starts with when there is no data to load
        public GameData()
        {
            this.stepsTaken = 0;
            this.deathCount = 0;
            this.playerCoins = 0;
            this.currentPlayerPositionOnLvl = 0;
        }

        public GameData(int stepsTaken, int deathCount, double playerCoins, double currentPlayerPositionOnLvl)
        {
            this.stepsTaken = stepsTaken;
            this.deathCount = deathCount;
            this.playerCoins = playerCoins;
            this.currentPlayerPositionOnLvl = currentPlayerPositionOnLvl;
        }
        
    }
}