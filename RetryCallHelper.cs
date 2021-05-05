using System;
using System.Threading;
using System.Threading.Tasks;

namespace Program
{
    public static class RetryCallHelper
    {
        public static async Task RetryCall(Action retriableCall, CancellationToken token, ILogger logger)
        {
            Task<int> WrappingFunc()
            {
                retriableCall();
                return Task.FromResult(1);
            }

            await RetryCall(WrappingFunc, token, logger);
        }

        public static async Task RetryCall(Func<Task> retriableCall, CancellationToken token, ILogger logger)
        {
            async Task<int> WrappingFunc()
            {
                await retriableCall();
                return 1;
            }

            await RetryCall(WrappingFunc, token, logger);
        }

        public static async Task<T> RetryCall<T>(Func<Task<T>> retriableCall, CancellationToken token, ILogger logger)
        {
            var backPressureDelay = TimeSpan.FromSeconds(1);
            while (true)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    return await retriableCall();
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    if (backPressureDelay > TimeSpan.FromSeconds(4))
                    {
                        if (logger?.IsWarningEnabled ?? false)
                            logger.TraceWarning(e, "Maximal retries exceeded.");

                        throw;
                    }

                    if (logger?.IsWarningEnabled ?? false)
                        logger.TraceWarning(e, $"Call failed, trying again in {backPressureDelay.TotalMilliseconds} ms.");

                    await Task.Delay(backPressureDelay, token);
                    backPressureDelay += backPressureDelay;
                }
            }
        }
    }
}
