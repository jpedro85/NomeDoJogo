using System;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.UI;

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
    private bool zooming = false;
    private bool zoomOutActive = false;
    private bool zoomInActive = false;
    private float zoomTime = 0;
    public float HintTime = 10 ;
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
    private Button pistabutton;
    private Sprite spriteOff;

    public void pista(Button button,Sprite off)
    {
        pistabutton = button;
        spriteOff = off;
        if (!zoomOutActive)
        {
            zoomOutActive = true;
        }
        else
        {
            button.image.sprite = off ;
        }

    }

    public bool isPista
    {
        get { return zoomOutActive;}
    }

    private void auxiliarpista()
    {
        if (zoomOutActive)
        {
            if (!zooming && zoomTime == 0)
                toPistaView();
            else
                zoomTime += Time.deltaTime;
        }

        if (zoomTime >= HintTime && !zooming && !zoomInActive)
        {
            zoomInActive = true;
            toNormalView();
        }

        if (zoomInActive && !zooming)
        {
            zoomOutActive = false;
            zoomInActive = false;
            pistabutton.image.sprite = spriteOff;
            zoomTime = 0;
        }
    }

    public GameObject[] fades;
    private bool fadeIn = false, fadeOut = false;
    public float fadeInterval;
    private float fadeTime = 0;
    private float fade_nextime = 0;
    private int fade_atual = 1;

    private int fade_maxin = 0;
    private int fade_maxout = 0;

    public void startfadeIn(int maxin, float speed)
    {
        fadeIn = true;
        fadeOut = false;
        fade_maxin = maxin;

        fadeInterval = speed;
        fadeTime = 0;
        fade_nextime = fadeTime + fadeInterval;

    }

    public void startfadeOut(int maxout, float speed)
    {
        fadeOut = true; 
        fadeIn = false;
        fade_maxout = maxout;

        fadeInterval = speed;
        
        fadeTime = 0;
        fade_nextime = fadeTime + fadeInterval;
    }

    private void auxiliarFadeOut()
    {
        if (fadeOut)
        {
            Debug.Log("aa_" + fade_atual);
            if (fade_atual == fade_maxout && fadeTime >= fade_nextime)
            {
                fade_atual = fade_maxout;
                fadeOut = false;
                Debug.Log("ddd");
            }
            else if (fade_atual >= fade_maxout && fadeTime >= fade_nextime)
            {
               
                fades[fade_atual].SetActive(false);

                fade_atual--;
                if (fade_atual - 1 > 0)
                    fades[fade_atual].SetActive(true);

                fade_nextime = fadeTime + fadeInterval;
                Debug.Log("ccc");
            }
            else
            {

                fadeTime += Time.deltaTime;
                Debug.Log("bb");
            }
        }
    }

    private void auxiliarFadeIn()
    {
        if (fadeIn)
        {
            Debug.Log("aa1_"+ fade_atual);
            if (fade_atual == fade_maxin && fadeTime >= fade_nextime)
            {
                fade_atual = fade_maxin;
                fadeIn = false;
                Debug.Log("ddd2");
            }
            else if (fade_atual < fade_maxin && fadeTime >= fade_nextime)
            {
                if (fade_atual - 1 > 0)
                    fades[fade_atual].SetActive(false);

                fade_atual++;
                fades[fade_atual].SetActive(true);

                fade_nextime = fadeTime + fadeInterval;
                Debug.Log("ccc2");
            }
            else
            {
                Debug.Log("bbb2");
                fadeTime += Time.deltaTime;
            }
        }
    }

    public float fade_s = (float)0.5;
    public bool fade_sin = false;
    public int fade_max = 10;

    public float fadeo_s = (float)0.5;
    public bool fadeo_sin = false;
    public int fadeo_min = 10;

    void Update()
    {

        auxiliarpista();

        //cameraEfects
        if (fade_sin)
        {
            startfadeIn(fade_max, fade_s);
            fade_sin = false ;
        }

        if (fadeo_sin)
        {
            startfadeOut(fadeo_min, fadeo_s);
            fadeo_sin = false;
        }

        auxiliarFadeIn();
        auxiliarFadeOut();

        //if (zoomOutActive && !zooming && teste == 0)
        //{
        //    toPistaView();
        //    teste++;
        //}
        //else if (!zoomOutActive && teste > 0)
        //{
        //    toNormalView();
        //    teste = 0;
        //}

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
            Vector3 cameraForward = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;
            transform.position = transform.position + cameraForward * forceBack;

            distanceX -= forceBack;
            maxDistanceX = distanceX + margem;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        coliding = true;
    }

    private void OnTriggerExit(Collider collision)
    {

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