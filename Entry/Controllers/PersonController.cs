using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Entry.Controllers
{
    [ApiController]
    public class PersonController : ControllerBase
    {
        static string url = "https://62c6b96d74e1381c0a67231a.mockapi.io/";
        //static string path = "User/";
        static string userpath = "User";

        [HttpGet("All Persons")]
        public ActionResult GetAllPersons()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage responsemessage = client.GetAsync(url + userpath).Result;
                if (responsemessage.IsSuccessStatusCode)
                {
                    var response = responsemessage.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<List<PersonModel>>(response);
                    return Ok(result);
                }
                return BadRequest();

            }

        }
        [HttpGet("PersonWithId")]
        public async Task<ActionResult> GetPersonById(int id)
        {
            using (var client = new HttpClient())
            {
                var clientresponse = await client.GetAsync($"{url}{userpath}/{id.ToString()}");
                if (clientresponse.IsSuccessStatusCode)
                {

                    var response = clientresponse.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<PersonModel>(response);
                    return Ok(result);

                }
                return BadRequest("Sorry the Person with id {id} cannot be found");


            }

        }
        [HttpPost("PostPerson")]
        public ActionResult PostPerson(PersonModelContext model)
        {
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync<PersonModelContext>($"{url}{userpath}/", model).Result;
                var result = JsonConvert.DeserializeObject<PersonModel>(response.Content.ReadAsStringAsync().Result);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(model);
                }
                return BadRequest("Request Not Commited");
            }
        }
        [HttpPut("Edit Person")]
        public ActionResult EditPerson(PersonModelContext person, int id)
        {
            using (var client = new HttpClient())
            {

                var response = client.GetAsync($"{url}{userpath}/{id}").Result;
                if (response != null)
                {
                    var responseadd = client.PutAsJsonAsync<PersonModelContext>($"({url}{userpath}/{id.ToString()}", person).Result;
                    var responsejson = JsonConvert.DeserializeObject(responseadd.Content.ReadAsStringAsync().Result);
                    return Ok(responsejson);
                }
                return BadRequest($"The Person with id {id} is not found");
            }

        }
        [HttpDelete("Delete Person")]
        public ActionResult DeletePerson(int id)
        {
            using (var client = new HttpClient())
            {
                var httresponsemessage = client.GetAsync($"{url}{userpath}/{id.ToString()}");
                if (httresponsemessage.IsCompletedSuccessfully)
                {
                    var response = httresponsemessage.Result;
                    if (response.IsSuccessStatusCode)
                    {

                        var responseadd = client.DeleteAsync($"({url}{userpath}/{id.ToString()})").Result;
                        //   var responseobject = response.Content.ReadAsStringAsync().Result;
                        //  var responsejson = JsonConvert.SerializeObject(responseadd).Result;
                        var result = JsonConvert.DeserializeObject<PersonModel>(response.Content.ReadAsStringAsync().Result);
                        return Ok(result);
                    }
                    return BadRequest($"The Person with id {id} was not found");
                }

                return BadRequest($"The Person with id {id} is not found");
            }
        }
    }
}
