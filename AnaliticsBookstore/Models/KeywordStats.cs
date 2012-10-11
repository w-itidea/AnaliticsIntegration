using Google.GData.Analytics;
using System;
using System.Globalization;

namespace MvcApplication1.Models
{
    partial class GoogleKeywordAnalytic
    {
        public GoogleKeywordAnalytic(DataEntry entry)
        {
                pageUri = (entry.Dimensions[0].Value + entry.Dimensions[1].Value);
                googleKeyword = entry.Dimensions[2].Value;
                sumcount = Convert.ToInt32(entry.Metrics[0].Value);
                bounceRate = (float)double.Parse(entry.Metrics[1].Value.Replace(",", "."), CultureInfo.InvariantCulture); // Zamienia string na float niezale¿nie od u¿ytego znaku odzielaj¹cego czêœæ u³amkow¹ od ca³kowitej
        }
    }
}
