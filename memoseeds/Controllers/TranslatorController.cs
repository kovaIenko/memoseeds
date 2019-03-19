using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using memoseeds.Database;
using memoseeds.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using memoseeds.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace memoseeds.Controllers
{
    [Authorize]
    [ApiController]
    public class TranslatorController : Controller
    {
        private readonly ITranslatorService translatorService;

        public TranslatorController(ITranslatorService service)
        {
            this.translatorService = service;
        }

        //All languages format should be Full. I.E. English, Ukrainian, Bulgarian
        [HttpPost("/translate")]
        public IActionResult Translate([FromBody]TermToTranslate term)
        {
            string transResponse = translatorService.translate(term.SourceText, term.SourceLanguage, term.TargetLanguage);
            return Json(transResponse);
        }

        public class TermToTranslate
        {
            [Required(ErrorMessage = "Source not specified")]
            public string SourceText { get; set; }

            [Required(ErrorMessage = "Source language not specified")]
            public string SourceLanguage { get; set; }

            [Required(ErrorMessage = "Target language not specified")]
            public string TargetLanguage { get; set; }
        }
    }

}

