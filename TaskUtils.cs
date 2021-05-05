using System;
using System.Threading;
using System.Threading.Tasks;

namespace Program
{
    internal static class TaskUtils
    {
        internal static async Task WaitUntil<T>(T @object, Func<T, bool> func, int backoffTime, TimeSpan? maxWaitTime = null)
        {
            var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            using (var cancellationTokenSource = maxWaitTime.HasValue ? new CancellationTokenSource(maxWaitTime.Value) : new CancellationTokenSource())
            {
                cancellationTokenSource.Token.Register(() =>
                {
                    tcs.SetException(new TimeoutException());
                    tcs.TrySetCanceled();
                });

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        if (!func(@object))
                        {
                            await Task.Delay(backoffTime);
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }

                    tcs.SetResult(0);
                    break;
                }

                await tcs.Task;
            }
        }
    }
}
