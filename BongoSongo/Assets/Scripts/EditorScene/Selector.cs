using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {
    public bool isSelectable = true;

    public virtual void Select () {

    }

    public virtual void Unselect() {

    }

    public virtual void CreateNew() {
        EditorManager.instance.CreateNewEditor();
    }

    public virtual void Delete() {
        Destroy(gameObject);
    }

    public virtual void OnToolsActive(RectTransform toolsUI) {
        
    }
}
