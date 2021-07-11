using BigSchoolFull;
using BigSchoolFull.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BigSchool.Controllers
{
    public class CoursesController : Controller
    {

        // GET: Courses
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.ListCategory = context.Categories.ToList();
            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            BigSchoolContext con = new BigSchoolContext();

            ModelState.Remove("LecturerID");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = con.Categories.ToList();
                return View("Create", objCourse);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            con.Courses.Add(objCourse);
            con.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigSchoolContext con = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var ListAttendances = con.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in ListAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LecturerName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            BigSchoolContext con = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var courses = con.Courses.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();

            foreach (Course i in courses)
            {
                i.LecturerName = currentUser.Name;
            }
            return View(courses);
        }
        public ActionResult Edit(int? id)
        {
            BigSchoolContext con = new BigSchoolContext();
            Course course = con.Courses.Find(id);
            course.ListCategory = con.Categories.ToList();
            if (id == null)
            {
                return HttpNotFound();
            }
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course objcourse)
        {
            BigSchoolContext con = new BigSchoolContext();
            if (!ModelState.IsValid)
            {
                objcourse.ListCategory = con.Categories.ToList();
                return View("Edit", objcourse);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objcourse.LecturerId = user.Id;

            con.Courses.AddOrUpdate(objcourse);
            con.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int? id)
        {
            BigSchoolContext con = new BigSchoolContext();
            Course course = con.Courses.Find(id);
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            BigSchoolContext con = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Course course = con.Courses.Find(id);
            Attendance attendance = con.Attendances.Find(id, currentUser.Id);
            con.Attendances.Remove(attendance);
            con.SaveChanges();
            con.Courses.Remove(course);
            con.SaveChanges();
            return RedirectToAction("Mine", "Courses");
        }
    }

}
