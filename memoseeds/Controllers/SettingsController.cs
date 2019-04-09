using System;
using System.ComponentModel.DataAnnotations;
using memoseeds.Models;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{
    [ApiController]
    //[Authorize]
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
                if (UserRepository.GetUserByEmail(data.New) != null)
                    return Ok(new { Error = "This email is alredy taken, please re-login" });

                User entity = UserRepository.GetById(id);
                if (entity != null)
                {
                    if (entity.Email.Equals(data.Old))
                    {
                        if (!data.Old.Equals(data.New))
                        {
                            entity.Email = data.New;
                            User updated = UserRepository.Update(entity);
                            int count = UserRepository.NumbOfModules(entity.UserId);
                            response = Ok(new { UpdatedUser = updated, CountOfModules = count });
                        }
                        else
                        {
                            response = Ok(new { Error = "Equal Emails" });
                        }
                    }
                    else
                    {
                        response = Ok(new { Error = "This id has another email." });
                    }
                }
                else
                {
                    response = Ok(new { Error = "Id not exist" });
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
                string oldHashed = HashPassword.Encrypt(data.Old);
                    if (entity.Password.Equals(oldHashed))
                    {
                        string newHashed = HashPassword.Encrypt(data.New);
                        if (!oldHashed.Equals(newHashed))
                        {
                            entity.Password = newHashed;
                            User updated = UserRepository.Update(entity);
                            int count = UserRepository.NumbOfModules(entity.UserId);
                            response = Ok(new { UpdatedUser = updated, CountOfModules = count });
                        }
                        else                     
                            response = Ok(new { Error = "Equals Passwords." });                       
                    }
                    else                    
                        response = Ok(new { Error = "This id has another password." });       
                }
                else               
                    response = Ok(new { Error = "Id not exist." });
            }
            return response;
        }

        [HttpPost("{id}/change/username")]
        public IActionResult UpdateUsername([FromRoute] long id, [FromBody] Data data)
        {
            IActionResult response = Unauthorized();
            if (ModelState.IsValid)
            {
                if (UserRepository.GetUserByName(data.New) != null)
                    return Ok(new { Error = "This username is alredy taken." });
                User entity = UserRepository.GetById(id);
                if (entity != null)
                {
                    if (entity.Username.Equals(data.Old))
                    {
                        if (!data.Old.Equals(data.New))
                        {
                            entity.Username = data.New;
                            User updated = UserRepository.Update(entity);
                            int count = UserRepository.NumbOfModules(entity.UserId);
                            response = Ok(new { UpdatedUser = updated, CountOfModules = count });
                        }
                        else response = Ok(new { Error = "Equals names." });
                    }
                    else response = Ok(new { Error = "This id has another username." });
                }
                else response = Ok(new { Error = "Id not exist." });
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
