using UnityEngine;

namespace UI.TestedModules
{
    public interface IScrollItem
    {
        /// <summary>
        /// Returns the object in which the given item is located
        /// </summary>
        public GameObject GetContentView();
    }
}