using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer; // assign the VideoPlayer component in the Inspector
    public string nextSceneName; // the name of the scene to load after the video ends

    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
