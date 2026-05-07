using System.Net;

namespace luxe.Server.Application.DTOs
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public T? Data { get; set; }
    }
}
