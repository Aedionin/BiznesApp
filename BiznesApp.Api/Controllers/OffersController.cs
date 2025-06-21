using BiznesApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiznesApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private static List<Offer> _offers = new List<Offer>
        {
            new Offer { Id = 1, Name = "Strona internetowa dla firmy X", Description = "Stworzenie nowoczesnej strony WWW.", Price = 5000, Status = "Wysłana" },
            new Offer { Id = 2, Name = "Aplikacja mobilna dla sklepu", Description = "Projekt i wdrożenie aplikacji na Android/iOS.", Price = 25000, Status = "Zaakceptowana" },
            new Offer { Id = 3, Name = "Optymalizacja SEO", Description = "Audyt i pozycjonowanie serwisu.", Price = 2000, Status = "Odrzucona" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Offer>> GetOffers()
        {
            return Ok(_offers);
        }

        [HttpPost]
        public ActionResult<Offer> AddOffer([FromBody] Offer offer)
        {
            offer.Id = _offers.Any() ? _offers.Max(o => o.Id) + 1 : 1;
            _offers.Add(offer);
            return CreatedAtAction(nameof(GetOffers), new { id = offer.Id }, offer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOffer(int id, [FromBody] Offer updatedOffer)
        {
            var offer = _offers.FirstOrDefault(o => o.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            offer.Name = updatedOffer.Name;
            offer.Description = updatedOffer.Description;
            offer.Price = updatedOffer.Price;
            offer.Status = updatedOffer.Status;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOffer(int id)
        {
            var offer = _offers.FirstOrDefault(o => o.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            _offers.Remove(offer);
            return NoContent();
        }
    }
} 