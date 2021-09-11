using System;
using System.Threading;
using System.Threading.Tasks;

namespace Program
{
    internal static class CancellationTokenExtensions
    {      
        internal static Task WaitAsync(this CancellationToken cancellationToken)
        {
            var cancelationTaskCompletionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            cancellationToken.Register(
                (object? taskCompletionSource) => ((TaskCompletionSource<bool>)taskCompletionSource!).SetResult(true),
                cancelationTaskCompletionSource);

            return cancellationToken.IsCancellationRequested
                ? Task.CompletedTask
                : cancelationTaskCompletionSource.Task;
        }
    }
}
