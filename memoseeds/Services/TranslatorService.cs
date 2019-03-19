using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace memoseeds.Services
{
    public class TranslatorService : ITranslatorService
    {
            #region Properties

            /// <summary>
            /// Gets the supported languages.
            /// </summary>
            public static IEnumerable<string> Languages
            {
                get
                {
                TranslatorService.EnsureInitialized();
                    return TranslatorService._languageModeMap.Keys.OrderBy(p => p);
                }
            }

            /// <summary>
            /// Gets the time taken to perform the translation.
            /// </summary>
            public TimeSpan TranslationTime
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the url used to speak the translation.
            /// </summary>
            /// <value>The url used to speak the translation.</value>
            public string TranslationSpeechUrl
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the error.
            /// </summary>
            public Exception Error
            {
                get;
                private set;
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Translates the specified source text.
            /// </summary>
            /// <param name="sourceText">The source text.</param>
            /// <param name="sourceLanguage">The source language.</param>
            /// <param name="targetLanguage">The target language.</param>
            /// <returns>The translation.</returns>
            public string translate
                (string sourceText,
                 string sourceLanguage,
                 string targetLanguage)
            {
            // Initialize
                this.Error = null;
                this.TranslationSpeechUrl = null;
                this.TranslationTime = TimeSpan.Zero;
                DateTime tmStart = DateTime.Now;
                string translation = string.Empty;

                try
                {
                    // Download translation
                    string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                                                TranslatorService.LanguageEnumToIdentifier(sourceLanguage),
                                                TranslatorService.LanguageEnumToIdentifier(targetLanguage),
                                                HttpUtility.UrlEncode(sourceText));
                    string outputFile = Path.GetTempFileName();
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
                        wc.DownloadFile(url, outputFile);
                    }

                    // Get translated text
                    if (File.Exists(outputFile))
                    {

                        // Get phrase collection
                        string text = File.ReadAllText(outputFile);
                        int index = text.IndexOf(string.Format(",,\"{0}\"", TranslatorService.LanguageEnumToIdentifier(sourceLanguage)));
                        if (index == -1)
                        {
                            // Translation of single word
                            int startQuote = text.IndexOf('\"');
                            if (startQuote != -1)
                            {
                                int endQuote = text.IndexOf('\"', startQuote + 1);
                                if (endQuote != -1)
                                {
                                    translation = text.Substring(startQuote + 1, endQuote - startQuote - 1);
                                }
                            }
                        }
                        else
                        {
                            // Translation of phrase
                            text = text.Substring(0, index);
                            text = text.Replace("],[", ",");
                            text = text.Replace("]", string.Empty);
                            text = text.Replace("[", string.Empty);
                            text = text.Replace("\",\"", "\"");

                            // Get translated phrases
                            string[] phrases = text.Split(new[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; (i < phrases.Count()); i += 2)
                            {
                                string translatedPhrase = phrases[i];
                                if (translatedPhrase.StartsWith(",,"))
                                {
                                    i--;
                                    continue;
                                }
                                translation += translatedPhrase + "  ";
                            }
                        }

                        // Fix up translation
                        translation = translation.Trim();
                        translation = translation.Replace(" ?", "?");
                        translation = translation.Replace(" !", "!");
                        translation = translation.Replace(" ,", ",");
                        translation = translation.Replace(" .", ".");
                        translation = translation.Replace(" ;", ";");

                    // And translation speech URL
                    // this.TranslationSpeechUrl = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx",
                    //                                          HttpUtility.UrlEncode(translation), TranslatorService.LanguageEnumToIdentifier(targetLanguage), translation.Length);
                }
            }
                catch (Exception ex)
                {
                    this.Error = ex;
                }

                // Return result
                this.TranslationTime = DateTime.Now - tmStart;
                return translation;
            }

            #endregion

            #region Private methods

            /// <summary>
            /// Converts a language to its identifier.
            /// </summary>
            /// <param name="language">The language."</param>
            /// <returns>The identifier or <see cref="string.Empty"/> if none.</returns>
            private static string LanguageEnumToIdentifier
                (string language)
            {
                string mode = string.Empty;
            TranslatorService.EnsureInitialized();
            TranslatorService._languageModeMap.TryGetValue(language, out mode);
                return mode;
            }

            /// <summary>
            /// Ensures the translator has been initialized.
            /// </summary>
            private static void EnsureInitialized()
            {
                if (TranslatorService._languageModeMap == null)
                {
                TranslatorService._languageModeMap = new Dictionary<string, string>();
                TranslatorService._languageModeMap.Add("Afrikaans", "af");
                TranslatorService._languageModeMap.Add("Albanian", "sq");
                TranslatorService._languageModeMap.Add("Arabic", "ar");
                TranslatorService._languageModeMap.Add("Armenian", "hy");
                TranslatorService._languageModeMap.Add("Azerbaijani", "az");
                TranslatorService._languageModeMap.Add("Basque", "eu");
                TranslatorService._languageModeMap.Add("Belarusian", "be");
                TranslatorService._languageModeMap.Add("Bengali", "bn");
                TranslatorService._languageModeMap.Add("Bulgarian", "bg");
                TranslatorService._languageModeMap.Add("Catalan", "ca");
                TranslatorService._languageModeMap.Add("Chinese", "zh-CN");
                TranslatorService._languageModeMap.Add("Croatian", "hr");
                TranslatorService._languageModeMap.Add("Czech", "cs");
                TranslatorService._languageModeMap.Add("Danish", "da");
                TranslatorService._languageModeMap.Add("Dutch", "nl");
                TranslatorService._languageModeMap.Add("English", "en");
                TranslatorService._languageModeMap.Add("Esperanto", "eo");
                TranslatorService._languageModeMap.Add("Estonian", "et");
                TranslatorService._languageModeMap.Add("Filipino", "tl");
                TranslatorService._languageModeMap.Add("Finnish", "fi");
                TranslatorService._languageModeMap.Add("French", "fr");
                TranslatorService._languageModeMap.Add("Galician", "gl");
                TranslatorService._languageModeMap.Add("German", "de");
                TranslatorService._languageModeMap.Add("Georgian", "ka");
                TranslatorService._languageModeMap.Add("Greek", "el");
                TranslatorService._languageModeMap.Add("Haitian Creole", "ht");
                TranslatorService._languageModeMap.Add("Hebrew", "iw");
                TranslatorService._languageModeMap.Add("Hindi", "hi");
                TranslatorService._languageModeMap.Add("Hungarian", "hu");
                TranslatorService._languageModeMap.Add("Icelandic", "is");
                TranslatorService._languageModeMap.Add("Indonesian", "id");
                TranslatorService._languageModeMap.Add("Irish", "ga");
                TranslatorService._languageModeMap.Add("Italian", "it");
                TranslatorService._languageModeMap.Add("Japanese", "ja");
                TranslatorService._languageModeMap.Add("Korean", "ko");
                TranslatorService._languageModeMap.Add("Lao", "lo");
                TranslatorService._languageModeMap.Add("Latin", "la");
                TranslatorService._languageModeMap.Add("Latvian", "lv");
                TranslatorService._languageModeMap.Add("Lithuanian", "lt");
                TranslatorService._languageModeMap.Add("Macedonian", "mk");
                TranslatorService._languageModeMap.Add("Malay", "ms");
                TranslatorService._languageModeMap.Add("Maltese", "mt");
                TranslatorService._languageModeMap.Add("Norwegian", "no");
                TranslatorService._languageModeMap.Add("Persian", "fa");
                TranslatorService._languageModeMap.Add("Polish", "pl");
                TranslatorService._languageModeMap.Add("Portuguese", "pt");
                TranslatorService._languageModeMap.Add("Romanian", "ro");
                TranslatorService._languageModeMap.Add("Russian", "ru");
                TranslatorService._languageModeMap.Add("Serbian", "sr");
                TranslatorService._languageModeMap.Add("Slovak", "sk");
                TranslatorService._languageModeMap.Add("Slovenian", "sl");
                TranslatorService._languageModeMap.Add("Spanish", "es");
                TranslatorService._languageModeMap.Add("Swahili", "sw");
                TranslatorService._languageModeMap.Add("Swedish", "sv");
                TranslatorService._languageModeMap.Add("Tamil", "ta");
                TranslatorService._languageModeMap.Add("Telugu", "te");
                TranslatorService._languageModeMap.Add("Thai", "th");
                TranslatorService._languageModeMap.Add("Turkish", "tr");
                TranslatorService._languageModeMap.Add("Ukrainian", "uk");
                TranslatorService._languageModeMap.Add("Urdu", "ur");
                TranslatorService._languageModeMap.Add("Vietnamese", "vi");
                TranslatorService._languageModeMap.Add("Welsh", "cy");
                TranslatorService._languageModeMap.Add("Yiddish", "yi");
                }
            }

            #endregion

            #region Fields

            /// <summary>
            /// The language to translation mode map.
            /// </summary>
            private static Dictionary<string, string> _languageModeMap;

            #endregion
        
    }
}
