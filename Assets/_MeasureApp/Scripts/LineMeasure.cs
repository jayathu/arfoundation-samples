using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class LineMeasure : MonoBehaviour
{
    public GameObject pointPrefab;
    public Material lrMaterialOne;
    public Material lrMaterialTwo;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARSessionOrigin m_SessionOrigin;

    private List<LineSegment> points;
    private GameObject marker;
    private bool startMeasuring = true;

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        marker = Instantiate(pointPrefab);
        points = new List<LineSegment>();
    }

    // Update is called once per frame
    void Update()
    {
        //marker is always at the center of the plane
        if (marker != null)
        {
            Vector3 center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);

            if (m_SessionOrigin.Raycast(center, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = s_Hits[0].pose;
                marker.transform.position = hitPose.position;
                marker.transform.rotation = hitPose.rotation;
            }
        }

        RenderLineBetweenPoints();
    }

    /// <summary>
    /// Invoked by a button click. Adds a point at the marked position
    /// </summary>
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


    private void RenderLineBetweenPoints()
    {
        foreach (LineSegment lineSegment in points)
        {
            lineSegment.DrawLine();
            lineSegment.DrawMeasurementText();
        }
    }

    /// <summary>
    /// Invoked by a button clicks to clear all points and lines and associated data
    /// </summary>
    public void ClearPoints()
    {
        foreach(LineSegment line in points)
        {
            if(line.pointOne != marker)
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
