using UnityEngine;

public class ScaleAnim : MonoBehaviour
{
    public float scalingSpeed = 1;
    public float waitTime = 0;
    private Transform _transform;

    public bool useTargetScale = false;
    public Vector3 startingScale;
    
    private Vector3 _savedScale, _scale;
    private float _time;
    
    private void Start ()
    {
        _transform = transform;

        _scale = useTargetScale ? startingScale : Vector3.zero;

        _savedScale = _transform.localScale;

        _transform.localScale = _scale;
        _time = 0;
    }

    private void Update()
    {
        _time += Time.unscaledDeltaTime;
        
        if (_time > waitTime)
        {
            _scale = Vector3.Lerp(_scale, _savedScale, Time.unscaledDeltaTime * scalingSpeed);
            _transform.localScale = _scale;
        }
    }
}