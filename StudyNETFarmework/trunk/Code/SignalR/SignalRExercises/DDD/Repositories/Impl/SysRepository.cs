using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SignalRExercises.DDD.Entities;

namespace SignalRExercises.DDD.Repositories.Impl
{
    public class SysRepository : DbContext, ISysRepository
    {
        public SysRepository(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public IDbSet<User> Users { get; set; }

        public IDbSet<Question> Questions { get; set; }

        public IDbSet<QuestionOption> QuestionOptions { get; set; }

        public IDbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }

        public IDbSet<Questionnaire> Questionnaires { get; set; }

        bool IBaseRepository.ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            return base.ShouldValidateEntity(entityEntry);
        }

        DbEntityValidationResult IBaseRepository.ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            return base.ValidateEntity(entityEntry, items);
        }
    }
}