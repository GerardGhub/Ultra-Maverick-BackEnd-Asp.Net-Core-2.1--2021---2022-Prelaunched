using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;

namespace MvcTaskManager.Controllers
{
    public class ClientLocationsController : Controller
    {
        private ApplicationDbContext db;

        public ClientLocationsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("api/clientlocations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            List<ClientLocation> clientLocations = await db.ClientLocations.ToListAsync();
            return Ok(clientLocations);
        }

        [HttpGet]
        [Route("api/clientlocations/searchbyclientlocationid/{ClientLocationID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetByClientLocationID(int ClientLocationID)
        {
            ClientLocation clientLocation = await db.ClientLocations.Where(temp => temp.ClientLocationID == ClientLocationID).FirstOrDefaultAsync();
            if (clientLocation != null)
            {
                return Ok(clientLocation);
            }
            else
                return NoContent();
        }

        [HttpPost]
        [Route("api/clientlocations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ClientLocation> Post([FromBody] ClientLocation clientLocation)
        {
            db.ClientLocations.Add(clientLocation);
            await db.SaveChangesAsync();

            ClientLocation existingClientLocation = await db.ClientLocations.Where(temp => temp.ClientLocationID == clientLocation.ClientLocationID).FirstOrDefaultAsync();
            return clientLocation;
        }

        [HttpPut]
        [Route("api/clientlocations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ClientLocation> Put([FromBody] ClientLocation project)
        {
            ClientLocation existingClientLocation = await db.ClientLocations.Where(temp => temp.ClientLocationID == project.ClientLocationID).FirstOrDefaultAsync();
            if (existingClientLocation != null)
            {
                existingClientLocation.ClientLocationName = project.ClientLocationName;
                await db.SaveChangesAsync();
                return existingClientLocation;
            }
            else
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("api/clientlocations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<int> Delete(int ClientLocationID)
        {
            ClientLocation existingClientLocation = await db.ClientLocations.Where(temp => temp.ClientLocationID == ClientLocationID).FirstOrDefaultAsync();
            if (existingClientLocation != null)
            {
                db.ClientLocations.Remove(existingClientLocation);
               await db.SaveChangesAsync();
                return ClientLocationID;
            }
            else
            {
                return -1;
            }
        }
    }
}


