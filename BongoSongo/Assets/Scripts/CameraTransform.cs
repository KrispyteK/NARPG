using UnityEngine;

public class CameraTransform {
    public static Vector3 ScreenPointToWorld (Vector2 position)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(
            position.x * Camera.main.pixelWidth,
            position.y * Camera.main.aspect * Camera.main.pixelHeight,
            -Camera.main.transform.position.z
        ));
    }

    public static Vector3 Scale(Vector3 scale) {
        var amount = Camera.main.orthographicSize / Camera.main.aspect;

        return scale * amount;
    }
}
