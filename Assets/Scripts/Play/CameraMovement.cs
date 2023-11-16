using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private AnalogicManager analogic;
    private Transform player;
    public float speed = 200;
    public float distance = 100;
    public float tolerance = 20;

    void Start()
    {
        analogic = GameObject.FindWithTag("Analogico").GetComponent<AnalogicManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void LateUpdate()
    {

        Vector3 a = new Vector3(transform.position.x,0 ,transform.position.z);
        Vector3 b = new Vector3(player.transform.position.x, 0,player.transform.position.z);
        Vector3 dist = a - b;

        float aprocimationSpeed = speed;

        if(analogic.direction != Vector2.zero)
        {
            aprocimationSpeed = analogic.speed * speed;
        }


        //if( distance + tolerance > dist.magnitude && dist.magnitude > distance - tolerance)
        //{

        //} 
        //else
        //{

        //    if (dist.magnitude > distance)
        //    {
        //        transform.position = transform.position + (dist.normalized * -aprocimationSpeed * Time.deltaTime);
        //    }
        //    else if (dist.magnitude < distance)
        //    {
        //        transform.position = transform.position + (dist.normalized * aprocimationSpeed * Time.deltaTime);
        //    }
        //}



        //transform.RotateAround(player.transform.position, Vector3.up, 2*speed * Mathf.Pow(analogic.deltaX, 3) * Time.deltaTime );

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 pointAux = transform.position + (-transform.right.normalized * analogic.deltaX);
        Vector3 point = pointAux + (forward.normalized * analogic.deltaY);
        Vector3 velocity = (point - transform.position);

        transform.position = transform.position + (velocity * Time.deltaTime * speed * analogic.speed);

        transform.forward = player.position - transform.position;

    }
}
