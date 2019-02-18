using UnityEngine;
using System;

/// <summary>
/// Data Structure to store two end points for a line segment, line renderer and text mesh to display measurements
/// </summary>
public class LineSegment
{
    public GameObject pointOne;
    public GameObject pointTwo;
    public GameObject textMeshGO;
    private LineRenderer lineRenderer;
    private TextMesh textMesh;

    private const float CHAR_SIZE = 0.025f;
    private const float WIDTH = 0.007f;

    public LineSegment(GameObject pos1, GameObject pos2)
    {
        pointOne = pos1;
        pointTwo = pos2;
        lineRenderer = pointOne.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = WIDTH;

        textMeshGO = new GameObject("TextMesh");
        textMesh = textMeshGO.AddComponent<TextMesh>();
        textMesh.characterSize = CHAR_SIZE;

    }

    /// <summary>
    /// Uses LineRenderer to draw line between the two end points
    /// </summary>
    public void DrawLine()
    {
        LineRenderer lr = pointOne.GetComponent<LineRenderer>();
        lr.SetPosition(0, pointOne.transform.position);
        lr.SetPosition(1, pointTwo.transform.position);
    }

    /// <summary>
    /// UpdateMaterial is used to change line renderer material when a line segment is already formed.
    /// This is used to visually differentiate between when a line segment is being measured vs a line segment is already measured.
    /// </summary>
    /// <param name="material">Material.</param>
    public void UpdateMaterial(Material material)
    {
        LineRenderer lr = pointOne.GetComponent<LineRenderer>();
        lr.material = material;
    }

    /// <summary>
    /// Computes Measurement in Meters and displays as a text mesh
    /// </summary>
    public void DrawMeasurementText()
    {
        float dist = Vector3.Distance(pointOne.transform.position, pointTwo.transform.position);
        double truncatedDist = Math.Truncate(dist * 100) / 100;
        Vector3 dir = (pointTwo.transform.position - pointOne.transform.position) * 0.5f;
        textMesh.transform.position = pointOne.transform.position + dir;
        textMesh.text = truncatedDist + " meters";
        textMesh.transform.rotation = Quaternion.LookRotation(Vec3_ZeroY(textMesh.transform.position - Camera.main.transform.position));
    }

    Vector3 Vec3_ZeroY(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }
}