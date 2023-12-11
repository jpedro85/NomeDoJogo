using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 10;

    private AnalogicManager analogic;
    private Camera mainCamera;
    public Rigidbody rigidBody;
    private Animator animator;
    private CapsuleCollider playerColiderCapsule;
    private BoxCollider playerColiderBoxColider;
    public float forceBack = 5;
    private bool coliding = false;
    private float idleTime = 0;
    private Vector3 jumpingStart;
    public float jumpingSpeed = 250;
    public float jumpingUpSpeed = 250;
    public float jumpingUpSpeedWaitTime = 2;
    public float jumpDistance = 100;


    public float dizzinessX = (float)0.5;
    public float dizzinessY = (float)0.5;
    public bool isDizziness = false;
    private float activedizzinessX = 0;
    private float activedizzinessY = 0;
    private float counterSgnalChange = 0;
    private short mult = 1;

    public bool isColiding
    {
        get { return coliding; }
    }
    // Start is called before the first frame update
    void Start()
    {
        analogic = GameObject.FindWithTag("Analogico").GetComponent<AnalogicManager>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        animator = GameObject.Find("Armature").GetComponent<Animator>();
        playerColiderCapsule = GetComponent<CapsuleCollider>();
        playerColiderBoxColider = GetComponent<BoxCollider>();
        Physics.IgnoreCollision(mainCamera.GetComponent<Collider>(), playerColiderCapsule);
        Physics.IgnoreCollision(mainCamera.GetComponent<Collider>(), playerColiderBoxColider);


    }

    private float jumpTimer = 0;
    // Update is called once per frame
    void Update()
    {


        if (isDizziness)
        {
            if (counterSgnalChange >= Random.Range(2, 6))
            {
                activedizzinessX = 0;
                activedizzinessY = 0;
                counterSgnalChange = 0;
                mult = (mult == 1) ? (short)-1 : (short)1;
            }
            else
                counterSgnalChange += Time.deltaTime;

            if (counterSgnalChange == 0)
            {
                activedizzinessX += Random.Range(0, dizzinessX);
                activedizzinessY += Random.Range(0, dizzinessY);
                activedizzinessX *= mult;
            }
        }
        else
        {
            activedizzinessX = 0;
            activedizzinessY = 0;
        }

        if (analogic.direction != Vector2.zero )
        {
            animator.SetBool("IdleTime", false);
            animator.SetBool("isMoving",true);
            animator.SetBool("isRunning", analogic.isRunning);
            animator.SetBool("isWalking", !analogic.isRunning);
            idleTime = 0;
     
            Vector3 forward = new Vector3(mainCamera.transform.forward.x,0, mainCamera.transform.forward.z);
            Vector3 rigth = new Vector3(mainCamera.transform.right.x,0, mainCamera.transform.right.z);
            Vector3 pointAux = mainCamera.transform.position + (rigth.normalized * (analogic.deltaX + activedizzinessX));
            Vector3 point = pointAux + (forward.normalized * (analogic.deltaY + activedizzinessY) );
            transform.forward = (point - mainCamera.transform.position);
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isRunning", false);
            idleTime += Time.deltaTime;
            if(idleTime > 2)
                animator.SetBool("IdleTime",true);

            if (isDizziness)
            {
                Vector3 forward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
                Vector3 rigth = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z);
                Vector3 pointAux = mainCamera.transform.position + (rigth.normalized * (analogic.deltaX + activedizzinessX));
                Vector3 point = pointAux + (forward.normalized * (analogic.deltaY + activedizzinessY));
                transform.forward = (point - mainCamera.transform.position);
            }
            
        }

        
        if (!coliding)
        {
            if (isJumping)
            {

                float dist = Vector3.Distance(jumpingStart, transform.position);

                if (dist < jumpDistance)
                {


                    if (dist < (jumpDistance / 4) * 3)
                    {
                        playerColiderCapsule.center = new Vector3(0, playerColiderCapsule.center.y + jumpingUpSpeed * Time.deltaTime, 0);
                        float sp = maxSpeed * analogic.speed;
                        transform.position = transform.position + transform.forward * ((sp) + (jumpingSpeed - sp)) * Time.deltaTime;
                        transform.position = transform.position + Vector3.up * +jumpingUpSpeed * Time.deltaTime;

                    }
                    else
                    {
                        float sp = maxSpeed * analogic.speed;
                        transform.position = transform.position + transform.forward * ((sp) + (jumpingSpeed - sp)) * Time.deltaTime;

                        jumpTimer += Time.deltaTime;
                    }


                    Debug.Log("a" + dist + ":" + jumpTimer);

                    if (jumpTimer > 0 && Vector3.Distance(jumpingStart, transform.position) == 0)
                    {
                        Debug.Log("a" + dist);
                        animator.SetBool("isJumping", false);
                        playerColiderCapsule.center = new Vector3(0, 1, 0);
                        isJumping = false;
                        jumpTimer = 0;
                    }

                }
                else
                {

                    if (jumpTimer >= jumpingUpSpeedWaitTime)
                    {
                        playerColiderCapsule.center = new Vector3(0, 1, 0);
                        isJumping = false;
                        animator.SetBool("isJumping", false);
                        jumpTimer = 0;
                    }
                    else
                    {
                        jumpTimer += Time.deltaTime;
                    }
                  
                }

            }
            else
            {

                if (isDizziness && (analogic.direction == Vector2.zero) ) 
                {
                    animator.SetBool("isMoving", true);
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isRunning", false);
                    transform.position = transform.position + (transform.forward * Time.deltaTime * maxSpeed * 0.25f );
                }
                else
                {
                    transform.position = transform.position + (transform.forward * Time.deltaTime * maxSpeed * analogic.speed);
                }
            }

        }else if (isJumping)
        {
            animator.SetBool("isJumping", false);
        }

    }

    public void setCrouched(bool Crouching)
    {
        animator.SetBool("isCrouched", Crouching);

        if (Crouching)
        {
            playerColiderCapsule.height = 0.6f;
            playerColiderCapsule.center = new Vector3( 0, 0.5f, 0.25f);
            playerColiderCapsule.radius = 0.45f;
        }
        else
        {
            playerColiderCapsule.height = 2;
            playerColiderCapsule.center = new Vector3( 0, 1, 0);
            playerColiderCapsule.radius = 0.30f;
        }
    }

    public void setCrawling(bool Crawling, bool Crouching = false)
    {
        animator.SetBool("isCrawling", Crawling);

        if (Crawling)
        {

            playerColiderBoxColider.size = new Vector3(0.8f, 0.60f, 2);
            playerColiderBoxColider.center = new Vector3(0, 0.3f, 0);
            playerColiderCapsule.height = 0.05f;
            playerColiderCapsule.center = new Vector3(0, 0.3f, 0);
        }
        else
        {
            playerColiderBoxColider.size = new Vector3(0,0.2f, 0);
            playerColiderBoxColider.center = new Vector3(0, 0.2f, 0);

            if (Crouching)
            {
                playerColiderCapsule.height = 0.6f;
                playerColiderCapsule.center = new Vector3(0, 0.5f, 0.25f);
                playerColiderCapsule.radius = 0.45f;
            }
            else 
            {
                playerColiderCapsule.height = 2;
                playerColiderCapsule.center = new Vector3(0, 1, 0);
                playerColiderCapsule.radius = 0.30f;
            }
           
        }


    }
    public bool isJumping = false;
    public void setJumping()
    {
        animator.SetBool("isJumping", true);
        isJumping = true;
        jumpingStart = transform.position;
    }

    private bool canStandUp = true;
    private bool canCrouchUp = true;
    private bool canCrawlingUp = true;
    private bool inCrouchingZone = false;

    public bool getInCrouchingZone
    {
        get { return inCrouchingZone; }
    }
    public bool getcanStandUp
    {
        get { return canStandUp; }
    }

    public bool getCanCrouchUp
    {
        get { return canCrouchUp; }
    }

    public bool getCanCrawlUp
    {
        get { return canCrawlingUp; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obj")
        {
            coliding = true;
        }

        if (collision.gameObject.tag == "WallCrouching" && !animator.GetBool("isCrouched"))
        {

            coliding = true;
        }

        
    }
    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obj" )
        //{
        //    coliding = false;
        //}

        //if (collision.gameObject.tag == "WallCrouching")
        //{
        coliding = false;
        //}

    }


    private bool enterTrigger;
  
    public void OnTriggerEnter(Collider colider)
    {
        if (colider.gameObject.tag == "Crouchonly" && !enterTrigger)
        {

            if (animator.GetBool("isCrouched"))
            {
                canStandUp = false;
                canCrouchUp = false;
                canCrawlingUp = true;

                enterTrigger = true;

            }

            inCrouchingZone = true;


        }
    }

    public void OnTriggerStay(Collider colider)
    {
        if (colider.gameObject.tag == "Crouchonly" && !enterTrigger)
        {

            if (animator.GetBool("isCrouched") )
            {
                canStandUp = false;
                canCrouchUp = false;
                canCrawlingUp = true;

                enterTrigger = true;
            }

            inCrouchingZone = true;
        }
    }

    public void OnTriggerExit(Collider colider)
    {
        if (colider.gameObject.tag == "Crouchonly" && enterTrigger)
        {

            Debug.LogWarning("Coliding Crouchoing exit :");
            if (animator.GetBool("isCrouched"))
            {
                canStandUp = true;
                canCrouchUp = true;
                canCrawlingUp = true;

                enterTrigger = false;
            }

            Debug.LogWarning("zone");
            inCrouchingZone = false;

        }
    }

}
