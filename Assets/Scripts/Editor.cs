using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{
    [SerializeField] private Note prefab_Note;
    [SerializeField] private SpriteRenderer hoverSprite;

    private Dictionary<Vector3, Note> placedNotes = new Dictionary<Vector3, Note>();

    private bool running = false;

    private void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        hoverSprite.transform.position = pos;

        if (Input.GetMouseButtonUp(0))
            PlaceNote(pos);
        if (Input.GetMouseButtonUp(1))
            RemoveNote(pos);

        if (Input.GetKeyUp(KeyCode.R))
            hoverSprite.transform.Rotate(0, 0, -90f);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (var note in placedNotes)
            {
                if (running)
                    note.Value.ResetNote();
                else
                    note.Value.Run();
            }

            running = !running;
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            foreach (var note in placedNotes)
                Destroy(note.Value.gameObject);
            
            placedNotes.Clear();
        }
    }

    private void PlaceNote(Vector3 pos)
    {
        Vector3 snapPos = GetSnapPos(pos);

        RemoveNote(pos);

        Note note = Instantiate(prefab_Note);
        note.transform.position = snapPos;
        note.transform.rotation = hoverSprite.transform.rotation;

        placedNotes[snapPos] = note;
    }

    private void RemoveNote(Vector3 pos)
    {
        Vector3 snapPos = GetSnapPos(pos);

        foreach (var note in placedNotes)
        {
            if ((note.Key - snapPos).magnitude <= 0.2f)
            {
                Destroy(note.Value.gameObject);
                placedNotes.Remove(note.Key);
                return;
            }
        }
    }

    private Vector2 GetSnapPos(Vector3 pos)
    {
        return new Vector2(Mathf.Floor(pos.x) + 0.5f, Mathf.Floor(pos.y) + 0.5f);
    }
}
