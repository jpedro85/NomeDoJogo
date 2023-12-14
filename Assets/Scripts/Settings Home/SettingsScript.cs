using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    
    public RectTransform canvasRecttransForm;
    public GameObject[] GameObjectToResize;
    // Start is called before the first frame update
    void Start()
    {
        float width = canvasRecttransForm.rect.width;
        float heigth = canvasRecttransForm.rect.height;

        foreach (GameObject obj_transform in GameObjectToResize)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
           // rectTransform.localScale = Vector2.one;
            Debug.LogWarning(width + "::" + heigth );
            rectTransform.sizeDelta = new Vector2(width, heigth);
        }

    }

}
