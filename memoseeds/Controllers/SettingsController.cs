 using System;
using System.ComponentModel.DataAnnotations;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{
    [ApiController]
    [Route("settings")]
    public class SettingsController : Controller
    {
        private IUserRepository UserRepository;

        public SettingsController(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }

        [HttpPost("{id}/change/email")]
        public IActionResult UpdateEmail([FromRoute] long id, [FromBody] Data data)
        {
            IActionResult response = Unauthorized();
            if (ModelState.IsValid)
            {
                User entity = UserRepository.GetById(id);
                if (entity != null)
                {
                    if(entity.Email.Equals(data.Old))
                          entity.Email = data.New;
                    User updated = UserRepository.Update(entity);
                    response = Ok( new { UpdatedUser = updated });
                }
            }
            return response;
        }


        [HttpPost("{id}/change/password")]
        public IActionResult UpdatePassword([FromRoute] long id, [FromBody] Data data)
        {
            IActionResult response = Unauthorized();
            if (ModelState.IsValid)
            {
                User entity = UserRepository.GetById(id);
                if (entity != null)
                {
                    if (entity.Password.Equals(data.Old))
                        entity.Password = data.New;
                    User updated = UserRepository.Update(entity);
                    response = Ok(new { UpdatedUser = updated });
                }
            }
            return response;
        }

        [HttpPost("{id}/change/username")]
        public IActionResult UpdateUsername([FromRoute] long id, [FromBody] Data data)
        {
            IActionResult response = Unauthorized();
            if (ModelState.IsValid)
            {
                User entity = UserRepository.GetById(id);
                if (entity != null)
                {
                    if (entity.Username.Equals(data.Old))
                        entity.Username = data.New;
                    User updated = UserRepository.Update(entity);
                    response = Ok(new { UpdatedUser = updated });
                }
            }
            return response;
        }


        public class Data
        {
            [Required(ErrorMessage = "Not specified")]
            public string Old { get; set; }

            [Required(ErrorMessage = "Not specified")]
            public string New { get; set; }
        }
    }
}
