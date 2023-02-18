using Microsoft.AspNetCore.Mvc;
using Octokit;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TotsChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepoController : ControllerBase
    {
        private GitHubClient client;

        [HttpGet]
        private void GetAuth()
        {
            try
            {
                //Set the personal access token from GitHub page to get user's credential
                Credentials credentials = new Credentials(Environment.GetEnvironmentVariable("PERSONAL_ACCESS_TOKEN"));
                client = new GitHubClient(new ProductHeaderValue(Environment.GetEnvironmentVariable("USER_GITHUB"))) { Credentials = credentials };
            }
            catch (Exception e)
            {
                Console.WriteLine("[TotsChallenge.Controllers.Repo.GetAuth.Error]: " + e.Message);
            }
        }

        [HttpGet("/CreateRepo")]
        public async Task<IActionResult> CreateRepo(string name)
        {
            try
            {
                string newNameRepo = RemoveSpecialCharacters(name);

                if (newNameRepo.Length > 30)
                {
                    return BadRequest("The name of the new repo add is large, try to use another. The limit is 30 characters.");
                }

                var newRepo = new NewRepository(name)
                {
                    AutoInit = true,
                    Description = "This is an new repository from source code with number: " + (DateTime.UnixEpoch.Ticks / 1000000000),
                    Private = false,
                    IsTemplate = true
                };

                this.GetAuth();

                var repositoryResponse = await client.Repository.Create(newRepo);

                return Ok("The repository was created successfully.");
            }
            catch (Exception e)
            {
                return BadRequest("[TotsChallenge.Controllers.Repo.CreateRepo.Error]: " + e.Message);
            }
        }

        [HttpPost("/DeleteRepo")]
        public async Task<IActionResult> DeleteRepo(string name)
        {
            try
            {
                this.GetAuth();

                var repositoryList = await client.Repository.GetAllForCurrent();

                foreach (var repo in repositoryList)
                {
                    if (repo.Name.Equals(name) && repo.IsTemplate == true)
                    {
                        //Only can be delete the repository when its attribute "template" is true
                        await client.Repository.Delete(repo.Id);
                        return Ok("The repository was deleted successfully.");
                    }
                }

                return BadRequest("The repository don't exist.");
            }
            catch (Exception e)
            {
                return BadRequest("[TotsChallenge.Controllers.Repo.DeleteRepo.Error]: " + e.Message);
            }
        }

        //This function only was used to control the characters's name file
        private string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^.a-zA-Z0-9_]+", "", RegexOptions.Compiled);
        }
    }
}
