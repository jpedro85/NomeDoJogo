using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private AnalogicManager analogic;
    private Transform player;
    private PlayerMovement playerMov;
    private float maxSpeed;
    private float speed;
    private float atualSpeed;

    public float startMovingDistance = 1400;
    public float minDistanceX = 500;
    public float minDistanceY = 500;
    private float maxDistanceX;
    private float distanceX;
    private float maxDistanceY;
    private float distanceY;

    public float ZoomOutOffsetX = 100;
    public float ZoomOutOffsetY = 5000;

    public float offsetY;
    private bool rotated = false;
    private Rigidbody rigidBody;
    public float forceBack = 100;


    private bool stabelizedX = false;
    private bool stabelizedY = false;
    public bool zooming = false;
    public bool zoomOutActive = false;
    public float zoomOutSpeed = 500;
    private bool coliding = false;

    public float timeToInvert = 2;
    private float time;


    public float margem = 25;

    void Start()
    {
        analogic = GameObject.FindWithTag("Analogico").GetComponent<AnalogicManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerMov = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        maxSpeed = player.GetComponent<PlayerMovement>().maxSpeed;
        atualSpeed = maxSpeed;
        speed = maxSpeed;

        maxDistanceX = minDistanceX + margem;
        maxDistanceY = minDistanceY + margem;
        distanceX = minDistanceX;
        distanceY = minDistanceY;
    }

    public int teste = 0;

    void Update()
    {

        if (zoomOutActive && !zooming && teste == 0)
        {
            toPistaView();
            teste++;
        }
        else if (!zoomOutActive && teste > 0)
        {
            toNormalView();
            teste = 0;
        }
     
        if (playerMov.isColiding)
            atualSpeed = 10;
        else
        {
            if (analogic.direction == Vector2.zero && zooming)
                atualSpeed = zoomOutSpeed;
            else
                atualSpeed = maxSpeed;

        }

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 rigth = new Vector3(transform.right.x, 0, transform.right.z);
        Vector3 pointAux = transform.position + (-rigth.normalized * analogic.deltaX);
        Vector3 point = pointAux + (forward.normalized * analogic.deltaY);
        Vector3 velocity = (point - transform.position);

        if (!coliding)
        {
            transform.position = transform.position + (velocity * Time.deltaTime * atualSpeed * analogic.speed);
        }

        transform.forward = new Vector3(player.position.x, offsetY, player.position.z) - transform.position;

        checkInvert();
        Vector2 transformPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerTransformPosition2D = new Vector2(player.transform.position.x, player.transform.position.z);
        float distX = Vector2.Distance(playerTransformPosition2D, transformPosition2D);
        float distY = transform.position.y - offsetY;

        zoom();

        if (!coliding)
            updateDistanceX(distX, atualSpeed);

        updateDistanceY(distY, atualSpeed);

        if (stabelizedX && stabelizedY)
        {
            zooming = false;
        }
    }

    private void checkInvert()
    {
        if (analogic.deltaX > -0.25 && analogic.deltaX < 0.25 && analogic.deltaY < -0.75 && !rotated)
        {
            if(time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                transform.RotateAround(player.position, player.transform.up, 180 * Time.deltaTime);
                player.forward = transform.forward;
                rotated = true;
            }
        }
        else
        {
            time = timeToInvert;
            rotated = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Cameracolision");
            Vector3 cameraForward = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;
            transform.position = transform.position + cameraForward * forceBack;

            distanceX -= forceBack;
            maxDistanceX = distanceX + margem;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Cameracolision1");
        coliding = true;
    }

    private void OnTriggerExit(Collider collision)
    {

        Debug.Log("Cameracolision2");
        if (collision.gameObject.tag == "Wall")
        {
            distanceX = minDistanceX;
            maxDistanceX = distanceX + margem;
        }

        coliding = false;
    }


    public void toPistaView()
    {

        distanceX += ZoomOutOffsetX;
        distanceY += ZoomOutOffsetY;
        maxDistanceX = distanceX + margem;
        maxDistanceY = distanceY + margem;
        zooming = true;
    }

    public void toNormalView()
    {
        distanceX = minDistanceX;
        distanceY = minDistanceY;
        maxDistanceX = distanceX + margem;
        maxDistanceY = distanceY + margem;
        zooming = true;
    }

    public void zoom()
    {
        if (analogic.direction == Vector2.zero && !zooming)
        {
            distanceX = startMovingDistance;
            maxDistanceX = distanceX + margem;
            zooming = true;
        }
        else if(!zooming)
        {
            zooming = false;
            distanceX = minDistanceX;
            maxDistanceX = distanceX + margem;
        }
    }

    private void updateDistanceX(float dist, float speed)
    {
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        if (dist < maxDistanceX && dist > distanceX)
        {
            stabelizedX = true;
        }
        else
        {
            if (dist < distanceX)
            {
                transform.position = transform.position + (forward * -speed * Time.deltaTime);
            }
            else if (dist > maxDistanceX)
            {
                transform.position = transform.position + (forward  * speed * Time.deltaTime);
            }

            stabelizedX = false;
        }
    }

    private void updateDistanceY(float dist, float speed)
    {
        if (dist < maxDistanceY && dist > distanceY)
        {
            stabelizedY = true;
        }
        else
        {
            if (dist < distanceY)
            {
                transform.position = transform.position + Vector3.up * (speed * Time.deltaTime);
            }
            else if (dist > maxDistanceY)
            {
                transform.position = transform.position + Vector3.up * (-speed * Time.deltaTime);
            }

            stabelizedY = false;
        }
    }

}