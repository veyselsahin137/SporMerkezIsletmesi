using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    //  Sadece giriş yapmış kullanıcılar (Admin + Üye) erişebilsin
    [Authorize]
    public class YapayZekaController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public YapayZekaController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        // GET: /YapayZeka/Oneri
        [HttpGet]
        public IActionResult Oneri()
        {
            // Boş model ile formu göster
            return View(new YapayZekaOneriViewModel());
        }

        // POST: /YapayZeka/Oneri
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Oneri(YapayZekaOneriViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Form hatalıysa tekrar göster
                return View(model);
            }

            // appsettings.Development.json içinden API key'i oku
            var apiKey = _configuration["OpenAI:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                model.YapayZekaCevap =
                    "⚠️ Yapay zekâ API anahtarı tanımlanmamış. Lütfen sistem yöneticisine haber verin.";
                return View(model);
            }

            // İstek için metin (prompt)
            var girdiMetni = $@"
Boy: {model.BoyCm} cm,
Kilo: {model.KiloKg} kg,
Hedef: {model.Hedef}
Ek bilgi: {model.EkBilgi}

Bu kullanıcı için Türkçe ve madde madde olacak şekilde:
- 5–7 maddelik egzersiz önerisi,
- Kısa yaşam tarzı tavsiyeleri
hazırla. Çok teknik olma, sade ve uygulanabilir olsun.
";

            // HTTP istemcisi
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.openai.com/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            // Chat Completions isteği gövdesi
            var body = new
            {
                model = "gpt-4.1-mini",   
                messages = new[]
                {
                    new { role = "system", content = "Sen bir spor ve beslenme uzmanısın. Kısa, net ve uygulanabilir öneriler ver." },
                    new { role = "user",   content = girdiMetni }
                },
                max_tokens = 300
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("v1/chat/completions", content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                model.YapayZekaCevap =
                    $"⚠️ API isteği başarısız oldu. Durum kodu: {httpResponse.StatusCode}";
                return View(model);
            }

            var responseString = await httpResponse.Content.ReadAsStringAsync();

            // Cevabı JSON'dan çek
            string cevap = "Cevap çözümlenemedi.";

            try
            {
                using var doc = JsonDocument.Parse(responseString);
                var root = doc.RootElement;

                var choices = root.GetProperty("choices");
                if (choices.GetArrayLength() > 0)
                {
                    var message = choices[0].GetProperty("message");

                    // Yeni format: content bir array olabilir
                    if (message.TryGetProperty("content", out var contentElement))
                    {
                        if (contentElement.ValueKind == JsonValueKind.Array)
                        {
                            // content[0].text.value
                            var first = contentElement[0];
                            if (first.TryGetProperty("text", out var textObj) &&
                                textObj.TryGetProperty("value", out var valueObj))
                            {
                                cevap = valueObj.GetString() ?? cevap;
                            }
                        }
                        else if (contentElement.ValueKind == JsonValueKind.String)
                        {
                            // Eski basit string formatı
                            cevap = contentElement.GetString() ?? cevap;
                        }
                    }
                }
            }
            catch
            {
                // Hata olursa en azından ham JSON'u gösterme, basit bir mesaj bırak
                cevap = "Yapay zekâ cevabı okunurken bir hata oluştu.";
            }

            model.YapayZekaCevap = cevap;
            return View(model);
        }
    }
}
