namespace Shared.Behaviours.Progressable {
    /// <summary>
    /// Changes its appearance according to progress
    /// </summary>
    public interface IProgressable {
        float Progress { get; set; }
    }
}