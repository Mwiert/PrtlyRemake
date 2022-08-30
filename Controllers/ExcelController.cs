using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remake.Models;
using Remake.Security.Validations.excelOps;
using System.Globalization;
using System.Security.Claims;
using System.IO;

namespace Remake.Controllers
{
    [Authorize(Roles = "ADMIN,MUHASEBE")]
    public class ExcelController : Controller
    {
        List<Kesifler> kesifler = new List<Kesifler>();
        Kesifler kesif = new Kesifler();
        kesifdbContext db = new kesifdbContext();
        ExcelEntites xle = new ExcelEntites();
        List<Urunler> urunlerList = new List<Urunler>();
        Urunholder UrunHolder = new Urunholder();
        Alanholder alanholder = new Alanholder();
        public static string kur;
        public static int kesifidd;
        string temp;
        public IActionResult Index()
        {
            using (var db = new kesifdbContext())
            {
                kesifler = new List<Kesifler>();
                kesifler = db.Kesiflers.ToList();

            }
            return View(kesifler);
        }
        public JsonResult setValues(int kesifid, string DolarKuru)
        {
            kur = DolarKuru;
            kesifidd = kesifid;
            return Json(0);
        }
        public IActionResult ExcelKaydet(int kesifid, string DolarKuru)
        {
            List<string> CatHolder = new List<string>();
            kesifid = kesifidd;
            DolarKuru = kur;

            string temp = DolarKuru.Replace(",", ".");
            DolarKuru = temp;
            if (string.IsNullOrEmpty(kesif.ToString()) || string.IsNullOrEmpty(DolarKuru))
            {
                return RedirectToAction("Index");
            }
            else
            {
            db = new kesifdbContext();
            float dolar = float.Parse(DolarKuru, CultureInfo.InvariantCulture.NumberFormat);
            xle = new ExcelEntites();
            urunlerList = new List<Urunler>();
            kesif = new Kesifler();
            xle.HazirlayanAdi = User.Identity.Name;
            xle.DolarKuru = dolar;

            kesif = db.Kesiflers.FirstOrDefault(x => x.Id == kesifid);
            var AlanAndUrun = (from alan in db.Alanholders
                               join kesf in db.Kesiflers
                               on alan.KesifId equals kesf.Id
                               where kesf.Id == kesifid
                               join uholder in db.Urunholders
                               on alan.Id equals uholder.AlanId
                               join urun in db.Urunlers
                               on uholder.UrunId equals urun.Id
                               select new
                               {
                                   UrunAdi = urun.UrunAdi,
                                   urunAdedi = uholder.UrunAdet,
                                   UrunKodu = urun.UrunKodu,
                                   UrunKategorisi = urun.UrunKategorisi,
                                   BirimFiyat = urun.UrunFiyati,
                                   SatisFiyati = urun.SatisFiyati,
                                   UrunAdedi = uholder.UrunAdet
                               });

            xle.KesifAdi = kesif.Ad;

                foreach(var item in AlanAndUrun)
                {
                    if(item.UrunKategorisi != temp)
                    {
                        temp = item.UrunKategorisi;
                        CatHolder.Add(item.UrunKategorisi);
                    }
                }


            using (var workbook = new XLWorkbook())
            {
                  
                    var worksheet = workbook.Worksheets.Add("Döküman");

                    worksheet.Columns(6, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#c4d9f0");
                    worksheet.Columns(11, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#f2dcda");
                    worksheet.Columns(15, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ffe1e1");


                    worksheet.Cell(7, 3).Value = "Açıklama";
                    worksheet.Cell(7, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(7, 3).Style.Font.Bold = true;

                    worksheet.Cell(2, 2).Value = xle.KesifAdi + "  " + "Maaliyet Tablosu";
                    worksheet.Cell(2, 2).Style.Font.FontSize = 16;
                    worksheet.Cell(2, 2).Style.Font.Bold = true;

                    worksheet.Cell(2, 5).Value ="Dolar Kuru : ";
                    worksheet.Cell(2, 6).Value = dolar;
                    worksheet.Cell(2, 7).Value = "TL";

                    worksheet.Cell(4, 2).Value = "Hazırlayan : " + xle.HazirlayanAdi;
                    worksheet.Cell(4, 2).Style.Font.FontSize = 12;
                    worksheet.Cell(4, 2).Style.Font.Bold = true;

                    worksheet.Cell(7, 1).Value = "Ürün Kodu";
                    worksheet.Cell(7, 1).Style.Font.Bold = true;

                    worksheet.Rows(7,8).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(8, 3).Value = "Kategori Adı";
                    worksheet.Cell(8, 3).Style.Font.Bold = true;
                    worksheet.Cell(8, 3).Style.Font.Italic = true;


                    worksheet.Columns("Q").Width = 12;
                    worksheet.Columns("P").Width = 12;
                    worksheet.Columns("O").Width = 12;
                    worksheet.Columns("N").Width = 12;
                    worksheet.Columns("M").Width = 12;
                    worksheet.Columns("L").Width = 12;
                    worksheet.Columns("K").Width = 12;
                    worksheet.Columns("J").Width = 12;
                    worksheet.Columns("I").Width = 12;
                    worksheet.Columns("H").Width = 12;
                    worksheet.Columns("G").Width = 12;
                    worksheet.Columns("F").Width = 12;
                    worksheet.Columns("E").Width = 12;
                    worksheet.Columns("D").Width = 12;
                    worksheet.Columns("C").Width = 48;
                    worksheet.Rows(7, 8).Height = 36;

                    worksheet.Cell(7, 4).Value = "Tedarikçi Firma";
                    worksheet.Cell(7, 4).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 4).Style.Font.Bold = true;

                    worksheet.Cell(7, 5).Value = "Adet";
                    worksheet.Cell(7, 5).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 5).Style.Font.Bold = true;

                    worksheet.Cell(7, 6).Value = "Tedarikçi Liste Fiyatı";
                    worksheet.Cell(7, 6).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 6).Style.Font.Bold = true;
                    worksheet.Cell(7, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                    worksheet.Cell(7, 7).Value = "Birim Fiyatı";
                    worksheet.Cell(7, 7).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 7).Style.Font.Bold = true;
                    worksheet.Cell(7, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                    worksheet.Cell(7, 8).Value = "Toplam Fiyatı";
                    worksheet.Cell(7, 8).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 8).Style.Font.Bold = true;
                    worksheet.Cell(7, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                    worksheet.Cell(7, 9).Value = "Kuru (TL)";
                    worksheet.Cell(7, 9).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 9).Style.Font.Bold = true;
                    worksheet.Cell(7, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                    worksheet.Cell(7, 10).Value = " KDV'li Toplam Maliyet (TL)";
                    worksheet.Cell(7, 10).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 10).Style.Font.Bold = true;
                    worksheet.Column("J").Width = 18;
                    worksheet.Cell(7, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");



                    worksheet.Cell(7, 11).Value = "Birim Fiyatı";
                    worksheet.Cell(7, 11).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 11).Style.Font.Bold = true;
                    worksheet.Cell(7, 11).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                    worksheet.Cell(8, 11).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                    worksheet.Cell(7, 12).Value = "Toplam Fiyatı";
                    worksheet.Cell(7, 12).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 12).Style.Font.Bold = true;
                    worksheet.Cell(7, 12).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                    worksheet.Cell(8, 12).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                    worksheet.Cell(7, 13).Value = "Kuru (TL)";
                    worksheet.Cell(7, 13).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 13).Style.Font.Bold = true;
                    worksheet.Cell(7, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                    worksheet.Cell(8, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                    worksheet.Cell(7, 14).Value = "KDV'li Toplam Maliyet (TL)";
                    worksheet.Cell(7, 14).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 14).Style.Font.Bold = true;
                    worksheet.Column("N").Width = 18;
                    worksheet.Cell(7, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                    worksheet.Cell(8, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                    worksheet.Cell(7, 15).Value = "Kar Tutuar (USD)";
                    worksheet.Cell(7, 15).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 15).Style.Font.Bold = true;
                    worksheet.Cell(7, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                    worksheet.Cell(8, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                    worksheet.Cell(7, 16).Value = "Kar Tutarı (TL)";
                    worksheet.Cell(7, 16).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 16).Style.Font.Bold = true;
                    worksheet.Column("P").Width = 18;
                    worksheet.Cell(7, 16).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                    worksheet.Cell(8, 16).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                    worksheet.Cell(7, 17).Value = "Kar Oranı";
                    worksheet.Cell(7, 17).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 17).Style.Font.Bold = true;
                    worksheet.Cell(7, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                    worksheet.Cell(8, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");


                    worksheet.Rows(1, 6).Style.Fill.BackgroundColor = XLColor.NoColor;

                    worksheet.Columns("E", "Q").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Column("G").Style.Font.Bold = true;
                    worksheet.Column("K").Style.Font.Bold = true;
                    worksheet.Column("P").Style.Font.Bold = true;
                    worksheet.Column("O").Style.Font.Bold = true;
                    worksheet.Column("Q").Style.Font.Bold = true;

                    int rowCount = 9;
                    int counter = CatHolder.Count;
                    int incrmnt = 0;
                    int lines = 0;

                    for (int i = 0; i < CatHolder.Count; i++)
                    {
                        if(counter >= 0)
                        {
                            var bul = AlanAndUrun.Where(x => x.UrunKategorisi == CatHolder[counter-1]).ToList();
                            incrmnt = bul.Count;
                            if (counter == CatHolder.Count)
                            {
                                worksheet.Cell(8, 3).Value = CatHolder[counter-1];
                                foreach (var item in bul)
                                {
                                    worksheet.Row(rowCount).Style.Border.TopBorder = XLBorderStyleValues.None;
                                    worksheet.Cell(rowCount, 1).Value = item.UrunKodu;
                                    worksheet.Cell(rowCount, 3).Value = item.UrunAdi;
                                    worksheet.Cell(rowCount, 5).Value = item.urunAdedi;

                                    worksheet.Cell(rowCount, 7).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 7).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 7).Value = item.BirimFiyat;

                                    worksheet.Cell(rowCount, 8).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 8).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 8).FormulaA1 = "=E"+rowCount+"*$G$"+rowCount+"";

                                    worksheet.Cell(rowCount, 9).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 9).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 9).Value = dolar;
                                    
                                    worksheet.Cell(rowCount, 10).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 10).FormulaA1 = "=PRODUCT(H" + rowCount + "*(1.18)*F2)";


                                    worksheet.Cell(rowCount, 11).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 11).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 11).Value = item.SatisFiyati;

                                    worksheet.Cell(rowCount, 12).FormulaA1 = "=E" + rowCount + "*$K$" + rowCount + "";


                                    worksheet.Cell(rowCount, 13).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 13).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 13).Value = dolar;

                                    worksheet.Cell(rowCount, 14).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 14).FormulaA1 = "=PRODUCT(L" + rowCount + "*(0.18)*F2)";

                                    worksheet.Cell(rowCount, 15).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 15).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 15).FormulaA1 = "=SUM(L" + rowCount + "-H" + rowCount + ")";

                                    worksheet.Cell(rowCount, 16).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 16).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 16).FormulaA1 = "=((L" + rowCount + "*M" + rowCount + ")-(H" + rowCount + "*I" + rowCount + "))";

                                    worksheet.Cell(rowCount, 17).FormulaA1 = "=(P" + rowCount + "/(H" + rowCount + "*I" + rowCount + "))";
                                    worksheet.Cell(rowCount, 17).Style.NumberFormat.Format = "0.0%";


                                    rowCount++;
                                }
                                worksheet.Cell(rowCount, 8).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 8).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 8).FormulaA1 = "=SUM(H" + (rowCount - incrmnt) + ":H" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 8).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 10).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 10).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 10).FormulaA1 = "=SUM(J" + (rowCount - incrmnt) + ":J" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 10).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 12).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 12).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 12).FormulaA1 = "=SUM(L" + (rowCount - incrmnt) + ":L" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 12).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 14).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 14).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 14).FormulaA1 = "=SUM(J" + (rowCount - incrmnt) + ":J" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 14).Style.Font.Bold = true;


                                worksheet.Cell(rowCount, 15).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 15).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 15).FormulaA1 = "=SUM(L" + (rowCount - incrmnt) + ":L" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 15).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 16).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 16).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 16).FormulaA1 = "=SUM(P" + (rowCount - incrmnt) + ":P" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 16).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 17).FormulaA1 = "=(P" + rowCount + ")/(J" + rowCount + "/(1.18))";
                                worksheet.Cell(rowCount, 17).Style.NumberFormat.Format = "0.0%";
                                worksheet.Cell(rowCount, 17).Style.Font.Bold = true;
                            }
                            else
                            {
                                //rowCount += incrmnt;  // ???
                                lines = rowCount;
                                rowCount += 2;
                                worksheet.Rows(lines, lines + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                worksheet.Cell(lines + 1, 3).Style.Font.Bold = true;
                                worksheet.Cell(lines + 1, 3).Style.Font.Italic = true;

                                worksheet.Cell(lines + 1, 3).Value = CatHolder[counter-1];

                                worksheet.Cell(lines, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines + 1, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines + 1, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines + 1, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines + 1, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines+1, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 11).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                                worksheet.Cell(lines + 1, 11).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                                worksheet.Cell(lines, 12).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                                worksheet.Cell(lines + 1, 12).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                                worksheet.Cell(lines, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                                worksheet.Cell(lines + 1, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                                worksheet.Cell(lines, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                                worksheet.Cell(lines+1, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                                worksheet.Cell(lines, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                                worksheet.Cell(lines + 1, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                                worksheet.Cell(lines, 16).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                                worksheet.Cell(lines + 1, 16).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                                worksheet.Cell(lines, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                                worksheet.Cell(lines+1, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                                foreach (var item in bul)
                                {
                                    worksheet.Cell(rowCount, 1).Value = item.UrunKodu;
                                    worksheet.Cell(rowCount, 3).Value = item.UrunAdi;
                                    worksheet.Cell(rowCount, 5).Value = item.urunAdedi;
                                    worksheet.Cell(rowCount, 7).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 7).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 7).Value = item.BirimFiyat;

                                    worksheet.Cell(rowCount, 8).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 8).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 8).FormulaA1 = "=E" + rowCount + "*$G$" + rowCount + "";


                                    worksheet.Cell(rowCount, 9).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 9).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 9).Value = dolar;

                                    worksheet.Cell(rowCount, 10).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 10).FormulaA1 = "=PRODUCT(H" + rowCount + "*(1.18)*F2)";


                                    worksheet.Cell(rowCount, 11).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 11).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 11).Value = item.SatisFiyati;

                                    worksheet.Cell(rowCount, 12).FormulaA1 = "=E" + rowCount + "*$K$" + rowCount + "";

                                    worksheet.Cell(rowCount, 13).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 13).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 13).Value = dolar ;

                                    worksheet.Cell(rowCount, 14).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 14).FormulaA1 = "=PRODUCT(L" + rowCount + "*(0.18)*F2)";
                                    

                                    worksheet.Cell(rowCount, 15).FormulaA1 = "=SUM(L"+rowCount+"-H"+rowCount+")";


                                    worksheet.Cell(rowCount, 16).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 16).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 16).FormulaA1 = "=((L" + rowCount + "*M" + rowCount + ")-(H" + rowCount + "*I" + rowCount + "))";

                                    worksheet.Cell(rowCount, 17).FormulaA1 = "=(P" + rowCount + "/(H" + rowCount + "*I" + rowCount + "))";
                                    worksheet.Cell(rowCount, 17).Style.NumberFormat.Format = "0.0%";


                                    rowCount++;
                                }
                                worksheet.Cell(rowCount, 8).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 8).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 8).FormulaA1 = "=SUM(H" + (rowCount - incrmnt) + ":H" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 8).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 10).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 10).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 10).FormulaA1 = "=SUM(J" + (rowCount - incrmnt) + ":J" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 10).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 12).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 12).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 12).FormulaA1 = "=SUM(L" + (rowCount - incrmnt) + ":L" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 12).Style.Font.Bold = true;


                                worksheet.Cell(rowCount, 14).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 14).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 14).FormulaA1 = "=SUM(J" + (rowCount - incrmnt) + ":J" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 14).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 15).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 15).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 15).FormulaA1 = "=SUM(L" + (rowCount - incrmnt) + ":L" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 15).Style.Font.Bold = true;


                                worksheet.Cell(rowCount, 16).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 16).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 16).FormulaA1 = "=SUM(P" + (rowCount - incrmnt) + ":P" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 16).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 17).FormulaA1 = "=(P" + rowCount + ")/(J" + rowCount + "/(1.18))";
                                worksheet.Cell(rowCount, 17).Style.NumberFormat.Format = "0.0%";
                                worksheet.Cell(rowCount, 17).Style.Font.Bold = true;
                            }

                            counter--;
                        }

                    }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vdn.openxmlformats-officedocument.spreadsheetml.sheet", "deneme.xlsx");

                }
            }

            }
        }
        public JsonResult ExcelMailGonder(int kesifid, string DolarKuru)
        {
            excelSendMail xlMail = new excelSendMail();

            List<string> CatHolder = new List<string>();
            kesifid = kesifidd;
            DolarKuru = kur;

            string temp = DolarKuru.Replace(",", ".");
            DolarKuru = temp;
            if (string.IsNullOrEmpty(kesif.ToString()) || string.IsNullOrEmpty(DolarKuru))
            {
                return Json(0);
            }
            else
            {
                db = new kesifdbContext();
                float dolar = float.Parse(DolarKuru, CultureInfo.InvariantCulture.NumberFormat);
                xle = new ExcelEntites();
                urunlerList = new List<Urunler>();
                kesif = new Kesifler();
                xle.HazirlayanAdi = User.Identity.Name;
                xle.DolarKuru = dolar;

                kesif = db.Kesiflers.FirstOrDefault(x => x.Id == kesifid);
                var AlanAndUrun = (from alan in db.Alanholders
                                   join kesf in db.Kesiflers
                                   on alan.KesifId equals kesf.Id
                                   where kesf.Id == kesifid
                                   join uholder in db.Urunholders
                                   on alan.Id equals uholder.AlanId
                                   join urun in db.Urunlers
                                   on uholder.UrunId equals urun.Id
                                   select new
                                   {
                                       UrunAdi = urun.UrunAdi,
                                       urunAdedi = uholder.UrunAdet,
                                       UrunKodu = urun.UrunKodu,
                                       UrunKategorisi = urun.UrunKategorisi,
                                       BirimFiyat = urun.UrunFiyati,
                                       SatisFiyati = urun.SatisFiyati,
                                       UrunAdedi = uholder.UrunAdet
                                   });

                xle.KesifAdi = kesif.Ad;

                foreach (var item in AlanAndUrun)
                {
                    if (item.UrunKategorisi != temp)
                    {
                        temp = item.UrunKategorisi;
                        CatHolder.Add(item.UrunKategorisi);
                    }
                }


                using (var workbook = new XLWorkbook())
                {

                    var worksheet = workbook.Worksheets.Add("Döküman");

                    worksheet.Columns(6, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#c4d9f0");
                    worksheet.Columns(11, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#f2dcda");
                    worksheet.Columns(15, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ffe1e1");


                    worksheet.Cell(7, 3).Value = "Açıklama";
                    worksheet.Cell(7, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(7, 3).Style.Font.Bold = true;

                    worksheet.Cell(2, 2).Value = xle.KesifAdi + "  " + "Maaliyet Tablosu";
                    worksheet.Cell(2, 2).Style.Font.FontSize = 16;
                    worksheet.Cell(2, 2).Style.Font.Bold = true;

                    worksheet.Cell(2, 5).Value = "Dolar Kuru : ";
                    worksheet.Cell(2, 6).Value = dolar;
                    worksheet.Cell(2, 7).Value = "TL";

                    worksheet.Cell(4, 2).Value = "Hazırlayan : " + xle.HazirlayanAdi;
                    worksheet.Cell(4, 2).Style.Font.FontSize = 12;
                    worksheet.Cell(4, 2).Style.Font.Bold = true;

                    worksheet.Cell(7, 1).Value = "Ürün Kodu";
                    worksheet.Cell(7, 1).Style.Font.Bold = true;

                    worksheet.Rows(7, 8).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(8, 3).Value = "Kategori Adı";
                    worksheet.Cell(8, 3).Style.Font.Bold = true;
                    worksheet.Cell(8, 3).Style.Font.Italic = true;


                    worksheet.Columns("Q").Width = 12;
                    worksheet.Columns("P").Width = 12;
                    worksheet.Columns("O").Width = 12;
                    worksheet.Columns("N").Width = 12;
                    worksheet.Columns("M").Width = 12;
                    worksheet.Columns("L").Width = 12;
                    worksheet.Columns("K").Width = 12;
                    worksheet.Columns("J").Width = 12;
                    worksheet.Columns("I").Width = 12;
                    worksheet.Columns("H").Width = 12;
                    worksheet.Columns("G").Width = 12;
                    worksheet.Columns("F").Width = 12;
                    worksheet.Columns("E").Width = 12;
                    worksheet.Columns("D").Width = 12;
                    worksheet.Columns("C").Width = 48;
                    worksheet.Rows(7, 8).Height = 36;

                    worksheet.Cell(7, 4).Value = "Tedarikçi Firma";
                    worksheet.Cell(7, 4).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 4).Style.Font.Bold = true;

                    worksheet.Cell(7, 5).Value = "Adet";
                    worksheet.Cell(7, 5).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 5).Style.Font.Bold = true;

                    worksheet.Cell(7, 6).Value = "Tedarikçi Liste Fiyatı";
                    worksheet.Cell(7, 6).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 6).Style.Font.Bold = true;
                    worksheet.Cell(7, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                    worksheet.Cell(7, 7).Value = "Birim Fiyatı";
                    worksheet.Cell(7, 7).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 7).Style.Font.Bold = true;
                    worksheet.Cell(7, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                    worksheet.Cell(7, 8).Value = "Toplam Fiyatı";
                    worksheet.Cell(7, 8).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 8).Style.Font.Bold = true;
                    worksheet.Cell(7, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                    worksheet.Cell(7, 9).Value = "Kuru (TL)";
                    worksheet.Cell(7, 9).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 9).Style.Font.Bold = true;
                    worksheet.Cell(7, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                    worksheet.Cell(7, 10).Value = " KDV'li Toplam Maliyet (TL)";
                    worksheet.Cell(7, 10).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 10).Style.Font.Bold = true;
                    worksheet.Column("J").Width = 18;
                    worksheet.Cell(7, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                    worksheet.Cell(8, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");



                    worksheet.Cell(7, 11).Value = "Birim Fiyatı";
                    worksheet.Cell(7, 11).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 11).Style.Font.Bold = true;
                    worksheet.Cell(7, 11).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                    worksheet.Cell(8, 11).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                    worksheet.Cell(7, 12).Value = "Toplam Fiyatı";
                    worksheet.Cell(7, 12).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 12).Style.Font.Bold = true;
                    worksheet.Cell(7, 12).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                    worksheet.Cell(8, 12).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                    worksheet.Cell(7, 13).Value = "Kuru (TL)";
                    worksheet.Cell(7, 13).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 13).Style.Font.Bold = true;
                    worksheet.Cell(7, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                    worksheet.Cell(8, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                    worksheet.Cell(7, 14).Value = "KDV'li Toplam Maliyet (TL)";
                    worksheet.Cell(7, 14).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 14).Style.Font.Bold = true;
                    worksheet.Column("N").Width = 18;
                    worksheet.Cell(7, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                    worksheet.Cell(8, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                    worksheet.Cell(7, 15).Value = "Kar Tutuar (USD)";
                    worksheet.Cell(7, 15).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 15).Style.Font.Bold = true;
                    worksheet.Cell(7, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                    worksheet.Cell(8, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                    worksheet.Cell(7, 16).Value = "Kar Tutarı (TL)";
                    worksheet.Cell(7, 16).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 16).Style.Font.Bold = true;
                    worksheet.Column("P").Width = 18;
                    worksheet.Cell(7, 16).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                    worksheet.Cell(8, 16).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                    worksheet.Cell(7, 17).Value = "Kar Oranı";
                    worksheet.Cell(7, 17).Style.Alignment.WrapText = true;
                    worksheet.Cell(7, 17).Style.Font.Bold = true;
                    worksheet.Cell(7, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                    worksheet.Cell(8, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");


                    worksheet.Rows(1, 6).Style.Fill.BackgroundColor = XLColor.NoColor;

                    worksheet.Columns("E", "Q").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Column("G").Style.Font.Bold = true;
                    worksheet.Column("K").Style.Font.Bold = true;
                    worksheet.Column("P").Style.Font.Bold = true;
                    worksheet.Column("O").Style.Font.Bold = true;
                    worksheet.Column("Q").Style.Font.Bold = true;

                    int rowCount = 9;
                    int counter = CatHolder.Count;
                    int incrmnt = 0;
                    int lines = 0;

                    for (int i = 0; i < CatHolder.Count; i++)
                    {
                        if (counter >= 0)
                        {
                            var bul = AlanAndUrun.Where(x => x.UrunKategorisi == CatHolder[counter - 1]).ToList();
                            incrmnt = bul.Count;
                            if (counter == CatHolder.Count)
                            {
                                worksheet.Cell(8, 3).Value = CatHolder[counter - 1];
                                foreach (var item in bul)
                                {
                                    worksheet.Row(rowCount).Style.Border.TopBorder = XLBorderStyleValues.None;
                                    worksheet.Cell(rowCount, 1).Value = item.UrunKodu;
                                    worksheet.Cell(rowCount, 3).Value = item.UrunAdi;
                                    worksheet.Cell(rowCount, 5).Value = item.urunAdedi;

                                    worksheet.Cell(rowCount, 7).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 7).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 7).Value = item.BirimFiyat;

                                    worksheet.Cell(rowCount, 8).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 8).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 8).FormulaA1 = "=E" + rowCount + "*$G$" + rowCount + "";

                                    worksheet.Cell(rowCount, 9).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 9).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 9).Value = dolar;

                                    worksheet.Cell(rowCount, 10).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 10).FormulaA1 = "=PRODUCT(H" + rowCount + "*(1.18)*F2)";


                                    worksheet.Cell(rowCount, 11).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 11).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 11).Value = item.SatisFiyati;


                                    worksheet.Cell(rowCount, 12).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 12).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 12).FormulaA1 = "=E" + rowCount + "*$K$" + rowCount + "";


                                    worksheet.Cell(rowCount, 13).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 13).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 13).Value = dolar;

                                    worksheet.Cell(rowCount, 14).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 14).FormulaA1 = "=PRODUCT(L" + rowCount + "*(0.18)*F2)";

                                    worksheet.Cell(rowCount, 15).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 15).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 15).FormulaA1 = "=SUM(L" + rowCount + "-H" + rowCount + ")";

                                    worksheet.Cell(rowCount, 16).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 16).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 16).FormulaA1 = "=((L" + rowCount + "*M" + rowCount + ")-(H" + rowCount + "*I" + rowCount + "))";

                                    worksheet.Cell(rowCount, 17).FormulaA1 = "=(P" + rowCount + "/(H" + rowCount + "*I" + rowCount + "))";
                                    worksheet.Cell(rowCount, 17).Style.NumberFormat.Format = "0.0%";


                                    rowCount++;
                                }
                                worksheet.Cell(rowCount, 8).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 8).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 8).FormulaA1 = "=SUM(H" + (rowCount - incrmnt) + ":H" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 8).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 10).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 10).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 10).FormulaA1 = "=SUM(J" + (rowCount - incrmnt) + ":J" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 10).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 12).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 12).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 12).FormulaA1 = "=SUM(L" + (rowCount - incrmnt) + ":L" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 12).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 14).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 14).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 14).FormulaA1 = "=SUM(J" + (rowCount - incrmnt) + ":J" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 14).Style.Font.Bold = true;


                                worksheet.Cell(rowCount, 15).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 15).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 15).FormulaA1 = "=SUM(L" + (rowCount - incrmnt) + ":L" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 15).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 16).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 16).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 16).FormulaA1 = "=SUM(P" + (rowCount - incrmnt) + ":P" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 16).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 17).FormulaA1 = "=(P" + rowCount + ")/(J" + rowCount + "/(1.18))";
                                worksheet.Cell(rowCount, 17).Style.NumberFormat.Format = "0.0%";
                                worksheet.Cell(rowCount, 17).Style.Font.Bold = true;
                            }
                            else
                            {
                                //rowCount += incrmnt;  // ???
                                lines = rowCount;
                                rowCount += 2;
                                worksheet.Rows(lines, lines + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                worksheet.Cell(lines + 1, 3).Style.Font.Bold = true;
                                worksheet.Cell(lines + 1, 3).Style.Font.Italic = true;

                                worksheet.Cell(lines + 1, 3).Value = CatHolder[counter - 1];

                                worksheet.Cell(lines, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines + 1, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines + 1, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines + 1, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines + 1, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");
                                worksheet.Cell(lines + 1, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#8cb4e2");

                                worksheet.Cell(lines, 11).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                                worksheet.Cell(lines + 1, 11).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                                worksheet.Cell(lines, 12).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                                worksheet.Cell(lines + 1, 12).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                                worksheet.Cell(lines, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                                worksheet.Cell(lines + 1, 13).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                                worksheet.Cell(lines, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");
                                worksheet.Cell(lines + 1, 14).Style.Fill.BackgroundColor = XLColor.FromHtml("#da9694");

                                worksheet.Cell(lines, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                                worksheet.Cell(lines + 1, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                                worksheet.Cell(lines, 16).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                                worksheet.Cell(lines + 1, 16).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                                worksheet.Cell(lines, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");
                                worksheet.Cell(lines + 1, 17).Style.Fill.BackgroundColor = XLColor.FromHtml("#ff7070");

                                foreach (var item in bul)
                                {
                                    worksheet.Cell(rowCount, 1).Value = item.UrunKodu;
                                    worksheet.Cell(rowCount, 3).Value = item.UrunAdi;
                                    worksheet.Cell(rowCount, 5).Value = item.urunAdedi;
                                    worksheet.Cell(rowCount, 7).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 7).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 7).Value = item.BirimFiyat;

                                    worksheet.Cell(rowCount, 8).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 8).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 8).FormulaA1 = "=E" + rowCount + "*$G$" + rowCount + "";


                                    worksheet.Cell(rowCount, 9).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 9).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 9).Value = dolar;

                                    worksheet.Cell(rowCount, 10).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 10).FormulaA1 = "=PRODUCT(H" + rowCount + "*(1.18)*F2)";


                                    worksheet.Cell(rowCount, 11).Style.NumberFormat.Format = "$0.00";
                                    worksheet.Cell(rowCount, 11).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 11).Value = item.SatisFiyati;

                                    worksheet.Cell(rowCount, 12).FormulaA1 = "=E" + rowCount + "*$K$" + rowCount + "";

                                    worksheet.Cell(rowCount, 13).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 13).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 13).Value = dolar;

                                    worksheet.Cell(rowCount, 14).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 14).FormulaA1 = "=PRODUCT(L" + rowCount + "*(0.18)*F2)";


                                    worksheet.Cell(rowCount, 15).FormulaA1 = "=SUM(L" + rowCount + "-H" + rowCount + ")";


                                    worksheet.Cell(rowCount, 16).Style.NumberFormat.Format = "0.00 TL";
                                    worksheet.Cell(rowCount, 16).DataType = XLDataType.Number;
                                    worksheet.Cell(rowCount, 16).FormulaA1 = "=((L" + rowCount + "*M" + rowCount + ")-(H" + rowCount + "*I" + rowCount + "))";

                                    worksheet.Cell(rowCount, 17).FormulaA1 = "=(P" + rowCount + "/(H" + rowCount + "*I" + rowCount + "))";
                                    worksheet.Cell(rowCount, 17).Style.NumberFormat.Format = "0.0%";


                                    rowCount++;
                                }
                                worksheet.Cell(rowCount, 8).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 8).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 8).FormulaA1 = "=SUM(H" + (rowCount - incrmnt) + ":H" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 8).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 10).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 10).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 10).FormulaA1 = "=SUM(J" + (rowCount - incrmnt) + ":J" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 10).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 12).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 12).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 12).FormulaA1 = "=SUM(L" + (rowCount - incrmnt) + ":L" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 12).Style.Font.Bold = true;


                                worksheet.Cell(rowCount, 14).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 14).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 14).FormulaA1 = "=SUM(J" + (rowCount - incrmnt) + ":J" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 14).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 15).Style.NumberFormat.Format = "$0.00";
                                worksheet.Cell(rowCount, 15).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 15).FormulaA1 = "=SUM(L" + (rowCount - incrmnt) + ":L" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 15).Style.Font.Bold = true;


                                worksheet.Cell(rowCount, 16).Style.NumberFormat.Format = "0.00 TL";
                                worksheet.Cell(rowCount, 16).DataType = XLDataType.Number;
                                worksheet.Cell(rowCount, 16).FormulaA1 = "=SUM(P" + (rowCount - incrmnt) + ":P" + (rowCount - 1) + ")";
                                worksheet.Cell(rowCount, 16).Style.Font.Bold = true;

                                worksheet.Cell(rowCount, 17).FormulaA1 = "=(P" + rowCount + ")/(J" + rowCount + "/(1.18))";
                                worksheet.Cell(rowCount, 17).Style.NumberFormat.Format = "0.0%";
                                worksheet.Cell(rowCount, 17).Style.Font.Bold = true;
                            }

                            counter--;
                        }

                    }


                    string filePath = "./File/file.xlsx";
                    string userEmail = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    string name = User.Identity.Name;
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs("./File/File.xlsx");
                        var work = workbook;
                        xlMail.SendXLMail(userEmail,xle.HazirlayanAdi,xle.KesifAdi, filePath);

                        System.IO.File.Delete("./File/File.xlsx");
                        
                    }
                }

                return Json(1);
            }
        }
    }
}
