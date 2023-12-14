using TMPro;
using UnityEngine;
using DataPersistence;
using DataPersistence.Data;

namespace GPSLocation.Scripts
{
    public class GpsLocation : MonoBehaviour ,IDataPersistence
    {

        public static GpsLocation instance { get; private set; }

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
                Destroy(this.gameObject);
        }

       // public TMP_Text pseudDistance;
        double speed = 10.0; 
        public double totalDistance = 0.0;
        public float minAcceleration = 0.4f;
        public float maxAcceleration = 5;
        public float IntervaloDeRecolha = 1;
        private float IntervaloDeRecolhaCounter = 0;
        private double TimePlayed = 0;
        Vector3 lastPosition;

        void Start()
        {
            lastPosition = transform.position;
        }

        void Update () {

            TimePlayed += Time.deltaTime;

            Vector3 dir = Vector3.zero;
            
            dir.x = -Input.acceleration.y;
            dir.z = Input.acceleration.x;

            Debug.LogWarning("dirX:" + dir.x + "dirY:" + dir.y);
            
            if (dir.sqrMagnitude > 1)
                dir.Normalize();

            // Make it move 10 meters per second instead of 10 meters per frame...
            dir *= Time.deltaTime;
            transform.Translate (dir * (float)speed);
         
            var distanceTravelled = Vector3.Distance(transform.position, lastPosition);
            //var distanceTravelledTimed = distanceTravelled * Time.deltaTime;

            Debug.LogWarning("Dist "+ distanceTravelled);
            if (distanceTravelled > minAcceleration && distanceTravelled < maxAcceleration && IntervaloDeRecolhaCounter >= IntervaloDeRecolha) // Change as needed
            {
                Debug.LogWarning("DistAdded: " + distanceTravelled);
                totalDistance += distanceTravelled ;
                IntervaloDeRecolhaCounter = 0;
            }
            else
            {
                totalDistance += 0;
            }
            
            lastPosition = transform.position;
            IntervaloDeRecolhaCounter += Time.deltaTime;

            //Debug.LogWarning("Total distance travelled: " + totalDistance);
            // pseudDistance.text = " " + totalDistance.ToString("F2");
        }

        public void loadData(GameData gameData)
        {
            this.totalDistance = gameData.totalDistance;
            this.TimePlayed = gameData.TimePlayed;
        }

        public void saveData(GameData gameData)
        {
            gameData.totalDistance = totalDistance;
            gameData.TimePlayed = TimePlayed;
        }

    }
}
