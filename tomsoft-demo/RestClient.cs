using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace tomsoft_demo
{
    public enum httpVerb
    {
        GET
       /*POST, PUT, DELETE*/
    }

    public enum authenticationType
    {
        Basic
    }

    public enum authenticationMethod
    {
        RollYourOwn,
        NetworkCredential
    }

    class RestClient
    {
        public string endPoint { get; set; }
        public httpVerb httpMethod { get; set; }
        public authenticationType authenticationType { get; set; }
        public authenticationMethod authenticationMethod { get; set; }
        public string userName { get; set; }
        public string password { get; set; }

        public RestClient()
        {
            endPoint = string.Empty;
            httpMethod = httpVerb.GET;
        }

        public string makeRequest()
        {
            string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.Method = httpMethod.ToString();
            
            if(authenticationMethod == authenticationMethod.RollYourOwn)
            {
                String authHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(userName +
                ":" + password));
                request.Headers.Add("Authorization", "Basic " + authHeader);
            }
            
            else
            {
                NetworkCredential networkCredential = new NetworkCredential(userName, password);
                request.Credentials = networkCredential;
            }

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using(Stream responseStream = response.GetResponseStream())
                {
                    if(responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                strResponseValue = "{\"ErrorMessage\":[\"" + ex.Message.ToString() + "\"],\"Errors\":{}}";
            }

            finally
            {
                if(response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            return strResponseValue;
        }
    }
}
