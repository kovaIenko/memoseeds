using System;
using System.Collections.Generic;
namespace memoseeds.Services
{
    public interface ITranslatorService
    {
        string translate (string sourceText, string sourceLanguage, string targetLanguage);
    }
}
