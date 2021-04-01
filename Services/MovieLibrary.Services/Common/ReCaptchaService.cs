namespace MovieLibrary.Services.Common
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;
    using MovieLibrary.Services.Models;
    using Newtonsoft.Json;

    using static MovieLibrary.Common.GlobalConstants;

    public class ReCaptchaService
    {
        private readonly ReCaptchaSettings settings;

        public ReCaptchaService(IOptions<ReCaptchaSettings> settings)
        {
            this.settings = settings.Value;
        }

        public virtual async Task<GoogleResponse> ValidateResponse(string token)
        {
            GoogleReCapthaData data = new GoogleReCapthaData
            {
                ResponseToken = token,
                SecretKey = this.settings.ReCAPTCHA_Secret_Key,
            };

            HttpClient httpClient = new HttpClient();
            var response = await httpClient
                .GetStringAsync(
                ReCaptchaVerifyLink + $"?secret={data.SecretKey}&response={data.ResponseToken}");

            var capturedResponse = JsonConvert.DeserializeObject<GoogleResponse>(response);

            return capturedResponse;
        }
    }
}
