using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip[] clips;

    private Dictionary<AudioClip, AudioSource> sources = new Dictionary<AudioClip, AudioSource>();

    private int clipIdx = -1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        foreach (AudioClip clip in clips)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.clip = clip;
            sources[clip] = source;
        }
    }

    public static void PlayNextClip()
    {
        if (instance.clips.Length == 0) return;

        instance.clipIdx++;

        if (instance.clipIdx >= instance.clips.Length)
            instance.clipIdx = 0;

        instance.sources[instance.clips[instance.clipIdx]].Play();
    }
}
