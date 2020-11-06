using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KernelCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        public UploadController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }


        [HttpPost]
        public async Task Post([FromQuery] string registrationNumber)
        {
            string uniqueFileName = null;
            string filePath = registrationNumber;

            if (HttpContext.Request.Form.Files.Any())
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {

                   

                            ////string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, @"C:\Users\dzime\Source\Repos\Zimendm\KernelCars\wwwroot\AccountingDocuments\");
                            //string uploadFolder = Path.Combine(environment.WebRootPath, "AccountingDocuments");
                            //uploadFolder = Path.Combine(uploadFolder, registrationNumber);

                            //try
                            //{
                            //    if (!Directory.Exists(uploadFolder))
                            //    {
                            //        DirectoryInfo di = Directory.CreateDirectory(uploadFolder);
                            //    }

                            //}
                            //catch (Exception)
                            //{

                            //    //throw;
                            //}



                            //uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
                            //filePath = Path.Combine(uploadFolder, uniqueFileName);

                            ////using (var stream = new FileStream(filePath, FileMode.Create))
                            ////{
                            ////    model.DocumentScan.CopyTo(stream);
                            ////}

                            ////string z1 = (string)RouteData.Values["id"];
                        



























                    //var path = Path.Combine(environment.ContentRootPath, "uploads", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
        }
    }
}
