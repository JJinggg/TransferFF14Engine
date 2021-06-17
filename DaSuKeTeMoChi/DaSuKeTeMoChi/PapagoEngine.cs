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
using DaSuKeTeMoChi.JsonProperty;

namespace DaSuKeTeMoChi
{
    public class PapagoEngine
    {
        static string base_s = "";

        private ScriptEngine Script;
        CookieContainer cookie = new CookieContainer();
          
        public PapagoEngine()
        {
            base_s = UtilityClassDLL.Conversion.Base64Decoding("ZnVuY3Rpb24gRW5jb2RlVHJhbnNhbHRpb25SZXF1ZXN0KHJlcXZTdHIpIHsNCg0KICAgIHZhciBndWlkID0gR2V0UGFwZ29HdWlkKCk7DQogICAgdmFyIGd1aWRUaW1lID0gKG5ldyBEYXRlKS5nZXRUaW1lKCkgLSBnZXRSYW5kb21JbnQoNTAwLCAxNTAwKTsNCg0KICAgIHZhciBobWFjS2V5ID0gInYxLjUuOV8zM2U1M2JlODBmIjsNCiAgICB2YXIgaG1hY0lucHV0ID0gZ3VpZCArICJcbiIgKyAiaHR0cHM6Ly9wYXBhZ28ubmF2ZXIuY29tL2FwaXMvbjJtdC90cmFuc2xhdGUiICsgIlxuIiArIGd1aWRUaW1lLnRvU3RyaW5nKCk7DQoNCiAgICB2YXIgcmVxdk9iaiA9IEpTT04ucGFyc2UocmVxdlN0cik7DQoNCiAgICByZXF2T2JqLmRldmljZUlkID0gZ3VpZDsNCg0KICAgIHZhciBzdHJpbmdSZXF2ID0gc2VyaWFsaXplQm9keShyZXF2T2JqKTsNCg0KICAgIHZhciByZXN1bHQgPSB7fTsNCg0KICAgIHJlc3VsdC5zdHJpbmdSZXF2ID0gc3RyaW5nUmVxdjsNCiAgICByZXN1bHQuaG1hY0tleSA9IGhtYWNLZXk7DQogICAgcmVzdWx0LmhtYWNJbnB1dCA9IGhtYWNJbnB1dDsNCiAgICByZXN1bHQuZ3VpZCA9IGd1aWQ7DQogICAgcmVzdWx0Lmd1aWRUaW1lID0gZ3VpZFRpbWU7DQoNCiAgICByZXR1cm4gSlNPTi5zdHJpbmdpZnkocmVzdWx0KTsNCn0NCg0KZnVuY3Rpb24gR2V0SG1hY0luUHV0KCkNCnsNCiAgIHZhciBndWlkID0gR2V0UGFwZ29HdWlkKCk7DQogICB2YXIgZ3VpZFRpbWUgPSAobmV3IERhdGUpLmdldFRpbWUoKSAtIGdldFJhbmRvbUludCg1MDAsIDE1MDApOw0KICAgdmFyIGhtYWNJbnB1dCA9IGd1aWQgKyAiXG4iICsgImh0dHBzOi8vcGFwYWdvLm5hdmVyLmNvbS9hcGlzL24ybXQvdHJhbnNsYXRlIiArICJcbiIgKyBndWlkVGltZS50b1N0cmluZygpOw0KDQogICByZXR1cm4gaG1hY0lucHV0IDsNCn0NCg0KDQoNCg0KZnVuY3Rpb24gc2VyaWFsaXplQm9keShvYmopIHsNCg0KICAgIHJldHVybiBPYmplY3Qua2V5cyhvYmopLm1hcChmdW5jdGlvbiAodCkgew0KICAgICAgICB2YXIgYzEgPSBlbmNvZGVVUklDb21wb25lbnQodCk7DQogICAgICAgIHZhciBjMiA9ICIiOw0KDQogICAgICAgIGlmIChjMSAhPSAidGV4dCIpDQogICAgICAgICAgICBjMiA9IGVuY29kZVVSSUNvbXBvbmVudChvYmpbdF0pOw0KICAgICAgICBlbHNlDQogICAgICAgICAgICBjMiA9IG9ialt0XTsNCg0KICAgICAgICByZXR1cm4gYzEgKyAiPSIgKyBjMjsNCiAgICB9KS5qb2luKCImIik7DQp9DQoNCg0KZnVuY3Rpb24gZW5jb2RlVVJJQ29tcG9uZW50KHN0cikgew0KICAgIHZhciBoZXhEaWdpdHMgPSAnMDEyMzQ1Njc4OUFCQ0RFRic7DQogICAgdmFyIHJldCA9ICcnOw0KICAgIGZvciAodmFyIGkgPSAwOyBpIDwgc3RyLmxlbmd0aDsgaSsrKSB7DQogICAgICAgIHZhciBjID0gc3RyLmNoYXJDb2RlQXQoaSk7DQogICAgICAgIGlmICgoYyA+PSA0OC8qMCovICYmIGMgPD0gNTcvKjkqLykgfHwNCiAgICAgICAgICAgIChjID49IDk3LyphKi8gJiYgYyA8PSAxMjIvKnoqLykgfHwNCiAgICAgICAgICAgIChjID49IDY1LypBKi8gJiYgYyA8PSA5MC8qWiovKSB8fA0KICAgICAgICAgICAgYyA9PSA0NS8qLSovIHx8IGMgPT0gOTUvKl8qLyB8fCBjID09IDQ2LyouKi8gfHwgYyA9PSAzMy8qISovIHx8IGMgPT0gMTI2Lyp+Ki8gfHwNCiAgICAgICAgICAgIGMgPT0gNDIvKioqLyB8fCBjID09IDkyLypcXCovIHx8IGMgPT0gNDAvKigqLyB8fCBjID09IDQxLyopKi8pIHsNCiAgICAgICAgICAgIHJldCArPSBzdHJbaV07DQogICAgICAgIH0NCiAgICAgICAgZWxzZSB7DQogICAgICAgICAgICByZXQgKz0gJyUnOw0KICAgICAgICAgICAgcmV0ICs9IGhleERpZ2l0c1soYyAmIDB4RjApID4+IDRdOw0KICAgICAgICAgICAgcmV0ICs9IGhleERpZ2l0c1soYyAmIDB4MEYpXTsNCiAgICAgICAgfQ0KICAgIH0NCiAgICByZXR1cm4gcmV0Ow0KfTsNCg0KZnVuY3Rpb24gR2V0UGFwZ29HdWlkKCkgew0KDQogICAgdmFyIGEgPSAobmV3IERhdGUpLmdldFRpbWUoKTsNCg0KICAgIGUgPSAieHh4eHh4eHgteHh4eC00eHh4LXl4eHgteHh4eHh4eHh4eHh4Ii5yZXBsYWNlKC9beHldL2csIGZ1bmN0aW9uIChlKSB7DQogICAgICAgIHZhciB0ID0gKGEgKyAxNiAqIE1hdGgucmFuZG9tKCkpICUgMTYgfCAwOw0KICAgICAgICByZXR1cm4gYSA9IE1hdGguZmxvb3IoYSAvIDE2KSwgKCJ4IiA9PT0gZSA/IHQgOiAzICYgdCB8IDgpLnRvU3RyaW5nKDE2KQ0KICAgIH0pOw0KDQogICAgcmV0dXJuIGU7DQp9DQoNCmZ1bmN0aW9uIGdldFJhbmRvbUludChtaW4sIG1heCkgew0KICAgIG1pbiA9IE1hdGguY2VpbChtaW4pOw0KICAgIG1heCA9IE1hdGguZmxvb3IobWF4KTsNCiAgICByZXR1cm4gTWF0aC5mbG9vcihNYXRoLnJhbmRvbSgpICogKG1heCAtIG1pbikpICsgbWluOyAvLwQcBDAEOgRBBDgEPARDBDwgBD0ENSAEMgQ6BDsETgRHBDAENQRCBEEETywgBDwEOAQ9BDgEPARDBDwgBDIEOgQ7BE4ERwQwBDUEQgRBBE8NCn0NCg==");
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





        public async Task<Tuple<string,string>> PostAsync(string Text,string Region)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                Text = UtilityClassDLL.Conversion.HttpUrlEncoding(Text);
                string Source;
                string Target;
                try
                {

                    System.Diagnostics.Debug.WriteLine("Region>>>>" + Region);
                    if (Region == "Kor")
                    {
                        Source = "ja";
                        Target = "ko";
                         

                    }
                    else
                    {

                        Source = "ko";
                        Target = "ja";
                    }


                    if (Region == "Kor")
                    {
                        Source = "ja";
                        Target = "ko";
                    }
                    else if (Region == "Jp")
                    {

                        Source = "ko";
                        Target = "ja";
                    }
                    else if (Region == "KE")
                    {

                        Source = "ko";
                        Target = "en";
                    }
                    else if (Region == "EK")
                    {
                        Source = "en";
                        Target = "ko";
                    }
                    else
                    {
                        Source = "ko";
                        Target = "ja";
                    }


                    EncodeJsonProperty EncodeProperty = JsonConvert.DeserializeObject<EncodeJsonProperty>
                    (this.Script.CallGlobalFunction<string>("EncodeTransaltionRequest",
                    (object)JsonConvert.SerializeObject((object)senddata)));
                    string id = EncodeProperty.Guid; //this.Script.CallGlobalFunction<string>("GetPapgoGuid");
                    string key = EncodeProperty.HmacKey; //"v1.5.1_4dfe1d83c2";
                    string hmacInput = EncodeProperty.HmacInput;  //this.Script.CallGlobalFunction<string>("GetHmacInPut");
                    string Hmacincode = PapagoHmacFin(hmacInput, key);
                    string AuthHeader = string.Format($"PPG {id}:{Hmacincode}");



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
                        $"text={Text}&"+
                        $"authroization={AuthHeader}&"+
                        $"timestamp={EncodeProperty.GuidTime}";

                    using (Webformat webformat = new Webformat("https://papago.naver.com/apis/n2mt/translate"))
                    {
                        request = webformat.CreateNewFormat(cookie, CRUD.Post, Accept.OnlyJson, ContentType.X_www_form_A_UTF8, Certificate.True
                            , CreateHeader.CreateHeaderCollection(HeaderValue_Flags.Sec_Fetch_Dest_Empty, HeaderValue_Flags.Sec_Fetch_Mode_cors, HeaderValue_Flags.Sec_Fetch_Site_same_origin));

                        request.Referer = "https://papago.naver.com/";
                        request.Headers.Add("Timestamp", EncodeProperty.GuidTime);
                        request.Headers.Add("device-type", "pc");
                        request.Headers.Add("x-apigw-partnerid", "papago");
                        request.Headers.Add("Authorization", AuthHeader);
                        request.Headers.Add("Accept-Language", "ko");
                        request.Headers.Add("Origin", "https://papago.naver.com");
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.104 Safari/537.36";

                        request.Timeout = 4000;
                        response = await webformat.GetResponseAsync(request, sendData);
                    }
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string returnHtmlString = HttpWebIO.ReturnStrHtml(response.GetResponseStream(), Encoding.UTF8);
                        string TransText = StrCut.StrChange(returnHtmlString, "translatedText\":\"", "\"", false);

                        request?.Abort();
                        response?.Close();
                        string ttsCutText = StrCut.StrChange(returnHtmlString, "tlit\":{\"message", "delay", false);
                        string[] ttscutarray = StrCut.ArrSplit(ttsCutText, "phoneme\":\"");
                        string ttsText = "";
                        int length = ttscutarray.Length;
                        for (ushort i = 1; i < length; i++)
                        {
                            ttsText += $"{ StrCut.StrChange(ttscutarray[i], null, "\"", false)} ";
                        }
                        System.Diagnostics.Debug.WriteLine(ttsText);
                        return Tuple.Create(TransText, ttsText);
                    }
                    request?.Abort();
                    response?.Close();
                    
                }
                catch (Exception e)
                {
                    request?.Abort();
                    response?.Close();
                    //DebugLog.e(e);
                }
                return null;
            }
            catch
            {
                
                return null;
            }
            finally
            {
                request?.Abort();
                response?.Close();
            }
        }

        public async Task<string> TrueEmote(string Text, string Region)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
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
                    else if (Region == "Jp")
                    {
                        Source = "ja";
                        Target = "ko";
                    }
                    else if (Region == "KE")
                    {
                        Source = "en";
                        Target = "ko";
                    }
                    else if (Region == "EK")
                    {
                        Source = "ko";
                        Target = "en";
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

                        request.Headers.Add("Timestamp", EncodeProperty.GuidTime);
                        request.Headers.Add("device-type", "pc");
                        request.Headers.Add("x-apigw-partnerid", "papago");
                        request.Headers.Add("Authorization", AuthHeader);
                        request.Headers.Add("Accept-Language", "ko");

                        request.Timeout = 4000;
                        response = await webformat.GetResponseAsync(request, sendData);
                    }
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string returnHtmlString = HttpWebIO.ReturnStrHtml(response.GetResponseStream(), Encoding.UTF8);
                        string TransText = StrCut.StrChange(returnHtmlString, "translatedText\":\"", "\"", false);
                        return TransText;
                    }
                }
                catch (Exception e)
                {
                    DebugLog.e(e);
                }
                return null;
            }
            catch
            { return null; }

            finally
            {
                request?.Abort();
                response?.Close();
            }
        }

    }
}
