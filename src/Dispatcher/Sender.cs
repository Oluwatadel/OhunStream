using Microsoft.Extensions.DependencyInjection;

namespace Dispatcher
{
    public class Sender(IServiceProvider serviceProvider) : ISender
    {
        public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : IRequest<TResponse>
        {
            ArgumentNullException.ThrowIfNull(request);

            var handler = GetHandler<TRequest, TResponse>();
            var pipelines = GetBehaviors<TRequest, TResponse>(request);

            Task<TResponse> Core()
            {
                return handler.Handle(request, cancellationToken);
            }

            var pipeline = pipelines
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)Core, (next, behavior) =>
                () => behavior.Handle(request, next, cancellationToken));

            return await pipeline();
        }


        public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            ArgumentNullException.ThrowIfNull(request);

            var handler = GetHandler<TRequest>();
            var pipelines = GetBehaviors<TRequest>();

            Task Core()
            {
                return handler.Handle(request, cancellationToken);
            }

            var pipeline = pipelines
                .Reverse()
                .Aggregate((RequestHandlerDelegate)Core, (next, behavior) =>
                () => behavior.Handle(request, next, cancellationToken));
            await pipeline();
        }

        private IRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>() where TRequest : IRequest<TResponse>
        {
            return serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        }

        private IRequestHandler<TRequest> GetHandler<TRequest>() where TRequest : IRequest
        {
            return serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
        }

        private IEnumerable<IPipelineBehavior<TRequest, TResponse>> GetBehaviors<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
        {
            return serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>();
        }

        private IEnumerable<IPipelineBehavior<TRequest>> GetBehaviors<TRequest>() where TRequest : IRequest
        {
            return serviceProvider.GetServices<IPipelineBehavior<TRequest>>();
        }
    }
}
