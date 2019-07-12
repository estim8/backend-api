using Microsoft.Extensions.Logging;

namespace Estim8.Backend.Commands.Decorators
{
    public abstract class Decorator<T>
    {
        protected T Decorated { get; }
        protected ILogger<T> Log { get; }

        public Decorator(T decorated, ILogger<T> logger)
        {
            Decorated = decorated;
            Log = logger;
        }
    }
}