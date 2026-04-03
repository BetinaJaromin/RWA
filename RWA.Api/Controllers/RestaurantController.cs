using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA.Api.Entities;
using RWA.Api.Models;
using RWA.Api.Services.Interfaces;
using System.Security.Claims;

namespace RWA.Api.Controllers
{
    [Route("api/restaurant")]
    [ApiController]  //atrybut walidacji modelu [!ModelState.IsValid]
    [Authorize]      //wymaga nagłówka Authorization: Bearer {token} dla wszystkich zapytań do tego kontrolera
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
        {
            _restaurantService.Update(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            var id = _restaurantService.Create(dto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        [AllowAnonymous]//[Authorize(Policy = "CreatedAtleast2Restaurant")] 
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery] RestaurantQuery query)
        {
            var restaurantDtos = _restaurantService.GetAll(query);

            return Ok(restaurantDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]  //pozwala na zapytania bez nagłówka
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);

            return Ok(restaurant);
        }
    }
}
