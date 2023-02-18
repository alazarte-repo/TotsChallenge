using Microsoft.AspNetCore.Mvc;
using Octokit;
using System;

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
    }
}
