using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAnimator : MonoBehaviour
{
    [SerializeField] private GameObject cursorVisualizer;
    private void Start()
    {
        cursorVisualizer.SetActive(false);
    }

    public void CursorClick(Vector3 pos)
    {
        cursorVisualizer.transform.position = pos;
        StartCoroutine(AnimateCursor(0.2f));
    }

    IEnumerator AnimateCursor(float duration)
    {
        Vector3 startScale = new Vector3(0f, 0.1f, 0f);
        Vector3 endScale = new Vector3(3f, 0.1f, 3f);
        float elapsedTime = 0f;
        cursorVisualizer.SetActive(true);

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            cursorVisualizer.transform.localScale = Vector3.Lerp(startScale, endScale, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cursorVisualizer.transform.localScale = endScale;
        cursorVisualizer.SetActive(false);
    }
}
