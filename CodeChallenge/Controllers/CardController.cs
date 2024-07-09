using CodeChallenge.Models;
using CodeChallenge.Services.Interface;
using CodeChallenge.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        //I'm not going to write comments for all of this because quite frankly it's very simple and they seem
        //unnecessary. key points: I used a GET when it's returning a card, and a POST when it's updating the 
        //deck. I added a "RebuildFull" endpoint. You can read the comments/why in the CardService.cs file
        private readonly ICardService _cardService; 
        
        public CardController(ICardService cardServices)
        {
            _cardService = cardServices;
        }

        [HttpGet("dealcard")]
        public ActionResult<Card> DealCard()
        {
            return Ok(_cardService.DealCard());
        }

        [HttpPost("shuffle")]
        public ActionResult<string> Shuffle()
        {
            return Ok(_cardService.Shuffle());
        }

        [HttpPost("discard")]
        public ActionResult<string> Discard([FromBody] Card card)
        {
            return Ok(_cardService.Discard(card));
        }

        [HttpPost("cut")]
        public ActionResult<string> Cut([FromQuery] int cutspot)
        {
            return Ok(_cardService.Cut(cutspot));
        }

        [HttpPost("order")]
        public ActionResult<string> Order()
        {
            return Ok(_cardService.Order());
        }

        [HttpPost("rebuilddeck")]
        public ActionResult<string> RebuildDeck()
        {
            return Ok(_cardService.RebuildDeck());
        }

        [HttpPost("rebuildfull")]
        public ActionResult<string> RebuildDeckFull()
        {
            return Ok(_cardService.RebuildDeckFull());
        }

        [HttpGet("cheat")]
        public ActionResult<Card> Cheat()
        {
            return Ok(_cardService.Cheat());
        }
    }
}