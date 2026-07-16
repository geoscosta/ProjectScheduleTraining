using MediatR;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.Application.ScheduleLocks.Mappers;
using ProjectScheduleTraining.Application.ScheduleLocks.Queries;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Handlers;

/// <summary>
/// Handler responsável por buscar trancamentos de agenda.
/// </summary>
public class GetScheduleLockHandler
    : IRequestHandler<GetScheduleLocksByStudentIdQuery, IEnumerable<ScheduleLockResponse>>,
      IRequestHandler<GetActiveScheduleLockQuery, ScheduleLockResponse?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetScheduleLockHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ScheduleLockResponse>> Handle(
        GetScheduleLocksByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        var locks = await _unitOfWork.ScheduleLocks
            .GetByStudentIdAsync(request.StudentId, cancellationToken);

        return locks.Select(ScheduleLockMapper.ToResponse);
    }

    public async Task<ScheduleLockResponse?> Handle(
        GetActiveScheduleLockQuery request,
        CancellationToken cancellationToken)
    {
        var scheduleLock = await _unitOfWork.ScheduleLocks
            .GetActiveLockByStudentIdAsync(request.StudentId, cancellationToken);

        return scheduleLock is null ? null : ScheduleLockMapper.ToResponse(scheduleLock);
    }
}
