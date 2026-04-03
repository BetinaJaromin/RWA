namespace RWA.Api.Models
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }
}
