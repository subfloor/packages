using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//trigger
namespace Subfloor.Dtos
{
    public class ServiceResult
    {
        public ServiceResult() { }

        public ServiceResult(bool success)
        {
            Success = success;
            Message = String.Empty;
        }

        public ServiceResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
