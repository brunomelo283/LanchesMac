namespace LanchesMac.Models
{
    public class FileManagerModel
    {
        public FileInfo[] Files { get; set; }
        public IFormFile IFormFile { get; set; }

        public List<IFormFile> IFormFIles { get; set; }
        public string PathImagesProduto { get; set; }
    }
}
