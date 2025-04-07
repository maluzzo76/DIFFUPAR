using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace ADO
{
    public class APIs
    {
        //https://www.youtube.com/watch?v=P522qOKYzOM&t=443s&ab_channel=CodingconC

        public APIs(string url, string token) 
        {
           string _token =  GetHttpToken("https://bmc.dev.napse.global:45503/auth/login");
            GetHttpArrayCity(url,_token);
        }
        #region <Ejemplo>

        void clientHttp()
        {
            using (var client = new HttpClient())
            {
                string _url = "http://alusoft.ddns.net:8083/api/Productos/GetProducto/5";

                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Add("Authorization", "token");
                var _response = client.GetAsync(_url).Result;
                var _res = _response.Content.ReadAsStringAsync().Result;
                dynamic _r = JObject.Parse(_res);
                Console.WriteLine(_r);
                Console.ReadLine();               
            }
        }

        void clientHttpList()
        {
            using (var client = new HttpClient())
            {
                string _url = "http://alusoft.ddns.net:8083/api/Productos/GetProducto";

                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Add("Authorization", "token");
                var _response = client.GetAsync(_url).Result;
                var _res = _response.Content.ReadAsStringAsync().Result;
                dynamic _r = JArray.Parse(_res);
                Console.WriteLine(_r);
                Console.ReadLine();
            }
        }

        #endregion

        dynamic GetHttpObject(string url, string token)
        {
            using (var client = new HttpClient())
            {
                string _url = url;

                client.DefaultRequestHeaders.Clear();
                if(token != null)
                    client.DefaultRequestHeaders.Add("Authorization", token);

                var _response = client.GetAsync(_url).Result;
                var _res = _response.Content.ReadAsStringAsync().Result;
                dynamic _r = JObject.Parse(_res);
                return _r;
            }
        }

        public dynamic PruebaKiko(string url, string token)
        {
            using (var client = new HttpClient())
            {
                string _url = url;

                client.DefaultRequestHeaders.Clear();
                if (token != null)
                    client.DefaultRequestHeaders.Add("Authorization", token);

                var _response = client.GetAsync(_url).Result;
                var _res = _response.Content.ReadAsStringAsync().Result;
                dynamic _r = JObject.Parse(_res);
                return _r;
            }
        }

        dynamic GetHttpArray(string url, string token)
        {
            using (var client = new HttpClient())
            {
                string _url = url;

                //client.DefaultRequestHeaders.Clear();
                if(token != null)
                    client.DefaultRequestHeaders.Add("Authorization",  token);

                client.DefaultRequestHeaders.ExpectContinue = false;

                var stringContent = new StringContent("{\r\n\"clientId\": \"bridge-api\",\r\n\"clientSecret\": \"DA39A3EE5E6B4B0D3255BFEF95601890AFD80709\"\r\n}", UnicodeEncoding.UTF8, "application/json"); 
                var _response = client.PostAsync(_url,stringContent).Result;
                var _res = _response.Content.ReadAsStringAsync().Result;
                dynamic _r = JObject.Parse(_res);
                return null;
            }
        }

        string GetHttpToken(string url)
        {
            using (var client = new HttpClient())
            {
                string _url = url;

                //client.DefaultRequestHeaders.Clear();
                

                client.DefaultRequestHeaders.ExpectContinue = false;

                var stringContent = new StringContent("{\r\n\"clientId\": \"bridge-api\",\r\n\"clientSecret\": \"DA39A3EE5E6B4B0D3255BFEF95601890AFD80709\"\r\n}", UnicodeEncoding.UTF8, "application/json");
                var _response = client.PostAsync(_url, stringContent).Result;
                var _res = _response.Content.ReadAsStringAsync().Result;
                dynamic _r = JObject.Parse(_res);
                string _token = _r.token.Value;
                return _token;
            }
        }

        dynamic GetHttpArrayCity(string url, string token)
        {
            using (var client = new HttpClient())
            {
                string _url = url;

                //client.DefaultRequestHeaders.Clear();
                if (token != null)
                    client.DefaultRequestHeaders.Add("x-access-token",  token);

                var stringContent = new StringContent("{\r\n\"max\":4000,\r\n\"skip\":2\r\n}", UnicodeEncoding.UTF8, "application/json");
                var _response = client.PostAsync(_url, stringContent).Result;
                var _res = _response.Content.ReadAsStringAsync().Result;
                dynamic _r = JObject.Parse(_res);
                Console.WriteLine(_r);
                return null;
            }
        }


    }
}
