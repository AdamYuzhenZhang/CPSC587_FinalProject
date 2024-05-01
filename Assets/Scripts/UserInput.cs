using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserInput : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private Data data;
    //private Plane groundPlane;

    [SerializeField] private RobotMover robotMover;
    [SerializeField] private CursorAnimator cursor;

    private void Start()
    {
        indicator.SetActive(false);
        //groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // hit
            Vector3 pos = hit.point;
            indicator.SetActive(data.ShowCursor);
            indicator.transform.position = pos;
            // move robot head
            robotMover.TurnTowards(pos);
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("clicked");
                UserClicked(pos);
            }
        } else
        {
            // not hit
            indicator.SetActive(false);
        }
    }

    private void UserClicked(Vector3 hit)
    {
        cursor.CursorClick(hit);
        // update curve point positions
        robotMover.UpdateBezier(hit);
        // move robot along the curve
        robotMover.StopInterpolation();
        robotMover.StartInterpolation();
    }

}


