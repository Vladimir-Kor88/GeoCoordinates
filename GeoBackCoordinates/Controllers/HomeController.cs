using GeoBackCoordinates.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace GeoBackCoordinates.Controllers
{
    public class HomeController : Controller
    {
        IGetData _data;
        public HomeController(IGetData data)
        {
            _data = data;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.IsQuery = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index (Require require)
        {
            if (ModelState.IsValid)
            {
                var listCoordinates = _data.GetData(require.address);
                List<List<double[]>> clearListCoordinates = null;

                if (listCoordinates != null)
                {
                    using (FileStream fs = new FileStream(require.file + ".json", FileMode.OpenOrCreate))
                        await JsonSerializer.SerializeAsync<List<double[][]>>(fs, listCoordinates);

                    clearListCoordinates = FillList(listCoordinates, require.frequency);
                }

                ViewBag.IsQuery = true;
                ViewBag.ClearList = clearListCoordinates;
            }
            else
                ViewBag.IsQuery = false;
            return View(require);
        }

        private List<List<double[]>> FillList (List<double[][]> basicList, int frequency)
        {
            List<List<double[]>> result = new List<List<double[]>>();
            List<double[]> tempList;

            foreach (var elem in basicList)
            {
                tempList = new List<double[]>();

                for (int i = 0; i < elem.Length; i++)
                {
                    if (i % frequency == 0)
                        tempList.Add(elem[i]);
                }

                result.Add(tempList);
            }

            return result;
        }
    }
}
