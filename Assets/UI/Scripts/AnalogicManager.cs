using System;
using System.Drawing;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AnalogicManager : MonoBehaviour
{

    private RectTransform Analogico;
    public GameObject AnalogicoBlur;
    public GameObject BackgoundRun;
    public GameObject BackgoundWalk;
    public float raio = 70;

    private bool running = false;
    private Vector2 initialPosition;
    private Vector2 atualPosition;
    private Touch analogicoTouch;
    private Vector2 sizeHalf;
    private Vector2 directionVector;
    private float walkLimite;

    public float speed
    {
        get { return (directionVector.magnitude / raio); }
    }

    public bool isRunning
    {
        get { return running; }
    }
    public float deltaX
    {
        get { return (atualPosition.x - initialPosition.x) / raio; }
    }
    public float deltaY
    {
        get { return (atualPosition.y - initialPosition.y) / raio; }
    }

    public Vector2 direction
    {
        get { return directionVector; }
    }

    public float angle
    {
        get { return Vector2.Angle(directionVector, Vector2.up);  }
    }

    public void Start()
    {
        directionVector = Vector2.zero;

        Analogico = transform.GetComponent<RectTransform>();
        initialPosition.x = transform.position.x;
        initialPosition.y = transform.position.y;
        atualPosition = initialPosition;

        sizeHalf.x = Analogico.rect.width / 2;
        sizeHalf.y = Analogico.rect.height / 2;

        raio = (GameObject.FindWithTag("AnalogicoConteiner").GetComponent<RectTransform>().rect.width * GameObject.FindWithTag("PlayUICanvas").GetComponent<Transform>().localScale.x) / 2;
        walkLimite = BackgoundWalk.GetComponent<RectTransform>().rect.width / 2;

        analogicoTouch = new Touch { fingerId = -1 };
        AnalogicoBlur.SetActive(false);
        BackgoundRun.SetActive(false);
    }

    public void Update()
    {
        Touch input;

        if (Input.touchCount > 0)
        {

            if (analogicoTouch.fingerId == -1)
            {
                for (int toutchId = 0; toutchId < Input.touchCount; toutchId++)
                {
                    input = Input.GetTouch(toutchId);
                    if (
                        input.position.x >= atualPosition.x - sizeHalf.x && input.position.x <= atualPosition.x + sizeHalf.x
                        && input.position.y >= atualPosition.y - sizeHalf.y && input.position.y <= atualPosition.y + sizeHalf.y
                       )
                    {
                        analogicoTouch = input;
                        AnalogicoBlur.SetActive(true);

                    }
                }

            }
            else
            {
                for (int toutchId = 0; toutchId < Input.touchCount; toutchId++)
                {
                    input = Input.GetTouch(toutchId);

                    if (input.fingerId == analogicoTouch.fingerId)
                    {
                        analogicoTouch = input;
                    }
                }

                if (analogicoTouch.phase == TouchPhase.Ended || analogicoTouch.phase == TouchPhase.Canceled)
                {
                    analogicoTouch = new Touch { fingerId = -1 };
                    atualPosition = initialPosition;
                    Analogico.position = initialPosition;
                    AnalogicoBlur.transform.position = initialPosition;
                    directionVector = Vector2.zero;
                    AnalogicoBlur.SetActive(false);
                    BackgoundRun.SetActive(false);
                }
                else
                {
                    directionVector = (analogicoTouch.position - initialPosition );

                    atualPosition = initialPosition + Vector2.ClampMagnitude(directionVector, raio);
                    Analogico.position = atualPosition;
                    AnalogicoBlur.transform.position = atualPosition;

                    if( Vector2.Distance(atualPosition,initialPosition) > walkLimite)
                    {
                        running = true;
                        BackgoundRun.SetActive(true);
                    }
                    else
                    {
                        running = false;
                        BackgoundRun.SetActive(false);
                    }

                   
                }

            }
        }

    }
}