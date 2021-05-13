using System;
using System.Threading;
using System.Threading.Tasks;

namespace Program
{
    public static class TaskUtils
    {
        public static async Task WaitUntil<T>(T @object, Func<T, bool> func, int backoffTime, TimeSpan? maxWaitTime = null)
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

        public static async void ScheduleOnce(Func<Task> func, TimeSpan initialDelay = default)
        {
            if (initialDelay != default)
                await Task.Delay(initialDelay);

            try
            {
                await func();
            }
            catch (Exception e)
            {
                // Log(e);
            }
        }

        public static async void ScheduleOnce(Func<CancellationToken, Task> func, CancellationToken cancellationToken, TimeSpan initialDelay = default)
        {
            try
            {
                if (initialDelay != default)
                    await Task.Delay(initialDelay, cancellationToken);

                await func(cancellationToken);
            }
            catch (TaskCanceledException e)
            {
                // Log(e);
            }
            catch (Exception e)
            {
                // Log(e);
            }
        }

        public static async void ScheduleRepeatedly(Func<Task> func, TimeSpan repeatFrequency, TimeSpan initialDelay = default)
        {
            if (initialDelay != default)
                await Task.Delay(initialDelay);

            while (true)
            {
                var startTime = DateTime.UtcNow;

                try
                {
                    await func();
                }
                catch (Exception e)
                {
                    // Log(e);
                }

                var executionTimeInMs = Convert.ToInt32((DateTime.UtcNow - startTime).TotalMilliseconds);
                var repeatTimeInMs = Convert.ToInt32(repeatFrequency.TotalMilliseconds);

                if (executionTimeInMs > repeatTimeInMs)
                    { /* Log("Execution time exceeded repeat time"); */ }
                else
                    await Task.Delay(repeatTimeInMs - executionTimeInMs);
            }
        }

        public static async void ScheduleRepeatedly(Func<CancellationToken, Task> func, TimeSpan repeatFrequency, CancellationToken cancellationToken, TimeSpan initialDelay = default)
        {
            try
            {
                if (initialDelay != default)
                    await Task.Delay(initialDelay, cancellationToken);

                while (true)
                {
                    var startTime = DateTime.UtcNow;

                    try
                    {
                        await func(cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        // Log(e);
                    }

                    var executionTimeInMs = Convert.ToInt32((DateTime.UtcNow - startTime).TotalMilliseconds);
                    var repeatTimeInMs = Convert.ToInt32(repeatFrequency.TotalMilliseconds);

                    if (executionTimeInMs > repeatTimeInMs)
                        { /* Log("Execution time exceeded repeat time"); */ }
                    else
                        await Task.Delay(repeatTimeInMs - executionTimeInMs, cancellationToken);
                }
            }
            catch (TaskCanceledException e)
            {
                // Log(e);
            }
        }
    }
}