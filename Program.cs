using System;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

class Program
{
    static void Main(string[] args)
    {
        string cameraIpAddress = "192.168.1.100"; // Replace with the IP address of your HIKVISION LPR camera
        string username = "admin"; // Replace with your camera's username
        string password = "password"; // Replace with your camera's password
        string carNumber = "ABC123"; // Replace with the car number you want to add to the authorized list

        string apiUrl = $"http://{cameraIpAddress}/ISAPI/Intelligent/FDLib/FaceDataRecord?format=json";
        string credentials = $"{username}:{password}";
        byte[] credentialsBytes = Encoding.ASCII.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(credentialsBytes);

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.Headers.Add("Authorization", $"Basic {base64Credentials}");
            request.ContentType = "application/json";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                JObject requestBody = new JObject();
                requestBody.Add("carNumber", carNumber);
                writer.Write(requestBody.ToString());
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine($"HTTP status code: {response.StatusCode}");
        }
        catch (WebException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.ReadLine();
    }
}

