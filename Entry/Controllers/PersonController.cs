using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

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
                    var response = responsemessage.Content.ReadFromJsonAsync<List<PersonModel>>().Result;
                    return Ok(response);
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

                    var response = await clientresponse.Content.ReadFromJsonAsync<PersonModel>();
                    return Ok(response);

                }
                return BadRequest($"Sorry the Person with id {id} cannot be found");


            }

        }
        [HttpPost("PostPerson")]
        public ActionResult PostPerson(PersonModelContext model)
        {
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync<PersonModelContext>($"{url}{userpath}/", model).Result;

                var result = response.Content.ReadFromJsonAsync<PersonModel>();
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
                    var responsejson = responseadd.Content.ReadFromJsonAsync<PersonModel>();
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

                        var result = responseadd.Content.ReadFromJsonAsync<PersonModel>();
                        return Ok(result);
                    }
                    return BadRequest($"The Person with id {id} was not found");
                }

                return BadRequest($"The Person with id {id} is not found");
            }

        }
        [HttpGet("GetAllPersonRestClient")]
        public ActionResult GetPeopleRestClient()
        {
            var client = new RestClient(url + userpath);
            // client.Timeout = -1;
            var request = new RestRequest();
            RestResponse response = client.Execute(request);
            // var result = response.Content.ToList();
            var result = JsonConvert.DeserializeObject<List<PersonModel>>(response.Content.ToString());//.ToList();
            return Ok(result);
        }
        [HttpPost("PostPersonRestClient")]
        public IActionResult PostPersonRestClient(PersonModelContext model)
        {
            var client = new RestClient(url + userpath + "/");
            //client.Timeout = -1;
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", model, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return Ok("Person Added Successfully, Congrats");
            }
            return BadRequest("Sorry, we were unable to add the person");


            //Console.WriteLine(response.Content);
        }

    }
}
