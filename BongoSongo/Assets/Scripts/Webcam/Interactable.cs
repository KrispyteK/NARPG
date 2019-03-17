using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    [Range(0.01f, 1)] public float size = 0.25f;
    public float theshold = 0.8f;
    public bool isActive = true;
    public bool isInteractedWith = false;
    public bool circular;
    [Range(1,25)] public int density = 10;

    void Update()
    {
        if (!WebcamMotionCapture.instance.hasWebcam) return;

        isInteractedWith = CheckInteraction();
    }

    private bool CheckInteraction ()
    {
        var movement = 0f;
        var stepSize = 1f / density * size;
        var points = Mathf.Pow((size / stepSize), 2f);
        var texture = WebcamMotionCapture.instance.texture;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        var normalizedX = screenPoint.x / Camera.main.pixelWidth;
        var normalizedY = screenPoint.y / Camera.main.pixelHeight;
        var aspect = Camera.main.aspect;

        normalizedY /= aspect;

        for (float x = normalizedX - size / 2 + stepSize / 2; x <= normalizedX + size / 2; x += stepSize)
        {
            for (float y = normalizedY - size / 2 + stepSize / 2; y <= normalizedY + size / 2; y += stepSize)
            {
                movement += texture.GetPixelBilinear(x, y * aspect).grayscale / points;

                if (movement > theshold) {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        var stepSize = 1f / density * size;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        var normalizedX = screenPoint.x / Camera.main.pixelWidth;
        var normalizedY = screenPoint.y / Camera.main.pixelHeight;
        var aspect = Camera.main.aspect;
        var deltaZ = transform.position.z - Camera.main.transform.position.z;

        normalizedY /= aspect;

        for (float x = normalizedX - size / 2 + stepSize / 2; x <= normalizedX + size / 2; x += stepSize)
        {
            for (float y = normalizedY - size / 2 + stepSize / 2; y <= normalizedY + size / 2; y += stepSize)
            {
                Gizmos.DrawSphere(
                    Camera.main.ScreenToWorldPoint(new Vector3(
                        x * Camera.main.pixelWidth, 
                        y * aspect * Camera.main.pixelHeight,
                        deltaZ
                        )), 0.05f);
            }
        }

        // Top line
        Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3 (
                (normalizedX - size / 2) * Camera.main.pixelWidth,
                (normalizedY + size / 2) * aspect * Camera.main.pixelHeight,
                deltaZ
            )), Camera.main.ScreenToWorldPoint(new Vector3(
                (normalizedX + size / 2) * Camera.main.pixelWidth,
                (normalizedY + size / 2) * aspect * Camera.main.pixelHeight,
                deltaZ
            )));

        // Right line
        Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(
                (normalizedX + size / 2) * Camera.main.pixelWidth,
                (normalizedY + size / 2) * aspect * Camera.main.pixelHeight,
                deltaZ
            )), Camera.main.ScreenToWorldPoint(new Vector3(
                (normalizedX + size / 2) * Camera.main.pixelWidth,
                (normalizedY - size / 2) * aspect * Camera.main.pixelHeight,
                deltaZ
            )));

        // Bottom line
        Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(
                (normalizedX + size / 2) * Camera.main.pixelWidth,
                (normalizedY - size / 2) * aspect * Camera.main.pixelHeight,
                deltaZ
            )), Camera.main.ScreenToWorldPoint(new Vector3(
                (normalizedX - size / 2) * Camera.main.pixelWidth,
                (normalizedY - size / 2) * aspect * Camera.main.pixelHeight,
                deltaZ
            )));

        // Right line
        Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(
                (normalizedX - size / 2) * Camera.main.pixelWidth,
                (normalizedY - size / 2) * aspect * Camera.main.pixelHeight,
                deltaZ
            )), Camera.main.ScreenToWorldPoint(new Vector3(
                (normalizedX - size / 2) * Camera.main.pixelWidth,
                (normalizedY + size / 2) * aspect * Camera.main.pixelHeight,
                deltaZ
            )));
    }
}
