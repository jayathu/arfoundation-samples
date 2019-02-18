using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test script to simulate measuring points in EditorMode
/// </summary>
public class TestMeasureLine : MonoBehaviour
{
    public GameObject pointPrefab;
    public Material lrMaterialOne;
    public Material lrMaterialTwo;

    private GameObject marker;
    private Plane plane;
    private List<LineSegment> points;
    private bool startMeasuring = true;

    void Start()
    {
        marker = Instantiate(pointPrefab);
        plane = new Plane(gameObject.transform.up, transform.position);
        points = new List<LineSegment>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float fDist = 0.0f;
        plane.Raycast(ray, out fDist);
        marker.transform.position = ray.GetPoint(fDist);

        if(Input.GetMouseButtonDown(0))
        {
            AddPoint();
        }

        RenderLineBetweenPoints();

    }

    private void RenderLineBetweenPoints()
    { 
        foreach(LineSegment lineSegment in points)
        {
            lineSegment.DrawLine();
            lineSegment.DrawMeasurementText();
        }
    }

    public void AddPoint()
    {
        if (startMeasuring)
        {
            GameObject pointOne = Instantiate(pointPrefab, marker.transform.position, marker.transform.rotation);
            GameObject pointTwo = marker;
            LineSegment line = new LineSegment(pointOne, pointTwo);
            line.UpdateMaterial(lrMaterialOne);
            startMeasuring = false;
            points.Add(line);
        }
        else
        {
            LineSegment line = points[points.Count - 1];
            line.pointTwo = Instantiate(pointPrefab, marker.transform.position, marker.transform.rotation);
            startMeasuring = true;
            line.UpdateMaterial(lrMaterialTwo);
        }
    }

    public void ClearPoints()
    {
        foreach (LineSegment line in points)
        {
            if (line.pointOne != marker)
            {
                Destroy(line.pointOne);
            }
            if (line.pointTwo != marker)
            {
                Destroy(line.pointTwo);
            }
            Destroy(line.textMeshGO);
        }

        points.Clear();
        startMeasuring = true;
    }

}
