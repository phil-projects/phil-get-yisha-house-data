using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PHIL_GetYiShaHouseData
{
    class Program
    {
        public class DateSold
        {
            public DateTime Date { get; set; }
            public int Sold { get; set; }
        }


        static void Main(string[] args)
        {

            var lsDateSolds = new List<DateSold>();
            var sbDateSolds = new StringBuilder();
            for (var i = 31; i >= 1; i--)
            {
                var url = string.Format("http://house.yinsha.com/file/71/index{0}.shtml", i);
                WebRequest wrt;
                wrt = WebRequest.Create(url);
                wrt.Credentials = CredentialCache.DefaultCredentials;
                WebResponse wrp;
                wrp = wrt.GetResponse();
                string reader = new StreamReader(wrp.GetResponseStream(), Encoding.GetEncoding("gb2312")).ReadToEnd();
                try
                {
                    wrt.GetResponse().Close();
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                var matches =
                    new Regex("(?<Date>[0-9]{4}年[0-9]{1,2}月[0-9]{1,2}日)商品房签约(?<Sold>[0-9]{1,5})套", RegexOptions.IgnoreCase)
                    .Matches(reader);
                foreach (Match match in matches)
                {
                    var dateSold = new DateSold()
                    {
                        Date = DateTime.Parse(match.Groups["Date"].Value),
                        Sold = int.Parse(match.Groups["Sold"].Value)
                    };
                    lsDateSolds.Add(dateSold);
                    sbDateSolds.AppendLine(dateSold.Date.ToString("yyyy-MM-dd")+"   "+ dateSold.Sold);
                    Console.WriteLine(dateSold.Date.ToString("yyyy-MM-dd") + "   " + dateSold.Sold);
                }
            }


            System.IO.StreamWriter _Writer
                   = new System.IO.StreamWriter(
                       @"D:\DateSold.txt",
                       true,
                       System.Text.Encoding.Default);
            _Writer.WriteLine(sbDateSolds.ToString());
            _Writer.Close();


            Console.ReadKey();

        }
    }
}
   