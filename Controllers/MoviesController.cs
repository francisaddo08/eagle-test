using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EagleEyeTest.model;
using System.Collections.Generic;
using System.Linq;
using EagleEyeTest.database;

namespace EagleEyeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private static List<StatsEntity> _list;
        private List<MetadataEntity> _metadataEntities;
        private Dictionary<int, string> movieIdTitleDictionary = new Dictionary<int, string>();

        public MoviesController()
        {
            _metadataEntities = CSVReader.GetMetaDataFromFile(@"C:\Users\Francis T Addo\source\repos\EagleEyeTest\wwwroot\metadata.csv");
            _list = CSVReader.GetStatsDataFromFile(@"C:\Users\Francis T Addo\source\repos\EagleEyeTest\wwwroot\stats.csv");
        
        
        }
       
        [HttpGet("stats", Name = "stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StatsModel), 200)]
      
        public IActionResult Stats()
        {
            ISet<int> movieIdsSet = new HashSet<int>();
            Dictionary<int, long> movieIdTotalWatchDictionary = new Dictionary<int, long>();
            Dictionary<int, int> movieIdNumberOfWatchDictionary = new Dictionary<int, int>();
            Dictionary<int, MovieModel> movieIdToMovieModelDictionary = new Dictionary<int, MovieModel>();
            List<StatsEntity> statsEntities = new List<StatsEntity>();
            List<StatsModel> statsModelsList = new List<StatsModel>();
           

            int key;
            long value;
            MetadataEntity metadataEntity = new MetadataEntity();


            foreach (StatsEntity entity in _list)
            {
                key = entity.movieId; 
                value = entity.watches; //stats watchDurationMs for movieID

                if (movieIdTotalWatchDictionary.ContainsKey(key))
                {
                    long tempValue = (long)movieIdTotalWatchDictionary[key] + value; //stats watchDurationMs addition
                    int tempCount = movieIdNumberOfWatchDictionary[key]; // how many times currently movieId appears
                    movieIdNumberOfWatchDictionary[key] = tempCount + 1; // add to no of movieId appearance
                    movieIdTotalWatchDictionary[key] = tempValue; // set the total of stats watchDurationMs for movieId

                }
                else
                {
                    movieIdNumberOfWatchDictionary.Add(key, 1); // first iteration
                    movieIdTotalWatchDictionary.Add(key, value);
                }
               
                movieIdsSet.Add(entity.movieId);
               
            }
            //===================================================== END OF STATS DATA DICTIONARY=============
            foreach (MetadataEntity entity in _metadataEntities)
            {
               
                    if (movieIdToMovieModelDictionary.ContainsKey(entity.movieId))
                    {
                        MovieModel tempMovieModel = movieIdToMovieModelDictionary[entity.movieId];
                        tempMovieModel.noOfRelases = tempMovieModel.noOfRelases + 1;
                        movieIdToMovieModelDictionary[entity.movieId] = tempMovieModel;
                    }
                    else
                    {
                        MovieModel movieModel = new MovieModel();
                        movieModel.title = entity.title;
                        movieModel.releaseYear = entity.releaseYear;
                        movieModel.noOfRelases = 1;
                        movieIdToMovieModelDictionary.Add(entity.movieId, movieModel);
                    
                }
            }
            //============================================END OF METADATA DICTIONARY============================
           
               
                foreach (KeyValuePair<int, int> keyValue in movieIdNumberOfWatchDictionary)
                {

                    StatsModel model = new StatsModel();
                    model.movieId = keyValue.Key;

                    if (movieIdToMovieModelDictionary.ContainsKey(model.movieId))
                    {
                        model.title = movieIdToMovieModelDictionary[keyValue.Key].title; //METADATA 
                        model.releaseYear = movieIdToMovieModelDictionary[keyValue.Key].releaseYear;
                    }
                    model.averageWatchDurationS = movieIdTotalWatchDictionary[keyValue.Key] / keyValue.Value;
                    model.watches = keyValue.Value;



                    statsModelsList.Add(model);
                   

                
              

            }
            //=====================END OF STATS AND METADATA CONSTRUCTION OF STATS MODEL======================
            List<StatsModel> completeDataModelsOnly = new List<StatsModel>();
            foreach(StatsModel completeModel in statsModelsList)
            {
                if (!string.IsNullOrEmpty(completeModel.title))
                {
                    completeDataModelsOnly.Add(completeModel);
                }
            }
            List<StatsModel> sorted = completeDataModelsOnly.OrderByDescending(sm => sm.watches )
                .ThenByDescending(m => m.releaseYear)
                .ToList();
            return new JsonResult(sorted);
        }
    }
}
