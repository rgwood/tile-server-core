using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace TileServerCore.Controllers
{
    [Route("api/[controller]")]
    public class TilesController : Controller
    {
        private readonly AppSettings _appSettings;

        public TilesController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        // GET tiles/z/x/y
        [HttpGet("{z}/{x}/{y}")]
        public byte[] Get(int z, int x, int y)
        {
            Console.WriteLine($"Tile path: {_appSettings.TilePath}");
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = _appSettings.TilePath
            };

            using (var connection = new SqliteConnection(connectionStringBuilder.ToString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var selectCommand = connection.CreateCommand();
                    selectCommand.Transaction = transaction;
                    //SQL injection not a concern b/c these are ints
                    selectCommand.CommandText = $"SELECT tile_data FROM `tiles` where zoom_level = {z} and tile_column = {x} and tile_row = {y}";
                    var tileData = (byte[]) selectCommand.ExecuteScalar();
                    return tileData;
                }
            }
            
        }
    }
}
