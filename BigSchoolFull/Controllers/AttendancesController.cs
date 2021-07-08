using BigSchoolFull.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace BigSchoolFull.Controllers
{
    public class AttendancesController : ApiController
    {
        [System.Web.Http.HttpPost]
        public IHttpActionResult Attend(Course attendanceDto)
        {
            var userID = User.Identity.GetUserId();
            BigSchoolContext con = new BigSchoolContext();
            if (con.Attendances.Any(p => p.Attendee == userID && p.CourseId == attendanceDto.Id))
            {
                return BadRequest("The attendance already exists!");
            }
            var attendance = new Attendance() { CourseId = attendanceDto.Id, Attendee = User.Identity.GetUserId() };
            con.Attendances.Add(attendance);
            con.SaveChanges();
            return Ok();
        }

        
    }
}
