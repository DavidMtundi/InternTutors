﻿using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Entry.Controllers
{
    [ApiController]
    public class PersonController : ControllerBase
    {
        static string url = "https://62c6b96d74e1381c0a67231a.mockapi.io/";
        static string path = "User/";
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
                var clientresponse = await client.GetAsync(url + path + id.ToString());
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
                var response = client.PostAsJsonAsync<PersonModelContext>(url + path, model).Result;
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
            //first find if the id is found
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url + path + id.ToString()).Result;
                if (response != null)
                {
                    //  var result = response.Content.ReadAsStringAsync().Result;
                    var responseadd = client.PutAsJsonAsync<PersonModelContext>(url + path + id.ToString(), person).Result;
                    var responsejson = JsonConvert.SerializeObject(person);
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
                var response = client.GetAsync(url + path + id.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {

                    var responseadd = client.DeleteAsync(url + path + id.ToString()).Result;
                    //   var responseobject = response.Content.ReadAsStringAsync().Result;
                    //  var responsejson = JsonConvert.SerializeObject(responseadd).Result;

                    return Ok(responseadd.Content.ReadAsStringAsync().Result);
                }
                return BadRequest($"The Person with id {id} is not found");
            }
        }
    }
}