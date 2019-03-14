using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public float size = 0.25f;
    public int pixelTheshold = 30;
    public bool isActive = true;
    public bool isInteractedWith = false;

    private Texture2D texture;

    void Update()
    {
        if (!WebcamMotionCapture.instance.hasWebcam) return;

        isInteractedWith = checkInteraction();
    }

    private bool checkInteraction ()
    {
        var texture = WebcamMotionCapture.instance.texture;
        var movement = 0f;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        var normalizedX = screenPoint.x / Camera.main.pixelWidth;
        var normalizedY = 1 - screenPoint.y / Camera.main.pixelHeight;
        var aspect = Camera.main.aspect;

        normalizedY /= aspect;

        for (float x = normalizedX - size / 2; x <= normalizedX + size / 2; x += 0.025f)
        {
            for (float y = normalizedY - size / 2; y <= normalizedY + size / 2; y += 0.025f)
            {
                movement += texture.GetPixelBilinear(x, y * aspect).grayscale;
            }
        }

        print(movement);

        return false;
    }

    private void OnDrawGizmos()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        var normalizedX = screenPoint.x / Camera.main.pixelWidth;
        var normalizedY = 1 - screenPoint.y / Camera.main.pixelHeight;
        var aspect = Camera.main.aspect;

        normalizedY /= aspect;

        for (float x = normalizedX - size / 2; x <= normalizedX + size / 2; x += 0.025f)
        {
            for (float y = normalizedY - size / 2; y <= normalizedY + size / 2; y += 0.025f)
            {
                Gizmos.DrawCube(Camera.main.ScreenToWorldPoint(new Vector3(x * Camera.main.pixelWidth, y * aspect * Camera.main.pixelHeight, 2)), Vector3.one * 0.1f);
            }
        }
    }
}
