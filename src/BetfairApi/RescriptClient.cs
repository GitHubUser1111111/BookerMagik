using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using BetfairApi.TO;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BetfairApi.Json;
using System.Net.Http.Headers;

namespace BetfairApi
{
    public class RescriptClient : IClient
    {
        public string EndPoint { get; private set; }
        private static readonly IDictionary<string, Type> operationReturnTypeMap = new Dictionary<string, Type>();
        public const string APPKEY_HEADER = "X-Application";
        public const string SESSION_TOKEN_HEADER = "X-Authentication";
        private static readonly string LIST_EVENT_TYPES_METHOD = "listEventTypes";
        private static readonly string LIST_MARKET_CATALOGUE_METHOD = "listMarketCatalogue";
        private static readonly string LIST_MARKET_TYPES_METHOD = "listMarketTypes";
        private static readonly string LIST_MARKET_BOOK_METHOD = "listMarketBook";
        private static readonly string PLACE_ORDERS_METHOD = "placeOrders";
        private static readonly string LIST_MARKET_PROFIT_AND_LOST_METHOD = "listMarketProfitAndLoss";
        private static readonly string LIST_CURRENT_ORDERS_METHOD = "listCurrentOrders";
        private static readonly string LIST_CLEARED_ORDERS_METHOD = "listClearedOrders";
        private static readonly string CANCEL_ORDERS_METHOD = "cancelOrders";
        private static readonly string REPLACE_ORDERS_METHOD = "replaceOrders";
        private static readonly string UPDATE_ORDERS_METHOD = "updateOrders";
        private static readonly String FILTER = "filter";
        private static readonly String LOCALE = "locale";
        private static readonly String CURRENCY_CODE = "currencyCode";
        private static readonly String MARKET_PROJECTION = "marketProjection";
        private static readonly String MATCH_PROJECTION = "matchProjection";
        private static readonly String ORDER_PROJECTION = "orderProjection";
        private static readonly String PRICE_PROJECTION = "priceProjection";
        private static readonly String SORT = "sort";
        private static readonly String MAX_RESULTS = "maxResults";
        private static readonly String MARKET_IDS = "marketIds";
        private static readonly String MARKET_ID = "marketId";
        private static readonly String INSTRUCTIONS = "instructions";
        private static readonly String CUSTOMER_REFERENCE = "customerRef";
        private static readonly string INCLUDE_SETTLED_BETS = "includeSettledBets";
        private static readonly String INCLUDE_BSP_BETS = "includeBspBets";
        private static readonly String NET_OF_COMMISSION = "netOfCommission";
        private static readonly String BET_IDS = "betIds";
        private static readonly String PLACED_DATE_RANGE = "placedDateRange";
        private static readonly String ORDER_BY = "orderBy";
        private static readonly String SORT_DIR = "sortDir";
        private static readonly String FROM_RECORD = "fromRecord";
        private static readonly String RECORD_COUNT = "recordCount";
        private static readonly string BET_STATUS = "betStatus";
        private static readonly string EVENT_TYPE_IDS = "eventTypeIds";
        private static readonly string EVENT_IDS = "eventIds";
        private static readonly string RUNNER_IDS = "runnerIds";
        private static readonly string SIDE = "side";
        private static readonly string SETTLED_DATE_RANGE = "settledDateRange";
        private static readonly string GROUP_BY = "groupBy";
        private static readonly string INCLUDE_ITEM_DESCRIPTION = "includeItemDescription";

        private readonly HttpClient httpClient;

        public RescriptClient(string endPoint, string appKey, string sessionToken)
		{
            EndPoint = endPoint;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(APPKEY_HEADER, appKey);
            httpClient.DefaultRequestHeaders.Add(SESSION_TOKEN_HEADER, sessionToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IList<EventTypeResult>> listEventTypes(MarketFilter marketFilter, string locale = null)
        {
            var args = new Dictionary<string, object>();
            args[FILTER] = marketFilter;
            args[LOCALE] = locale;
            return await Invoke<List<EventTypeResult>>(LIST_EVENT_TYPES_METHOD, args);

        }

        public async Task<IList<MarketCatalogue>> listMarketCatalogue(MarketFilter marketFilter, ISet<MarketProjection> marketProjections, MarketSort marketSort, string maxResult = "1", string locale = null)
        {
            var args = new Dictionary<string, object>();
            args[FILTER] = marketFilter;
            args[MARKET_PROJECTION] = marketProjections;
            args[SORT] = marketSort;
            args[MAX_RESULTS] = maxResult;
            args[LOCALE] = locale;
            return await Invoke<List<MarketCatalogue>>(LIST_MARKET_CATALOGUE_METHOD, args);
        }

        public async Task<IList<MarketBook>> listMarketBook(IList<string> marketIds, PriceProjection priceProjection, OrderProjection? orderProjection = null, MatchProjection? matchProjection = null, string currencyCode = null, string locale = null)
        {
            var args = new Dictionary<string, object>();
            args[MARKET_IDS] = marketIds;
            args[PRICE_PROJECTION] = priceProjection;
            args[ORDER_PROJECTION] = orderProjection;
            args[MATCH_PROJECTION] = matchProjection;
            args[LOCALE] = locale;
            args[CURRENCY_CODE] = currencyCode;
            return await Invoke<List<MarketBook>>(LIST_MARKET_BOOK_METHOD, args);
        }

        public async Task<PlaceExecutionReport> placeOrders(string marketId, string customerRef, IList<PlaceInstruction> placeInstructions, string locale = null)
        {
            var args = new Dictionary<string, object>();

            args[MARKET_ID] = marketId;
            args[INSTRUCTIONS] = placeInstructions;
            args[CUSTOMER_REFERENCE] = customerRef;
            args[LOCALE] = locale;

            return await Invoke<PlaceExecutionReport>(PLACE_ORDERS_METHOD, args);
        }
        
        public async Task<T> Invoke<T>(string method, IDictionary<string, object> args = null)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            if (method.Length == 0)
                throw new ArgumentException(null, "method");

            var postData = JsonConvert.Serialize<IDictionary<string, object>>(args);

            try
            {
                HttpContent content = new StringContent(postData, Encoding.UTF8, "application/json");
                var uri = new Uri($"{EndPoint}/{method}/");
                var result = await httpClient.PostAsync(uri, content);
                result.EnsureSuccessStatusCode();
                var jsonResponse = await result.Content.ReadAsStringAsync();
                return JsonConvert.Deserialize<T>(jsonResponse);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw e;
            }
        }
        
        private static System.Exception ReconstituteException(BetfairApi.TO.Exception ex)
        {
            var data = ex.Detail;
        
            // API-NG exception -- it must have "data" element to tell us which exception
            var exceptionName = data.Property("exceptionname").Value.ToString();
            var exceptionData = data.Property(exceptionName).Value.ToString();
            return JsonConvert.Deserialize<APINGException>(exceptionData);
            
        }

        public async Task<IList<MarketProfitAndLoss>> listMarketProfitAndLoss(IList<string> marketIds, bool includeSettledBets = false, bool includeBspBets = false, bool netOfCommission = false)
        {
            var args = new Dictionary<string, object>();
            args[MARKET_IDS] = marketIds;
            args[INCLUDE_SETTLED_BETS] = includeSettledBets;
            args[INCLUDE_BSP_BETS] = includeBspBets;
            args[NET_OF_COMMISSION] = netOfCommission;

            return await Invoke<List<MarketProfitAndLoss>>(LIST_MARKET_PROFIT_AND_LOST_METHOD, args);
        }

        public async Task<CurrentOrderSummaryReport> listCurrentOrders(ISet<String> betIds, ISet<String> marketIds, OrderProjection? orderProjection = null, TimeRange placedDateRange = null, OrderBy? orderBy = null, SortDir? sortDir = null, int? fromRecord = null, int? recordCount = null)
        {
            var args = new Dictionary<string, object>();
            args[BET_IDS] = betIds;
            args[MARKET_IDS] = marketIds;
            args[ORDER_PROJECTION] = orderProjection;
            args[PLACED_DATE_RANGE] = placedDateRange;
            args[ORDER_BY] = orderBy;
            args[SORT_DIR] = sortDir;
            args[FROM_RECORD] = fromRecord;
            args[RECORD_COUNT] = recordCount;

            return await Invoke<CurrentOrderSummaryReport>(LIST_CURRENT_ORDERS_METHOD, args);
        }

        public async Task<ClearedOrderSummaryReport> listClearedOrders(BetStatus betStatus, ISet<string> eventTypeIds = null, ISet<string> eventIds = null, ISet<string> marketIds = null, ISet<RunnerId> runnerIds = null, ISet<string> betIds = null, Side? side = null, TimeRange settledDateRange = null, GroupBy? groupBy = null, bool? includeItemDescription = null, String locale = null, int? fromRecord = null, int? recordCount = null)
        {
            var args = new Dictionary<string, object>();
            args[BET_STATUS] = betStatus;
            args[EVENT_TYPE_IDS] = eventTypeIds;
            args[EVENT_IDS] = eventIds;
            args[MARKET_IDS] = marketIds;
            args[RUNNER_IDS] = runnerIds;
            args[BET_IDS] = betIds;
            args[SIDE] = side;
            args[SETTLED_DATE_RANGE] = settledDateRange;
            args[GROUP_BY] = groupBy;
            args[INCLUDE_ITEM_DESCRIPTION] = includeItemDescription;
            args[LOCALE] = locale;
            args[FROM_RECORD] = fromRecord;
            args[RECORD_COUNT] = recordCount;

            return await Invoke<ClearedOrderSummaryReport>(LIST_CLEARED_ORDERS_METHOD, args);
        }

        public async Task<CancelExecutionReport> cancelOrders(string marketId, IList<CancelInstruction> instructions, string customerRef)
        {
            var args = new Dictionary<string, object>();
            args[MARKET_ID] = marketId;
            args[INSTRUCTIONS] = instructions;
            args[CUSTOMER_REFERENCE] = customerRef;

            return await Invoke<CancelExecutionReport>(CANCEL_ORDERS_METHOD, args);
        }

        public async Task<ReplaceExecutionReport> replaceOrders(String marketId, IList<ReplaceInstruction> instructions, String customerRef)
        {
            var args = new Dictionary<string, object>();
            args[MARKET_ID] = marketId;
            args[INSTRUCTIONS] = instructions;
            args[CUSTOMER_REFERENCE] = customerRef;

            return await Invoke<ReplaceExecutionReport>(REPLACE_ORDERS_METHOD, args);
        }

        public async Task<UpdateExecutionReport> updateOrders(String marketId, IList<UpdateInstruction> instructions, String customerRef)
        {
            var args = new Dictionary<string, object>();
            args[MARKET_ID] = marketId;
            args[INSTRUCTIONS] = instructions;
            args[CUSTOMER_REFERENCE] = customerRef;

            return await Invoke<UpdateExecutionReport>(UPDATE_ORDERS_METHOD, args);
        }


        public async Task<IList<MarketTypeResult>> listMarketTypes(MarketFilter marketFilter, string stringLocale)
        {
            var args = new Dictionary<string, object>();
            args[FILTER] = marketFilter;
            args[LOCALE] = stringLocale;
            return await Invoke<List<MarketTypeResult>>(LIST_MARKET_TYPES_METHOD, args);

        }

        public static string GetToken(string p12CertificateLocation, string p12CertificatePassword, string appKey, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(p12CertificateLocation)) throw new ArgumentException("p12CertificateLocation");
            //  if (string.IsNullOrWhiteSpace(p12CertificatePassword)) throw new ArgumentException("p12CertificatePassword");
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("username");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password");
            if (!File.Exists(p12CertificateLocation)) throw new ArgumentException("p12CertificateLocation not found! Full path: " + p12CertificateLocation);

            string postData = string.Format("username={0}&password={1}", username, password);
            X509Certificate2 x509certificate = new X509Certificate2(p12CertificateLocation, p12CertificatePassword);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://identitysso-cert.betfair.com/api/certlogin");
            request.UseDefaultCredentials = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("X-Application", appKey);
            request.ClientCertificates.Add(x509certificate);
            request.Accept = "*/*";

            using (Stream stream = request.GetRequestStream())
            using (StreamWriter writer = new StreamWriter(stream, Encoding.Default))
            {
                writer.Write(postData);
            }

            using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.Default))
            {
                var jsonResponse = reader.ReadToEnd();
                var loginResponse = JsonConvert.Deserialize<LoginResponse>(jsonResponse);
                return loginResponse.SessionToken;
            }
        }
    }
}
