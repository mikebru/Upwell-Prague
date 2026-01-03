using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlaylistManager : MonoBehaviour
{
    // The list of video clips to play. Assign these in the Inspector.
    public List<VideoClip> videoClips;

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
            PlayVideo(currentVideoIndex);
        }
    }

    void PlayVideo(int index)
    {
        if (index >= 0 && index < videoClips.Count)
        {
            videoPlayer.clip = videoClips[index];
            videoPlayer.Play();
        }
    }

    // This method is called when the current video finishes
    void OnVideoFinished(VideoPlayer vp)
    {
        currentVideoIndex++;

        // If there are more videos, play the next one
        if (currentVideoIndex < videoClips.Count)
        {
            PlayVideo(currentVideoIndex);
        }
        else
        {
            Debug.Log("Playlist finished!");
            // Optional: loop back to the start or stop
             currentVideoIndex = 0;
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
