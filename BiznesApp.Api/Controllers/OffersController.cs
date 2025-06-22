using BiznesApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiznesApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OffersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offer>>> GetOffers()
        {
            return await _context.Offers.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Offer>> AddOffer([FromBody] Offer offer)
        {
            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOffers), new { id = offer.Id }, offer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOffer(int id, [FromBody] Offer updatedOffer)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            offer.Name = updatedOffer.Name;
            offer.Description = updatedOffer.Description;
            offer.Price = updatedOffer.Price;
            offer.Status = updatedOffer.Status;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 