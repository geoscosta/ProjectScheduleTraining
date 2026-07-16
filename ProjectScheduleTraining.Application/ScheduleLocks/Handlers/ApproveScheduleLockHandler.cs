using MediatR;
using ProjectScheduleTraining.Application.ScheduleLocks.Commands;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.Application.ScheduleLocks.Mappers;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Handlers;

/// <summary>
/// Handler responsável por aprovar um trancamento de agenda.
/// </summary>
public class ApproveScheduleLockHandler
    : IRequestHandler<ApproveScheduleLockCommand, ScheduleLockResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveScheduleLockHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ScheduleLockResponse> Handle(
        ApproveScheduleLockCommand request,
        CancellationToken cancellationToken)
    {
        var scheduleLock = await _unitOfWork.ScheduleLocks
            .GetByIdAsync(request.Id, cancellationToken);

        if (scheduleLock is null)
            throw new DomainException("Trancamento não encontrado.", "SCHEDULE_LOCK_NOT_FOUND");

        if (scheduleLock.IsApproved)
            throw new DomainException(
                "Trancamento já foi aprovado.", "SCHEDULE_LOCK_ALREADY_APPROVED");

        scheduleLock.IsApproved = true;
        _unitOfWork.ScheduleLocks.Update(scheduleLock);
        await _unitOfWork.CommitAsync(cancellationToken);

        return ScheduleLockMapper.ToResponse(scheduleLock);
    }
}
