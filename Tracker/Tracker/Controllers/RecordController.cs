using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tracker.Models;


namespace Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public RecordController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Record
        [HttpGet]
        public IEnumerable<Record> GetRecords()
        {
            return _context.Records;
        }

        // GET: api/Record/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecord([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = await _context.Records.FindAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        // POST: api/Record
        [HttpPost]
        public async Task<IActionResult> PostRecord([FromBody] Record record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _context.Records.Add(record);
            User user = record.Owner;
            user.Records.Add(record);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Record/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteRecord([FromRoute] string id)
        {
            var record = await _context.Records.FindAsync(id);
            if (record == null)
            {
                return false;
            }

            User user = record.Owner;
            user.Records.Remove(record);
            _context.Records.Remove(record);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}