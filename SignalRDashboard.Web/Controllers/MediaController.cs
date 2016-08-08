using System;
using System.Web.Mvc;
using SignalRDashboard.Web.Utilities;

namespace SignalRDashboard.Web.Controllers
{
    public class MediaController : Controller
    {
        private readonly IFilePathToUrlConverter _filePathConverter;
        private readonly ISoundFilePicker _soundFilePicker;

        public MediaController(IFilePathToUrlConverter filePathConverter, ISoundFilePicker soundFilePicker)
        {
            _filePathConverter = filePathConverter;
            _soundFilePicker = soundFilePicker;
        }

        [HttpGet]
        public JsonResult GetRandomErrorSound(string component)
        {
            return GetRandomSoundFile(component, SoundFileCategory.Error, null);
        }

        [HttpGet]
        public JsonResult GetRandomSuccessSound(string component)
        {
            return GetRandomSoundFile(component, SoundFileCategory.Success, null);
        }

        [HttpGet]
        public JsonResult GetCustomErrorSound(string component, string customText)
        {
            return GetRandomSoundFile(component, SoundFileCategory.Error, customText);
        }

        private JsonResult GetRandomSoundFile(string component, SoundFileCategory category, string customText)
        {
            var keyword = KeywordSelector(customText);
            var file = _soundFilePicker.GetRandomSoundFile(component, category, keyword);
            var relative = _filePathConverter.ToFullWebUrl(file);
            return Json(Url.Content(relative),
                JsonRequestBehavior.AllowGet);           
        }

        private string KeywordSelector(string customText)
        {
            if (customText.Contains("coffee") || customText.Contains("cafe"))
            {
                return "Coffee";
            }

            if (customText.Contains("curry"))
            {
                return "Curry";
            }

            if (customText.Contains("french") || customText.Contains("terrace"))
            {
                return "French";
            }

            if (customText.Contains("ice") || customText.Contains("cream"))
            {
                return "Ice";
            }

            if (customText.Contains("fire") || customText.Contains("alarm"))
            {
                return "Fire";
            }

            return "AirHorn";
        }
    }
}