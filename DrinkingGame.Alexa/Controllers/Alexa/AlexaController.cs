using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DrinkingGame.WebService.Speechlets;

namespace DrinkingGame.WebService.Controllers.Alexa
{
    public class AlexaController : ApiController
    {
        public async Task<HttpResponseMessage> Post()
        {
            var speechlet = new GameSpeechlet();
            return await speechlet.GetResponseAsync(Request);
        }
    }
}
