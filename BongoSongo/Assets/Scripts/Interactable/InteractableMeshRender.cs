using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMeshRender : MonoBehaviour
{

    public Transform mesh;
    private Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        SetScale();

        mesh.GetComponent<Renderer>().material.color = interactable.isInteractedWith ? Color.green : Color.white;
    }

    void SetScale () {
        mesh.localScale = CameraTransform.Scale(Vector3.one * interactable.size / 2);
    }
}
