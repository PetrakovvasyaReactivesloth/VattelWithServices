using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextIntCounter : MonoBehaviour
{
    public int currentValue;

    public float waitTime = 0;
    public float speed = 1;
    
    private Text _text;
    private float value = 0, _time;

    private void Awake()
    {
        _text = gameObject.GetComponent<Text>();
        speed *= 100;
        _time = 0;
    }

    public void SetValue(int value)
    {
        currentValue = value;
    }

    private void Update()
    {
        _time += Time.unscaledDeltaTime;
        
        if (_time > waitTime)
        {
            if (Mathf.Approximately(currentValue, value))
            {
                value = currentValue;
            }

            if (value < currentValue)
            {
                value = Mathf.Lerp(value, currentValue, Time.unscaledDeltaTime * speed / 10);
            }

            _text.text = value.ToString("F0");
        }
    }
}
