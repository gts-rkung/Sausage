using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCursor : MonoBehaviour
{
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite touchSprite;
    Image image;
    [SerializeField] Vector3 positionShift = new Vector3(28f, -90f, 0f);
    [SerializeField] float minTouchDownTime = 0.1f;
    float touchDownTime;
    bool touchDown;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(normalSprite, "normalSprite not assigned");
        Debug.Assert(touchSprite, "tapSprite not assigned");
        image = GetComponent<Image>();
        Debug.Assert(image, "image not found");
    }

    // Update is called once per frame
    void Update()
    {
        image.rectTransform.anchoredPosition3D = Input.mousePosition + positionShift;

        if (TouchInput.GetTouched(false))
        {
            touchDown = true;
            touchDownTime = 0f;
            image.sprite = touchSprite;
        }
        if (touchDown)
        {
            touchDownTime += Time.deltaTime;
            if (touchDownTime > minTouchDownTime)
            {
                touchDown = false;
                image.sprite = normalSprite;
            }
        }
    }
}
