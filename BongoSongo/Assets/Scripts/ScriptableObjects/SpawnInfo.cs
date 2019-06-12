[System.Serializable]
public class SpawnInfo {
    public int beat;
    public int beatLength;
    public int spriteIndex = -1;

    public Indicators indicator;

    public SerializableVector2 position;
    public SerializableVector2[] points;
}