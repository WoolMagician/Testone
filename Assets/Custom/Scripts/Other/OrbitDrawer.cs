using UnityEngine;

[ExecuteInEditMode]
public class OrbitDrawer : MonoBehaviour
{
    public Gradient colorGradient;
    public float lineWidth;
    public float radius;

    public void Update()
    {
        LineRenderer line = this.gameObject.GetComponent<LineRenderer>();

        if (line == null)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
        }

        var segments = 360;
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;
        line.colorGradient = colorGradient;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }
}