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
                DontDestroyOnLoad(this.gameObject);
        }

       // public TMP_Text pseudDistance;
        double speed = 10.0; 
        public double totalDistance = 0.0;
        public float minAcceleration = 0.4f;
        public float maxAcceleration = 4;
        Vector3 lastPosition;

        void Start()
        {
            lastPosition = transform.position;
        }

        void Update () {
            Vector3 dir = Vector3.zero;
            
            dir.x = -Input.acceleration.y;
            dir.z = Input.acceleration.x;

            
            if (dir.sqrMagnitude > 1)
                dir.Normalize();

            // Make it move 10 meters per second instead of 10 meters per frame...
            dir *= Time.deltaTime;
            
            transform.Translate (dir * (float)speed);
         
            var distanceTravelled = Vector3.Distance(transform.position, lastPosition);
            Debug.LogWarning("Dist "+distanceTravelled);
            if (distanceTravelled > 0.4 && distanceTravelled > 4) // Change as needed
            {
                totalDistance += distanceTravelled;
            }
            else
            {
                totalDistance += 0;
            }
            
            lastPosition = transform.position;

            Debug.LogWarning("Total distance travelled: " + totalDistance);
            // pseudDistance.text = " " + totalDistance.ToString("F2");
        }

        public void loadData(GameData gameData)
        {
            this.totalDistance = gameData.totalDistance;
        }

        public void saveData(GameData gameData)
        {
            gameData.totalDistance = totalDistance;
        }

    }
}
