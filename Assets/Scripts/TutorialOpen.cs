using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOpen : MonoBehaviour
{
    public float fadeTime = 2f; // time in seconds to fade out the game object
    public float disableTime = 3f; // time in seconds before the game object is disabled

    private Renderer rend;
    private Color initialColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        initialColor = rend.material.color;

        // Set opacity to max when the game starts
        Color startColor = initialColor;
        startColor.a = 1f;
        rend.material.color = startColor;

        StartFadeOutCoroutine();
    }

    public void StartFadeOutCoroutine()
    {
        StartCoroutine(DisableAndFadeCoroutine());
    }

    private IEnumerator DisableAndFadeCoroutine()
    {
        yield return new WaitForSeconds(disableTime);

        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            float t = elapsedTime / fadeTime;
            Color newColor = Color.Lerp(initialColor, Color.clear, t);
            rend.material.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set opacity back to max when the game object is disabled
        Color disabledColor = initialColor;
        disabledColor.a = 1f;
        rend.material.color = disabledColor;

        gameObject.SetActive(false);
    }
}
