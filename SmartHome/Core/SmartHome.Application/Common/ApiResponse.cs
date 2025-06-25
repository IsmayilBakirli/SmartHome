using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Common
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public Object? Data { get; set; } 
        public ApiResponse(int code,string message,Object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
