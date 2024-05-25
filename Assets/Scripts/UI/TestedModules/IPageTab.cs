namespace UI.TestedModules
{
    /// <summary>
    /// Interface for interaction and logic of page tabs, switched between each other using arrow buttons
    /// </summary>
    public interface IPageTab
    {
        /// <summary>
        /// Implements the creation of animation of moving objects on the page, as well as other necessary logic. 
        /// </summary>
        /// <param name="arrowDirection">Direction arrow button</param>
        public void NavigationButtonPressed(ArrowDirection arrowDirection);
    }
    
    public enum ArrowDirection
    {
        LeftArrow,
        RightArrow
    }
}