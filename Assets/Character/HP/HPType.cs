namespace Character.HP {
    /// <summary>
    /// Ship's health types. Lower the value, closer to be destroyed first.
    /// <para>
    ///     Determines ordering of various levels of hit points
    /// </para>
    /// </summary>
    public enum HPType {
        Shields,
        Armor
    }
}