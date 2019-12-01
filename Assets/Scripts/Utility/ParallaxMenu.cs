using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParallaxMenu : MonoBehaviour
{

    public RectTransform canvas;
    public float smoothTime = 0.3f;
    public float[] minMaxXAxis = new float[2]; //0 min, 1 max
    public float[] minMaxYAxis = new float[2]; //0 min, 1 max
    public float mouseSensitivity = 50.0f;
    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;
    private float x;
    private float y;
    private float xSmooth;
    private float ySmooth;
    Vector2 pos;
    
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{

        //    pos = new Vector2(Screen.width * 0.3f - Input.mousePosition.x, Screen.height * 0.4f - Input.mousePosition.y);
        //}

        //x -= pos.x * mouseSensitivity * .002f ;
        //y -= pos.y * mouseSensitivity * 002f ;

        x = (Input.GetAxis("Mouse X") * 0.5f )* mouseSensitivity * 1;
        y = (Input.GetAxis("Mouse Y") * 0.5f )* mouseSensitivity * 1;

        if (Time.timeScale < 0.1f) {
            x /= 0.0001f;
            y /= 0.0001f;
        }

        if (y > minMaxYAxis[1]) y = minMaxYAxis[1];
        if (y < minMaxYAxis[0]) y = minMaxYAxis[0];
        if (x > minMaxXAxis[1]) x = minMaxXAxis[1];
        if (x < minMaxXAxis[0]) x = minMaxXAxis[0];

        xSmooth = Mathf.SmoothDamp(xSmooth, x, ref xVelocity, smoothTime);
        ySmooth = Mathf.SmoothDamp(ySmooth, y, ref yVelocity, smoothTime);

        Quaternion parallax = new Quaternion();
        parallax = Quaternion.Euler(ySmooth / 7, xSmooth / 7, 0);

        canvas.transform.localPosition = new Vector3(xSmooth, ySmooth, 0f);
        canvas.transform.localRotation = parallax;
    }
}