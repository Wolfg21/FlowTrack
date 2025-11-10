using FlowTrack.Domain.Abstractions;
using MediatR;

namespace FlowTrack.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}