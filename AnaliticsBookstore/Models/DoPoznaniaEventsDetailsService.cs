using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.GData.Analytics;
using Google.GData.Client;
using System.Text;

namespace MvcApplication1.Models
{

    //to jest kopia TransactionDetailsService
    public class DoPoznaniaEventsDetailsService 
    {


    public static string Start(string eventValue)
    {
      DataFeedExample2 example;

      try
      {
          example = new DataFeedExample2();
          example.GetDoPoznaniaData(eventValue);
      }
      catch (AuthenticationException e)
      {
        ///Console.Error.WriteLine("Authentication failed : " + e.Message);
        return "Bład autoryzacji Google - sprawć login i hasło " + e.Message;
      }
      catch (Google.GData.Client.GDataRequestException e)
      {
        //Console.Error.WriteLine("Authentication failed : " + e.Message);
          return "Googlowi nie podoba się Twoje zapytanie - zobacz pon o co pytosz. Może za dużo tych zapytań?" + e.Message;
      }

      example.printFeedData();
      example.printFeedDataSources();
      example.printFeedAggregates();
      example.printSegmentInfo();
      example.printDataForOneEntry();

      return example.Console.Append(example.getEntriesAsTable()).ToString();
    }

 }
}



/// <summary>
/// Przykładowa klasa do pobierania danych z analitics
/// </summary>
class DataFeedExample2 : DataFeedExample
{


    //private const String CLIENT_USERNAME = "jbrawo@gmail.com";
    //private const String CLIENT_PASS = "maciek22";
    public const String TABLE_ID = "ga:49809674"; 



    /**
 * Creates a new service object, attempts to authorize using the Client Login
 * authorization mechanism and requests data from the Google Analytics API.
 */
    public void GetDoPoznaniaData(string value)
    {



        // Configure GA API.
        AnalyticsService asv = new AnalyticsService("gaExportAPI_acctSample_v2.0");

        // Client Login Authorization.
        asv.setUserCredentials(CLIENT_USERNAME, CLIENT_PASS);

        // GA Data Feed query uri.
        String baseUrl = "https://www.google.com/analytics/feeds/data";

        DataQuery query = new DataQuery(baseUrl);
        query.Ids = TABLE_ID;
        query.Dimensions = "ga:source,ga:medium";//,ga:adSlotPosition,ga:adSlot,ga:adPlacementUrl,ga:adDisplayUrl,ga:adDestinationUrl";//max 7  -takie ogranicznie
        query.Metrics = "ga:visits,ga:bounces";
        //query.Segment = "gaid::-11";
        //query.Filters = "ga:transactionid==4059";
        //query.Filters = "ga:eventValue==" + value;
        query.Filters = "ga:eventValue==" + "51";
        query.Sort = "-ga:visits";
        query.NumberToRetrieve = 5;

        query.GAStartDate = "2012-03-01";
        query.GAEndDate = System.DateTime.Now.Date.ToString("RRRR-MM-DD");// ToShortDateString();// "2012-07-25";
        Uri url = query.Uri;
        
        
        Console.Append("URL: " + url.ToString());


        // Send our request to the Analytics API and wait for the results to
        // come back.

        feed = asv.Query(query);


    }



   


}