using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class OpacityController : MonoBehaviour
{
    public RawImage image;
    public float maxOpacity = 1f;
    public float fadeDuration = 2f;
    public float fadeDelay = 2f; // added variable for fade-out delay

    private float currentOpacity = 0f;

    private void Start()
    {
        // Set the initial opacity to 0
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }

    public void Opacity()
    {
        // Start the fade-in effect
        StartCoroutine(FadeToMaxOpacity());

        // Start the fade-out effect after a delay
        StartCoroutine(FadeOutAfterDelay());
    }

    private IEnumerator FadeToMaxOpacity()
    {
        // Loop until the max opacity has been reached
        while (currentOpacity < maxOpacity)
        {
            // Calculate the new opacity for this frame
            currentOpacity += Time.deltaTime / fadeDuration;
            currentOpacity = Mathf.Clamp01(currentOpacity);

            // Update the image's color with the new opacity
            image.color = new Color(image.color.r, image.color.g, image.color.b, currentOpacity);

            yield return null;
        }
    }

    private IEnumerator FadeOutAfterDelay()
    {
        // Wait for the fade delay before starting the fade-out effect
        yield return new WaitForSeconds(fadeDelay);

        // Loop until the opacity has reached 0
        while (currentOpacity > 0f)
        {
            // Calculate the new opacity for this frame
            currentOpacity -= Time.deltaTime / fadeDuration;
            currentOpacity = Mathf.Clamp01(currentOpacity);

            // Update the image's color with the new opacity
            image.color = new Color(image.color.r, image.color.g, image.color.b, currentOpacity);

            yield return null;
        }
    }
}
