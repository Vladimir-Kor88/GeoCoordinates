using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GeoBackCoordinates.Models
{
    public interface IGetData
    {
        List<double[][]> GetData(string address);
    }

    public class GetDataOSM : IGetData
    {
        public List<double[][]> GetData(string address)
        {
            var query = "https://nominatim.openstreetmap.org/search?q=" + address + "&format=json&polygon_geojson=1 ";
            string json;

            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add("User-Agent", "MyApp");
                json = webClient.DownloadString(query);
            }

            JArray jArray = JArray.Parse(json);

            if (jArray.Count > 0)
                return GetCoordinates(jArray);
            else
                return null;
        }

        private List<double[][]> GetCoordinates(JToken token)
        {
            var geoJson = token[0]["geojson"];
            var coordinates = geoJson["coordinates"];
            var listCoor = new List<double[][]>();
            var countArrays = coordinates.Children().Count();

            foreach (var element in coordinates.Children())
            {
                if (countArrays > 1)
                    foreach (var subElement in element.Children())
                        listCoor.Add(subElement.ToObject<double[][]>());
                else
                    listCoor.Add(element.ToObject<double[][]>());
            }

            return listCoor;
        }
    }
}
