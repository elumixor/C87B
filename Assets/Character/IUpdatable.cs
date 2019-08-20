namespace Character {
    public delegate void UpdateHandler<in T>(T updateValue);

    public interface IUpdatable<out T> {
        event UpdateHandler<T> Update;
    }
}