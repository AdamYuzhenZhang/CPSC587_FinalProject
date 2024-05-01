using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// cubic bezier curve
public class BezierCurve : MonoBehaviour
{
    [SerializeField] private GameObject startPt;
    [SerializeField] private GameObject control1;
    [SerializeField] private GameObject control2;
    [SerializeField] private GameObject endPt;
    [SerializeField] private int segments = 30;
    private LineRenderer lineRenderer;
    public float length = 0;

    [SerializeField] private Data data;

    private void Start()
    {
        // create line renderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        DrawCurve();
    }

    private void Update()
    {
        DrawCurve();
    }

    private void DrawCurve()
    {
        Vector3 lastPt = startPt.transform.position;
        Vector3 pt = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            pt = GetPoint(t);
            lineRenderer.SetPosition(i, pt);
        }

        lineRenderer.enabled = data.ShowLine;
        startPt.SetActive(data.ShowLine);
        control1.SetActive(data.ShowLine);
        control2.SetActive(data.ShowLine);
        endPt.SetActive(data.ShowLine);
    }

    private void UpdateLength()
    {
        length = 0;
        Vector3 lastPt = startPt.transform.position;
        Vector3 currentPt;

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            currentPt = GetPoint(t);
            length += Vector3.Distance(lastPt, currentPt);
            lastPt = currentPt;
        }
        //Debug.Log("update length");
        Debug.Log(length);
    }

    private Vector3 GetPoint(float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 p0 = startPt.transform.position;
        Vector3 p1 = control1.transform.position;
        Vector3 p2 = control2.transform.position;
        Vector3 p3 = endPt.transform.position;
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }

    private Vector3 GetTangent(float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 p0 = startPt.transform.position;
        Vector3 p1 = control1.transform.position;
        Vector3 p2 = control2.transform.position;
        Vector3 p3 = endPt.transform.position;
        Vector3 tangent = 3 * Mathf.Pow(1 - t, 2) * (p1 - p0) +
                          6 * (1 - t) * t * (p2 - p1) +
                          3 * Mathf.Pow(t, 2) * (p3 - p2);
        return tangent.normalized;
    }

    public void UpdateTransform(Transform robotT, float t)
    {
        Vector3 pos = GetPoint(t);
        pos.y = 0;
        robotT.position = pos;
        Vector3 tangent = GetTangent(t);
        tangent.y = 0;
        Vector3 rightVector = Vector3.Cross(Vector3.up, tangent.normalized);
        Quaternion targetRotation = Quaternion.LookRotation(rightVector, Vector3.up);
        robotT.rotation = targetRotation;
    }

    public void UpdateControlPts(Transform robotT, Vector3 endPos)
    {
        startPt.transform.position = new Vector3(robotT.position.x, 3, robotT.position.z);
        Vector3 forward = -robotT.transform.right;
        endPt.transform.position = new Vector3(endPos.x, 3, endPos.z);
        float factor = Vector3.Distance(startPt.transform.position, endPt.transform.position);
        control1.transform.position = startPt.transform.position + forward * factor / 2;
        control2.transform.position = new Vector3(endPos.x, 3, endPos.z);

        UpdateLength();
    }


}
