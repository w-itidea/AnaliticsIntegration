using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace GoogleFeeds
{
    public class Rss
    {
        /// <summary>
        /// Generate froogle RSS. 
        /// 
        /// Klasa pobrana z projekty byawy.pl LibBLL.RSS
        /// </summary>
        /// <example>
        /// context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        /// int categoryId = 2;
        /// LibBLL.Rss.GenerateRSS(context.Response.OutputStream, 1, 2, categoryId);
        /// </example>
        /// <param name="stream">Stream</param>
        public static void GenerateRSS(Stream stream, int _start = 0, int _stop = 10, int _categoryId = 0)
        {
            const string googleBaseNamespace = "http://base.google.com/ns/1.0";

            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8
            };

//database connection

            using (var writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("rss");
                writer.WriteAttributeString("version", "2.0");
                writer.WriteAttributeString("xmlns", "g", null, googleBaseNamespace);
                writer.WriteStartElement("channel");
                writer.WriteElementString("title", string.Format("{0}", "Co w trawie piszczy dla turysty w byway.pl "));
                writer.WriteElementString("link", "http://byway.pl/");
                writer.WriteElementString("description", "Artykuły, newsy, podpowiedzi, inspiracje, pomysły na weekend, najnowsze atrakcje, ciekawosty ze świata turystyki  ");




                //**************************************** */
                //POCZATEK GENEROWANIA ELEMENTÓW
                //**************************************** */

           
                //var p = LibBLL.CMSBLL._context.Portal_GetArticles(null, null, null, 20);
                //string TEMP_LINK = "http://byway.pl/Magazyn/Article.aspx?guid=";


                //int i = 0;
                //foreach (var item in p)
                //{
     
                    

                //    writer.WriteStartElement("item");
                //    //TODO: to spowalnia ..

                //    writer.WriteElementString("link", TEMP_LIN + item.guid);
                //    writer.WriteElementString("title", item.title);
                //    writer.WriteElementString("pubDate", item.publicationdatestart.Value.ToString("r"));
                //    writer.WriteElementString("comments", TEMP_LIN + item.guid + "#comments");
                //    writer.WriteStartElement("description");

                //    /*** Zavzynamy budować description */

                //    writer.WriteCData(item.@abstract);


                //    writer.WriteEndElement(); // description                        
        
                //    writer.WriteEndElement(); // item

                //}//end   foreach (var item in p)

                //**************************************** */
                //KONIEC GENEROWANIA ELEMENTÓW
                //**************************************** */


                writer.WriteEndElement(); // channel
                writer.WriteEndElement(); // rss
                writer.WriteEndDocument();
            }
        }

    }
}
