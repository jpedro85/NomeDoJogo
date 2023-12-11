
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    private AnalogicManager analogic;
    private Transform player;
    private PlayerMovement playerMov;

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


    public bool stabelizedX = false;
    private bool stabelizedY = false;
    private bool zooming = false;
    private bool zoomOutActive = false;
    private bool zoomInActive = false;
    private float zoomTime = 0;
    public float HintTime = 10 ;

    private bool coliding = false;

    public float timeToInvert = 2;
    public float timeStopDistanceManager = 2;
    private float time;

    public float margem = 25;
    private float playSpeed;
    public float zoomOutSpeed = 3;
    public float zoomOutSpeedPista = 9;
    public float zoomSpeed = 1;
    public float AtualzoomSpeed = 1;

    void Start()
    {
        analogic = GameObject.FindWithTag("Analogico").GetComponent<AnalogicManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerMov = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        playSpeed = player.GetComponent<PlayerMovement>().maxSpeed;

        maxDistanceX = minDistanceX + margem;
        maxDistanceY = minDistanceY + margem;
        distanceX = minDistanceX;
        distanceY = minDistanceY;

        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindWithTag("Player").GetComponent<Collider>());
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
            {
                toPistaView();
            }
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
    public GameObject turveEfect;
    private bool fadeIn = false, fadeOut = false;
    public bool isFadeOut
    {
        get { return fadeOut; }
    }

    public bool isFadeIn
    {
        get { return fadeIn; }
    }
    public float fadeInterval;
    private float fadeTime = 0;
    private float fade_nextime = 0;
    private int fade_atual = 1;

    private int fade_maxin = 0;
    private int fade_maxout = 0;

    public void setTurveEfect(bool set)
    {
        turveEfect.SetActive(set);
    }

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
   
            if (fade_atual == fade_maxout && fadeTime >= fade_nextime)
            {
                fade_atual = fade_maxout;
                fadeOut = false;
    
            }
            else if (fade_atual >= fade_maxout && fadeTime >= fade_nextime)
            {
               
                fades[fade_atual].SetActive(false);

                fade_atual--;
                if (fade_atual - 1 > 0)
                    fades[fade_atual].SetActive(true);

                fade_nextime = fadeTime + fadeInterval;

            }
            else
            {

                fadeTime += Time.deltaTime;
            }
        }
    }

    private void auxiliarFadeIn()
    {
        if (fadeIn)
        {

            if (fade_atual == fade_maxin && fadeTime >= fade_nextime)
            {
                fade_atual = fade_maxin;
                fadeIn = false;
            }
            else if (fade_atual < fade_maxin && fadeTime >= fade_nextime)
            {
                if (fade_atual - 1 > 0)
                    fades[fade_atual].SetActive(false);

                fade_atual++;
                fades[fade_atual].SetActive(true);

                fade_nextime = fadeTime + fadeInterval;
            }
            else
            {
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

        //if (playerMov.isColiding)
        //{
        //    AtualzoomSpeed = zoomSpeed/2;
        //    //sppeed when coliding ?
        //}
        if(!playerMov.isColiding)
        {
            if (!zoomOutActive && !zoomInActive)
            {

                if (analogic.direction == Vector2.zero && zooming)
                {
                    AtualzoomSpeed = zoomOutSpeed;
                }
                else
                {

                    AtualzoomSpeed = zoomSpeed;
                }

            }
            else if (zooming)
                AtualzoomSpeed = zoomOutSpeedPista;
            else
                AtualzoomSpeed = zoomSpeed;
        }

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 rigth = new Vector3(transform.right.x, 0, transform.right.z);
        Vector3 pointAux = transform.position + (-rigth.normalized * analogic.deltaX);
        Vector3 point = pointAux + (forward.normalized * analogic.deltaY);
        Vector3 velocity = (point - transform.position);

        if (playerMov.canMov && !coliding && !playerMov.isColiding  || ( playerMov.isDizziness && !playerMov.isColiding && playerMov.canMov) )
        {
            if(playerMov.isDizziness && analogic.direction == Vector2.zero)
                transform.position = transform.position + (velocity.normalized * Time.deltaTime * playSpeed * 0.25f);
            else
                transform.position = transform.position + (velocity.normalized * Time.deltaTime * playSpeed * analogic.speed);
        }
       
        transform.forward = new Vector3(player.position.x, offsetY, player.position.z) - transform.position;

        checkInvert();
        float distX = calDistance();
        float distY = transform.position.y - offsetY;

        if(!zoomOutActive && !zoomInActive)
            zoom();

       // if (!coliding)
        {
            updateDistanceY(distY, AtualzoomSpeed);
            updateDistanceX(distX, AtualzoomSpeed);
        }


        if (stabelizedX && stabelizedY)
        {
            zooming = false;
        }
    }

    private float calDistance()
    {
        Vector2 transformPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerTransformPosition2D = new Vector2(player.transform.position.x, player.transform.position.z);
        return Vector2.Distance(playerTransformPosition2D, transformPosition2D);
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Vector3 cameraForward = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;
            transform.position = transform.position + cameraForward * forceBack;            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            coliding = true;
            distanceX = calDistance();
            maxDistanceX = distanceX + margem;
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Wall")
        {
            distanceX = minDistanceX;
            maxDistanceX = distanceX + margem;
            coliding = false;
        }
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
                transform.position = transform.position + (forward.normalized * -speed * Time.deltaTime);
            }
            else if (dist > maxDistanceX)
            {
                transform.position = transform.position + (forward.normalized * speed * Time.deltaTime);
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