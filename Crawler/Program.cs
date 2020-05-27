using Aspose.Cells;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class Program
    {
        public static IConfiguration Configuration;
        static void Main(string[] args)
        {

            string filePath = @".\Resources\Aspose.Total.lic";

            Aspose.Cells.License wordLicense = new Aspose.Cells.License();
            FileStream filestream = new FileStream(filePath, FileMode.Open);
            wordLicense.SetLicense(filestream);
            StartCrawler();
            Console.ReadLine();
        }
        private static async void StartCrawler()
        {

            // tạo list add list car
            int flag = 0;
            var Product = new List<IfmModel>();
            var TechnicalDetails = new List<TechnicalDetails>();
            var DataDescriptions = new List<DataDescription>();
            var TechnicalDetailId = "";
            var ItemId = Guid.NewGuid().ToString();
            for (int j = 1; j < 10000; j++)
            {
                try
                {
                    var url = "https://www.ifm.com/au/en/product/IGT" + j.ToString();
                    Console.WriteLine(url);
                    var httpClient = new HttpClient();
                    var html = await httpClient.GetStringAsync(url);

                    var htmlDocument = new HtmlDocument();
                    var ImageUrl = "";
                    var DataName = "";
                    var DataDescription = "";
                    htmlDocument.LoadHtml(html);

                    var ProductName = htmlDocument.DocumentNode.Descendants("div")
                       .Where(node => node.GetAttributeValue("class", "").Equals("item-header")).FirstOrDefault().Descendants("h1").FirstOrDefault().InnerText;

                    var ListImageUrl = htmlDocument.DocumentNode.Descendants("div")
                       .Where(node => node.GetAttributeValue("class", "").Equals("carousel-inner")).ToList();
                    foreach (var picture in ListImageUrl)
                    {
                        ImageUrl = picture.Descendants("source").FirstOrDefault().ChildAttributes("srcset").FirstOrDefault().Value;
                    }
                    var divs = htmlDocument.DocumentNode.Descendants("table")
                        .Where(node => node.GetAttributeValue("class", "").Equals("pd-table")).ToList();

                    foreach (var div in divs)
                    {
                        var Title = div.Descendants("caption").FirstOrDefault().InnerText;
                        var Description = div.Descendants("tbody").FirstOrDefault().InnerText;
                        var tbody = div.Descendants("tbody").FirstOrDefault().Descendants("tr").ToArray();
                        for (int i = 0; i < tbody.Count(); i++)
                        {
                            if (tbody[i].InnerText.Contains("Electrical data"))
                            {
                                flag = 1;
                                TechnicalDetailId = Guid.NewGuid().ToString();
                                DataName = tbody[i].InnerText;
                                TechnicalDetails.Add(new TechnicalDetails { Id = TechnicalDetailId, ItemId = ItemId, DataName = DataName });
                            }
                            else if (tbody[i].InnerText.Contains("Inputs"))
                            {
                                flag = 2;
                                TechnicalDetailId = Guid.NewGuid().ToString();
                                DataName = tbody[i].InnerText;
                                TechnicalDetails.Add(new TechnicalDetails { Id = TechnicalDetailId, ItemId = ItemId, DataName = DataName });
                            }
                            else if (tbody[i].InnerText.Contains("Inputs / outputs"))
                            {
                                flag = 3;
                                TechnicalDetailId = Guid.NewGuid().ToString();
                                DataName = tbody[i].InnerText;
                                TechnicalDetails.Add(new TechnicalDetails { Id = TechnicalDetailId, ItemId = ItemId, DataName = DataName });
                            }
                            else if (tbody[i].InnerText.Contains("Outputs"))
                            {
                                flag = 4;
                                TechnicalDetailId = Guid.NewGuid().ToString();
                                DataName = tbody[i].InnerText;
                                TechnicalDetails.Add(new TechnicalDetails { Id = TechnicalDetailId, ItemId = ItemId, DataName = DataName });
                            }
                            else if (tbody[i].InnerHtml.Contains("th"))
                            {
                                flag = 0;
                            }
                            else if (flag == 1)
                            {
                                DataDescription = tbody[i].InnerText;
                                DataDescriptions.Add(new DataDescription { Id = Guid.NewGuid().ToString(), TechnicalDetailId = TechnicalDetailId, flag = flag, DataDescriptionDetail = DataDescription });
                            }
                            else if (flag == 2)
                            {
                                DataDescription = tbody[i].InnerText;
                                DataDescriptions.Add(new DataDescription { Id = Guid.NewGuid().ToString(), TechnicalDetailId = TechnicalDetailId, flag = flag, DataDescriptionDetail = DataDescription });
                            }
                            else if (flag == 3)
                            {
                                DataDescription = tbody[i].InnerText;
                                DataDescriptions.Add(new DataDescription { Id = Guid.NewGuid().ToString(), TechnicalDetailId = TechnicalDetailId, flag = flag, DataDescriptionDetail = DataDescription });
                            }
                            else if (flag == 4)
                            {
                                DataDescription = tbody[i].InnerText;
                                DataDescriptions.Add(new DataDescription { Id = Guid.NewGuid().ToString(), TechnicalDetailId = TechnicalDetailId, flag = flag, DataDescriptionDetail = DataDescription });
                            }
                        }
                        Product.Add(new IfmModel { Id = ItemId, ProductName = ProductName, Title = Title, Description = Description.Replace("  ", "").Replace("\n", "").Replace(",", ""), ImageUrl = ImageUrl, TechnicalDetails = TechnicalDetails });
                    }
                }
                catch (Exception)
                {
                }

                //}
                //Workbook book = new Workbook();
                //book.FileName = DateTime.Now.ToString("dd-MM-yyyy-hh-mm") + "Export.CSV";
                //StreamWriter sw = null;
                //string strFileType = null;
                //string sPathName = "D:/Users/namht1/source/repos/Crawler" + "/ExportCSV/" + DateTime.Now.ToString("yyyy/MM");
                //Directory.CreateDirectory(sPathName + "/");
                //using (sw = new StreamWriter(sPathName + "/" + "/" + strFileType + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + "_ApplicationFilter.csv", true, Encoding.UTF8))
                //{
                //    for (int i = 0; i < Ifms.Count; i++)
                //    {
                //        sw.Write(
                //            TechnicalDetails[i].DataName?.ToString() + "\n"
                //            );
                //    }
                //    sw.Flush();
                //}

                //book.Save(sPathName);
            }

            string fileName = "CrawlerData.xlsx";
            string fileDir = "\\Resources\\Template\\Excel\\";
            string rootDir = Directory.GetCurrentDirectory();
            //Load template
            Workbook workbook = new Workbook(rootDir + fileDir + fileName);

            workbook.Worksheets[0].Name = "Product";
            workbook.Worksheets[0].AutoFitColumns();

            workbook.Worksheets[1].Name = "DataDescription";
            workbook.Worksheets[1].AutoFitColumns();

            workbook.Worksheets[2].Name = "TechnicalDetails";
            workbook.Worksheets[2].AutoFitColumns();
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = workbook;

            designer.SetDataSource("product", Product);
            designer.SetDataSource("technical_details", TechnicalDetails);
            designer.SetDataSource("data_descriptions", DataDescriptions);
            designer.Process(false);


            MemoryStream stream = new MemoryStream();
            var returnFileName = "CrawlerData" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xlsx";

            // tạo thư mục lưu
            var folderName = Path.Combine("Resources", "Files", DateTime.Now.ToString("yyyy-MM-dd"));
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            Directory.CreateDirectory(pathToSave);

            var fullPath = Path.Combine(pathToSave, "_" + returnFileName);

            designer.Workbook.Save(stream, new XlsSaveOptions(SaveFormat.Xlsx));
            designer.Workbook.Save(fullPath);
        }
    }
}
