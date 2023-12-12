using UnityEngine;
using Scripts.Item;
using System.Collections.Generic;
using GPSLocation.Scripts;

namespace CharacterManagername
{
    // This is going to be where you are going to write code relevant to the player actions
    public class CharacterManager : MonoBehaviour
    {

        private PlayerMovement playerMovement;
        private PlayUI playUI;

        private CameraMovement cameraMovement;
        private Animator animator;
        private GameObject player;
        private GameObject camera;
        private Inventory.Inventory inventory;
        private questionsUI questionUi;

        public float convertionFactor = 1;
        public float convertionAddEnergy = 5;
        public float convertionTimeInterval = 25;
        public float convertionTimeCounter = 25;

        private GpsLocation gpsLocation;
        private CharacterManager characterManager;


        public float dizzinessLevel = 30;
        public float faintStart = 25;
        private float faintCounter = 0;
        private float turveVision = 5;
        private bool hasEnteredHitbox = false;

        public float standUpMov = 0.11111111f;
        public float standUpRunningMov = 0.22222222f;
        public float crouchingMov = 0.17f;
        public float crawlingMov = 0.05f;
        public float jumping = 0.25f;

        public bool canMov
        {
            get { return playUI.AtualEneergy > 0; }
        }


        public float updateIntervale;

        public Vector3 []responPoints;
        private List<GameObject> gameObjectGameItemList;

        public void Start()
        {
            inventory =  Inventory.Inventory.instance;
            gameObjectGameItemList = new List<GameObject>();
        }


        public void Awake()
        {
            animator = GameObject.Find("Armature").GetComponent<Animator>();
            camera = GameObject.FindWithTag("MainCamera");
            cameraMovement = camera.GetComponent<CameraMovement>();
            playUI = GameObject.Find("UI_Buttons").GetComponent<PlayUI>();
            player = GameObject.FindWithTag("Player");
            playerMovement = player.GetComponent<PlayerMovement>();
            questionUi = GameObject.FindWithTag("questionManager").GetComponent<questionsUI>();
            questionUi.answer += afterQuestionResult;
            questionUi.backEvent += afterBack;

            gpsLocation = GpsLocation.instance;
        }
        private void convertion()
        {

            if (playUI.AtualEneergy < 100 && gpsLocation.totalDistance > (convertionAddEnergy * convertionFactor) && convertionTimeCounter >= convertionTimeInterval)
            {
                playUI.addDeltaEnergy(convertionAddEnergy);
                gpsLocation.totalDistance -= convertionAddEnergy * convertionFactor;
                convertionTimeCounter = 0;
            }
            else
                convertionTimeCounter += Time.deltaTime;
        }

        public void Update()
        {

            //if (animator == null)
            //    animator = GameObject.Find("Armature").GetComponent<Animator>();


            turveVisionEfect();
            faintEfect();
            dizzinnesEfect();
            EnergyChange();
            convertion();
        }
        private void turveVisionEfect()
        {
            cameraMovement.setTurveEfect(playUI.AtualEneergy < turveVision);
        }

        private bool subJump = false;
        private void EnergyChange()
        {
            bool moving = animator.GetBool("isMoving");
            bool walking = animator.GetBool("isWalking");
            bool running = animator.GetBool("isRunning");
            bool jumpingb = animator.GetBool("isJumping");
            bool crouching = animator.GetBool("isCrouched");
            bool crawling = animator.GetBool("isCrawling");

            if (moving)
            {

                if (walking && !crouching)
                {
                    playUI.addDeltaEnergy(-standUpMov*Time.deltaTime);

                }
                else if (running )
                {
                    playUI.addDeltaEnergy(-standUpRunningMov * Time.deltaTime);
                }
                
                if (crouching && !crawling )
                {
                    playUI.addDeltaEnergy(-crouchingMov * Time.deltaTime);
                }
                else if(crawling)
                {
                    playUI.addDeltaEnergy(-crawlingMov * Time.deltaTime);
                }

            }

            if (jumpingb)
            {
                playUI.addDeltaEnergy(-jumping);   
            }

        }

        private void dizzinnesEfect()
        {
            playerMovement.isDizziness = (playUI.AtualEneergy < dizzinessLevel && !changedPosition);
        }

        private bool donein = false, changedPosition = false;
        private bool faintStarted = false;
        private bool faintStarted_first = true;
        public float reviveTime = 5;
        private float reviveTimeCounter = 0;
        private void faintEfect()
        {
            if (!changedPosition)
            {

                if(playUI.AtualHealth < faintStart && playUI.AtualHealth != 0 && !changedPosition)
                {
                    faintStarted = true;

                    if(!donein && !cameraMovement.isFadeIn  && faintCounter >= playUI.AtualHealth || faintStarted_first )
                    {

                        cameraMovement.startfadeIn( 
                            (int)Mathf.Clamp(cameraMovement.fade_max - playUI.AtualHealth + 2, cameraMovement.fadeo_min + 2, cameraMovement.fade_max) ,
                            cameraMovement.fade_s * (playUI.AtualHealth / faintStart ) + 0.01f
                        );

                        donein = true;
                        faintStarted_first = false;

                    }
                    else if(donein && !cameraMovement.isFadeIn )
                    {

                        cameraMovement.startfadeOut(
                            (int)Mathf.Clamp(cameraMovement.fade_max - playUI.AtualHealth - 5, cameraMovement.fadeo_min, cameraMovement.fade_max), 
                            cameraMovement.fadeo_s * (playUI.AtualHealth / faintStart ) + 0.01f
                        );

                        donein = false;
                        faintCounter = 0;
                        faintCounter += Time.deltaTime;
                    }
                    else
                        faintCounter += Time.deltaTime;

                }
                else if(playUI.AtualHealth == 0 && !changedPosition)
                {
                    faintStarted = true;
                    cameraMovement.startfadeIn(cameraMovement.fade_max, cameraMovement.fadeo_s);
                    changedPosition = true;
                    reviveTimeCounter = 0;
                }
                else if (faintStarted && !changedPosition)
                {
                    cameraMovement.startfadeOut(0, cameraMovement.fadeo_s);
                    faintStarted = false;
                    faintStarted_first = true;

                }
            }
            else
            {
                if(reviveTimeCounter >= 2) 
                {
                    changePosition();
                }


                if (reviveTimeCounter >= reviveTime)
                {
                    cameraMovement.startfadeOut(0, cameraMovement.fadeo_s);
                    faintStarted = false;
                    faintStarted_first = true;
                    changedPosition = false;
                }

                reviveTimeCounter += Time.deltaTime;
            }

        }


        private void changePosition()
        {
            playUI.resetToAfterDeadHealth();

            player.transform.position = responPoints[Random.Range(0, responPoints.Length-1)] ;
            player.transform.forward = Vector3.back;

            camera.transform.position = (player.transform.position - Vector3.back * cameraMovement.minDistanceX) + (Vector3.up * (cameraMovement.minDistanceY+cameraMovement.offsetY) );
            camera.transform.forward = (player.transform.position + Vector3.up * cameraMovement.offsetY) - camera.transform.position;

        }

        public void OnTriggerEnter(Collider other)
        {
            if(!questionUi.isOpen)
            {

                if (other.GetComponent<GameItem>() != null)
                {
                    if ( !gameObjectGameItemList.Contains(other.gameObject) )
                    {
                        gameObjectGameItemList.Add(other.gameObject);
                        questionUi.open();
                    }

                }

            }
          //  if (!item || hasEnteredHitbox) return;
            
           // hasEnteredHitbox = true;
           


            //bool wasPickup = inventory.addToInventory(item.item);

            //if (wasPickup)
            //{
            //    Destroy(other.gameObject);
            // //   hasEnteredHitbox = false;
            //}
        }


        public virtual void afterQuestionResult(bool result)
        {
            if (gameObjectGameItemList.Count > 0)
            {
                if (inventory.addToInventory(gameObjectGameItemList[0].GetComponent<GameItem>().item))
                {
                    Destroy(gameObjectGameItemList[0].gameObject);
                    gameObjectGameItemList.RemoveAt(0);
                    //   hasEnteredHitbox = false;

                    if (result)
                    {
                        playUI.addHint();
                    }
                }
            }
            
        }

        public virtual void afterBack()
        {
            if (gameObjectGameItemList.Count > 0)
            {
                gameObjectGameItemList.RemoveAt(0);
            }
        }
    }
}