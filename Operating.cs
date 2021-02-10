using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hhru
{
    //V2
    public class Operating
    {
        private static string request = "";
        private static string text;
        private List<Description> collection = new List<Description>();
        List<Description> intermediateCollection = new List<Description>();
        string path = @"C:\object.json";
        string _path = @"C:\newvacancies.json";
        public string message = "";

        public Operating(string text)
        {
            //C# == C%23 !!!

            Operating.text = text;
        }

        public static string VacanciesRequest(int i = 0)
        {
            //C# == C%23 !!!

            string answer = "";
            request = $"http://api.hh.ru/vacancies?text={text}&page={i}&per_page=100";

            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("user-agent", "Chrome");

                using (Stream stream = webClient.OpenRead(request))
                {
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        string line;

                        while ((line = streamReader.ReadLine()) != null)
                        {
                            answer += line;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);
            }

            return answer;
        }
        
        public string Parse()
        {
            if (collection != null || collection.Count > 0)
                collection.Clear();

            int pages = (int)JObject.Parse(VacanciesRequest())["pages"];

            if (pages > 0)
            {
                for (int i = 0; i < pages + 1; i++)
                {
                    for (int j = 0; j < VacanciesCounter(i); j++)
                    {
                        CollectionFillingAsync(collection, new Description(), i, j);

                        Debug.WriteLine($"page -> {i},  item -> {j}");
                    }
                }

                if (intermediateCollection.Count > 0)
                    intermediateCollection.Clear();

                if (File.Exists(path))
                {
                    var list = DeserializeJson(path);

                    
                    foreach (var item in collection)
                    {
                        bool flag = true;

                        //Parallel.ForEach()
                        foreach (var i in list)
                        {
                            if (i.Id == item.Id)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            intermediateCollection.Add(item);
                        }
                    }
                }

                message = "";

                Debug.WriteLine("DownLoaded successfully");
                SerializeJson(collection, path);
                Debug.WriteLine("Saved successfully");

                if (intermediateCollection != null && intermediateCollection.Count > 0)
                {
                    //SerializeJson(intermediateCollection, _path);
                    Debug.WriteLine("New vacancies\n");
                    message = ShowVacancies(intermediateCollection);
                }
                else
                {
                    message = "Nothing new";
                }                
            }
            else
            {
                message = "No vacancies found";
            }

            return message;
        }


        public int VacanciesCounter(int i)
        {
            bool trigger = false;
            int number = 0;

            while (!trigger)
            {
                try
                {
                    number = JObject.Parse(VacanciesRequest(i))["items"].Count();
                    trigger = true;
                }
                catch
                {
                    trigger = false;
                }
            }
            return number;
        }
        private void CollectionFilling(int i, int j)
        {
            collection.Add(new Description()
            {
                Id = (int)JObject.Parse(VacanciesRequest(i))["items"][j]["id"],
                Name = JObject.Parse(VacanciesRequest(i))["items"][j]["name"].ToString(),
                AreaName = JObject.Parse(VacanciesRequest(i))["items"][j]["area"]["name"].ToString(),
                SnipRequirement = JObject.Parse(VacanciesRequest(i))["items"][j]["snippet"]["requirement"].ToString(),
                Url = JObject.Parse(VacanciesRequest(i))["items"][j]["alternate_url"].ToString()
            });
        }

        private static async void CollectionFillingAsync(List<Description> collection, Description description, int i, int j)
        {
            Description _description = await Asyncronous(description, i, j);
            collection.Add(_description);
            //Debug.WriteLine($"page -> {i},  item -> {j}");
        }

        public static async Task<Description> Asyncronous(Description description, int i, int j)
        {
            await Task.Run(() =>
            {
                bool trigger = false;

                while (!trigger)
                {
                    try
                    {
                        description.Id = (decimal)JObject.Parse(VacanciesRequest(i))["items"][j]["id"];
                        description.Name = (string)JObject.Parse(VacanciesRequest(i))["items"][j]["name"];
                        description.AreaName = (string)JObject.Parse(VacanciesRequest(i))["items"][j]["area"]["name"];
                        description.SnipRequirement = (string)JObject.Parse(VacanciesRequest(i))["items"][j]["snippet"]["requirement"];
                        description.Url = (string)JObject.Parse(VacanciesRequest(i))["items"][j]["alternate_url"];
                        trigger = true;
                    }
                    catch (Exception exp)
                    {
                        Debug.WriteLine(exp.Message);
                    }
                }
                               
            });
            return description;
        }

        public void SerializeJson(List<Description> descriptions, string path)
        {
            string json = "";

            json = JsonConvert.SerializeObject(descriptions);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, json);
        }

        public List<Description> DeserializeJson(string path)
        {
            string json = File.ReadAllText(path);

            List<Description> list = JsonConvert.DeserializeObject<List<Description>>(json);
            
            return list;
        }

        public string ShowVacancies(List<Description> descriptions)
        {
            string vacancies = "";

            foreach (var item in descriptions)
            {
                vacancies += string.Format($"Name -> {item.Name}\nAreaName -> {item.AreaName}\n" +
                    $"SnipRequirement -> {item.SnipRequirement}\nUrl -> {item.Url}\n" + new string('-', 40) + "\n");
            }

            return vacancies;
        }
    }
}
