using EagleEyeTest.database;
using EagleEyeTest.model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EagleEyeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
       
        private List<MetadataEntity> _metadataEntities;
        private static List<MetadataEntity> _database;
        private static int Id = 0;
        public MetadataController()
        {
            _database = new List<MetadataEntity>();

           _metadataEntities = CSVReader.GetMetaDataFromFile(@"C:\Users\Francis T Addo\source\repos\EagleEyeTest\wwwroot\metadata.csv");
        }

        // GET: api/<MetadataController>
        //[HttpGet]
        //public IActionResult Get()
        //{

        //    return new JsonResult(_metadataEntities);
        //}

        // GET api/<MetadataController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
       {
            List<MetadataEntity> metadataById = new List<MetadataEntity>();
         
                foreach(MetadataEntity m in _metadataEntities)
                {
                    if(m.movieId == id && !string.IsNullOrEmpty(m.title) 
                        && !string.IsNullOrEmpty(m.language) && !string.IsNullOrEmpty(m.duration) && m.releaseYear 
                        != 0)
                    {
                        metadataById.Add(m);
                    }

                   
                }
                if(metadataById.Count == 0)
                {
                return BadRequest(StatusCode(400, "No data posted for your request"));
                }
                List<MetadataEntity> sorted = metadataById.OrderByDescending(s => s.Id).ToList();
            List<MetadataEntity> filtedByLanguage = new List<MetadataEntity>();

            Dictionary<int, MetadataEntity> filterForLanguageDictionary = new Dictionary<int, MetadataEntity>();
            foreach(MetadataEntity metadataEntity in sorted)
            {
                List<MetadataEntity> metadataByLanguageList = sorted.Where(O => O.language == metadataEntity.language).ToList();
                MetadataEntity outPutData = metadataByLanguageList.OrderByDescending(d => d.Id).Take(1).FirstOrDefault();
                if (!filtedByLanguage.Contains(outPutData))
                {
                    filtedByLanguage.Add(outPutData);
                }
                
                //if (filterForLanguageDictionary.ContainsKey(metadataEntity.movieId))
                //{
                //    MetadataEntity currentDictionaryValue = filterForLanguageDictionary[metadataEntity.movieId];
                //    if (currentDictionaryValue.language.Contains(metadataEntity.language))
                //    {
                //        MetadataEntity entity = currentDictionaryValue.Id > metadataEntity.Id ? currentDictionaryValue : metadataEntity;

                //    }

                   
                //}
                //else
                //{
                //    //MovieModel movieModel = new MovieModel();
                //    //movieModel.title = entity.title;
                //    //movieModel.releaseYear = entity.releaseYear;
                //    //movieModel.noOfRelases = 1;
                //    //movieIdToMovieModelDictionary.Add(entity.movieId, movieModel);

                //}
            }

            List<MetadataModel> metadataModelsList = new List<MetadataModel>();
            
             foreach(MetadataEntity entity in filtedByLanguage)
            {
                MetadataModel model = new MetadataModel();
                model.movieId = entity.movieId;
                model.title = entity.title;
                model.language = entity.language;
                model.duration = entity.duration;
                model.releaseYear = entity.releaseYear;

                metadataModelsList.Add(model);
            }
          
            return new JsonResult(metadataModelsList);
            
           

        }

        // POST api/<MetadataController>
        [HttpPost]
        public IActionResult Post([FromBody] MetadataModel model)
        {
            MetadataEntity entity = new MetadataEntity();
            Id = Id + 1;
            entity.Id = Id;
            entity.movieId = model.movieId;
            entity.title = model.title;
            entity.language = model.language;
            entity.releaseYear = model.releaseYear;
            _database.Add(entity);
            if(_database.Count > 0)
            {
                return Created("added", model);
            }
            return BadRequest(StatusCode(500, "Could not be saved"));
        }

     
    }
}
