using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebformatInjection.WebNeedDefualtHeader;
using WebformatInjection;
using UtilityClassDLL;
using System.Net;
using Jurassic;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace DaSuKeTeMoChi
{
    public class PapagoEngine
    {
        static string base_s = "";

        private ScriptEngine Script;
        CookieContainer cookie = new CookieContainer();

        public PapagoEngine()
        {
            base_s = UtilityClassDLL.Conversion.Base64Decoding("ZnVuY3Rpb24gRW5jb2RlVHJhbnNhbHRpb25SZXF1ZXN0KHJlcXZTdHIpIHsKCiAgICB2YXIgZ3VpZCA9IEdldFBhcGdvR3VpZCgpOwogICAgdmFyIGd1aWRUaW1lID0gKG5ldyBEYXRlKS5nZXRUaW1lKCkgLSBnZXRSYW5kb21JbnQoNTAwLCAxNTAwKTsKCiAgICB2YXIgaG1hY0tleSA9ICJ2MS41LjFfNGRmZTFkODNjMiI7CiAgICB2YXIgaG1hY0lucHV0ID0gZ3VpZCArICJcbiIgKyAiaHR0cHM6Ly9wYXBhZ28ubmF2ZXIuY29tL2FwaXMvbjJtdC90cmFuc2xhdGUiICsgIlxuIiArIGd1aWRUaW1lLnRvU3RyaW5nKCk7CgogICAgdmFyIHJlcXZPYmogPSBKU09OLnBhcnNlKHJlcXZTdHIpOwoKICAgIHJlcXZPYmouZGV2aWNlSWQgPSBndWlkOwoKICAgIHZhciBzdHJpbmdSZXF2ID0gc2VyaWFsaXplQm9keShyZXF2T2JqKTsKCiAgICB2YXIgcmVzdWx0ID0ge307CgogICAgcmVzdWx0LnN0cmluZ1JlcXYgPSBzdHJpbmdSZXF2OwogICAgcmVzdWx0LmhtYWNLZXkgPSBobWFjS2V5OwogICAgcmVzdWx0LmhtYWNJbnB1dCA9IGhtYWNJbnB1dDsKICAgIHJlc3VsdC5ndWlkID0gZ3VpZDsKICAgIHJlc3VsdC5ndWlkVGltZSA9IGd1aWRUaW1lOwoKICAgIHJldHVybiBKU09OLnN0cmluZ2lmeShyZXN1bHQpOwp9CgpmdW5jdGlvbiBHZXRIbWFjSW5QdXQoKQp7CiAgIHZhciBndWlkID0gR2V0UGFwZ29HdWlkKCk7CiAgIHZhciBndWlkVGltZSA9IChuZXcgRGF0ZSkuZ2V0VGltZSgpIC0gZ2V0UmFuZG9tSW50KDUwMCwgMTUwMCk7CiAgIHZhciBobWFjSW5wdXQgPSBndWlkICsgIlxuIiArICJodHRwczovL3BhcGFnby5uYXZlci5jb20vYXBpcy9uMm10L3RyYW5zbGF0ZSIgKyAiXG4iICsgZ3VpZFRpbWUudG9TdHJpbmcoKTsKCiAgIHJldHVybiBobWFjSW5wdXQgOwp9CgoKCgpmdW5jdGlvbiBzZXJpYWxpemVCb2R5KG9iaikgewoKICAgIHJldHVybiBPYmplY3Qua2V5cyhvYmopLm1hcChmdW5jdGlvbiAodCkgewogICAgICAgIHZhciBjMSA9IGVuY29kZVVSSUNvbXBvbmVudCh0KTsKICAgICAgICB2YXIgYzIgPSAiIjsKCiAgICAgICAgaWYgKGMxICE9ICJ0ZXh0IikKICAgICAgICAgICAgYzIgPSBlbmNvZGVVUklDb21wb25lbnQob2JqW3RdKTsKICAgICAgICBlbHNlCiAgICAgICAgICAgIGMyID0gb2JqW3RdOwoKICAgICAgICByZXR1cm4gYzEgKyAiPSIgKyBjMjsKICAgIH0pLmpvaW4oIiYiKTsKfQoKCmZ1bmN0aW9uIGVuY29kZVVSSUNvbXBvbmVudChzdHIpIHsKICAgIHZhciBoZXhEaWdpdHMgPSAnMDEyMzQ1Njc4OUFCQ0RFRic7CiAgICB2YXIgcmV0ID0gJyc7CiAgICBmb3IgKHZhciBpID0gMDsgaSA8IHN0ci5sZW5ndGg7IGkrKykgewogICAgICAgIHZhciBjID0gc3RyLmNoYXJDb2RlQXQoaSk7CiAgICAgICAgaWYgKChjID49IDQ4LyowKi8gJiYgYyA8PSA1Ny8qOSovKSB8fAogICAgICAgICAgICAoYyA+PSA5Ny8qYSovICYmIGMgPD0gMTIyLyp6Ki8pIHx8CiAgICAgICAgICAgIChjID49IDY1LypBKi8gJiYgYyA8PSA5MC8qWiovKSB8fAogICAgICAgICAgICBjID09IDQ1LyotKi8gfHwgYyA9PSA5NS8qXyovIHx8IGMgPT0gNDYvKi4qLyB8fCBjID09IDMzLyohKi8gfHwgYyA9PSAxMjYvKn4qLyB8fAogICAgICAgICAgICBjID09IDQyLyoqKi8gfHwgYyA9PSA5Mi8qXFwqLyB8fCBjID09IDQwLyooKi8gfHwgYyA9PSA0MS8qKSovKSB7CiAgICAgICAgICAgIHJldCArPSBzdHJbaV07CiAgICAgICAgfQogICAgICAgIGVsc2UgewogICAgICAgICAgICByZXQgKz0gJyUnOwogICAgICAgICAgICByZXQgKz0gaGV4RGlnaXRzWyhjICYgMHhGMCkgPj4gNF07CiAgICAgICAgICAgIHJldCArPSBoZXhEaWdpdHNbKGMgJiAweDBGKV07CiAgICAgICAgfQogICAgfQogICAgcmV0dXJuIHJldDsKfTsKCmZ1bmN0aW9uIEdldFBhcGdvR3VpZCgpIHsKCiAgICB2YXIgYSA9IChuZXcgRGF0ZSkuZ2V0VGltZSgpOwoKICAgIGUgPSAieHh4eHh4eHgteHh4eC00eHh4LXl4eHgteHh4eHh4eHh4eHh4Ii5yZXBsYWNlKC9beHldL2csIGZ1bmN0aW9uIChlKSB7CiAgICAgICAgdmFyIHQgPSAoYSArIDE2ICogTWF0aC5yYW5kb20oKSkgJSAxNiB8IDA7CiAgICAgICAgcmV0dXJuIGEgPSBNYXRoLmZsb29yKGEgLyAxNiksICgieCIgPT09IGUgPyB0IDogMyAmIHQgfCA4KS50b1N0cmluZygxNikKICAgIH0pOwoKICAgIHJldHVybiBlOwp9CgpmdW5jdGlvbiBnZXRSYW5kb21JbnQobWluLCBtYXgpIHsKICAgIG1pbiA9IE1hdGguY2VpbChtaW4pOwogICAgbWF4ID0gTWF0aC5mbG9vcihtYXgpOwogICAgcmV0dXJuIE1hdGguZmxvb3IoTWF0aC5yYW5kb20oKSAqIChtYXggLSBtaW4pKSArIG1pbjsgLy8EHAQwBDoEQQQ4BDwEQwQ8IAQ9BDUgBDIEOgQ7BE4ERwQwBDUEQgRBBE8sIAQ8BDgEPQQ4BDwEQwQ8IAQyBDoEOwROBEcEMAQ1BEIEQQRPCn0K");
            Script = new ScriptEngine();
            this.Script.Evaluate(base_s);

        }
        public object senddata = new SendDataRequest()
            {
                deviceId = "",
                dict = false,
                dictDisplay = 0L,
                honorific = false,
                instant = false,
                paging = false,
                source = "ko",
                target = "ja",
                locale = "ko-KR",
                text = "asfed"
            };

        private static string PapagoHmacFin(string plaintext, string transactionKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
            using (HMACMD5 hmacmD5 = new HMACMD5(Encoding.UTF8.GetBytes(transactionKey)))
                return Convert.ToBase64String(hmacmD5.ComputeHash(bytes));
        }





        public async Task<string> PostAsync(string Text,string Region)
        {

            Text = UtilityClassDLL.Conversion.HttpUrlEncoding(Text);
            string Source;
            string Target;
            try
            {
                if (Region == "Kor")
                {
                    Source = "ko";
                    Target = "ja";

                }
                else
                {
                    Source = "ja";
                    Target = "ko";
                }


                EncodeJsonProperty EncodeProperty = JsonConvert.DeserializeObject<EncodeJsonProperty>
                (this.Script.CallGlobalFunction<string>("EncodeTransaltionRequest", 
                (object)JsonConvert.SerializeObject((object)senddata)));
                string id = EncodeProperty.Guid; //this.Script.CallGlobalFunction<string>("GetPapgoGuid");
                string key = EncodeProperty.HmacKey; //"v1.5.1_4dfe1d83c2";
                string hmacInput = EncodeProperty.HmacInput;  //this.Script.CallGlobalFunction<string>("GetHmacInPut");
                string Hmacincode = PapagoHmacFin(hmacInput, key);
                string AuthHeader = string.Format($"PPG {id}:{Hmacincode}");

                HttpWebRequest request = null;
                HttpWebResponse response = null;

                DateTime date = DateTime.Now;
                long TimeStamp = (long)date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                string sendData = $"" +
                    $"deviceId={id}&" +
                    $"locale=ko&" +
                    $"dict=true&" +
                    $"dictDisplay=30&" +
                    $"honorific=false&" +
                    $"instant=false&" +
                    $"paging=false&" +
                    $"source={Source}&" +
                    $"target={Target}&" +
                    $"text={Text}";

                using (Webformat webformat = new Webformat("https://papago.naver.com/apis/n2mt/translate"))
                {
                    request = webformat.CreateNewFormat(cookie, CRUD.Post, Accept.OnlyJson, ContentType.X_www_form_A_UTF8, Certificate.True
                        , CreateHeader.CreateHeaderCollection(HeaderValue_Flags.Sec_Fetch_Dest_Empty, HeaderValue_Flags.Sec_Fetch_Mode_cors, HeaderValue_Flags.Sec_Fetch_Site_same_origin));

                    request.Headers.Add("Timestamp",EncodeProperty.GuidTime ); 
                    request.Headers.Add("device-type", "pc");
                    request.Headers.Add("x-apigw-partnerid", "papago");
                    request.Headers.Add("Authorization", AuthHeader);
                    request.Timeout = 4000;
                    response = await webformat.GetResponseAsync(request, sendData);
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string returnHtmlString = HttpWebIO.ReturnStrHtml(response.GetResponseStream(), Encoding.UTF8);
                    string TransText = StrCut.StrChange(returnHtmlString, "translatedText\":\"", "\"",false);
                    return TransText;
                }
            }
            catch(Exception e)
            {
                DebugLog.e(e);
            }
            return null;

        }



    }
}
