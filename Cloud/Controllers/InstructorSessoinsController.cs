using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorSessoinsController : ControllerBase
    {
        private List<InstructorSession> instructorSessions = new List<InstructorSession>
        {
            new InstructorSession { Id = 1, InstructorId = 1, SessionId = 101 },
            new InstructorSession { Id = 2, InstructorId = 1, SessionId = 102 },
            new InstructorSession { Id = 3, InstructorId = 2, SessionId = 103 },
        };

        // GET api/<InstructorSessoinsController>/5
        [HttpGet("{instructorId}")]
        public ActionResult<IEnumerable<InstructorSession>> Get(int instructorId)
        {
            var sessions = instructorSessions.Where(s => s.InstructorId == instructorId).ToList();
            return Ok(sessions);
        }

        // POST api/<InstructorSessoinsController>
        [HttpPost]
        public ActionResult Post(InstructorSession instructorSession)
        {
            instructorSession.Id = instructorSessions.Max(s => s.Id) + 1;
            instructorSessions.Add(instructorSession);
            return CreatedAtAction("Get", new { instructorId = instructorSession.InstructorId }, instructorSession);
        }

        // DELETE api/<InstructorSessoinsController>/5
        [HttpDelete("{instructorId}")]
        public ActionResult Delete(int instructorId, [FromBody] List<int> sessionIds)
        {
            var sessionsToRemove = instructorSessions
                .Where(s => s.InstructorId == instructorId && sessionIds.Contains(s.SessionId))
                .ToList();

            if (sessionsToRemove.Count == 0)
            {
                return NotFound("No matching sessions found.");
            }

            instructorSessions.RemoveAll(sessionsToRemove.Contains);
            return Ok("Sessions deleted successfully.");
        }
    }
}
