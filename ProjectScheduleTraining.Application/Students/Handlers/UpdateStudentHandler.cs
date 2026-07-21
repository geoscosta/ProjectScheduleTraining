using MediatR;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Application.Students.Mappers;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Students.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de atualização de um aluno existente.
    /// Valida a existência do aluno antes de persistir as alterações.
    /// </summary>
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, StudentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de atualização do aluno.
        /// Lança exceção caso o aluno não seja encontrado.
        /// </summary>
        public async Task<StudentResponse> Handle(
            UpdateStudentCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.Id, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            student.Name = request.Name.Trim();
            student.Email = request.Email.Trim().ToLower();
            student.Phone = request.Phone.Trim();
            student.BirthDate = request.BirthDate;
            student.Profession = request.Profession?.Trim();
            student.MaritalStatus = request.MaritalStatus;
            student.IdentityDocument = request.IdentityDocument?.Trim();
            student.Street = request.Street?.Trim();
            student.AddressNumber = request.AddressNumber?.Trim();
            student.Complement = request.Complement?.Trim();
            student.District = request.District?.Trim();
            student.City = request.City?.Trim();
            student.State = request.State?.Trim();
            student.ZipCode = request.ZipCode?.Trim();
            student.GuardianName = request.GuardianName?.Trim();
            student.GuardianCpf = request.GuardianCpf?.Trim();
            student.EmergencyContact = request.EmergencyContact?.Trim();
            student.ImageRightsAccepted = request.ImageRightsAccepted;
            student.InternalRegulationAccepted = request.InternalRegulationAccepted;

            _unitOfWork.Students.Update(student);
            await _unitOfWork.CommitAsync(cancellationToken);

            return StudentMapper.ToResponse(student);
        }
    }
}
