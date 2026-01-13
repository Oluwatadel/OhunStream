namespace Dispatcher
{
    public delegate Task<TResponse> RequestHandlerDeligate<TResponse>();

    public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : notnull
    {
        Task<TResponse> Handle(TRequest request, RequestHandlerDeligate<TResponse> next, CancellationToken cancellationToken);
    }

    public delegate Task RequestHandlerDeligate();

    public interface IPipelineBehavior<in TRequest> where TRequest: notnull
    {
        Task Handle(TRequest request, RequestHandlerDeligate next, CancellationToken cancellationToken);
    }
}
