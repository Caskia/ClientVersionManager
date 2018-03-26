using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ClientVersionManager.Controllers
{
    public class AppController : Controller
    {
        private readonly IConfiguration _configuration;

        public AppController()
        {
            _configuration = Startup.Configuration;
        }

        public IActionResult Android()
        {
            return new JsonResult(new
            {
                downloadUrl = _configuration[$"App:{AppName}:IOS:DownloadUrl"],
                updateMessageTitle = _configuration[$"App:{AppName}:IOS:UpdateMessageTitle"],
                updateMessage = _configuration[$"App:{AppName}:IOS:UpdateMessage"],
                version = _configuration[$"App:{AppName}:IOS:Version"],
                updateTimeUtc = _configuration[$"App:{AppName}:IOS:UpdateTimeUtc"],
                platform = _configuration[$"App:{AppName}:IOS:Platform"]
            });
        }

        public IActionResult Ios()
        {
            return new JsonResult(new
            {
                downloadUrl = _configuration[$"App:{AppName}:Android:DownloadUrl"],
                updateMessageTitle = _configuration[$"App:{AppName}:Android:UpdateMessageTitle"],
                updateMessage = _configuration[$"App:{AppName}:Android:UpdateMessage"],
                version = _configuration[$"App:{AppName}:Android:Version"],
                updateTimeUtc = _configuration[$"App:{AppName}:Android:UpdateTimeUtc"],
                platform = _configuration[$"App:{AppName}:Android:Platform"]
            });
        }

        public IActionResult Status()
        {
            return new JsonResult(new
            {
                a = bool.Parse(_configuration[$"App:{AppName}:Status"])
            });
        }

        #region Private Methods

        private string AppName
        {
            get
            {
                if (RouteData.Values.TryGetValue("appName", out var value))
                {
                    return value.ToString();
                }
                return null;
            }
        }

        #endregion Private Methods
    }
}