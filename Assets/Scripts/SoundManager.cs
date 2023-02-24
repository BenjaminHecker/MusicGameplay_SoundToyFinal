using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip[] clips;

    private int clipIdx = -1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static AudioClip GetNextClip()
    {
        if (instance.clips.Length == 0) return null;

        instance.clipIdx++;

        if (instance.clipIdx >= instance.clips.Length)
            instance.clipIdx = 0;

        return instance.clips[instance.clipIdx];
    }
}
