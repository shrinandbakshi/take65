using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserWidgetWeather
    /// </summary>
    public class UserWidgetWeather : REST
    {


        protected override void Get(string parameter)
        {
            if (parameter.ToUpper() == "CURRENTWEATHER") //New
            {
                string address = this.GetParameterValue("p2").ToString();
                if (!string.IsNullOrEmpty(address))
                {
                    //Check if already have Lat Long for this zip code on cache if not, add to
                    if (HttpRuntime.Cache["GeoCode." + address.Replace(" ", "")] == null)
                    {
                        var client = new WebClient();
                        string html = client.DownloadString(string.Format(ConfigurationManager.AppSettings["Google.Maps.GeoCode"], address));
                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        dynamic jsonReturn =
                               (object)json_serializer.DeserializeObject(html);

                        if (jsonReturn["results"].Length > 0)
                        {
                            Model.REST.GeoLocation geo = new Model.REST.GeoLocation();
                            geo.zipcode = ExtractZipCode(jsonReturn["results"][0]["address_components"]);
                            geo.location = jsonReturn["results"][0]["formatted_address"];
                            geo.lat = jsonReturn["results"][0]["geometry"]["location"]["lat"].ToString();
                            geo.lng = jsonReturn["results"][0]["geometry"]["location"]["lng"].ToString();
                            geo.lastupdate = DateTime.Now;

                            HttpRuntime.Cache.Insert("GeoCode." + address.Replace(" ", ""), geo, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);

                        }
                    }
                    //Check if already have Lat Long for this zip code on cache if not, add to


                    if (HttpRuntime.Cache["GeoCode." + address.Replace(" ", "")] != null)
                    {
                        Model.REST.GeoLocation geo = (Model.REST.GeoLocation)HttpRuntime.Cache["GeoCode." + address.Replace(" ", "")];
                        Model.User user = this.GetSessionUser();
                        if (user != null)
                        {
                            if (this.GetParameterValue("WidgetId") != null)
                            {
                                if (!string.IsNullOrEmpty(this.GetParameterValue("WidgetId")))
                                {
                                    long widgetId = Convert.ToInt64(this.GetParameterValue("WidgetId"));
                                    Model.Widget.ExtraInfo.Weather weatherUserOption = new Model.Widget.ExtraInfo.Weather();
                                    weatherUserOption.PreferredZipCode = geo.zipcode;
                                    Bll.UserWidget bllUserWidget = new Bll.UserWidget();
                                    bllUserWidget.SaveExtraInfo(user.Id, widgetId, Model.Util.SerializeObject(weatherUserOption, typeof(Model.Widget.ExtraInfo.Weather)));

                                }
                            }
                        }
                                
                        
                        if (!string.IsNullOrEmpty(geo.lat) && !string.IsNullOrEmpty(geo.lng))
                        {
                            string latlng = geo.lat + "," + geo.lng;
                            if (HttpRuntime.Cache["Weather." + latlng] == null)
                            {
                                 var client = new WebClient();
                                 string html = client.DownloadString(string.Format(ConfigurationManager.AppSettings["ForecastIO.REST"], latlng));
                                 JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                                 dynamic jsonReturn =
                                        (object)json_serializer.DeserializeObject(html);

                                 geo.timezone = jsonReturn.ContainsKey("timezone") ? jsonReturn["timezone"] : null;
                                 geo.offset = jsonReturn.ContainsKey("offset") ? Convert.ToInt32(jsonReturn["offset"]) : null;

                                 if (jsonReturn["daily"]["data"].Length > 0)
                                 {
                                     
                                     List<Model.REST.Weather> ltWeather = new List<Model.REST.Weather>();
                                     foreach (var weather in jsonReturn["daily"]["data"])
                                     {
                                         Model.REST.Weather restWeather = new Model.REST.Weather();
                                         
                                         restWeather.icon = weather["icon"];
                                         restWeather.lastcache = DateTime.Now;
                                         restWeather.tempMax = Math.Round(Convert.ToDouble(weather["temperatureMax"]));
                                         restWeather.tempMin = Math.Round(Convert.ToDouble(weather["temperatureMin"]));
                                         restWeather.time = Convert.ToInt64(weather["time"]);
                                         restWeather.summary = weather["summary"];

                                         restWeather.sunriseTime = weather.ContainsKey("sunriseTime") ? Convert.ToInt64(weather["sunriseTime"]) : null;
                                         restWeather.sunsetTime = weather.ContainsKey("sunsetTime") ? Convert.ToInt64(weather["sunsetTime"]) : null;
                                         restWeather.moonPhase = weather.ContainsKey("moonPhase") ? Math.Round(Convert.ToDouble(weather["moonPhase"])) : null;
                                         restWeather.precipType = weather.ContainsKey("precipType") ? weather["precipType"] : null;
                                         restWeather.tempMinTime = weather.ContainsKey("temperatureMinTime") ? Convert.ToInt64(weather["temperatureMinTime"]) : null;
                                         restWeather.tempMaxTime = weather.ContainsKey("temperatureMaxTime") ? Convert.ToInt64(weather["temperatureMaxTime"]) : null;
                                         restWeather.dewPoint = weather.ContainsKey("dewPoint") ? Math.Round(Convert.ToDouble(weather["dewPoint"])) : null;
                                         restWeather.humidity = weather.ContainsKey("humidity") ? Math.Round(Convert.ToDouble(weather["humidity"])) : null;
                                         restWeather.windSpeed = weather.ContainsKey("windSpeed") ? Math.Round(Convert.ToDouble(weather["windSpeed"])) : null;
                                         restWeather.visibility = weather.ContainsKey("visibility") ? Math.Round(Convert.ToDouble(weather["visibility"])) : null;
                                         restWeather.cloudCover = weather.ContainsKey("cloudCover") ? Math.Round(Convert.ToDouble(weather["cloudCover"])) : null;
                                         restWeather.pressure = weather.ContainsKey("pressure") ? Math.Round(Convert.ToDouble(weather["pressure"])) : null;
                                         restWeather.ozone = weather.ContainsKey("ozone") ? Math.Round(Convert.ToDouble(weather["ozone"])) : null;
                                        
                                         if (ltWeather.Count == 0)
                                         {
                                             restWeather.currentTemp = Math.Round(Convert.ToDouble(jsonReturn["currently"]["temperature"]));
                                             restWeather.dateWeather = Bll.Util.UnixTimeStampToDateTime(Convert.ToDouble(jsonReturn["currently"]["time"]));
                                             restWeather.dateWeatherLabel = restWeather.dateWeather.ToString("dddd hh:mm tt");
                                         }
                                         else
                                         {
                                             restWeather.dateWeather = Bll.Util.UnixTimeStampToDateTime(Convert.ToDouble(weather["time"]));
                                             restWeather.dateWeatherLabel = restWeather.dateWeather.ToString("dddd hh:mm tt");
                                         }

                                         if (restWeather.dateWeather.DayOfWeek == DateTime.Now.DayOfWeek)
                                         {
                                             restWeather.dateLabel = "Today";
                                         }
                                         else if (restWeather.dateWeather.DayOfWeek == DateTime.Now.AddDays(1).DayOfWeek)
                                         {
                                             restWeather.dateLabel = "Tomorrow";
                                         }
                                         else
                                         {
                                             restWeather.dateLabel = restWeather.dateWeather.DayOfWeek.ToString();
                                         }
                                         

                                         ltWeather.Add(restWeather);
                                     }
                                     HttpRuntime.Cache.Insert("Weather." + latlng, ltWeather, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                                 }
                            }

                            if (HttpRuntime.Cache["Weather." + latlng] != null)
                            {
                                List<Model.REST.Weather> weather = (List<Model.REST.Weather>)HttpRuntime.Cache["Weather." + latlng];
                                geo.weather = weather.ToArray();

                                this.Response<Model.REST.Response>(new Model.REST.Response()
                                {
                                    status = true,
                                    response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(geo)

                                });
                            }
                            else
                            {
                                this.Response<Model.REST.Response>(new Model.REST.Response()
                                {
                                    status = false,
                                    response = "Weather unavailable"
                                });
                            }
                        }
                        else
                        {
                            this.Response<Model.REST.Response>(new Model.REST.Response()
                            {
                                status = false,
                                response = "Address not found"
                            });
                        }
                    }
                    else
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "Address not found"
                        });
                    }

                }
                
            }
        }

        private string ExtractZipCode(dynamic[] pObjects)
        {
            foreach (dynamic obj in pObjects)
            {
                if (obj["types"][0] == "postal_code")
                    return obj["long_name"];
            }
            return "";
        }

        protected override void Get()
        {
            throw new NotImplementedException();
        }

        protected override void Post(string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Post()
        {
            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                Bll.UserWidget bllUserWidget = new Bll.UserWidget();


                long userWidgetId = bllUserWidget.Save(new Model.UserWidget()
                {
                    Name = "Weather",
                    Size = 1,
                    SystemTagId = (int)Model.Enum.enWidgetType.WEATHER,
                    UserId = this.GetSessionUser().Id
                });

                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = true,
                    response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ModelUserWidgetToRESTModel(bllUserWidget.GetUserWidget(this.GetSessionUser().Id, 0).ToList().Where(x => x.Id == userWidgetId).First()))
                });
            }
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        public Model.REST.UserWidget ModelUserWidgetToRESTModel(Model.UserWidget model)
        {
            Model.REST.UserWidget restModel = new Model.REST.UserWidget();
            restModel.id = model.Id;
            restModel.title = model.Name;
            restModel.typeId = model.SystemTagId;
            restModel.typeName = Bll.Util.EnumToDescription((Model.Enum.enWidgetType)model.SystemTagId);

            restModel.isDeletable = true;

            if (!String.IsNullOrEmpty(model.Category))
            {
                String[] category = model.Category.Split(',');
                restModel.categoryId = new int[category.Length];
                for (int i = 0; i < category.Length; i++)
                {
                    /*
                    string[] category2 = category[i].Split('|');

                    restModel.category[i] = new Model.REST.TrustedSource();
                    restModel.category[i].id = Int32.Parse(category2[0]);
                    restModel.category[i].title = category2[1];
                     */

                    restModel.categoryId[i] = Int32.Parse(category[i]);
                }
            }

            restModel.size = model.Size;

            return restModel;
        }
    }
}