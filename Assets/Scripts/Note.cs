using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private int BPM = 20;

    private Rigidbody2D rb;

    private Vector3 originalPos;
    private Vector3 dir;
    private float speed;

    [HideInInspector] public Vector3 Direction { get { return dir; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        transform.position = new Vector2(Mathf.Round(transform.position.x * 2) / 2f, Mathf.Round(transform.position.y * 2) / 2f);

        if (collision.CompareTag("Note"))
        {
            rb.velocity = collision.transform.GetComponent<Note>().Direction * speed;
            SoundManager.PlayNextClip();
        }
        else
            rb.velocity *= -1;
    }
}
