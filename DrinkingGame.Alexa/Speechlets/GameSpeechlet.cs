using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.UI;
using NLog;

namespace DrinkingGame.WebService.Speechlets
{
    public class GameSpeechlet: ISpeechletAsync
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public bool OnRequestValidation(SpeechletRequestValidationResult result, DateTime referenceTimeUtc,
            SpeechletRequestEnvelope requestEnvelope)
        {
            Log.Info("OnRequestValidation requestId={0}, result={1}", requestEnvelope.Request.RequestId, result);
            return result == SpeechletRequestValidationResult.OK;
        }

        public async Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session)
        {
            Log.Info("OnIntent requestId={0}, sessionId={1}", intentRequest.RequestId, session.SessionId);
            throw new SpeechletException("Invalid Intent");
        }

        public async Task<SpeechletResponse> OnLaunchAsync(LaunchRequest launchRequest, Session session)
        {
            Log.Info("OnLaunch requestId={0}, sessionId={1}", launchRequest.RequestId, session.SessionId);
            return GetWelcomeResponse();
        }

        public async Task OnSessionStartedAsync(SessionStartedRequest sessionStartedRequest, Session session)
        {
            Log.Info("OnSessionStarted requestId={0}, sessionId={1}", sessionStartedRequest.RequestId, session.SessionId);
        }

        public async Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session)
        {
            Log.Info("OnSessionEnded requestId={0}, sessionId={1}", sessionEndedRequest.RequestId, session.SessionId);
        }

        private SpeechletResponse GetWelcomeResponse()
        {
            // Create the welcome message.
            string speechOutput =
                "Ein herzliches Prost von Poldi";

            // Here we are setting shouldEndSession to false to not end the session and
            // prompt the user for input
            return BuildSpeechletResponse("Welcome", speechOutput, false);
        }

        private SpeechletResponse BuildSpeechletResponse(string title, string output, bool shouldEndSession)
        {
            // Create the Simple card content.
            SimpleCard card = new SimpleCard
            {
                Title = $"SessionSpeechlet - {title}",
                Content = $"SessionSpeechlet - {output}"
            };

            // Create the plain text output.
            PlainTextOutputSpeech speech = new PlainTextOutputSpeech {Text = output};

            // Create the speechlet response.
            return new SpeechletResponse
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = speech,
                Card = card
            };
        }
    }
}