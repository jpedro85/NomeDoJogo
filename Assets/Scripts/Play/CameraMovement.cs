using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private AnalogicManager analogic;
    private Transform player;
    private float maxSpeed;
    private float speed;
    public float minSpeed = 50;
    public float percaVelocidade = 10;
    public float minDistance = 500;
    public float maxDistance = 1000;
    public float tolerance = 20;
    public float teste = 1;
    private bool rotated = false;
    public BoxCollider boxColider;

    void Start()
    {
        analogic = GameObject.FindWithTag("Analogico").GetComponent<AnalogicManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        maxSpeed = player.GetComponent<PlayerMovement>().maxSpeed;
        speed = maxSpeed;
    }

    void LateUpdate()
    {

        float angle  = Vector3.Angle(transform.forward,-Vector3.up);
         
        if (analogic.deltaY < -0.8 && analogic.deltaX < 0.1 && analogic.deltaX > -0.1 && !rotated)
        {
            transform.RotateAround(player.position, player.transform.up, 180 );
            player.forward = -player.forward;
            rotated = true;
        }
        else if (analogic.deltaY >= 0)
            rotated = false;


        float dist = Vector3.Distance(transform.position,player.transform.position);
        Debug.Log("a:" + speed + " d:" + dist + " A: " + angle);

        //if (analogic.deltaY < 0)
        //{
        //    speed = analogic.deltaX * maxSpeed;
        //}

        //if (analogic.deltaY > 0 && angle > 25)
        //{
        //    speed = maxSpeed;
        //}

        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        if ( dist < maxDistance && dist > minDistance)
        {

        }
        else
        {
            if (dist < minDistance )
            {
                transform.position = transform.position + (transform.forward * -speed * Time.deltaTime);
            }
            else if (dist > maxDistance)
            {
                transform.position = transform.position + (transform.forward * speed * Time.deltaTime);
            }
        }
        



        //transform.RotateAround(player.transform.position, Vector3.up, 2*speed * Mathf.Pow(analogic.deltaX, 3) * Time.deltaTime );

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 pointAux = transform.position + (-transform.right.normalized * analogic.deltaX);
        Vector3 point = pointAux + (forward.normalized * analogic.deltaY);
        Vector3 velocity = (point - transform.position);

        transform.position = transform.position + (velocity * Time.deltaTime * speed * analogic.speed * teste);

        transform.forward = player.position - transform.position;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {

        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {

        }
    }
}
