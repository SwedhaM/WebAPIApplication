namespace WebAPI.Repository.Abstract
{
    public interface IFileService
    {
        public Tuple<int, string> saveImage(IFormFile imageFile);
        public bool deleteImage(string imageFileName);
    }
}
