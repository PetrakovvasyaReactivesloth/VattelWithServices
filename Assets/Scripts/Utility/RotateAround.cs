using UnityEngine;

public class RotateAround : MonoBehaviour {

    public Vector3 rotationVector = new Vector3(0, 0, 10);

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.Rotate(rotationVector * Time.unscaledDeltaTime);
    }
}