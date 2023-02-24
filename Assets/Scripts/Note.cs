using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private int BPM = 20;

    private Rigidbody2D rb;
    private AudioSource source;

    private Vector3 originalPos;
    private Vector3 dir;
    private float speed;

    [HideInInspector] public Vector3 Direction { get { return dir; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
    }

    public void Run()
    {
        originalPos = transform.position;
        dir = transform.up;

        speed = BPM / 60f;
        rb.velocity = dir * speed;
    }

    public void ResetNote()
    {
        transform.position = originalPos;
        rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 colPos = collision.transform.position;
        transform.position = new Vector2(Mathf.Round(colPos.x * 2) / 2f, Mathf.Round(colPos.y * 2) / 2f);

        AudioClip clip = SoundManager.GetNextClip();
        source.PlayOneShot(clip);

        if (collision.CompareTag("Note"))
        {
            rb.velocity = collision.transform.GetComponent<Note>().Direction * speed;
        }

        Debug.Log(clip.name);
    }
}
