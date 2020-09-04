using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Vuforia;

public class csTrakingAd : MonoBehaviour, ITrackableEventHandler
{
    TrackableBehaviour track;

    [SerializeField]
    VideoPlayer[] scripts;

    void Start()
    {
        scripts = GameObject.FindObjectsOfType<VideoPlayer>();
        track = GetComponent<TrackableBehaviour>();
        if (track)
        {
            track.RegisterTrackableEventHandler(this);
        }
    }

    void OnDisable()
    {
        track.UnregisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            ScriptEnable(true);
        }
        else
        {
            ScriptEnable(false);
        }
    }

    void ScriptEnable(bool isEnabled)
    {
        foreach (var script in scripts)
        {
            if (isEnabled)
                script.Play();
            else
                script.Pause();
        }
    }
}
