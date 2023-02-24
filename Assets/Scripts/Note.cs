using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private NoteInteraction prefab_NoteInteraction;
    [SerializeField] private int BPM = 20;

    public enum NoteType { Keystation, Guitar, Mallet }
    private NoteType type;

    private Rigidbody2D rb;
    private SpriteRenderer sRender;

    private Vector3 originalPos;
    private Vector3 dir;
    private float speed;

    [HideInInspector] public Vector3 Direction { get { return dir; } }
    [HideInInspector] public bool triggered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sRender = GetComponent<SpriteRenderer>();
    }

    public void Setup(Vector3 pos, Quaternion rot, NoteType noteType, Color noteColor)
    {
        transform.position = pos;
        transform.rotation = rot;
        type = noteType;
        sRender.color = noteColor;
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
            Note other = collision.transform.GetComponent<Note>();
            rb.velocity = other.Direction * speed;

            if (!triggered)
            {
                SoundManager.PlayNextClip(type);
                NoteInteraction noteInteraction = Instantiate(prefab_NoteInteraction);
                noteInteraction.Setup(transform.position, sRender.color);
            }

            triggered = true;

            if (other.type == type)
                other.triggered = true;

            Invoke("Untrigger", 60f / BPM / 4f);
        }
        else
            rb.velocity *= -1;
    }

    private void Untrigger()
    {
        triggered = false;
    }
}
