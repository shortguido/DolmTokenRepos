using DolmToken.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DolmToken.Controllers
{
    public class CryptoAPI : Controller
    {
        public IActionResult Index()
        {
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(cmcAPI.makeAPICall());
            return View(myDeserializedClass);
        }
    }
}
