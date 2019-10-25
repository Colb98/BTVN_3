using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayVideo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();

        InvokeRepeating("checkOver", .1f, .1f);
        audioSource.Play();
    }

    private void checkOver()
    {
        long playerCurrentFrame = videoPlayer.GetComponent<VideoPlayer>().frame;
        long playerFrameCount = Convert.ToInt64(videoPlayer.GetComponent<VideoPlayer>().frameCount);
        
        if (playerCurrentFrame < playerFrameCount - 2)
        {
            // print("VIDEO IS PLAYING");
        }
        else
        {
            //print("VIDEO IS OVER");
            GetComponent<MainMenu>().StartGame();
            //Do w.e you want to do for when the video is done playing.

            //Cancel Invoke since video is no longer playing
            CancelInvoke("checkOver");
        }
    }
}
