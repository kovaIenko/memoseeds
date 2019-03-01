using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using memoseeds.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace memoseeds.Controllers
{
    public class TranslatorController : Controller
    {
        // TODO реалізувати API для фронтенду.  
        // Інформація передаватиметься за допомогою json

            //All languages format should be Full. I.E. English, Ukrainian, Bulgarian 
        public void translateMethod(String json){
            //getting json object
            JObject jObject = JObject.Parse(json);
            //parsing it
            string sourceText = (string)jObject.SelectToken("sourceText");
            string sourceLanguage = (string)jObject.SelectToken("sourceLanguage");
            string targetLanguage = (string)jObject.SelectToken("targetLanguage");

            //creates object that manages translation
            TranslatorService ts = new TranslatorService();
            //translates user text from A language to B language
            String transResponse = ts.translate(sourceText, sourceLanguage, targetLanguage);

            //sending response
            string response_json = "{\"translation\":\"" + transResponse + "\"}";
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();

        } 


    }

}

