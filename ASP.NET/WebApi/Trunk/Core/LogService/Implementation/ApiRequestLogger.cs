using LoggerService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService.Implementation
{
    public class ApiRequestLogger : IApiRequestLogger
    {
        private static String _connectionString;

        public ApiRequestLogger(String connectionString)
        {
            _connectionString = connectionString;
        }

        public void Write(ApiRequestLogMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
