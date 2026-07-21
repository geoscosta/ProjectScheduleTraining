using MediatR;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Application.Students.Mappers;
using ProjectScheduleTraining.CrossCutting.Helpers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Students.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de criação de um novo aluno.
    /// Valida duplicidade de CPF e persiste o aluno no banco de dados.
    /// </summary>
    public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, StudentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de criação do aluno.
        /// Verifica se o CPF já está cadastrado antes de persistir.
        /// </summary>
        public async Task<StudentResponse> Handle(
            CreateStudentCommand request,
            CancellationToken cancellationToken)
        {
            var cpf = CpfHelper.Clean(request.Cpf);

            var cpfExists = await _unitOfWork.Students
                .CpfExistsAsync(cpf, cancellationToken: cancellationToken);

            if (cpfExists)
                throw new DomainException(
                    "Já existe um aluno cadastrado com esse CPF.",
                    "STUDENT_CPF_ALREADY_EXISTS");

            /// Valida se o aluno é menor de idade e exige responsável.
            var age = DateTime.UtcNow.Year - request.BirthDate.Year;
            if (age < 18 && string.IsNullOrWhiteSpace(request.GuardianName))
                throw new DomainException(
                    "Alunos menores de 18 anos devem ter um responsável cadastrado.",
                    "STUDENT_GUARDIAN_REQUIRED");

            var student = new Student
            {
                Name = request.Name.Trim(),
                Cpf = cpf,
                Email = request.Email.Trim().ToLower(),
                Phone = request.Phone.Trim(),
                BirthDate = request.BirthDate,
                Profession = request.Profession?.Trim(),
                MaritalStatus = request.MaritalStatus,
                IdentityDocument = request.IdentityDocument?.Trim(),
                Street = request.Street?.Trim(),
                AddressNumber = request.AddressNumber?.Trim(),
                Complement = request.Complement?.Trim(),
                District = request.District?.Trim(),
                City = request.City?.Trim(),
                State = request.State?.Trim(),
                ZipCode = request.ZipCode?.Trim(),
                GuardianName = request.GuardianName?.Trim(),
                GuardianCpf = request.GuardianCpf?.Trim(),
                EmergencyContact = request.EmergencyContact?.Trim(),
                ImageRightsAccepted = request.ImageRightsAccepted,
                InternalRegulationAccepted = request.InternalRegulationAccepted,
                Status = Domain.Enums.StudentStatus.Active,
                StartDate = DateTime.UtcNow
            };

            await _unitOfWork.Students.AddAsync(student, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return StudentMapper.ToResponse(student);
        }
    }
}
