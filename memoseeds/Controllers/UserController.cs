using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using memoseeds.Repositories.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private IModuleRepository ModuleRepository;
        private ISubjectRepository SubjectRepository;
        private IUserRepository UserRepository;

        public UserController(IModuleRepository ModuleRepository, ISubjectRepository SubjectRepository, IUserRepository UserRepository)
        {
            this.ModuleRepository = ModuleRepository;
            this.SubjectRepository = SubjectRepository;
            this.UserRepository = UserRepository;
        }


        [HttpGet("/{userID}/getImage")]
        public IActionResult getImage([FromRoute] long userId)
        {
            IActionResult response = Unauthorized();
            try
            {
                User a = UserRepository.GetById(userId);
                if (a != null)
                {
                    if (a.Img != null)
                        response = Ok(new {photo = bytesToImage(a.Img) });
                    else
                        response = Ok(new { err = "User haven't image" });
                }
                else
                    response = Ok(new { err = "User doesn't exist" });
            }
            catch
            {
                response = Ok(new { err = "User doesn't exist" });

            }
            return response;
        }

        [HttpPost("/{userID}/setImage")]
        public IActionResult setImage([FromRoute] long userId, [FromBody] UploadImageModel data)
        {
            IActionResult response = Unauthorized();
            try
            {
                User a = UserRepository.GetById(userId);
                if (a != null)
                {
                    a.Img = stringToBytes(data.ImageData);
                    UserRepository.Update(a);
                    return bytesToImage(a.Img);
                }
                else
                    response = Ok(new { err = "User doesn't exist" });
            }
            catch
            {
                response = Ok(new { err = "User doesn't exist" });

            }
            return response;
        }

        [HttpPost("/updateImage")]
        public IActionResult updateImage([FromBody]UserData usr)
        {
            IActionResult response = Unauthorized();
            User user = UserRepository.GetById(usr.id);
            if (user != null)
            {
                user.Img = usr.image;
                UserRepository.Update(user);
                response = Ok(new { data = user });
            }
            else
                response = Ok(new { Error = "User with that username not found" });
            return response;
        }

        public FileContentResult bytesToImage(Byte[] s)
        {
            var imageDataByteArray = s;
            //When creating a stream, you need to reset the position, without it you will see that you always write files with a 0 byte length. 
            var imageDataStream = new MemoryStream(imageDataByteArray);
            imageDataStream.Position = 0;

            //Go and do something with the actual data.
            //_customerImageService.Upload([...])

            //For the purpose of the demo, we return a file so we can ensure it was uploaded correctly. 
            //But otherwise you can just return a 204 etc. 
            return File(imageDataByteArray, "image/png");
        }

        public Byte[] stringToBytes(string s)
        {
            var imageDataByteArray = Convert.FromBase64String(s);
            //When creating a stream, you need to reset the position, without it you will see that you always write files with a 0 byte length. 
            var imageDataStream = new MemoryStream(imageDataByteArray);
            imageDataStream.Position = 0;
            return imageDataByteArray;
        }


        public FileContentResult stringToImage(string s)
        {

            return File(stringToBytes(s), "image/png");
        }


        [HttpPost("create/module")]
        public IActionResult CreateModule([FromBody] ModuleData module)
        {
            IActionResult response = Unauthorized();
            try
            {
                Category category = SubjectRepository.GetCategoryName(module.Category);
                Module created = CreateModule(module, category.CategoryId);
                ModuleRepository.Insert(created);

                created.Terms = AddTermsFromIDict(null, module.Terms, created.ModuleId);
                Module added = ModuleRepository.Update(created);
                AquiredModules aquiredModules = CreateUserModule(module.Author, created.ModuleId);

                UserRepository.InsertUserModule(aquiredModules);
                response = Ok(new { moduleId = added.ModuleId });
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }
            return response;
        }

        [HttpPost("update/module")]
        public IActionResult UpdateModule([FromBody] EditModuleData module)
        {
            IActionResult response = Unauthorized();
            try
            {
                if (!IsExist(module.UserId, module.ModuleId)) return Ok(new { result = "User doesn't have this module." });
                Module old = ModuleRepository.GetModuleWithTerms(module.ModuleId);

                if (!module.Category.Equals("default"))
                {
                    Category category = SubjectRepository.GetCategoryName(module.Category);
                    old.CategoryId = category.CategoryId;
                }
                old.IsLocal = module.IsLocal;
                old.Price = module.Price;
                old.Name = module.Name;
                AddTerms(old.Terms, module.Terms, old.ModuleId);
                Module updated = ModuleRepository.Update(old);

                response = Ok(new { result = "success", moduleId = updated.ModuleId });
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }
            return response;
        }

        private ICollection<Term> AddTermsFromIDict(ICollection<Term> current, IDictionary<string, string> keyValue, long moduleid)
        {
            ICollection<Term> dictionary = null;
            if (current == null) dictionary = new List<Term>();
            else dictionary = current;
            foreach (string str in keyValue.Keys)
            {
                dictionary.Add(new Term()
                {
                    ModuleId = moduleid,
                    Name = str,
                    Definition = keyValue[str],
                });
            }
            return dictionary;
        }

        private void AddTerms(ICollection<Term> old, ICollection<Term> terms, long moduleid)
        {
            //ICollection<Term> dictionary = new List<Term>();   
            List<Term> lists = (System.Collections.Generic.List<memoseeds.Models.Entities.Term>)terms;

            foreach (Term t in old)
            {
                if (lists.Contains(t))
                {
                    Term f = lists.FirstOrDefault(i => i.TermId == t.TermId);
                    t.Name = f.Name;
                    t.Definition = f.Definition;
                }
            }
            foreach (Term t in terms)
            {
                if (!old.Contains(t))
                {
                    if (t.TermId == 0)
                        t.ModuleId = moduleid;
                    old.Add(t);
                }
            }
            //return old;
        }

        [HttpPost("{userid}/has/module/{moduleid}")]
        public IActionResult UserHasModule([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {
                bool userHasModule = IsExist(userid, moduleid);
                if(!userHasModule) return Ok(new { result = userHasModule });
                Module real = UserRepository.GetModuleWithTerms(userid, moduleid);
                response = Ok(new { result = userHasModule, moduleId = real.ModuleId });
            }
            catch (Exception e)
            {
                response = BadRequest(new { e });
            }
            return response;
        }

        [HttpPost("{userid}/resources/module/{moduleid}")]
        public IActionResult UserHasEnoughMoney([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {
                Module module = ModuleRepository.GetById(moduleid);
                int moduleCost = module.Price;
                User user = UserRepository.GetById(userid);
                response = Ok(new { result = (user.Credits >= moduleCost) });
            }
            catch (Exception e)
            {
                response = BadRequest(new { e });
            }
            return response;
        }


        [HttpPost("{userid}/get/module/{moduleid}")]
        public IActionResult GetModule([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {
                if (IsExist(userid, moduleid)) return Ok(new { result = "User has this module." });
                Module module = ModuleRepository.GetById(moduleid);
                int moduleCost = module.Price;
                User user = UserRepository.GetById(userid);
                if (user.Credits >= moduleCost)
                {
                    Module copied = Copy(module);
                    user.Credits -= moduleCost;
                    ModuleRepository.Insert(copied);
                    copied.Terms = CopyTerms(module.Terms, copied.ModuleId);
                    //має працювати
                    Module added = ModuleRepository.Update(copied);

                    AquiredModules aquiredModules = CreateUserModule(userid, added.ModuleId);
                    UserRepository.InsertUserModule(aquiredModules);
                    response = Ok(new { result = "success", moduleId = copied.ModuleId, CreditsLeft = user.Credits
                   });
                }
                else response = Ok(new { result = "Not enough credits." });
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }
            return response;
        }

        /* check if user has this module */
        private bool IsExist(long useid, long moduleid)
        {
            bool userHas = UserRepository.UserHasModel(useid, moduleid);
            return userHas;
        }

        private Module Copy(Module module)
        {
            return new Module()
            {
                CategoryId = module.CategoryId,
                Name = module.Name,
                ModuleId = default(long),
                UserId = module.UserId,
                IsLocal = true,
                InheritedFrom = module.ModuleId,
                Price = 0
            };
        }

        private AquiredModules CreateUserModule(long userid, long moduleid)
        {
            return new AquiredModules()
            {
                UserId = userid,
                ModuleId = moduleid,
                LastEdit = DateTime.Now
            };
        }

        private Module CreateModule(ModuleData module, long categoryId)
        {
            if (!module.IsLocal && module.Category == "default") throw new FormatException("Non-local module must contain a category!");
            return new Module()
            {
                CategoryId = categoryId,
                InheritedFrom = module.InheritedFrom,
                IsLocal = module.IsLocal,
                Name = module.Name,
                Price = module.Price,
                UserId = module.Author
            };
        }

        private ICollection<Term> GetTermFromIDict(IDictionary<string, string> keyValue, long moduleid)
        {
            ICollection<Term> dictionary = new List<Term>();
            foreach (string str in keyValue.Keys)
            {
                dictionary.Add(new Term()
                {
                    ModuleId = moduleid,
                    Name = str,
                    Definition = keyValue[str],
                });
            }
            return dictionary;
        }

        public ICollection<Term> CopyTerms(ICollection<Term> terms, long moduleId)
        {
            ICollection<Term> coppied = new List<Term>();
            foreach (Term t in terms)
            {
                coppied.Add(new Term()
                {
                    ModuleId = moduleId,
                    Name = t.Name,
                    Definition = t.Definition
                });
            }
            return coppied;
        }

        public class ModuleData
        {
            [Required(ErrorMessage = "Author not specified")]
            public long Author { set; get; }

            [Required(ErrorMessage = "Category name not specified")]
            public string Category { set; get; }

            [Required(ErrorMessage = "Inherited id not specified")]
            public long InheritedFrom { set; get; }

            [Required(ErrorMessage = "Name not specified")]
            public string Name { set; get; }

            [Required(ErrorMessage = "Enviroment not specified")]
            public bool IsLocal { set; get; }

            [Required(ErrorMessage = "Price not specified")]
            public int Price { set; get; }

            [Required(ErrorMessage = "Terms not specified")]
            public IDictionary<string, string> Terms { set; get; }
        }

        public class EditModuleData
        {
            [Required(ErrorMessage = "Category name not specified")]
            public string Category { set; get; }

            [Required(ErrorMessage = "User id not specified")]
            public long UserId { set; get; }

            [Required(ErrorMessage = "Module id not specified")]
            public long ModuleId { set; get; }

            [Required(ErrorMessage = "Name not specified")]
            public string Name { set; get; }

            [Required(ErrorMessage = "Enviroment not specified")]
            public bool IsLocal { set; get; }

            [Required(ErrorMessage = "Price not specified")]
            public int Price { set; get; }

            [Required(ErrorMessage = "Terms not specified")]
            public ICollection<Term> Terms { set; get; }
        }

        public class UploadImageModel
        {
            [Required(ErrorMessage ="Description not specified")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Data not specified")]
            public string ImageData { get; set; }
        }
    }
}
