using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextOpacityController : MonoBehaviour
{
    public TMP_Text textElement;
    public float fadeInTime;
    public float visibleTime;
    public float fadeOutTime;
    public float startDelay;

    IEnumerator OpacityCoroutine()
    {
        // Delay before starting animation
        yield return new WaitForSeconds(startDelay);

        // Fade in
        float startTime = Time.time;
        Color startColor = textElement.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
        while (Time.time < startTime + fadeInTime)
        {
            float t = (Time.time - startTime) / fadeInTime;
            textElement.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // Wait for visible time
        yield return new WaitForSeconds(visibleTime);

        // Fade out
        startTime = Time.time;
        startColor = textElement.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        while (Time.time < startTime + fadeOutTime)
        {
            float t = (Time.time - startTime) / fadeOutTime;
            textElement.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // Disable text element
        textElement.gameObject.SetActive(false);
    }

    public void StartOpacityAnimation()
    {
        // Enable text element
        textElement.gameObject.SetActive(true);

        StartCoroutine(OpacityCoroutine());
    }
}
