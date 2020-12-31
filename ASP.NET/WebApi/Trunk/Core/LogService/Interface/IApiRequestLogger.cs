using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace LoggerService.Interface
{
    public interface IApiRequestLogger
    {
        void Write(ApiRequestLogMessage message);
    }
}
