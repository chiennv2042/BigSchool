using BigSchoolFull.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace BigSchoolFull.Controllers
{
    public class AttendancesController : ApiController
    {
        BigSchoolContext con = new BigSchoolContext();
        [System.Web.Mvc.HttpPost]
        public IHttpActionResult Attend(Course attendanceDto)
        {
            var userID = User.Identity.GetUserId();

            if (con.Attendances.Any(p => p.Attendee == userID && p.CourseId ==
            attendanceDto.Id))
            {
                con.Attendances.Remove(con.Attendances.SingleOrDefault(p =>
                p.Attendee == userID && p.CourseId == attendanceDto.Id));
                con.SaveChanges();
                return Ok("cancel");
            }
            var attendance = new Attendance()
            {
                CourseId = attendanceDto.Id,
                Attendee =
                User.Identity.GetUserId()
            };
            con.Attendances.Add(attendance);
            con.SaveChanges();
            return Ok();
        }


    }
}
