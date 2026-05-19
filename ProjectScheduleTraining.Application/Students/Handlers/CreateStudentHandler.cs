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

            var student = new Student
            {
                Name = request.Name.Trim(),
                Cpf = cpf,
                Email = request.Email.Trim().ToLower(),
                Phone = request.Phone.Trim(),
                BirthDate = request.BirthDate,
                Address = request.Address?.Trim(),
                EmergencyContact = request.EmergencyContact?.Trim(),
                Status = Domain.Enums.StudentStatus.Active,
                StartDate = DateTime.UtcNow
            };

            await _unitOfWork.Students.AddAsync(student, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return StudentMapper.ToResponse(student);
        }
    }
}
