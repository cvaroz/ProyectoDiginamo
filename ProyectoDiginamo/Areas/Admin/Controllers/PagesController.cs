using ProyectoDiginamo.Models.Data;
using ProyectoDiginamo.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoDiginamo.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare list of PageN
            List<PageVM> pageList;

            
            using (Db db= new Db())
            {
                //Init the list
                pageList = db.Pages.ToList().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();

            }

            //Return the view with list


            return View(pageList);
        }
        

        
        // Get: Admin/Pages/addPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/addPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {

            //Chesck the state
            if (! ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {



                //Declare Slug
                string slug;

                //Initialaze pageDTO
                PageDTO dto = new PageDTO();

                //DTO title
                dto.Title = model.Title;

                //Check for and set slug if need be
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make sure title and slug are unique
                if (db.Pages.Any( x => x.Title == model.Title ) || db.Pages.Any(x => x.Slug == slug) )
                {

                    ModelState.AddModelError("", " That tilte or slug already exists");
                    return View(model);
                }

                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                //Save the DTO
                db.Pages.Add(dto);
                db.SaveChanges();

 }
            //Set TempData message
            TempData["SM"] = "You have added a new page!!";

            //Redirect

            return RedirectToAction("AddPage");
        }

        // Get: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declere the pageVM
            PageVM model;

           
            using (Db db = new Db())
            {
                //get the page
                PageDTO dto = db.Pages.Find(id);

                //confirm page exist
                if (dto==null)
                {
                    return Content("The page does not exist.");
                }

                //Init pageVM
                model = new PageVM(dto);



            }

            //Return view with model

            return View(model);
        }

        // POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //check model
            if (! ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {

                //Get Page Id
                int id = model.Id;

                //Declare slug
                String slug = "home";

                //Get the Page
                PageDTO dto = db.Pages.Find(id);

                //Dto the title
                dto.Title = model.Title;

                //check for slug and set it if need be
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();

                    }
                }

                //Make sure the title and slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exist");
                    return View(model);
                }

                //DTO the result
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                //save DTO
                db.SaveChanges();

            }
            //Set TemData message
            TempData["SM"] = "You have edited the page!!";

            //Redirect
            return RedirectToAction("EditPage");
        }

        // Get: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {

            //Declare the pageVm
            PageVM model;

            
            using (Db db = new Db())
            {
                //Get the Page
                PageDTO dto = db.Pages.Find(id);

                //Comfirm page exists
                if (dto == null)
                {
                    return Content("The page does not exist.");
                }
               
                    //Init PageMV
                    model = new PageVM(dto);

            }
            //Return View with model
            return View(model);
        }

        // Get: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {

            using (Db db = new Db())
            {


                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Remove the page
                db.Pages.Remove(dto);
                db.SaveChanges();

                //Save
                db.SaveChanges();

            }

            //redirect
            return RedirectToAction("Index");
        }

        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {

            using (Db db = new Db())
            {


                //Set intial count
                int count = 1;


                //Declare PageDTO
                PageDTO dto;


                // Set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;

                }


            }
        }

        // Get: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //Declare the model
            SidebarVM model;

            
            using (Db db = new Db())
            {
                //Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                //Init model
                model = new SidebarVM(dto);

            }
            //Return view with model
            return View(model);
        }

        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {

            using (Db db = new Db())
            {

                //Get DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                //DTO body
                dto.Body = model.Body;

                //Save
                db.SaveChanges();
            }
            //Set TempData message
            TempData["SM"] = "You have edited the sidebar!!";

            //Redirect
            return RedirectToAction("EditSidebar");
        }

    }
}