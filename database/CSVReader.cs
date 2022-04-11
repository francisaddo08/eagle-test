
using EagleEyeTest.database;
using EagleEyeTest.model;
using LumenWorks.Framework.IO.Csv;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace EagleEyeTest.database
{
    public static class CSVReader
    {
        public static List<MetadataEntity> GetMetaDataFromFile(string filePath)
        {
            var csvTable = new DataTable();
            List<MetadataEntity> metadaList = new List<MetadataEntity>();

            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(filePath)), true))
            {
                csvTable.Load(csvReader);
                for (int i = 0; i < csvTable.Rows.Count; i++)
                {
                    MetadataEntity model = new MetadataEntity();
                    model.Id = int.Parse(csvTable.Rows[i][0].ToString());
                    model.movieId = int.Parse(csvTable.Rows[i][1].ToString());
                    model.title = csvTable.Rows[i][2].ToString();
                    model.language = csvTable.Rows[i][3].ToString();
                    model.duration = csvTable.Rows[i][4].ToString();
                    model.releaseYear = int.Parse(csvTable.Rows[i][5].ToString());
                    metadaList.Add(model);
                  
                }

            }
            return metadaList;

        }
        public static List<StatsEntity> GetStatsDataFromFile(string filePath)
        {
            var csvTable = new DataTable();
            List<StatsEntity> statsList = new List<StatsEntity>();

            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(filePath)), true))
            {
                csvTable.Load(csvReader);
                for (int i = 0; i < csvTable.Rows.Count; i++)
                {
                    StatsEntity model = new StatsEntity();
                 
                    model.movieId = int.Parse(csvTable.Rows[i][0].ToString());
                   
                
                    model.watches = int.Parse( csvTable.Rows[i][1].ToString());
                  
                    statsList.Add(model);

                }

            }
            return statsList;

        }
    }
}
