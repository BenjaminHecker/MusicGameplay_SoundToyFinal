using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Note;

public class Editor : MonoBehaviour
{
    [SerializeField] private Note prefab_Note;
    [SerializeField] private SpriteRenderer hoverSprite;

    [System.Serializable]
    public struct NoteColor
    {
        public NoteType type;
        public Color color;
    }
    public NoteColor[] noteColors;

    private NoteType type = 0;

    private Dictionary<Vector3, Note> placedNotes = new Dictionary<Vector3, Note>();

    private bool running = false;

    private void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        hoverSprite.transform.position = pos;

        if (!running)
        {
            if (Input.GetMouseButton(0))
                PlaceNote(pos);
            if (Input.GetMouseButton(1))
                RemoveNote(pos);

            if (Input.GetKeyUp(KeyCode.R))
                hoverSprite.transform.Rotate(0, 0, -90f);
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
            type = (NoteType) 0;
        if (Input.GetKeyUp(KeyCode.Alpha2))
            type = (NoteType) 1;
        hoverSprite.color = GetNoteColor();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (running)
                ResetNotes();
            else
                RunNotes();

            running = !running;

            hoverSprite.enabled = !running;
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            running = false;

            foreach (var note in placedNotes)
                Destroy(note.Value.gameObject);
            
            placedNotes.Clear();
        }
    }

    private void ResetNotes()
    {
        foreach (var note in placedNotes)
            note.Value.ResetNote();
    }

    private void RunNotes()
    {
        foreach (var note in placedNotes)
            note.Value.Run();
    }

    private void PlaceNote(Vector3 pos)
    {
        Vector3 snapPos = GetSnapPos(pos);

        RemoveNote(pos);

        Note note = Instantiate(prefab_Note);
        note.Setup(snapPos, hoverSprite.transform.rotation, type, GetNoteColor());

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

    private Color GetNoteColor()
    {
        foreach (NoteColor noteColor in noteColors)
            if (noteColor.type == type)
                return noteColor.color;

        return Color.white;
    }
}
