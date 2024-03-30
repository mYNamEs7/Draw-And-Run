using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private HeapMovement heap;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - heap.transform.position;
    }

    void LateUpdate()
    {
        transform.position = heap.transform.position + offset;
    }
}
