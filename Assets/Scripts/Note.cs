using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private int BPM = 20;

    private Rigidbody2D rb;
    private AudioSource source;

    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();

        speed = BPM / 60f;

        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioClip clip = SoundManager.GetNextClip();
        source.PlayOneShot(clip);

        Debug.Log(clip.name);
    }
}
