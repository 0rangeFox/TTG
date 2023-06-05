namespace TTG_Server.Managers; 

public class TaskManager : IDisposable {

    private readonly CancellationTokenSource _networkCancellationTokenSource = new();

    public readonly List<Task> Tasks = new();

    public bool IsNetworkCancelled => this._networkCancellationTokenSource.Token.IsCancellationRequested;

    public void Dispose() {
        foreach (var task in this.Tasks)
            task.Dispose();

        this._networkCancellationTokenSource.Cancel();

        GC.SuppressFinalize(this);
    }

}