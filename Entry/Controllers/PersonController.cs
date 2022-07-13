using BusinessLogic.Models;
using BusinessLogic.ModelValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace Entry.Controllers
{
    [ApiController]
    public class PersonController : ControllerBase
    {
        public string url;
        static readonly string userpath = "User";
        readonly HttpClient client = new();
        readonly IConfiguration configuration;
        public PersonController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            //this.client = httpclient;
            url = configuration["MockApiUrl"];
        }

        [HttpGet("all-persons")]
        public async Task<ActionResult> GetAllPersons()
        {
            HttpResponseMessage responsemessage = await client.GetAsync(url + userpath);
            if (responsemessage.IsSuccessStatusCode)
            {
                var response = await responsemessage.Content.ReadFromJsonAsync<List<PersonModel>>();
                return Ok(response);
            }
            client.Dispose();
            return NotFound();
        }
        [HttpGet("person-with-id")]
        public async Task<ActionResult> GetPersonById(int id)
        {
            // make sure that id is a number
            if (id.ToString().IsIdValid())
            {
                var clientresponse = await client.GetAsync($"{url}{userpath}/{id.ToString()}");
                if (clientresponse.IsSuccessStatusCode)
                {

                    var response = await clientresponse.Content.ReadFromJsonAsync<PersonModel>();
                    return Ok(response);

                }
                client.Dispose();
                return NotFound($"Sorry the Person with id {id} cannot be found");
            }
            return BadRequest("The provided id is not valid, it should be an int");

        }
        [HttpPost("post-person")]
        public async Task<ActionResult> PostPerson(PersonModelContext model)
        {
            //validate the personmodelcontext
            if (!model.IsModelValid())
            {
                return BadRequest("The provided Model is Invalid");
            }

            var response = await client.PostAsJsonAsync<PersonModelContext>($"{url}{userpath}/", model);

            //var result = await response.Content.ReadFromJsonAsync<PersonModel>();
            if (response.IsSuccessStatusCode)
            {
                return Created(url, model);
            }
            client.Dispose();
            return Ok("Request Not Commited");

        }
        [HttpPut("edit-person")]
        public async Task<IActionResult> EditPerson(PersonModelContext person, int id)
        {
            if (!person.IsModelValid())
            {
                return BadRequest("The provided Model is Invalid");
            }
            if (!id.ToString().IsIdValid())
            {
                return BadRequest("The provided id is Invalid");

            }
            var response = await client.GetAsync($"{url}{userpath}/{id}");
            if (response != null)
            {
                var responseAdd = await client.PutAsJsonAsync($"({url}{userpath}/{id.ToString()}", person);
                var responseJson = await responseAdd.Content.ReadFromJsonAsync<PersonModel>();
                return Ok(responseJson);
            }
            client.Dispose();
            return NotFound($"The Person with id {id} is not found");
        }
        [HttpDelete("delete-person")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            if (id.ToString().IsIdValid())
            {
                return BadRequest("The provided id is Invalid");
            }
            var httresponsemessage = await client.GetAsync($"{url}{userpath}/{id.ToString()}");
            if (httresponsemessage.IsSuccessStatusCode)
            {
                var response = httresponsemessage;
                //
                var responseadd = await client.DeleteAsync($"({url}{userpath}/{id.ToString()})");
                var result = await responseadd.Content.ReadFromJsonAsync<PersonModel>();
                httresponsemessage.Dispose();
                response.Dispose();
                return Ok(result);

            }
            client.Dispose();

            return NotFound($"The Person with id {id} is not found");
        }
        [HttpGet("GetAllPersonRestClient")]
        public ActionResult GetPeopleRestClient()
        {
            var client = new RestClient(url + userpath);
            // client.Timeout = -1;
            var request = new RestRequest();
            RestResponse response = client.Execute(request);
            client.Dispose();
            if (response.Content != null)
            {
                var result = JsonConvert.DeserializeObject<List<PersonModel>>(response.Content.ToString());//.ToList();
                return Ok(result);
            }
            return NotFound("No Person Found");
            // var result = response.Content.ToList();

        }
        [HttpPost("PostPersonRestClient")]
        public IActionResult PostPersonRestClient(PersonModelContext model)
        {
            if (!model.IsModelValid())
            {
                return BadRequest("The provided model is incorrect");
            }

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
            return NotFound("Sorry, we were unable to add the person");
        }

    }
}
