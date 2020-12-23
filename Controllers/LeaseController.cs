using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.IO.Compression;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using KernelCars.Data;
using Microsoft.EntityFrameworkCore;
using KernelCars.Models;
using DocumentFormat.OpenXml.InkML;

namespace KernelCars.Controllers
{
    public class LeaseController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public LeaseController(DataContext ctx, IWebHostEnvironment hostingEnvironment)
        {
            _context = ctx;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {

            return View(new LeaseContract());
            //return View(_context.LeaseContracts.ToList());
            //return RedirectToAction("Test");// View();
        }

        public IActionResult Create(int Id)
        {
            int carId = Id;
            
            var carQuery = _context.Cars
                .Include(c=>c.CarOwner)
                .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
                .Include(c=>c.CarStatuses).ThenInclude(c => c.Unit).ThenInclude(c => c.Firm).ThenInclude(c => c.Employee);
            var car = carQuery.Where(c => c.Id == carId).First();
            LeaseContract leaseContract = new LeaseContract
            {
                Car=car//,
                //ToDate = new DateTime(DateTime.Now.Year, 6, 30)
            };

            ViewBag.ManagerID = KernelCars.Infrastructure.Utils.PopulateManagersDropDownList(_context);

            return View(leaseContract);
        }

        public IActionResult Edit(int? id)
        {
            //var 0carQuery = _context.Cars
            //    .Include(c => c.CarOwner)
            //    .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
            //    .Include(c => c.CarStatuses).ThenInclude(c => c.Unit).ThenInclude(c => c.Firm).ThenInclude(c => c.Employee);

            var leaseContract = _context.LeaseContracts
                .Include(l=>l.Car)
                .ThenInclude(l=>l.CarModel).ThenInclude(l=>l.Manufacturer)
                .Include(l=>l.Car).ThenInclude(l=>l.CarOwner)
                .Where(l=>l.ID==id).FirstOrDefault();

            return View("Create", leaseContract);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LeaseContract model)
        {
            var carQuery = _context.Cars
              .Include(c => c.CarOwner)
              .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
              .Include(c => c.CarStatuses).ThenInclude(c => c.Unit).ThenInclude(c => c.Firm).ThenInclude(c => c.Employee);
            
            model.Car = carQuery.Where(c => c.Id == model.CarId).First();

            model.LeaseAmmount = (decimal)model.LeaseAmmount;

            //CreateLeaseContractArchive(model);

            model.ManagerId = 3;
            //Save document to storage
            //string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "AccountingDocuments");
            //uploadFolder = Path.Combine(uploadFolder, model.Car.RegistrationNumber);

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
            _context.Add(model);
            _context.SaveChanges();

            return RedirectToPage($"/cars/details/{model.CarId}");//  RedirectToAction("Edit", new {id= model.ID } );
            //return View(model);
            ///



            using (MemoryStream mem = new MemoryStream())
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(mem, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {
                    wordDoc.AddMainDocumentPart();
                    // siga a ordem
                    Document doc = new Document();
                    Body body = new Body();

                    // 1 paragrafo
                    Paragraph para = new Paragraph();

                    ParagraphProperties paragraphProperties1 = new ParagraphProperties();
                    ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "Normal" };
                    Justification justification1 = new Justification() { Val = JustificationValues.Center };
                    ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();

                    paragraphProperties1.Append(paragraphStyleId1);
                    paragraphProperties1.Append(justification1);
                    paragraphProperties1.Append(paragraphMarkRunProperties1);

                    Run run = new Run();
                    RunProperties runProperties1 = new RunProperties();

                    Text text = new Text() { Text = "The OpenXML SDK rocks!" };

                    // siga a ordem 
                    run.Append(runProperties1);
                    run.Append(text);
                    para.Append(paragraphProperties1);
                    para.Append(run);

                    // 2 paragrafo
                    Paragraph para2 = new Paragraph();

                    ParagraphProperties paragraphProperties2 = new ParagraphProperties();
                    ParagraphStyleId paragraphStyleId2 = new ParagraphStyleId() { Val = "Normal" };
                    Justification justification2 = new Justification() { Val = JustificationValues.Start };
                    ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();

                    paragraphProperties2.Append(paragraphStyleId2);
                    paragraphProperties2.Append(justification2);
                    paragraphProperties2.Append(paragraphMarkRunProperties2);

                    Run run2 = new Run();
                    RunProperties runProperties3 = new RunProperties();
                    Text text2 = new Text();
                    text2.Text = "Teste aqui";

                    run2.AppendChild(new Break());
                    run2.AppendChild(new Text("Hello"));
                    run2.AppendChild(new Break());
                    run2.AppendChild(new Text("world"));

                    para2.Append(paragraphProperties2);
                    para2.Append(run2);

                    // todos os 2 paragrafos no main body
                    body.Append(para);
                    body.Append(para2);

                    doc.Append(body);

                    wordDoc.MainDocumentPart.Document = doc;

                    wordDoc.Close();
                }
                return File(mem.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "ABC.docx");

            }















        }
        
        private void CreateLeaseContractArchive(LeaseContract contract)
        {
            string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "LeaseContracts");
            string pathLocal = Path.Combine(hostingEnvironment.WebRootPath, "Templates/Lease");
            string destinationFile = null;
            string sourceFile = Path.Combine(pathLocal, "AcceptanceCertificate.docx");

            uploadFolder = Path.Combine(uploadFolder, contract.Car.RegistrationNumber);
            try
            {
                if (!Directory.Exists(uploadFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(uploadFolder);
                }

            }
            catch (Exception)
            {

                //throw;
            }
            destinationFile = Path.Combine(pathLocal, "AcceptanceCertificate.docx");


            var carQuery = _context.Cars
                .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
                .Include(c => c.CarUsers).ThenInclude(c => c.Employee);//.Find((long)2097);
            var car = carQuery.Where(c => c.Id == (long)2097).FirstOrDefault();

            try
            {
                // Create a copy of the template file and open the copy
                //File


                System.IO.File.Copy(sourceFile, destinationFile, true);
                using (WordprocessingDocument document = WordprocessingDocument.Open(destinationFile, true))
                {
                    // Change the document type to Document
                    //document.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);






                    // Find all structured document tags
                    IEnumerable<Text> placeHolders = document.MainDocumentPart.RootElement.Descendants<Text>();

                    foreach (var item in placeHolders)
                    {
                        switch (item.Text)
                        {
                            case "CarOwner":
                                item.Text = car.CarUserForView;
                                break;
                            case "RegistrationNumber":
                                item.Text = car.RegistrationNumber;
                                break;
                            case "FirstRegistrationYear":
                                item.Text = car.FirstRegistrationYear.ToString();
                                break;
                            case "VinNumber":
                                item.Text = car.VinNumber;
                                break;
                            case "Manufacturer":
                                item.Text = car.CarModel.Manufacturer.Name;
                                break;
                            case "Model":
                                item.Text = car.CarModel.Model;
                                break;


                            default:
                                break;
                        }
                        //item.InsertAt(new Text("Hello!"), 0);
                        //item.RemoveAllChildren();
                        //Text txt = new Text("Hello, Word!");
                        //item.AppendChild<Text>(new Text("Hell0, world")); // .Descendants<Text>(). .First().Text = "Hello, world!";

                    }

                    List<SdtBlock> sdtList = document.MainDocumentPart.RootElement.Descendants<SdtBlock>().ToList();

                    //sdtList[0].InnerText = "Changed By Code";

                    // Get the MainPart of the document
                    MainDocumentPart mainPart = document.MainDocumentPart;



                    // Get the Document Settings Part
                    //DocumentSettingsPart documentSettingPart1 = mainPart.DocumentSettingsPart;

                    // Create a new attachedTemplate and specify a relationship ID
                    // AttachedTemplate attachedTemplate1 = new AttachedTemplate() { Id = "relationId1" };

                    // Append the attached template to the DocumentSettingsPart
                    //documentSettingPart1.Settings.Append(attachedTemplate1);

                    // Add an ExternalRelationShip of type AttachedTemplate.
                    // Specify the path of template and the relationship ID
                    //documentSettingPart1.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate", new Uri(sourceFile, UriKind.Absolute), "relationId1");

                    // Save the document
                    mainPart.Document.Save();

                    Console.WriteLine("Document generated at " + destinationFile);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                //Console.WriteLine("\nPress Enter to continue…");
                //Console.ReadLine();
            }







            try
            {
                if (!Directory.Exists(uploadFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(uploadFolder);
                }

                string zipPath = Path.Combine(uploadFolder,"result.zip"); 
                

                ZipFile.CreateFromDirectory(uploadFolder, zipPath);
            }
            catch (Exception)
            {

                //throw;
            }

        }
        
        public async Task<IActionResult> Test()
        {
            string pathLocal = Path.Combine(hostingEnvironment.WebRootPath, "Templates/Lease");

            string destinationFile = Path.Combine(pathLocal, "SampleDocument.docx");
            string sourceFile = Path.Combine(pathLocal, "AcceptanceCertificate.docx");

            var carQuery = _context.Cars
                .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
                .Include(c=>c.CarUsers).ThenInclude(c=>c.Employee);//.Find((long)2097);
            var car = carQuery.Where(c => c.Id == (long)2097).FirstOrDefault();

            try
            {
                // Create a copy of the template file and open the copy
                //File


                System.IO.File.Copy(sourceFile, destinationFile, true);
                using (WordprocessingDocument document = WordprocessingDocument.Open(destinationFile, true))
                {
                    // Change the document type to Document
                    //document.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);






                    // Find all structured document tags
                    IEnumerable<Text> placeHolders = document.MainDocumentPart.RootElement.Descendants<Text>();

                    foreach (var item in placeHolders)
                    {
                        switch (item.Text)
                        {
                            case "CarOwner":
                                item.Text = car.CarUserForView;
                                break;
                            case "RegistrationNumber":
                                item.Text = car.RegistrationNumber;
                                break;
                            case "FirstRegistrationYear":
                                item.Text = car.FirstRegistrationYear.ToString();
                                break;
                            case "VinNumber":
                                item.Text = car.VinNumber;
                                break;
                            case "Manufacturer":
                                item.Text = car.CarModel.Manufacturer.Name;
                                break;
                            case "Model":
                                item.Text = car.CarModel.Model;
                                break;


                            default:
                                break;
                        }
                        //item.InsertAt(new Text("Hello!"), 0);
                        //item.RemoveAllChildren();
                        //Text txt = new Text("Hello, Word!");
                        //item.AppendChild<Text>(new Text("Hell0, world")); // .Descendants<Text>(). .First().Text = "Hello, world!";
                        
                    }

                    List<SdtBlock> sdtList = document.MainDocumentPart.RootElement.Descendants<SdtBlock>().ToList();

                    //sdtList[0].InnerText = "Changed By Code";

                    // Get the MainPart of the document
                    MainDocumentPart mainPart = document.MainDocumentPart;

                    

                    // Get the Document Settings Part
                    //DocumentSettingsPart documentSettingPart1 = mainPart.DocumentSettingsPart;

                    // Create a new attachedTemplate and specify a relationship ID
                    // AttachedTemplate attachedTemplate1 = new AttachedTemplate() { Id = "relationId1" };

                    // Append the attached template to the DocumentSettingsPart
                    //documentSettingPart1.Settings.Append(attachedTemplate1);

                    // Add an ExternalRelationShip of type AttachedTemplate.
                    // Specify the path of template and the relationship ID
                    //documentSettingPart1.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate", new Uri(sourceFile, UriKind.Absolute), "relationId1");

                    // Save the document
                    mainPart.Document.Save();

                    Console.WriteLine("Document generated at " + destinationFile);




                    
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                //Console.WriteLine("\nPress Enter to continue…");
                //Console.ReadLine();
            }
        


        






















            using (MemoryStream mem = new MemoryStream())
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(mem, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {
                    wordDoc.AddMainDocumentPart();
                    // siga a ordem
                    Document doc = new Document();
                    Body body = new Body();

                    // 1 paragrafo
                    Paragraph para = new Paragraph();

                    ParagraphProperties paragraphProperties1 = new ParagraphProperties();
                    ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "Normal" };
                    Justification justification1 = new Justification() { Val = JustificationValues.Center };
                    ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();

                    paragraphProperties1.Append(paragraphStyleId1);
                    paragraphProperties1.Append(justification1);
                    paragraphProperties1.Append(paragraphMarkRunProperties1);

                    Run run = new Run();
                    RunProperties runProperties1 = new RunProperties();

                    Text text = new Text() { Text = "The OpenXML SDK rocks!" };

                    // siga a ordem 
                    run.Append(runProperties1);
                    run.Append(text);
                    para.Append(paragraphProperties1);
                    para.Append(run);

                    // 2 paragrafo
                    Paragraph para2 = new Paragraph();

                    ParagraphProperties paragraphProperties2 = new ParagraphProperties();
                    ParagraphStyleId paragraphStyleId2 = new ParagraphStyleId() { Val = "Normal" };
                    Justification justification2 = new Justification() { Val = JustificationValues.Start };
                    ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();

                    paragraphProperties2.Append(paragraphStyleId2);
                    paragraphProperties2.Append(justification2);
                    paragraphProperties2.Append(paragraphMarkRunProperties2);

                    Run run2 = new Run();
                    RunProperties runProperties3 = new RunProperties();
                    Text text2 = new Text();
                    text2.Text = "Teste aqui";

                    run2.AppendChild(new Break());
                    run2.AppendChild(new Text("Hello"));
                    run2.AppendChild(new Break());
                    run2.AppendChild(new Text("world"));

                    para2.Append(paragraphProperties2);
                    para2.Append(run2);

                    // todos os 2 paragrafos no main body
                    body.Append(para);
                    body.Append(para2);

                    doc.Append(body);

                    wordDoc.MainDocumentPart.Document = doc;

                    wordDoc.Close();
                }
                return File(mem.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "ABC.docx");

            }


             






            








        }

        public async Task<IActionResult> Acts(int? id,int? mode)
        {
            var contractQuery = _context.LeaseContracts
                .Include(l => l.Car).ThenInclude(c=>c.CarModel).ThenInclude(m=>m.Manufacturer)
                .Include(l => l.Manager).ThenInclude(l => l.Employee);
            
            LeaseContract contract = contractQuery.Where(l => l.ID == id).FirstOrDefault();


            string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "LeaseContracts");
            string pathLocal = Path.Combine(hostingEnvironment.WebRootPath, "Templates/Lease");
            string destinationFile = null;
            string sourceFile = Path.Combine(pathLocal, "AcceptanceCertificate.docx");

            uploadFolder = Path.Combine(uploadFolder, contract.Car.RegistrationNumber);
            try
            {
                if (!Directory.Exists(uploadFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(uploadFolder);
                }

            }
            catch (Exception)
            {

                //throw;
            }
            destinationFile = Path.Combine(uploadFolder, "AcceptanceCertificate.docx");


            var carQuery = _context.Cars
                .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
                .Include(c => c.CarUsers).ThenInclude(c => c.Employee);//.Find((long)2097);
            var car = carQuery.Where(c => c.Id == (long)2097).FirstOrDefault();

            try
            {
                // Create a copy of the template file and open the copy
                //File


                System.IO.File.Copy(sourceFile, destinationFile, true);
                using (WordprocessingDocument document = WordprocessingDocument.Open(destinationFile, true))
                {
                    // Change the document type to Document
                    //document.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);






                    // Find all structured document tags
                    IEnumerable<Text> placeHolders = document.MainDocumentPart.RootElement.Descendants<Text>();

                    foreach (var item in placeHolders)
                    {
                        switch (item.Text)
                        {
                            case "CarOwner":
                                item.Text = car.CarUserForView;
                                break;
                            case "TaxNumber":
                                item.Text = car.CarOwner.TaxNumber;
                                break;
                            case "Address":
                                item.Text = car.CarOwner.Address;
                                break;
                            case "RegistrationNumber":
                                item.Text = car.RegistrationNumber;
                                break;
                            case "FirstRegistrationYear":
                                item.Text = car.FirstRegistrationYear.ToString();
                                break;
                            case "VinNumber":
                                item.Text = car.VinNumber;
                                break;
                            case "Manufacturer":
                                item.Text = car.CarModel.Manufacturer.Name;
                                break;
                            case "Model":
                                item.Text = car.CarModel.Model;
                                break;


                            default:
                                break;
                        }
                        //item.InsertAt(new Text("Hello!"), 0);
                        //item.RemoveAllChildren();
                        //Text txt = new Text("Hello, Word!");
                        //item.AppendChild<Text>(new Text("Hell0, world")); // .Descendants<Text>(). .First().Text = "Hello, world!";

                    }

                    List<SdtBlock> sdtList = document.MainDocumentPart.RootElement.Descendants<SdtBlock>().ToList();

                    //sdtList[0].InnerText = "Changed By Code";

                    // Get the MainPart of the document
                    MainDocumentPart mainPart = document.MainDocumentPart;



                    // Get the Document Settings Part
                    //DocumentSettingsPart documentSettingPart1 = mainPart.DocumentSettingsPart;

                    // Create a new attachedTemplate and specify a relationship ID
                    // AttachedTemplate attachedTemplate1 = new AttachedTemplate() { Id = "relationId1" };

                    // Append the attached template to the DocumentSettingsPart
                    //documentSettingPart1.Settings.Append(attachedTemplate1);

                    // Add an ExternalRelationShip of type AttachedTemplate.
                    // Specify the path of template and the relationship ID
                    //documentSettingPart1.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate", new Uri(sourceFile, UriKind.Absolute), "relationId1");

                    // Save the document
                    mainPart.Document.Save();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                //Console.WriteLine("\nPress Enter to continue…");
                //Console.ReadLine();
            }

            try
            {

                string uploadZipFolder = Path.Combine(hostingEnvironment.WebRootPath, @"LeaseContracts\Zips");
                string zipFile = Path.Combine(uploadZipFolder, Guid.NewGuid() + "_"+contract.Car.RegistrationNumber + ".zip");
                //ZipFile zip = new ZipFile();

                //if (!Directory.Exists(uploadFolder))
                //{
                //    DirectoryInfo di = Directory.CreateDirectory(uploadFolder);
                //}

                //string zipPath = Path.Combine(uploadFolder, contract.Car.RegistrationNumber+".zip");


                //ZipFile.CreateFromDirectory(uploadFolder, zipPath);
                //
                ZipFile.CreateFromDirectory(uploadFolder,zipFile);
                //Response.Headers.Add("Content-Disposition", $"inline; filename=\"{zipFile}\"");
                //return File(zipFile, "application/zip", contract.Car.RegistrationNumber + ".zip");


                FileStream fs = System.IO.File.OpenRead(zipFile);
                byte[] data;
                data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();
                System.IO.File.Delete(zipFile);
                return File(data, "application/x-zip-compressed", contract.Car.RegistrationNumber + ".zip");


            }
            catch (Exception ex)
            {

                throw;
            }

            

            using (var ms=new MemoryStream())
            {


            }

            //var memory = new MemoryStream();
            //using ( ZipArchiveEntry zip = new ZipArchiveEntry())
            //{

            //}

            //using (var stream = new FileStream(destinationFile, FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;

            ////GetContentType(filePath)

            //Response.Headers.Add("Content-Disposition", $"inline; filename=\"{destinationFile}\"");






            //try
            //{
            //    if (!Directory.Exists(uploadFolder))
            //    {
            //        DirectoryInfo di = Directory.CreateDirectory(uploadFolder);
            //    }

            //    string zipPath = Path.Combine(uploadFolder, "result.zip");


            //    ZipFile.CreateFromDirectory(uploadFolder, zipPath);
            //}
            //catch (Exception)
            //{

            //    //throw;
            //}

            // return null;
           // return RedirectToAction("Index", "Home");

        }















            //return View();
    }
    }

