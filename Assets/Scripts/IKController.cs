using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    // shoot ray from here
    [SerializeField] private Transform FR;
    [SerializeField] private Transform FL;
    [SerializeField] private Transform BR;
    [SerializeField] private Transform BL;
    // intersection visualizer
    [SerializeField] private GameObject FRTarget;
    [SerializeField] private GameObject FLTarget;
    [SerializeField] private GameObject BRTarget;
    [SerializeField] private GameObject BLTarget;
    // current attractor
    [SerializeField] private GameObject FRAttr;
    [SerializeField] private GameObject FLAttr;
    [SerializeField] private GameObject BRAttr;
    [SerializeField] private GameObject BLAttr;
    // data
    [SerializeField] private Data data;
    // bools
    private bool FRMoving;
    private bool FLMoving;


    private float stepTime = 0.1f;
    private float threshold = 1.2f;
    private float stepHeight = 0.2f;
    private void Update()
    {
        UpdateVisualizerPosition(FRTarget, FR);
        UpdateVisualizerPosition(FLTarget, FL);
        UpdateVisualizerPosition(BRTarget, BR);
        UpdateVisualizerPosition(BLTarget, BL);
        if (!FRMoving && !FLMoving)
        {
            Debug.Log("distance");
            Debug.Log(Vector3.Distance(FRAttr.transform.position, FRTarget.transform.position));
            if (Vector3.Distance(FRAttr.transform.position, FRTarget.transform.position) > threshold)
            {
                StartCoroutine(MoveToTarget(FRAttr, FRTarget, stepTime));
                StartCoroutine(MoveToTarget(BLAttr, BLTarget, stepTime));
                FRMoving = true;
                StartCoroutine(MovingFR(stepTime));
            }
            if (!FRMoving && Vector3.Distance(FLAttr.transform.position, FLTarget.transform.position) > threshold)
            {
                StartCoroutine(MoveToTarget(FLAttr, FLTarget, stepTime));
                StartCoroutine(MoveToTarget(BRAttr, BRTarget, stepTime));
                FLMoving = true;
                StartCoroutine(MovingFL(stepTime));
            }
        }
        FRTarget.SetActive(data.ShowIKTarget);
        FLTarget.SetActive(data.ShowIKTarget);
        BRTarget.SetActive(data.ShowIKTarget);
        BLTarget.SetActive(data.ShowIKTarget);
        FRAttr.SetActive(data.ShowIKTarget);
        FLAttr.SetActive(data.ShowIKTarget);
        BRAttr.SetActive(data.ShowIKTarget);
        BLAttr.SetActive(data.ShowIKTarget);
    }

    private void UpdateVisualizerPosition(GameObject target, Transform rayOrigin)
    {
        Ray ray = new Ray(rayOrigin.position, new Vector3(0, -1, 0));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                target.transform.position = hit.point;
                target.transform.rotation = rayOrigin.rotation;
            }
        }
    }

    private IEnumerator MoveToTarget(GameObject attr, GameObject target, float duration)
    {
        Debug.Log("moving");
        Vector3 startPosition = attr.transform.position;
        Vector3 endPosition = target.transform.position;
        Quaternion startRotation = attr.transform.rotation;
        Quaternion endRotation = target.transform.rotation;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            Vector3 targetPosition = Vector3.Lerp(startPosition, endPosition, progress);
            float verticalOffset = Mathf.Sin(progress * Mathf.PI/2) * stepHeight;
            //attr.transform.position = targetPosition;
            attr.transform.position = new Vector3(targetPosition.x, targetPosition.y + verticalOffset, targetPosition.z);
            attr.transform.rotation = Quaternion.Slerp(startRotation, endRotation, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        attr.transform.position = new Vector3(endPosition.x, endPosition.y, endPosition.z);
        attr.transform.rotation = endRotation;
    }

    private IEnumerator MovingFR(float delay)
    {
        yield return new WaitForSeconds(delay);
        FRMoving = false;
    }
    private IEnumerator MovingFL(float delay)
    {
        yield return new WaitForSeconds(delay);
        FLMoving = false;
    }
}
