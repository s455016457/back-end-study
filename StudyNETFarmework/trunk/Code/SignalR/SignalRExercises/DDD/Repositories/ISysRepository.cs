using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;

namespace SignalRExercises.DDD.Repositories
{
    public interface ISysRepository: IBaseRepository
    {
        IDbSet<Entities.User> Users { get; }
        IDbSet<Entities.Question> Questions { get; }
        IDbSet<Entities.QuestionOption> QuestionOptions { get; }
        IDbSet<Entities.QuestionnaireQuestion> QuestionnaireQuestions { get; }
        IDbSet<Entities.Questionnaire> Questionnaires { get; }
    }
}
