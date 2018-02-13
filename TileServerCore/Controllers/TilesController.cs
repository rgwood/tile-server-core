using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace TileServerCore.Controllers
{
    [Route("api/[controller]")]
    public class TilesController : Controller
    {
        // GET tiles/z/x/y
        [HttpGet("{z}/{x}/{y}")]
        public byte[] Get(int z, int x, int y)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = @"d:\home\tiles\parcels.mbtiles"
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
