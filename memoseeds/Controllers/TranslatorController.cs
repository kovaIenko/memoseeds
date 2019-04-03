using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using memoseeds.Services;
using System;

namespace memoseeds.Controllers
{
   // [Authorize]
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
            IActionResult response = Unauthorized();
            try
            {
                string transResponse = translatorService.translate(term.SourceText, term.SourceLanguage, term.TargetLanguage);
                response = Ok(new { transResponse });
            }catch(Exception e)
            {
                response = Ok(new { e });
            }
            return response;
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

