namespace Character.Targeting {
    public interface ITargeter {
        ITargetable this[TargetType targetType] { get; }
    }
}