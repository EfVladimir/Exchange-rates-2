using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Data.Entity;

namespace Task3
{
    public class DbWorks
    {
        public static void AddData(List<TableExchangeRate> rates)
        {
            using (Model1 db = new Model1())
            {
                foreach (TableExchangeRate rate in rates)
                {
                    db.TableExchangeRate.Add(rate);
                    db.SaveChanges();
                }
            }
        }
        public static List<TableExchangeRate> ReadXml(out string msg)
        {
            List<TableExchangeRate> TableExchangeRates = new List<TableExchangeRate>();
            try
            {
                string path = "https://www.nationalbank.kz/rss/rates_all.xml";
                //string path = "test.txt";
                XmlDocument Xdoc = new XmlDocument();
                Xdoc.Load(path);
                XmlNodeList clist = Xdoc.GetElementsByTagName("title");
                XmlNodeList desc = Xdoc.GetElementsByTagName("description");
                string example = "Official exchange rates of National Bank of Republic Kazakhstan";
                for (int i = 0; i < clist.Count; i++)
                {
                    TableExchangeRate con = new TableExchangeRate();
                    if (desc[i].InnerText.ToString() != example)
                    {
                        con.Name = clist[i].InnerText.ToString();
                        con.Value = Convert.ToDouble(desc[i].InnerText.Replace(".", ","));
                        TableExchangeRates.Add(con);
                    }
                }
                msg = "XML успешно прочитан";
                return TableExchangeRates;
            }
            catch(Exception ex)
            {
                msg = ex.Message;
                return null;
            }
            

        }
        public static void CheckData()
        {
            int i = 1;
            using(Model1 db = new Model1())
            {
                List<TableExchangeRate> rates = db.TableExchangeRate.ToList();
                List<TableExchangeRate> tableExchangeRates = DbWorks.ReadXml();
                if (rates.Count < 1)
                {
                    AddData(tableExchangeRates);
                }
                foreach (var rate in rates.Zip(tableExchangeRates, (i1, i2) => Tuple.Create(i1, i2)))
                {
                    TableExchangeRate rate1 = new TableExchangeRate();
                    if (rate.Item1.Name == rate.Item2.Name && rate.Item1.Value == rate.Item2.Value) { }
                    else
                    {
                        rate1 = db.TableExchangeRate.Find(i);
                        rate1.Value = rate.Item2.Value;
                        db.SaveChanges();
                        Console.WriteLine("Изменения прошли у валюты {0}", rate.Item2.Name);
                    }
                    i++;
                }
            }
            Console.WriteLine("Код прошел");
        }
        public static void Print()
        {
            using(Model1 db = new Model1())
            {
                var data = db.TableExchangeRate.Select(s => s.Name + " " + s.Value).ToList();
                foreach (var item in data)
                    Console.WriteLine(item);
            }
            
        }
    }
}
