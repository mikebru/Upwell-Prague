using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlaylistManager : MonoBehaviour
{
    // The list of video clips to play. Assign these in the Inspector.
    public List<string> videoClips;

    public UnityEvent PlayCompleteEvent;

    private VideoPlayer videoPlayer;
    private int currentVideoIndex = 0;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Subscribe to the event that is triggered when a video finishes
        videoPlayer.loopPointReached += OnVideoFinished;

        // Start playing the first video
        if (videoClips.Count > 0)
        {
          //  PlayVideo(currentVideoIndex);
        }
    }


    public void PlayStreamingClip(int index)
    {
        if (index >= 0 && index < videoClips.Count)
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + videoClips[index];
            StartCoroutine(PlayVideo());
        }
    }

    private IEnumerator PlayVideo()
    {
        // We must set the audio before calling Prepare, otherwise it won't play the audio
        var audioSource = videoPlayer.GetComponent<AudioSource>();
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        // Wait until ready
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
    }


    // This method is called when the current video finishes
    void OnVideoFinished(VideoPlayer vp)
    {
        currentVideoIndex++;

        // If there are more videos, play the next one
        if (currentVideoIndex < videoClips.Count)
        {
            PlayStreamingClip(currentVideoIndex);
        }
        else
        {
            Debug.Log("Playlist finished!");
            // Optional: loop back to the start or stop
            currentVideoIndex = 0;
            PlayCompleteEvent.Invoke();
            // PlayVideo(currentVideoIndex);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}
