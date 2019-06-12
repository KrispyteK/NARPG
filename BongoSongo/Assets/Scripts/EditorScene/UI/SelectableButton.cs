using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : Button {

    public bool IsSelected => pool.selected == this;

    public class SelectableButtonPool {
        public SelectableButton selected;
        public Button button;
    }

    public SelectableButtonPool pool;


    public void AddToPool(SelectableButtonPool newPool) {
        pool = newPool;
    }

    public bool IsSelectedInPool() {
        return pool?.selected == this;
    }

    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);

        if (pool != null) {
            if (pool.selected == null) {
                pool.selected = this;
            } else if (pool.selected != this && pool.selected != null) {
                var previouslySelected = pool.selected;

                pool.selected = this;

                previouslySelected.OnDeselect(eventData);
            }
        }
    }

    public override void OnDeselect(BaseEventData eventData) {
        base.OnDeselect(eventData);

        if (pool != null) {
            if (pool.selected == this) {
                this.OnSelect(eventData);
            }
        }
    }
}
