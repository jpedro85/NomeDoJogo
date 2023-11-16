using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 10;

    private AnalogicManager analogic;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        analogic = GameObject.FindWithTag("Analogico").GetComponent<AnalogicManager>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (analogic.direction != Vector2.zero)
        {
            Vector3 forward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
            Vector3 pointAux = mainCamera.transform.position + (mainCamera.transform.right.normalized * analogic.deltaX );
            Vector3 point = pointAux + (forward.normalized * analogic.deltaY );
            transform.forward = (point - mainCamera.transform.position);
        }

        transform.position = transform.position + (transform.forward * Time.deltaTime * maxSpeed * analogic.speed);

    }
}
