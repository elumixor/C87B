namespace Combo {
    /// <summary>
    /// Specifies how the player should execute <see cref="ComboFrame"/>
    /// </summary>
    public enum ComboFrameType {
        /// <summary>
        /// Items can be executed in free order
        /// </summary>
        Unordered,
        /// <summary>
        /// Items should be executed in specified order
        /// </summary>
        Ordered,
        /// <summary>
        /// Items should be executed simultaneously, buttons
        /// </summary>
        SimultaneousButton,
        /// <summary>
        /// Items should be executed simultaneously, sliders
        /// </summary>
        SimultaneousSlider
    }
}