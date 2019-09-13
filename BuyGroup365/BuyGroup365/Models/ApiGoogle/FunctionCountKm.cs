﻿using Newtonsoft.Json.Linq;

namespace BuyGroup365.Models.ApiGoogle
{
    public class FunctionCountKm
    {
        public float GetDistance(string origin, string destination)
        {
            System.Threading.Thread.Sleep(300);
            float distance = 0;
            string url = "http://maps.googleapis.com/maps/api/directions/json?origin=" + origin + "&destination=" + destination + "&sensor=false";
            string requesturl = url;
            string content = FileGetContents(requesturl);
            JObject o = JObject.Parse(content);
            try
            {
                distance = (float)o.SelectToken("routes[0].legs[0].distance.value");
                return distance / 1000;
            }
            catch
            {
                return distance;
            }
        }

        protected string FileGetContents(string fileName)
        {
            string sContents;
            string me = string.Empty;
            try
            {
                if (fileName.ToLower().IndexOf("http:") > -1)
                {
                    var wc = new System.Net.WebClient();
                    byte[] response = wc.DownloadData(fileName);
                    sContents = System.Text.Encoding.ASCII.GetString(response);
                }
                else
                {
                    var sr = new System.IO.StreamReader(fileName);
                    sContents = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch { sContents = "unable to connect to server "; }
            return sContents;
        }
    }
}