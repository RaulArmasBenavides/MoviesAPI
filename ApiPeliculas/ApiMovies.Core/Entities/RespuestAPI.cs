using System.Net;

namespace ApiMovies.Core.Entities
{
    public class RespuestAPI
    {
        public RespuestAPI()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
