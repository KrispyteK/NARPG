using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsole : MonoBehaviour {

    public GUIStyle style;

    void Start() {
        DontDestroyOnLoad(gameObject);

        Application.logMessageReceived += Application_logMessageReceived;

        var otherConsoles = FindObjectsOfType<DebugConsole>();

        if (otherConsoles.Length > 2) {
            foreach (var console in otherConsoles) {
                if (console != this) Destroy(console.gameObject);
            }
        }
    }

    private List<LogMessage> logMessages = new List<LogMessage>();

    struct LogMessage {
        public string condition;
        public string stackTrace;
        public LogType type;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type) {
        logMessages.Add(new LogMessage {
            condition = condition,
            stackTrace = stackTrace,
            type = type
        });

        int count = Mathf.Min(logMessages.Count, 20);

        logMessages = logMessages.GetRange(logMessages.Count - count, count);
    }

    void OnGUI () {
        GUILayout.BeginVertical(style);

        foreach (var msg in logMessages) {

            Color color;

            switch (msg.type) {
                case LogType.Exception:
                case LogType.Error:
                    color = Color.red;
                    break;
                case LogType.Warning:
                    color = Color.yellow;
                    break;
                default:
                    color = Color.white;
                    break;
            }

            style.normal.textColor = color;

            var message = msg.condition;

            if (msg.type == LogType.Exception) {
                message += "\n" + msg.stackTrace;
            }

            GUILayout.Box(message, style);
        }

        GUILayout.EndVertical();
    }
}
