using Domain.Aggregate.ErySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository.Interface.ErySystem
{
    public interface IEmailRepository : IBaseRepository
    {
        EmailAggregate FindEmail(int NId);

        void AddEmail(EmailAggregate email);

        void UpdateEmail(EmailAggregate email);

        void DeleteEmail(EmailAggregate email);
    }
}
