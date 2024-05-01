using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMover : MonoBehaviour
{

    [SerializeField] Transform neck;
    [SerializeField] BezierCurve bezierCurve;

    public void TurnTowards(Vector3 pos)
    {
        Vector3 direction = pos - neck.position;
        direction.y = 0;
        Vector3 rightVector = Vector3.Cross(Vector3.up, direction.normalized);
        neck.rotation = Quaternion.LookRotation(rightVector, Vector3.up);
    }

    public void UpdateBezier(Vector3 endPos)
    {
        bezierCurve.UpdateControlPts(transform, endPos);
    }

    // move robot along bezier curve
    private Coroutine currentCoroutine;
    //private float interpolationTime = 5f;

    public void StartInterpolation()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        float length = bezierCurve.length;
        length /= 4;
        Debug.Log(length);
        currentCoroutine = StartCoroutine(InterpolateT(length));
    }

    public void StopInterpolation()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    private IEnumerator InterpolateT(float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // move robot to t
            bezierCurve.UpdateTransform(transform, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


}
