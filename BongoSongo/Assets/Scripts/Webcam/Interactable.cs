using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    [Range(0.01f, 1)] public float size = 0.25f;
    public float threshold = 0.8f;
    public bool isActive = true;
    public bool isInteractedWith = false;
    public bool isCircular;
    public bool measureAverage = false;
    [Range(1, 25)] public int density = 10;

    public Vector3 ScreenPosition => Camera.main.WorldToScreenPoint(transform.position);

    private float circularThreshold;
    private float interactionAmount;

    void OnValidate() {
        circularThreshold = threshold / Mathf.PI;
    }

    void Update()
    {
        if (!WebcamMotionCapture.instance.hasWebcam) return;

        isInteractedWith = CheckInteraction();
    }

    private float GetCircularOffset (float height, float y) {
        if (!isCircular) return 0;

        return (1 - Mathf.Pow(Mathf.Sin((y - height) / size * Mathf.PI + Mathf.PI/2),0.5f)) * size / 2;
    }

    private bool CheckInteraction ()
    {
        interactionAmount = 0f;
        var stepSize = 1f / density * size;
        var points = Mathf.Pow((size / stepSize), 2f);

        // Get normalized X and Y coordinates of object on the screen where the left of the screen is 0 and the right is 1.
        var screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        var normalizedX = screenPoint.x / Camera.main.pixelWidth;
        var normalizedY = screenPoint.y / Camera.main.pixelHeight;
        var aspect = Camera.main.aspect;

        normalizedY /= aspect;

        for (float y = normalizedY - size / 2 + stepSize / 2; y <= normalizedY + size / 2; y += stepSize) {
            for (float x = normalizedX - size / 2 + stepSize / 2 + GetCircularOffset(normalizedY, y); x <= normalizedX + size / 2 - GetCircularOffset(normalizedY, y); x += stepSize) {
                // Add gray scale value at the point on the motion texture thats behind the interactable.
                interactionAmount += WebcamMotionCapture.instance.texture.GetPixelBilinear(1-x, y * aspect).grayscale;

                if (measureAverage) interactionAmount /= points;

                if (interactionAmount > (isCircular ? circularThreshold : threshold)) return true;
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

        for (float y = normalizedY - size / 2 + stepSize / 2; y <= normalizedY + size / 2; y += stepSize) {
            for (float x = normalizedX - size / 2 + stepSize / 2 + GetCircularOffset(normalizedY, y); x <= normalizedX + size / 2 - GetCircularOffset(normalizedY, y); x += stepSize) {
                if (isCircular) x = Mathf.Round(x / stepSize) * stepSize;

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
