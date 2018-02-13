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
                DataSource = @"%HOME%\tiles\parcels.mbtiles"
            };

            using (var connection = new SqliteConnection(connectionStringBuilder.ToString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var selectCommand = connection.CreateCommand();
                    selectCommand.Transaction = transaction;
                    selectCommand.CommandText = $"SELECT tile_data FROM `tiles` where zoom_level = {z} and tile_column = {x} and tile_row = {y}";
                    var tileData = (byte[]) selectCommand.ExecuteScalar();
                    return tileData;
                }
            }
            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
