using System;
using DataPersistence;
using DataPersistence.Data;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class DeathCountText : MonoBehaviour, IDataPersistence
    {
        private int deathCount = 0;

        private TMP_Text deathCountText;

        private void Awake()
        {
            deathCountText = this.GetComponent<TextMeshProUGUI>();
        }

        public void loadData(GameData gameData)
        {
            this.deathCount = gameData.deathCount;
        }

        public void saveData(ref GameData gameData)
        {
            gameData.deathCount = this.deathCount;
        }

        private void Start()
        {
            this.deathCount = 10000;
        }

        private void OnDestroy()
        {
            this.deathCount = -1000;
        }

        private void Update()
        {
            deathCountText.text = "" + this.deathCount;
        }
    }
}