using UnityEngine;

[RequireComponent (typeof(FlowLayoutGroup))]
public class FlowLayoutCanvasForceUpdater : MonoBehaviour
{
    private FlowLayoutGroup flg;
    private Vector2 spacing;

    private void Awake()
    {
        flg = gameObject.GetComponent<FlowLayoutGroup>();
        spacing = flg.spacing;
    }

    private void Update()
    {
        spacing.x += Random.Range(-0.0001f, 0.0001f);
        flg.spacing = spacing;
    }
}