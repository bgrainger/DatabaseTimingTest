using System.Collections.Generic;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace DatabaseTimingTest.Controllers
{
	public class ValuesController : ApiController
	{
		// GET api/values
		public string Get()
		{
			using (var connection = new MySqlConnection(""))
			{
				connection.Open();

				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = @"
SELECT value
FROM new_relic_test AS b
WHERE b.value = @sourceId;";
					cmd.Parameters.AddWithValue("@sourceId", 49605);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							reader.GetInt32(0);
						}
					}
				}

				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = @"
SELECT value
FROM new_relic_test AS b
WHERE b.value != @sourceId;";
					cmd.Parameters.AddWithValue("@sourceId", 49605);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							reader.GetInt32(0);
						}
					}
				}

				var items = new List<int>();
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = @"select a.value, b.value, c.value, d.value, e.value, f.value, g.value
	from new_relic_test a
	inner join new_relic_test b
    inner join new_relic_test c
    inner join new_relic_test d
    inner join new_relic_test e
    inner join new_relic_test f
    inner join new_relic_test g
    where a.value + b.value + c.value + d.value + e.value + f.value + g.value = @accountId
    limit @limit;";
					cmd.Parameters.AddWithValue("@accountId", 42);
					cmd.Parameters.AddWithValue("@limit", 100_000);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							items.Add(reader.GetInt32(0));
						}
					}
				}

				return "Test passed: " + items.Count + " items";
			}
		}

		// GET api/values/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/values
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
		}
	}
}
