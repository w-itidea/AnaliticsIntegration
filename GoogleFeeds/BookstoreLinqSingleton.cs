using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace BookstoreData
//{
//    public static class BookstoreLinqSingleton
//    {
//        //database mocking
//        public static string connectionStringToUse;

//        /// <summary>
//        /// To jest po to byśmy przypadkiem nie podali bazy danych produkcyjnej dla Unit Testów.
//        /// </summary>
//        /// <returns></returns>
//        public static BookstoreData.BookstoreLinq GetBL()
//        {
//            if (connectionStringToUse == null)
//            {
//                //jeżeli nie został ustawiony inny connection string to znaczy się
//                //ze używamy defaultowego
//                return new BookstoreLinq();
//            }
//            else
//            {
//                //Unit Testing
//                return new BookstoreData.BookstoreLinq(connectionStringToUse);
//            }
//        }
//    }
//}
