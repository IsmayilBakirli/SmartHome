namespace SmartHome.Application.Common
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public Object? Data { get; set; } 
        public ApiResponse(int code,string message,Object? data=null)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
