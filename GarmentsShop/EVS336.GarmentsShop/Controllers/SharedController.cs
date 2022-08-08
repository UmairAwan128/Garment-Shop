using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EVS336.GarmentsShop.Controllers
{
    public class SharedController : Controller
    {
        //follwing is a Controler which is going to Contain all the Common Code/Action that more then 
        //one Controler may contain e.g UploadImage() to Server What we Did in Category-> create View what code does is
        //   when the user will select a file/image for category instantly we will send the image to Server to save it
        //    and once its safed we will show the image to the user and till now user didn, t pressed the Save Button.
     
            // Post: Shared
            //this method will save the Image on Server and Send Back the Path of that Image
            public JsonResult UploadImage()
            {
                JsonResult result = new JsonResult(); //first created JasonResult() obj as in this we will send our Data
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet; //set its behaviour i.e on Get request call this method or send Data..

                try
                {
                    //Request will have the FormData object we send using the JScript code we write in create.cshtml of Category Controller
                    var file = Request.Files[0]; //so Request.Files[0] will give the first file of the FormData we sent

                    //getting the Name of the File so that file will be saved with this Name in Server 
                    //var fileName = file.FileName;
                    //but problem is more then one file can have same Name so if another User upload a file with SameName as  
                    //this so this old file will be replaced with the new so the old file will be deleted so now we will assign
                    //each file a unique Name own our own using Guid is a struct when its instantiated i will randomly generate
                    //a Name/string and it will be unique always then we are getting the Extension of the file and concatenating
                    //with this Name so finally  a unique file Name and also Complicated/Unguessable fileName
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    //creating the path where the file will be saved and assigning it the Name we get above     
                    var path = Path.Combine(Server.MapPath("~/DataImages/products/"), fileName);
                    file.SaveAs(path);//storing the File on the server i.e here in images folder in content folder

                    //now as image is saved now sending the path of the Image back to Site but don,t send "path" here in ImageURL
                    //as this is complete path i.e c/user/umair/project/images/file.png for security issues and also browser don,t allow such pathes 
                    //so we will send it path without ~ also and used {0} its like %s and against this the value is fileName.. 
                    //i.e from server and will be shown to the User/Client
                    result.Data = new { Success = true, ImageURL = string.Format("/DataImages/products/{0}", fileName) };
                
                }
                catch (Exception ex)
                {
                    result.Data = new { Success = false, Message = ex.Message };
                }

                return result;
            }
    }
}