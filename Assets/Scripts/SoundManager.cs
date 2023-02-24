using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Note;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private float randomness = 0.5f;

    [System.Serializable]
    struct ClipList
    {
        public NoteType type;
        public AudioClip[] clips;
    }
    [SerializeField] private ClipList[] clipLists;

    struct ClipType
    {
        public NoteType type;
        public AudioClip clip;

        public ClipType (NoteType _type, AudioClip _clip)
        {
            type = _type;
            clip = _clip;
        }
    }
    private Dictionary<ClipType, AudioSource> sources = new Dictionary<ClipType, AudioSource>();

    private Dictionary<NoteType, int> clipIdx = new Dictionary<NoteType, int>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        foreach (ClipList clipList in clipLists)
        {
            clipIdx[clipList.type] = -1;

            foreach (AudioClip clip in clipList.clips)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.clip = clip;
                sources[new ClipType(clipList.type, clip)] = source;
            }
        }
    }

    public static void PlayNextClip(NoteType type)
    {
        AudioClip[] clips = { };

        foreach (ClipList clipList in instance.clipLists)
            if (clipList.type == type)
                clips = clipList.clips;

        if (clips.Length == 0) return;

        bool playRandom = Random.Range(0f, 1f) < instance.randomness;

        if (playRandom)
        {
            instance.sources[new ClipType(type, clips[Random.Range(0, clips.Length)])].Play();
        }
        else
        {
            instance.clipIdx[type]++;

            if (instance.clipIdx[type] >= clips.Length)
                instance.clipIdx[type] = 0;

            instance.sources[new ClipType(type, clips[instance.clipIdx[type]])].Play();
        }
    }
}
