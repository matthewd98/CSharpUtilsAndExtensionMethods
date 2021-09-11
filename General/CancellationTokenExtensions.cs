using System.Threading;
using System.Threading.Tasks;

namespace CSharpUtilsAndExtensionMethods.General
{
    internal static class CancellationTokenExtensions
    {
        // Wait async until token is cancelled
        internal static Task WaitAsync(this CancellationToken cancellationToken)
        {
            var cancelationTaskCompletionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            cancellationToken.Register(
                (taskCompletionSource) => ((TaskCompletionSource<bool>)taskCompletionSource!).SetResult(true),
                cancelationTaskCompletionSource);

            return cancellationToken.IsCancellationRequested
                ? Task.CompletedTask
                : cancelationTaskCompletionSource.Task;
        }
    }
}
