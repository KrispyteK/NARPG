using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : Button {

    public bool IsSelected => pool.selected == this;

    public class SelectableButtonPool {
        public SelectableButton selected;
    }

    private SelectableButtonPool pool;

    public void AddToPool(SelectableButtonPool newPool) {
        pool = newPool;
    }

    public bool IsSelectedInPool() {
        return pool?.selected == this;
    }

    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);

        if (pool != null) {
            pool.selected = this;
        }
    }
}
