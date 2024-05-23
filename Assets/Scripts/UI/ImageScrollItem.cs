using UI.TestedModules;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Returns the ContentView and Scrollbar of the current Item
    /// </summary>
    public class ImageScrollItem : MonoBehaviour, IScrollItem
    {
        public GameObject GetContentView() => transform.parent.gameObject;
    }
}