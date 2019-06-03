using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsole : MonoBehaviour {

    public GUIStyle style;

    void Start() {
        DontDestroyOnLoad(gameObject);

        Application.logMessageReceived += Application_logMessageReceived;
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
    }

    void OnGUI () {
        GUILayout.BeginVertical(style);

        foreach (var msg in logMessages) {

            Color color;

            switch (msg.type) {
                case LogType.Error:
                    color = Color.red;
                    break;
                case LogType.Warning:
                    color = Color.yellow;
                    break;
                default:
                    color = Color.green;
                    break;
            }

            style.normal.textColor = color;

            GUILayout.Box(msg.condition,style);
        }

        GUILayout.EndVertical();
    }
}
