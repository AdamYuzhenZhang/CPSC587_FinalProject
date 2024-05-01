using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
    private bool showCursor;
    private bool showIKTarget;
    private bool showLine;

    public bool ShowCursor
    {
        get { return showCursor; }
        set
        {
            showCursor = value;
        }
    }
    public bool ShowIKTarget
    {
        get { return showIKTarget; }
        set
        {
            showIKTarget = value;
        }
    }

    public bool ShowLine
    {
        get { return showLine; }
        set
        {
            showLine = value;
        }
    }

    public Toggle cursorToggle;
    public Toggle ikTargetToggle;
    public Toggle lineToggle;
    void Start()
    {
        cursorToggle.isOn = ShowCursor;
        ikTargetToggle.isOn = ShowIKTarget;
        lineToggle.isOn = ShowLine;

        cursorToggle.onValueChanged.AddListener((value) => { ShowCursor = value; });
        ikTargetToggle.onValueChanged.AddListener((value) => { ShowIKTarget = value; });
        lineToggle.onValueChanged.AddListener((value) => { ShowLine = value; });
    }
}
