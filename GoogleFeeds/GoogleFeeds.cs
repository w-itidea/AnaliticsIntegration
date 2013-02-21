using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;

namespace GoogleFeeds
{

    public class Feeds
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
        public static void GenerateRSS(Stream stream, string waluta = "USD", int _start = 0, int _stop = 10 )
        {
            
            
            #region Ustawienia

            const string googleBaseNamespace = "http://base.google.com/ns/1.0";

            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8
               
            };

            //String _waluta = " USD";
            //var p = LibBLL.CMSBLL._context.Portal_GetArticles(null, null, null, 20);
            string TEMP_LINK = "http://thepolishbookstore.com/p/";
            //string TEMP_IMG_LINK = "http://thepolishbookstore.com/product_images_thumbs/";
            string TEMP_SITE_URI = "http://thepolishbookstore.com";

            string shipingString =  @"
                    			<g:shipping>
				<g:country>US</g:country>
				<g:service>Standard</g:service>
				<g:price>4 USD</g:price>				
			</g:shipping>
			<g:shipping>
				<g:country>AUS</g:country>
				<g:service>Standard</g:service>
				<g:price>4 USD</g:price>				
			</g:shipping>
			<g:shipping>
				<g:country>CA</g:country>
				<g:service>Standard</g:service>
				<g:price>4 USD</g:price>				
			</g:shipping>                    
                    ".Replace("USD", waluta);


            #endregion             


            //database connection

            using (var writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("rss");
                writer.WriteAttributeString("version", "2.0");
                writer.WriteAttributeString("xmlns", "g", null, googleBaseNamespace);
                writer.WriteStartElement("channel");
                writer.WriteElementString("title", string.Format("{0}", "The Polish Bookstore - Polska Ksiegarnia w USA and Canada"));
                writer.WriteElementString("link", TEMP_SITE_URI);
                writer.WriteElementString("description", "Selection of Polish Books and AudioBooks from our shop");


                
                
                var context = new FeedsDataContextDataContext();

                var producst = context.ITidea_GoogleFeeds();

                //**************************************** */
                //POCZATEK GENEROWANIA ELEMENTÓW
                //**************************************** */




                 //int i = 0;
                foreach (var item in producst)
                {

                    writer.WriteStartElement("item");
                  
                    writer.WriteStartElement("title");
                    writer.WriteCData(getTitle(item));
                    writer.WriteEndElement();

                    writer.WriteElementString("link", TEMP_LINK + item.productID + "/" + BookstorConstDictionary.GetSeName(item.title, true, false));


                    string _desc =
                        (String.IsNullOrEmpty(item.tytuloryg) ? "Polish Book '" + item.title + "'" : ("This is a polish edition of " + item.tytuloryg))

                         + (String.IsNullOrEmpty(item.author) ? "" : (" by " + item.author))
                         
                            ///TODO: Dorobic Cover type, published, book language 
                         
                        + " Description in polish: "  + item.shortdescription

                         ;
                    writer.WriteStartElement("description");
                    writer.WriteCData(_desc);
                    writer.WriteEndElement(); // description                        

                    writer.WriteElementString("g","id", googleBaseNamespace, "bookitm_" + item.productID );
                    writer.WriteElementString("g", "condition", googleBaseNamespace, "new");

                    writer.WriteElementString("g", "price", googleBaseNamespace, item.price.ToString(CultureInfo.InvariantCulture) + " " + waluta);

                    writer.WriteElementString("g","availability", googleBaseNamespace,  "in stock");

                    writer.WriteElementString("g", "gtin", googleBaseNamespace,  String.IsNullOrEmpty(item.isbn) ? "98745" + item.productID : item.isbn);

                    //writer.WriteElementString("g", "image_link", googleBaseNamespace, TEMP_IMG_LINK + item.img_uri);
                    writer.WriteElementString("g", "image_link", googleBaseNamespace,TEMP_SITE_URI + item.img_uri_src);
                     //<g:image_link>http://thepolishbookstore.com/product_images_thumbs/0172457_300.jpg</g:image_link>


                    var _weight = (item.ciezar * (decimal)2.20).ToString(CultureInfo.InvariantCulture) + " lbs";

                    writer.WriteElementString("g","weight", googleBaseNamespace, _weight);



                    writer.WriteElementString("g","google_product_category", googleBaseNamespace, BookstorConstDictionary.dzial2GoogleCategory(item.dzial) );
                    writer.WriteElementString("g", "product_type", googleBaseNamespace,BookstorConstDictionary.dzial2ItemCategory(item.dzial));
                 
                    


            //                    <g:google_product_category>Media &gt; Books</g:google_product_category>
            //<g:product_type><![CDATA[Books > Books in Polish > Fiction > Literature]]></g:product_type>



                    
                    //shiping
                    writer.WriteRaw(shipingString);



                    writer.WriteElementString("g", "expiration_date", googleBaseNamespace, DateTime.Now.AddDays(28).ToString("yyyy-MM-dd"));

                    writer.WriteEndElement(); // item

                }//end   foreach (var item in p)

                //**************************************** */
                //KONIEC GENEROWANIA ELEMENTÓW
                //**************************************** */


                writer.WriteEndElement(); // channel
                writer.WriteEndElement(); // rss
                writer.WriteEndDocument();
            }
        }



        #region Metody odpowiedzialne za formatowanie tekstów pod kontem "marketingowym"

        /// <summary>
        /// generuje odpowieni tytuł. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string getTitle(ITidea_GoogleFeedsResult item)
        {
            
            string _title = "";

            string _author = (item.author == "Praca Zbiorowa" || item.author.Length > 40) ? "" : " by " + item.author;

            

            
            ///TODO - doropobuc działy np. Polish audio book polish book
            ///koncówka (fity shades of Grey in polish) 
            
            string _end = 
                (String.IsNullOrEmpty(item.tytuloryg) ?
                ""
                : " (" + item.tytuloryg + " in Polish )");

            
            //Sprawdzamy czy zmieści się tytuł z końcówką, jak nie ucinamy
            if ( item.title.Length - _end.Length > 70)
            {
                _title = item.title.Substring(0, 69 - _end.Length ) + _end;
                
                return _title;//konczymy , bo i tak nic sie już nie zmieści

            } else {
                _title = item.title + _author + _end;


                if (_title.Length > 70)
                { //sprawdzamy cy przypadkiem nie jest za długie 
                    _title = item.title + _end;
                }


            }

            return _title;

        }

        #endregion
    }

}
