using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.GData.Analytics;
using Google.GData.Client;
using System.Text;

namespace MvcApplication1.Models
{
    public class TransactionDetailsService
    {


    public static string Start(string transactionId)
    {
      DataFeedExample example;

      try
      {
          example = new DataFeedExample(transactionId);
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
class DataFeedExample
{


    protected const String CLIENT_USERNAME = "jbrawo@gmail.com";
    protected const String CLIENT_PASS = "maciek22";
    protected const String TABLE_ID = "ga:36906246";

    //tutaj wszystko zapisujemy zaby potem wypluc ;) 
    public StringBuilder Console = new StringBuilder("");

    public DataFeed feed;
    public DataFeed feed2;


    public DataFeedExample()
    {
        //a to jest bo mi sie dziedicznie nie chciał zrobic
    }


    /**
 * Creates a new service object, attempts to authorize using the Client Login
 * authorization mechanism and requests data from the Google Analytics API.
 */
    public DataFeedExample(string transactionId)
    {



        // Configure GA API.
        AnalyticsService asv = new AnalyticsService("gaExportAPI_acctSample_v2.0");

        // Client Login Authorization.
        asv.setUserCredentials(CLIENT_USERNAME, CLIENT_PASS);

        // GA Data Feed query uri.
        String baseUrl = "https://www.google.com/analytics/feeds/data";

        DataQuery query = new DataQuery(baseUrl);
        query.Ids = TABLE_ID;
        query.Dimensions = "ga:source,ga:medium,ga:landingPagePath,ga:keyword,ga:adDestinationUrl,ga:adGroup,ga:adMatchedQuery";//,ga:adSlotPosition,ga:adSlot,ga:adPlacementUrl,ga:adDisplayUrl,ga:adDestinationUrl";//max 7  -takie ogranicznie
        query.Metrics = "ga:visits,ga:bounces,ga:transactions";
        //query.Segment = "gaid::-11";
        //query.Filters = "ga:transactionid==4059";
        query.Filters = "ga:transactionid==" + transactionId;
        query.Sort = "-ga:visits";
        query.NumberToRetrieve = 5;

        query.GAStartDate = "2012-03-01";
        query.GAEndDate = System.DateTime.Now.Date.ToString("yyyy-MM-dd");//ToShortDateString();// "2012-07-25";
        Uri url = query.Uri;
        
        
        Console.Append("URL: " + url.ToString());


        // Send our request to the Analytics API and wait for the results to
        // come back.

        feed = asv.Query(query);


    }

    /**
     * Prints the important Google Analytics relates data in the Data Feed.
     */
    public void printFeedData()
    {
         Console.Append("\n-------- Important Feed Information --------");
         Console.Append(
          "\nFeed Title      = " + feed.Title.Text +
          "\nFeed ID         = " + feed.Id.Uri +
          "\nTotal Results   = " + feed.TotalResults +
          "\nSart Index      = " + feed.StartIndex +
          "\nItems Per Page  = " + feed.ItemsPerPage
          );
    }

    /**
     * Prints the important information about the data sources in the feed.
     * Note: the GA Export API currently has exactly one data source.
     */
    public void printFeedDataSources()
    {

        DataSource gaDataSource = feed.DataSource;
         Console.Append("\n-------- Data Source Information --------");
         Console.Append(
          "\nTable Name      = " + gaDataSource.TableName +
          "\nTable ID        = " + gaDataSource.TableId +
          "\nWeb Property Id = " + gaDataSource.WebPropertyId +
          "\nProfile Id      = " + gaDataSource.ProfileId +
          "\nAccount Name    = " + gaDataSource.AccountName);
    }

    /**
     * Prints all the metric names and values of the aggregate data. The
     * aggregate metrics represent the sum of the requested metrics across all
     * of the entries selected by the query and not just the rows returned.
     */
    public void printFeedAggregates()
    {
         Console.Append("\n-------- Aggregate Metric Values --------");
        Aggregates aggregates = feed.Aggregates;

        foreach (Metric metric in aggregates.Metrics)
        {
             Console.Append(
              "\nMetric Name  = " + metric.Name +
              "\nMetric Value = " + metric.Value +
              "\nMetric Type  = " + metric.Type +
              "\nMetric CI    = " + metric.ConfidenceInterval);
        }
    }

    /**
     * Prints segment information if the query has an advanced segment defined.
     */
    public void printSegmentInfo()
    {
        if (feed.Segments.Count > 0)
        {
             Console.Append("\n-------- Advanced Segments Information --------");
            foreach (Segment segment in feed.Segments)
            {
                 Console.Append(
                    "\nSegment Name       = " + segment.Name +
                    "\nSegment ID         = " + segment.Id +
                    "\nSegment Definition = " + segment.Definition.Value);
            }
        }
    }

    /**
     * Prints all the important information from the first entry in the
     * data feed.
     */
    public void printDataForOneEntry()
    {
         Console.Append("\n-------- Important Entry Information --------\n");
        if (feed.Entries.Count == 0)
        {
             Console.Append("No entries found");
        }
        else
        {
            DataEntry singleEntry = feed.Entries[0] as DataEntry;

            // Properties specific to all the entries returned in the feed.
             Console.Append("Entry ID    = " + singleEntry.Id.Uri);
             Console.Append("Entry Title = " + singleEntry.Title.Text);

            // Iterate through all the dimensions.
            foreach (Dimension dimension in singleEntry.Dimensions)
            {
                 Console.Append("Dimension Name  = " + dimension.Name);
                 Console.Append("Dimension Value = " + dimension.Value);
            }

            // Iterate through all the metrics.
            foreach (Metric metric in singleEntry.Metrics)
            {
                 Console.Append("Metric Name  = " + metric.Name);
                 Console.Append("Metric Value = " + metric.Value);
                 Console.Append("Metric Type  = " + metric.Type);
                 Console.Append("Metric CI    = " + metric.ConfidenceInterval);
            }
        }
    }

    /**
     * Get the data feed values in the feed as a string.
     * @return {String} This returns the contents of the feed.
     */
    public String getEntriesAsTable()
    {
        if (feed.Entries.Count == 0)
        {
            return "No entries found";
        }
        DataEntry singleEntry = feed.Entries[0] as DataEntry;

        StringBuilder feedDataLines = new StringBuilder("\n-------- All Entries In A Table --------\n");

        // Put all the dimension and metric names into an array.
        foreach (Dimension dimension in singleEntry.Dimensions)
        {
            String[] args = { dimension.Name, dimension.Value };
            feedDataLines.AppendLine(String.Format("\n{0} \t= {1}", args));
        }
        foreach (Metric metric in singleEntry.Metrics)
        {
            String[] args = { metric.Name, metric.Value };
            feedDataLines.AppendLine(String.Format("\n{0} \t= {1}", args));
        }

        feedDataLines.Append("\n");
        return feedDataLines.ToString();
    }


   


}