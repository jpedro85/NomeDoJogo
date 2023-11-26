using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 10;

    private AnalogicManager analogic;
    private Camera mainCamera;
    public Rigidbody rigidBody;
    private Animator animator;
    public float forceBack = 5;
    private bool coliding = false;
    private float idleTime = 0;
    private Vector3 jumpingStart;
    public float jumpingSpeed = 250;
    public float jumpDistance = 100;

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
    }

    // Update is called once per frame
    void Update()
    {

        if (analogic.direction != Vector2.zero)
        {
            animator.SetBool("IdleTime", false);
            animator.SetBool("isMoving",true);
            animator.SetBool("isRunning", analogic.isRunning);
            animator.SetBool("isWalking", !analogic.isRunning);
            idleTime = 0;
     


            Vector3 forward = new Vector3(mainCamera.transform.forward.x,0, mainCamera.transform.forward.z);
            Vector3 rigth = new Vector3(mainCamera.transform.right.x,0, mainCamera.transform.right.z);
           // Vector2 cameraPosition = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);

            Vector3 pointAux = mainCamera.transform.position + (rigth.normalized * analogic.deltaX );
            Vector3 point = pointAux + (forward.normalized * analogic.deltaY );
            transform.forward = (point - mainCamera.transform.position);
        }
        else
        {
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            animator.SetBool("isMoving", false);
            animator.SetBool("isRunning", false);
            idleTime += Time.deltaTime;
            if(idleTime > 2)
                animator.SetBool("IdleTime",true);
        }

        
        if (!coliding)
        {
            if (isJumping)
            {
                if (Vector3.Distance(jumpingStart, transform.position)  < jumpDistance)
                {
                    float sp = maxSpeed * analogic.speed;
                    transform.position = transform.position + transform.forward * ((sp) + (jumpingSpeed - sp)) * Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                    animator.SetBool("isJumping", false);
                }

            }else
                transform.position = transform.position + (transform.forward * Time.deltaTime * maxSpeed * analogic.speed);

        }

    }

    public void setCrouched(bool Crouching)
    {
        animator.SetBool("isCrouched", Crouching);
    }

    public void setCrawling(bool Crawling)
    {
        animator.SetBool("isCrawling", Crawling);
    }
    public bool isJumping = false;
    public void setJumping(bool jumping)
    {
        animator.SetBool("isJumping", jumping);
        isJumping = true;
        jumpingStart = transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            coliding = true;
        }

    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //foreach (ContactPoint contact in collision.contacts)
            //{
                //Vector3 awayFromCollision = contact.normal * forceBack;
                Vector3 awayFromCollision = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;
                transform.position = transform.position + (-awayFromCollision * forceBack);
              //  Vector3 cameraforward = new Vector3(mainCamera.transform.forward.x, mainCamera.transform.forward.y, mainCamera.transform.forward.z);
              // mainCamera.transform.position = -cameraforward.normalized * forceBack;

            //    break;
            //}
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            coliding = false;
        }
    }
}
