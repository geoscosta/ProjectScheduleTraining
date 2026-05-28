using MediatR;
using ProjectScheduleTraining.Application.ParQAssessments.Commands;
using ProjectScheduleTraining.Application.ParQAssessments.DTOs;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ParQAssessments.Handlers
{
    /// <summary>
    /// Handler responsável por registrar a avaliação PAR-Q de um aluno.
    /// Determina automaticamente se o aluno está liberado para atividades físicas.
    /// </summary>
    public class CreateParQAssessmentHandler
        : IRequestHandler<CreateParQAssessmentCommand, ParQAssessmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateParQAssessmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o registro da avaliação PAR-Q.
        /// O aluno é liberado apenas se todas as respostas forem negativas.
        /// Qualquer resposta positiva indica necessidade de avaliação médica.
        /// </summary>
        public async Task<ParQAssessmentResponse> Handle(
            CreateParQAssessmentCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            /// Determina se o aluno está liberado para atividade física.
            /// Qualquer resposta "Sim" indica risco e requer avaliação médica.
            var isCleared = !request.Question1 &&
                            !request.Question2 &&
                            !request.Question3 &&
                            !request.Question4 &&
                            !request.Question5 &&
                            !request.Question6 &&
                            !request.Question7;

            var assessment = new ParQAssessment
            {
                StudentId = request.StudentId,
                AssessmentDate = DateTime.UtcNow,
                Question1 = request.Question1,
                Question2 = request.Question2,
                Question3 = request.Question3,
                Question4 = request.Question4,
                Question5 = request.Question5,
                Question6 = request.Question6,
                Question7 = request.Question7,
                IsCleared = isCleared,
                Notes = request.Notes
            };

            await _unitOfWork.ParQAssessments.AddAsync(assessment, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return MapToResponse(assessment);
        }

        private static ParQAssessmentResponse MapToResponse(ParQAssessment assessment)
            => new(
                assessment.Id,
                assessment.StudentId,
                assessment.AssessmentDate,
                assessment.Question1,
                assessment.Question2,
                assessment.Question3,
                assessment.Question4,
                assessment.Question5,
                assessment.Question6,
                assessment.Question7,
                assessment.IsCleared,
                assessment.Notes,
                assessment.CreatedAt);
    }
}
