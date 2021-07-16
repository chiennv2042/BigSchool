using BigSchoolFull.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchoolFull.Controllers
{
    public class FollowingController : ApiController
    {
        public class FollowingsController : ApiController
        {
            BigSchoolContext con = new BigSchoolContext();

            [HttpPost]
            public IHttpActionResult Follow(Following follow)
            {
                var userID = User.Identity.GetUserId();
                if (userID == null)

                    return BadRequest("Please login first!");
                if (userID == follow.FolloweeId)
                    return BadRequest("Can not follow myself!");

                Following find = con.Followings.FirstOrDefault(p => p.FollowerId == userID
                && p.FolloweeId == follow.FolloweeId);
                if (find != null)

                {
                    con.Followings.Remove(con.Followings.SingleOrDefault(p =>
                    p.FollowerId == userID && p.FolloweeId == follow.FolloweeId));
                    con.SaveChanges();
                    return Ok("cancel");
                }
                follow.FollowerId = userID;
                con.Followings.Add(follow);
                con.SaveChanges();
                return Ok();
            }

        }
    }
}