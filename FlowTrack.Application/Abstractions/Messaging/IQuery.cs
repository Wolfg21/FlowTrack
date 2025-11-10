using FlowTrack.Domain.Abstractions;
using MediatR;

namespace FlowTrack.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}