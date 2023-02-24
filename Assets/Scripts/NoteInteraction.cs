using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInteraction : MonoBehaviour
{
    [SerializeField] private SpriteRenderer innerCircle;
    [SerializeField] private SpriteRenderer outerArcs;

    public void Setup(Vector3 pos, Color color)
    {
        transform.position = pos;
        transform.Rotate(0, 0, Random.Range(0f, 360f));

        innerCircle.color = color;
        outerArcs.color = color;
    }
}
