using DataPersistence;
using DataPersistence.Data;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour, IDataPersistence
{
    private AnalogicManager analogic;
    private Transform player;
    private PlayerMovement playerMov;
    private RectTransform UI_Buttons_RectTransform;
    
    void Awake()
    {
        UI_Buttons_RectTransform = GameObject.Find("UI_Buttons").GetComponent<RectTransform>();
        fadesAndTurveEfectResize();
    }

    void Start()
    {
        analogic = GameObject.FindWithTag("Analogico").GetComponent<AnalogicManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerMov = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindWithTag("Player").GetComponent<Collider>());

        AtualAngle = FollowNot_InPistaAngle;
        AtualDistance = FollowNoMovementDistance;
    }

    void Update()
    {
        auxiliarFadeIn();
        auxiliarFadeOut();

        //  AfterColision();
        if(!colided)
            zoom();

        pista();


        followTransform();
     
    }

    //New Vars
    // FollowWhileMovementDistance < FollowNoMovementDistance
    public float FollowWhileMovementDistance;
    public float FollowWhileMovementDistance_Speed;

    public float FollowNoMovementDistance;
    public float FollowNoMovementDistance_Speed;

    public float FollowInPistaDistance;
    public float FollowInPistaDistance_Speed;

    public float FollowNotInPistaDistance;
    public float FollowNotInPistaDistance_Speed;

    public float LookAtOffsetY;
    public float rotateSpeed;

    private float AtualDistance;
    private void followTransform()
    {

        Vector3 playerLookPosition = new Vector3(player.position.x, LookAtOffsetY, player.position.z);
        //Loock At with offSYet
        transform.forward = (playerLookPosition  - new Vector3(transform.position.x, LookAtOffsetY, transform.position.z) ).normalized;

        //newPoint
        transform.position = (playerLookPosition - transform.forward * AtualDistance) ;

        //turn Around
        transform.RotateAround(player.position, player.up , analogic.deltaX * rotateSpeed * Time.deltaTime);

        setAngle(playerLookPosition);

    }
    
    private void zoom()
    {
        if (!isPista)
        {
            if (analogic.direction == Vector2.zero || playerMov.isDizziness)
            {
                zoomOut(FollowNoMovementDistance, FollowNoMovementDistance_Speed);
            }
            else
            {
                zoomIn(FollowWhileMovementDistance, FollowWhileMovementDistance_Speed);
            }
        }
    }

    private bool zoomOut(float maxDist,float speed)
    {
        float sp = speed * Time.deltaTime;
        if (AtualDistance + sp < maxDist)
        {
            AtualDistance += sp;
            return false;
        }
        else //if(AtualDistance + sp >= maxDist)
        {
            AtualDistance = maxDist;
            return true;
        }

    }

    private bool zoomIn(float minDist, float speed)
    {
        float sp = speed * Time.deltaTime;
        if (AtualDistance - sp > minDist)
        {
            AtualDistance -= sp;
            return false;
        }
        else //if (AtualDistance - sp  < minDist)
        {
            AtualDistance = minDist;
            return true;
        }
    }
    public float FollowWhilePistaAngle;
    public float FollowWhilePistaAngle_Speed;
    public float FollowNot_InPistaAngle;
    public float FollowNot_InPistaAngle_Speed;
    private float AtualAngle;

    private bool ToLowerAngle(float minAndle ,float speed)
    {
        float sp = speed * Time.deltaTime;

        if (AtualAngle - sp > minAndle)
        {
            AtualAngle -= sp;
            return false;
        }
        else 
        {
            AtualAngle = minAndle;
            return true;
        }
    }

    private bool TohigherAngle(float maxAndle, float speed)
    {

        float sp = speed * Time.deltaTime;
        if (AtualAngle + sp < maxAndle)
        {
            AtualAngle += sp;
            return false;
        }
        else 
        {
            AtualAngle = maxAndle;
            return true;
        }
    }

    private void setAngle(Vector3 playerLookPosition)
    {

        float Angle = Vector3.Angle( new Vector3(transform.forward.x , 0 , transform.forward.z) , transform.forward);
       // Debug.Log("Angle:" + Angle + "AtualAngle" + AtualAngle);

        if( Angle > AtualAngle)
        {
            transform.RotateAround(playerLookPosition, transform.right, Angle - AtualAngle);
        }
        else if(Angle < AtualAngle)
        {
            transform.RotateAround(playerLookPosition, transform.right, AtualAngle - Angle);
        }

    }

    public int teste = 0;
    private Button pistabutton;
    private Sprite spriteOff;

    private bool isPista = false;
    public float PistaTime;
    private float PistaTimeCounter = 0;
    private bool GoingUp = false;
    private bool GoingDown = false;

    public bool getIsPista
    {
        get { return isPista; }
    }

    public void setActiveIsPista(Button button, Sprite off)   
    {
        isPista = true;
        GoingUp = true;
        GoingDown = false;
        pistabutton = button;
        spriteOff = off;
        PistaTimeCounter = 0;
    }

    private void pista()
    {
        if(isPista)
        {
            bool resultA;
            bool resultB;

            Debug.Log("In Pista");
            if(GoingUp) {

                resultA = TohigherAngle(FollowWhilePistaAngle, FollowWhilePistaAngle_Speed);
                resultB = zoomOut(FollowInPistaDistance, FollowInPistaDistance_Speed);
                if (resultA && resultB)
                    GoingUp = false;

              

            }
            else if(!GoingDown)
            {
                PistaTimeCounter += Time.deltaTime;

                if(PistaTimeCounter >= PistaTime)
                {
                    GoingDown = true;
                    ToLowerAngle(FollowNot_InPistaAngle, FollowNot_InPistaAngle_Speed);
                    zoomIn(FollowNotInPistaDistance, FollowNotInPistaDistance_Speed);

                }
            }
            else
            {
                resultA = ToLowerAngle(FollowNot_InPistaAngle, FollowNot_InPistaAngle_Speed);
                resultB = zoomIn(FollowNotInPistaDistance, FollowNotInPistaDistance_Speed);
                if (resultA && resultB)
                {
                    GoingDown = false;
                    isPista = false;
                    pistabutton.image.sprite = spriteOff;
                }
            }

        }

    }
    

    public GameObject[] fades;
    public GameObject turveEfect;
    private bool fadeIn = false, fadeOut = false;

    private void fadesAndTurveEfectResize()
    {
        float width = UI_Buttons_RectTransform.rect.width;
        float heigth = UI_Buttons_RectTransform.rect.height;

        RectTransform rectTransformTurve = turveEfect.GetComponent<RectTransform>();
        rectTransformTurve.localScale = Vector2.one;
        rectTransformTurve.sizeDelta = new Vector2(width , heigth );


        foreach (GameObject gameObject in fades)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.localScale = Vector2.one;
            rectTransform.sizeDelta = new Vector2(width , heigth );
        }

        
    }

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

    public float fade_s = (float)0.5;
    public bool fade_sin = false;
    public int fade_max = 10;

    public float fadeo_s = (float)0.5;
    public bool fadeo_sin = false;
    public int fadeo_min = 10;

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

    // private TimeToInverCounter;

    //private void checkInvert()
    //{
    //    if (analogic.deltaX > -0.25 && analogic.deltaX < 0.25 && analogic.deltaY < -0.75 && !rotated)
    //    {
    //        if (time > 0)
    //        {
    //            time -= Time.deltaTime;
    //        }
    //        else
    //        {
    //            transform.RotateAround(player.position, player.transform.up, 180 * Time.deltaTime);
    //            player.forward = transform.forward;
    //            rotated = true;
    //        }
    //    }
    //    else
    //    {
    //        time = timeToInvert;
    //        rotated = false;
    //    }
    //}
    public float AfterColisionTime = 2;
    private float AfterColisionTimeCounter = 0;
    private bool colided = false;
    private void AfterColision()
    {
        if (isPista)
        {
            colided = false;
        }

        if (colided && AfterColisionTimeCounter < AfterColisionTime)
        {
            if (playerMov.isDizziness || analogic.direction != Vector2.zero)
            {
                if (AtualDistance > FollowNoMovementDistance)
                {
                    if(zoomIn(FollowNoMovementDistance, FollowNoMovementDistance_Speed))
                        colided = false;

                }
                else
                {
                    if(zoomOut(FollowNoMovementDistance, FollowNoMovementDistance_Speed))
                        colided = false;
                }

            }
            else
            {
                if (AtualDistance > FollowWhileMovementDistance)
                {
                    if(zoomIn(FollowWhileMovementDistance, FollowWhileMovementDistance_Speed))
                        colided = false;
                }
                else
                {
                    if (zoomOut(FollowWhileMovementDistance, FollowWhileMovementDistance_Speed))
                        colided = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colided = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        colided = true;  
    }

    private void OnCollisionExit(Collision collision)
    {
        colided = false;
    }

    public void loadData(GameData gameData)
    {
        this.transform.position = new Vector3(gameData.currentCameraPositionOnLvl[0],
            gameData.currentCameraPositionOnLvl[1], gameData.currentCameraPositionOnLvl[2]);
    }

    public void saveData(GameData gameData)
    {
        // Deconstructing the Vector3 into an array of 3 floats
        var cameraPosition = this.transform.position;

        float[] cameraPositionData =
        {
            cameraPosition.x,
            cameraPosition.y,
            cameraPosition.z,
        };

        gameData.currentCameraPositionOnLvl = cameraPositionData;
    }
}