using LanchesMac.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LanchesMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminImagensController : Controller
    {
        private readonly ConfigurationImagens _myConfig;
        private readonly IWebHostEnvironment _hostingEnviroment;

        public AdminImagensController(IWebHostEnvironment hostingEnviroment, IOptions<ConfigurationImagens> myConfiguration)
        {
            _hostingEnviroment = hostingEnviroment;
            _myConfig = myConfiguration.Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> UploadFiles(List<IFormFile> files)
        {
            if(files == null || files.Count == 0)
            {

            }
            if(files.Count > 10)
            {
                ViewData["Erro"] = "Error: Quantidade de arquivo excedeu o limite!";
                return View(ViewData);
            }

            //tamanho em bytes da imagem
            long size = files.Sum(f => f.Length);

            //amazenar o nome das imagens
            var filePathsName = new List<string>();

            //caminho completo do local de onde as imagens serão armazenada
            //WebRootPath contém o caminho físico da pasta wwwroot
            var filePath = Path.Combine(_hostingEnviroment.WebRootPath, _myConfig.NomePastaImagensProdutos);
            foreach (var formFile in files)
            {
                if (formFile.FileName.Contains(".jpg") || formFile.FileName.Contains(".png") || formFile.FileName.Contains(".gif"))
                {
                    //concatena o local do arquivo com o nome do arquivo
                    var fileNameWithPath = string.Concat(filePath, "\\", formFile.FileName);
                    filePathsName.Add(fileNameWithPath);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                }
            }
            ViewData["Resultado"] = $"{files.Count} arquivos foram enviados ao servidor" +
                                    $"Com tamanho total de: {size} bytes";

            ViewBag.Arquivos = filePathsName;

            return View(ViewData);
        }
        public IActionResult GetImagens()
        {
            FileManagerModel model = new FileManagerModel();
            var userImagesPath = Path.Combine(_hostingEnviroment.WebRootPath, _myConfig.NomePastaImagensProdutos);

            DirectoryInfo dir = new DirectoryInfo(userImagesPath);

            FileInfo[] files = dir.GetFiles();

            model.PathImagesProduto = _myConfig.NomePastaImagensProdutos;

            if(files.Length == 0)
            {
                ViewData["Erro"] = $"nenhum arquivo encontrada na pasta {userImagesPath}";

            }
            model.Files = files;
            return View(model);
            
        }
        public IActionResult Deletefile(string fname)
        {
            string _imagemDeleta = Path.Combine(_hostingEnviroment.WebRootPath, _myConfig.NomePastaImagensProdutos + "\\" + fname );
            if(System.IO.File.Exists(_imagemDeleta))
            {
                System.IO.File.Delete(_imagemDeleta);
                ViewData["Deletado"] = $"Arquivo(s) {_imagemDeleta} deletaddo com sucesso";
            }
            return View("index");

        }        
    }
}
