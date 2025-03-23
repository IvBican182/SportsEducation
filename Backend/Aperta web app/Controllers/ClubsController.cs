using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aperta_web_app.Data;
using AutoMapper;
using Aperta_web_app.Models.Club;
using Aperta_web_app.Contracts;

namespace Aperta_web_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly IClubsRepository _clubsRepository;

        public ClubsController(IMapper mapper, IClubsRepository clubsRepository)
        {
            this._mapper = mapper; 
            this._clubsRepository = clubsRepository;
            
        }


        // GET: api/Clubs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Club>>> GetClubs()
        {
            var clubs = await _clubsRepository.GetAllAsync();
            var clubRecords = _mapper.Map<List<GetClubsDto>>(clubs);
            return Ok(clubRecords);
        }

        // GET: api/Clubs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClubDto>> GetClub(int id)
        {
            var club = await _clubsRepository.GetClubDetails(id);

            if (club == null)
            {
                return NotFound();
            }

            var clubDto = _mapper.Map<ClubDto>(club);

            return Ok(clubDto);
        }

        // PUT: api/Clubs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClub(int id, UpdateClubDto updateClubDto)
        {
            //If provided id is not equal to provided clubs id, throw a bad request
            if (id != updateClubDto.Id)
            {
                return BadRequest();
            }

            //_context.Entry(updateClubDto).State = EntityState.Modified;

            //fetch the club from database
            var club = await _clubsRepository.GetAsync(id);

            //If club is not found throw an error
            if (club == null)
            {  
                return NotFound(); 
            }

            //takes whatever it needs from updateClubDto and updates the club with these values
            //this tells entity framework we changed it to modified and we assigned values from left to right
            _mapper.Map(updateClubDto, club);

            try
            {
                //update changes in database
                await _clubsRepository.UpdateAsync(club);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClubExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Clubs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Club>> PostClub(CreateClubDto createClubDto)
        {
            var club = _mapper.Map<Club>(createClubDto);
            await _clubsRepository.AddAsync(club);
            
            return CreatedAtAction("GetClub", new { id = club.Id }, club);
        }

        [HttpPut("{id}/billingInfo")]

        public async Task<IActionResult> UpdateBillingInfo(int id, [FromBody] bool billingStatus)
        {
            try
            {
                await _clubsRepository.UpdateClubBillingInfo(id, billingStatus);
                return Ok(new { message = "Billing information updated successfully." });
            } 
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to update billing info: {ex.Message}" });
            }
        }

        // DELETE: api/Clubs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            
            await _clubsRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> ClubExists(int id)
        {
            return await _clubsRepository.Exists(id);
        }
    }
}
