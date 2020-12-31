using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOperatorInterpreter
{
    public interface IUserOperatorExepression
    {
        void Interpreter(UserOperatorInterpreterContext context);
    }
}
