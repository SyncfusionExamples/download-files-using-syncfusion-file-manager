using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Syncfusion.EJ2.FileManager.Base;
using Syncfusion.EJ2.FileManager.AzureFileProvider;
using Microsoft.Extensions.Configuration;

namespace BlazorApp1.Controllers
{
    [Route("api/[controller]")]
    public class DefaultController : Controller
    {
        public AzureFileProvider operation;
        private string domain;
        private string blobName;
        private string parentFolder;
        private string azureKey;
        private string devName;
       
        [Obsolete]
        public DefaultController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            domain = configuration.GetSection("AzureBlobs").GetSection("Domain").Value;
            blobName = configuration.GetSection("AzureBlobs").GetSection("BlobName").Value;
            parentFolder = configuration.GetSection("AzureBlobs").GetSection("ParentFolder").Value;
            azureKey = configuration.GetSection("AzureBlobs").GetSection("AzureKey").Value;
            devName = configuration.GetSection("AzureBlobs").GetSection("DevName").Value;
           
            this.operation = new AzureFileProvider();
            this.operation.RegisterAzure(devName, azureKey, blobName);
            this.operation.SetBlobContainer(domain + blobName + "/", domain + blobName + "/" + parentFolder);
        }
        [Route("AzureFileOperations")]
        public object AzureFileOperations([FromBody] FileManagerDirectoryContent args)
        {
            string ClientName = this.HttpContext.Request.Headers["Client_Name"].ToString();
            
            if (args.Path != "")
            {
                //set the dynamic blob container based on selected client name
                this.operation.SetBlobContainer(domain + blobName + "/", domain + blobName + "/" + parentFolder + "/" + ClientName);
                
                string startPath = domain + "/" + blobName + "/";
                string originalPath = (domain + "/" + blobName + "/" + parentFolder + "/" + ClientName).Replace(startPath, "");

                args.Path = (originalPath + "/" + args.Path).Replace("//", "/");
                args.TargetPath = (originalPath + args.TargetPath).Replace("//", "/");
            }
            switch (args.Action)
            {
                case "read":
                    // Reads the file(s) or folder(s) from the given path.
                    return Json(this.ToCamelCase(this.operation.GetFiles(args.Path, args.ShowHiddenItems, args.Data)));
                case "delete":
                    // Deletes the selected file(s) or folder(s) from the given path.
                    return this.ToCamelCase(this.operation.Delete(args.Path, args.Names, args.Data));
                case "details":
                    // Gets the details of the selected file(s) or folder(s).
                    return this.ToCamelCase(this.operation.Details(args.Path, args.Names, args.Data));
                case "create":
                    // Creates a new folder in a given path.
                    return this.ToCamelCase(this.operation.Create(args.Path, args.Name, args.Data));
                case "search":
                    // Gets the list of file(s) or folder(s) from a given path based on the searched key string.
                    return this.ToCamelCase(this.operation.Search(args.Path, args.SearchString, args.ShowHiddenItems, args.CaseSensitive, args.Data));
                case "rename":
                    // Renames a file or folder.
                    return this.ToCamelCase(this.operation.Rename(args.Path, args.Name, args.NewName, false, args.Data));
                case "copy":
                    // Copies the selected file(s) or folder(s) from a path and then pastes them into a given target path.
                    return this.ToCamelCase(this.operation.Copy(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData, args.Data));
                case "move":
                    // Cuts the selected file(s) or folder(s) from a path and then pastes them into a given target path.
                    return this.ToCamelCase(this.operation.Move(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData, args.Data));
            }
            return null;
        }
        public string ToCamelCase(object userData)
        {
            return JsonConvert.SerializeObject(userData, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
        }

        // Uploads the file(s) into a specified path
        [Route("AzureUpload")]
        public ActionResult AzureUpload(FileManagerDirectoryContent args)
        {
            string ClientName = this.HttpContext.Request.Headers["Client_Name"].ToString();
           
            if (args.Path != "")
            {
                //set the dynamic blob container based on selected client name
                this.operation.SetBlobContainer(domain + blobName + "/", domain + blobName + "/" + parentFolder + "/" + ClientName);
                string startPath = domain + "/" + blobName + "/";
                string originalPath = (domain + "/" + blobName + "/" + parentFolder + "/" + ClientName).Replace(startPath, "");
                args.Path = (originalPath + args.Path).Replace("//", "/");
            }
            FileManagerResponse uploadResponse = operation.Upload(args.Path, args.UploadFiles, args.Action, args.Data);
            if (uploadResponse.Error != null)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = Convert.ToInt32(uploadResponse.Error.Code);
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = uploadResponse.Error.Message;
            }
            return Json("");
        }

        // Downloads the selected file(s) and folder(s)
        [Route("AzureDownload")]
        public object AzureDownload(string downloadInput)
        {
            FileManagerDirectoryContent args = JsonConvert.DeserializeObject<FileManagerDirectoryContent>(downloadInput);
            string ClientName = args.Data[0].NewName;
            //set the dynamic blob container based on selected client name
            this.operation.SetBlobContainer(domain + blobName + "/", domain + blobName + "/" + parentFolder + "/" + ClientName);

            return operation.Download(args.Path, args.Names, args.Data);
        }

        // Gets the image(s) from the given path
        [Route("AzureGetImage")]
        public async Task<IActionResult> AzureGetImage(FileManagerDirectoryContent args)
        {
            //set the dynamic blob container based on selected client name
            this.operation.SetBlobContainer(domain + blobName + "/", domain + blobName + "/" + parentFolder + "/" + args.Name);
            
            return await this.operation.GetImageAsync(args.Path, args.Id, true, null, args.Data);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}