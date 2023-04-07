using UnityEngine;
using UnityEngine.UI;

public class OpacityController : MonoBehaviour
{
    public RawImage image;
    public float maxOpacity = 1f;
    public float fadeDuration = 2f;

    private float currentOpacity = 0f;
    private float timeElapsed = 0f;

    private void Start()
    {
        // Set the initial opacity to 0
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }

    public void Opacity()
    {
        // Increment the time elapsed since the script started running
        timeElapsed += Time.deltaTime;

        // Calculate the new opacity based on the elapsed time and fade duration
        currentOpacity = Mathf.Lerp(0f, maxOpacity, timeElapsed / fadeDuration);

        // Update the image's color with the new opacity
        image.color = new Color(image.color.r, image.color.g, image.color.b, currentOpacity);

        // If the max opacity has been reached, stop updating the image's color
        if (currentOpacity >= maxOpacity)
        {
            enabled = false;
        }
    }
}
