using BiznesApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            return await _context.Offers
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Offer>> AddOffer([FromBody] Offer offer)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            offer.UserId = userId; // Automatycznie przypisz właściciela

            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOffers), new { id = offer.Id }, offer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOffer(int id, [FromBody] Offer updatedOffer)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            // Sprawdź, czy użytkownik jest właścicielem oferty
            if (offer.UserId != userId)
            {
                return Forbid(); // Użytkownik nie ma uprawnień
            }

            offer.Name = updatedOffer.Name;
            offer.Description = updatedOffer.Description;
            offer.Price = updatedOffer.Price;
            offer.Status = updatedOffer.Status;
            offer.Location = updatedOffer.Location;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            // Sprawdź, czy użytkownik jest właścicielem oferty
            if (offer.UserId != userId)
            {
                return Forbid(); // Użytkownik nie ma uprawnień
            }

            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 