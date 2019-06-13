using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Indicators {
    Button,
    Slider
}

[System.Serializable]
public struct EditorPrefab {
    public Indicators indicator;
    public GameObject prefab;
    public Sprite preview;
}

public class EditorManager : MonoBehaviour {
    public static EditorManager instance;

    public EditorPrefab currentPrefab;
    public Selector selected;
    public Level level;
    public Transform indicatorParent;
    public int beat;
    public Text beatText;
    public LevelInfo levelInfo;
    public RectTransform selectedUI;
    public GameObject loadingScreen;
    public TimelineDrawer timelineDrawer;
    public TMPro.TMP_InputField bpmInput;
    public GUIStyle guiStyle;
  

    public List<EditorPrefab> editorPrefabs = new List<EditorPrefab>();

    public float beatLength;
    public int beatsTotal;
    public int indicatorSpriteIndex;
    public bool hasLevelLoaded = false;

    public IndicatorSprites indicatorSprites;

    public Camera canvasCamera;

    public List<Level> levelHistory = new List<Level>();
    public int undoDepth = 0;

    void Awake() {
        instance = this;
    }

    void Start() {
        canvasCamera.enabled = false;

        currentPrefab = editorPrefabs[0];

        indicatorSprites = Resources.Load<IndicatorSprites>("Settings/IndicatorSprites");
    }

    void Update() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete)) {
            DeleteIndicator();
        }

        if (selected) {
            var renderers = selected.GetComponentsInChildren<Renderer>();
            var bounding = new Bounds(renderers.First().bounds.center, Vector2.zero);

            foreach (var renderer in renderers) {
                bounding.Encapsulate(renderer.bounds);
            }

            var center = (Vector2)Camera.main.WorldToScreenPoint(bounding.center) - Camera.main.pixelRect.size / 2;
            var extents = bounding.extents / Camera.main.orthographicSize * Camera.main.pixelHeight;

            selectedUI.localPosition = center;
            selectedUI.sizeDelta = (Vector2)extents + new Vector2(50f, 50f);
        }
        else {
            selectedUI.localPosition = Vector2.zero;
            selectedUI.sizeDelta = Camera.main.pixelRect.size * 2;
        }
    }

    public void SetIndicatorSpriteIndex (int index) {
        var sprite = indicatorSprites.sprites[index];

        var temp = editorPrefabs[0];

        temp.preview = sprite;

        editorPrefabs[0] = temp;

        indicatorSpriteIndex = index;

        selected.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        selected.GetComponent<IndicatorInfo>().spriteIndex = index;

        var indicatorTools = FindObjectsOfType<IndicatorTool>();

        foreach (var indicatorTool in indicatorTools) {
            if (indicatorTool.indicator == Indicators.Button) {
                indicatorTool.GetComponentInChildren<Image>().sprite = sprite;
            }
        }
    }

    public void RegisterLevelHistory () {
        var copy = level.Copy();

        var spawnInfoList = GenerateSpawnInfoList();

        copy.spawnInfo = spawnInfoList;

        if (undoDepth > 0) {
            levelHistory.RemoveRange(levelHistory.Count - 1 - undoDepth, undoDepth);

            undoDepth = 0;
        }

        levelHistory.Add(copy);
    }

    public void Undo () {
        undoDepth++;

        LoadLevel(levelHistory[levelHistory.Count - 1 - undoDepth]);
    }

    public void Redo() {
        if (undoDepth > 0) {
            undoDepth--;

            LoadLevel(levelHistory[levelHistory.Count - 1 - undoDepth]);
        }
    }

    public Coroutine LoadSong() {
        if (level == null) {
            return null;
        }

        var clip = level.song.GenerateClip();

        var timelineCoroutine = StartCoroutine(timelineDrawer.RedrawTimelineCoroutine(clip));

        beatLength = 60f / level.bpm;

        beatsTotal = (int)Mathf.Floor(clip.length / beatLength);

        // SetBeat(1);
        ClampBeat();

        return timelineCoroutine;
    }

    public void ClampBeat () {
        SetBeat(Mathf.Clamp(beat,1, beatsTotal));
    }

    public void IncreaseBeat() {
        SetBeat(beat + 1);

        if (beat >= beatsTotal - 1) {
            SetBeat(1);
        }
    }

    public void DecreaseBeat() {
        SetBeat(beat - 1);

        if (beat <= 0) {
            SetBeat(beatsTotal - 1);
        }
    }

    public void SetBeat(int num) {
        beat = num;

        beatText.text = "Beat: " + beat;

        SetVisible();
    }

    private IndicatorInfo[] GetOrderedIndicators() {
        var infos = new List<IndicatorInfo>();

        foreach (Transform child in indicatorParent) {
            infos.Add(child.GetComponent<IndicatorInfo>());
        }

        var ordered = infos.OrderBy(x => x.beat).ToArray();

        return ordered;
    }

    private void SetVisible() {
        var ordered = GetOrderedIndicators();

        for (int i = 0; i < ordered.Length; i++) {
            ordered[i].gameObject.SetActive(Mathf.Abs(beat - ordered[i].beat) < 8);

            var renderers = ordered[i].gameObject.GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers) {
                var col = renderer.material.color;

                var alpha = (1f - Mathf.Abs(beat - ordered[i].beat) / 8f);

                renderer.material.color = new Color(col.r, col.g, col.b, alpha);
            }
        }
    }

    public void OrderIndicators() {
        var ordered = GetOrderedIndicators();

        for (int i = 0; i < ordered.Length; i++) {
            ordered[i].transform.SetSiblingIndex(i);
        }
    }

    public List<SpawnInfo> GenerateSpawnInfoList() {
        var ordered = GetOrderedIndicators();
        var list = new List<SpawnInfo>();

        for (int i = 0; i < ordered.Length; i++) {
            var spawnInfo = new SpawnInfo {
                beat = ordered[i].beat,
                indicator = ordered[i].indicator,
                beatLength = ordered[i].beatLenght,
                spriteIndex = ordered[i].spriteIndex
            };

            var pos = ordered[i].transform.position / Camera.main.orthographicSize;

            spawnInfo.position.x = pos.x;
            spawnInfo.position.y = pos.y;

            if (ordered[i].CompareTag("SliderEditor")) {
                var sliderHandles = ordered[i].GetComponentInChildren<SliderHandles>();
                var points = new List<SerializableVector2>();

                foreach (Transform child in sliderHandles.handleTransform) {
                    points.Add((Vector2)child.position / Camera.main.orthographicSize);
                }

                spawnInfo.points = points.ToArray();
            }

            list.Add(spawnInfo);
        }

        return list;
    }

    public void Unselect() {
        if (selected) {
            selected.Unselect();

            selected = null;
        }
    }

    public void CreateNewEditor() {
        var instance = Instantiate(currentPrefab.prefab, Vector3.zero, Quaternion.identity, indicatorParent);

        instance.GetComponent<IndicatorInfo>().beat = beat;

        if (instance.GetComponent<IndicatorInfo>().indicator == Indicators.Button) {
            instance.GetComponent<IndicatorInfo>().spriteIndex = indicatorSpriteIndex;
            instance.GetComponentInChildren<SpriteRenderer>().sprite = indicatorSprites.sprites[indicatorSpriteIndex];
        }

        OrderIndicators();
    }

    public void CreateNewIndicator() {
        if (selected) {
            selected.CreateNew();
        }
        else {
            CreateNewEditor();
        }

        RegisterLevelHistory();
    }

    public void DeleteIndicator() {
        if (selected) {
            var indicatorInfo = selected.GetComponent<IndicatorInfo>();

            if (indicatorInfo && level.spawnInfo.Count > 0) {
                level.spawnInfo.RemoveAt(indicatorInfo.spawnInfoIndex);
            }

            selected.Delete();
        }
    }

    public void SetName(TMPro.TMP_InputField inputField) {
        level.name = inputField.text;
    }

    public void SetBPM(TMPro.TMP_InputField inputField) {
        level.bpm = int.Parse(inputField.text);

        LoadSong();
    }

    public void SetBPMInt(int value) {
        level.bpm = value;

        bpmInput.text = "" + value;

        LoadSong();
    }

    public void Save() {
        var spawnInfoList = GenerateSpawnInfoList();

        level.spawnInfo = spawnInfoList;

        Level.Save(level);
    }

    public void Load(string file) {
        levelHistory.Clear();

        loadingScreen.SetActive(true);

        var t = Level.LoadAsync(file, level, (Level l) => {
            level = l;
            LoadLevel(level);
            });
    }

    IEnumerator LoadLevelCoroutine (Level level) {
        foreach (Transform child in indicatorParent) {
            Destroy(child.gameObject);
        }

        levelInfo.SetInfo();

        yield return LoadSong();

        var handlePrefab = Resources.Load<GameObject>("Prefabs/Editor/SliderHandle");

        var time = Time.timeSinceLevelLoad;

        foreach (var spawnInfo in level.spawnInfo) {
            var prefab = editorPrefabs.Find(x => x.indicator == spawnInfo.indicator).prefab;
            var position = new Vector2(spawnInfo.position.x, spawnInfo.position.y) * Camera.main.orthographicSize;

            var instance = Instantiate(prefab, position, Quaternion.identity, indicatorParent);
            var indicatorInfo = instance.GetComponent<IndicatorInfo>();

            indicatorInfo.beat = spawnInfo.beat;
            indicatorInfo.spawnInfoIndex = spawnInfo.beat;
            indicatorInfo.beatLenght = spawnInfo.beatLength;
            indicatorInfo.spriteIndex = spawnInfo.spriteIndex;

            if (instance.CompareTag("Button") && indicatorInfo.spriteIndex > -1) {
                instance.GetComponentInChildren<SpriteRenderer>().sprite = indicatorSprites.sprites[indicatorInfo.spriteIndex];
            }

            if (instance.CompareTag("SliderEditor")) {
                var sliderHandles = instance.GetComponentInChildren<SliderHandles>();

                foreach (Transform child in sliderHandles.handleTransform) {
                    Destroy(child.gameObject);
                }

                foreach (var point in spawnInfo.points) {
                    var newHandle = Instantiate(handlePrefab, sliderHandles.handleTransform);
                    var sliderHandleSelector = newHandle.GetComponent<SliderHandleSelector>();

                    sliderHandleSelector.slider = instance.transform.GetChild(0).gameObject;
                    sliderHandleSelector.sliderHandles = sliderHandles;

                    newHandle.transform.position = new Vector2(point.x, point.y) * Camera.main.orthographicSize;
                }
            }

            if (Time.timeSinceLevelLoad - time > 0.1f) {
                time = Time.timeSinceLevelLoad;

                yield return new WaitForEndOfFrame();
            }
        }

        OrderIndicators();

        SetVisible();

        hasLevelLoaded = true;

        loadingScreen.SetActive(false);
    }

    public void LoadLevel (Level level) {
        loadingScreen.SetActive(true);

        this.level = level;

        StartCoroutine(LoadLevelCoroutine(level));
    }

    public void New() {
        Save();

        level = new Level();

        levelInfo.SetInfo();
    }

    public void Play() {
        Save();

        var interEditor = FindObjectOfType<InterSceneEditorInformation>();

        interEditor.beat = beat;
        interEditor.playAtBeat = false;

        var interScene = InterScene.Instance;

        interScene.level = level;

        SceneManager.LoadScene("GameplayScene");
    }

    public void PlayAtBeat() {
        var interEditor = FindObjectOfType<InterSceneEditorInformation>();

        interEditor.beat = beat;
        interEditor.playAtBeat = true;

        var interScene = InterScene.Instance;

        interScene.level = level;

        SceneManager.LoadScene("GameplayScene");
    }

    public void ExitToMainMenu () {
        Destroy(FindObjectOfType<InterSceneEditorInformation>().gameObject);

        Level.Save(level);

        SceneManager.LoadScene("HomeScreen");
    }

    public void OnGUI() {
        var indicatorInfos = FindObjectsOfType<IndicatorInfo>();

        foreach (var indicatorInfo in indicatorInfos) {
            var root = indicatorInfo.transform;

            while (root.parent != indicatorParent) {
                root = root.parent;          
            }

            var renderers = root.GetComponentsInChildren<Renderer>();
            var bounding = new Bounds(renderers.First().bounds.center, Vector2.zero);

            foreach (var renderer in renderers) {
                bounding.Encapsulate(renderer.bounds);
            }

            var center = (Vector2)Camera.main.WorldToScreenPoint(bounding.center);
            center = new Vector2(center.x, Camera.main.pixelHeight - center.y);

            var extents = bounding.extents / Camera.main.orthographicSize * Camera.main.pixelHeight;


            GUI.Label(new Rect(center - (Vector2)extents / 2, extents), "" + indicatorInfo.beat, guiStyle);
        }

        if (Event.current.type == EventType.Repaint) {
            canvasCamera.Render(); // this will render the new UI
        }
    }
}
