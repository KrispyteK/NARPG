using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StandardLevels", menuName = "Levels/StandardLevels")]
class StandardLevels : ScriptableObject {
    public List<TextAsset> levels = new List<TextAsset>();
}
