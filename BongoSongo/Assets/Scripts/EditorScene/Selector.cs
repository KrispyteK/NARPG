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

    }

    public virtual void Delete() {
        Destroy(gameObject);
    }
}
