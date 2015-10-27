using ExpressMapper;
using System.Configuration;
using System.IO;
using TraveLayer.CustomTypes.Constants.Response;
using TraveLayer.CustomTypes.Constants.ViewModels;
using TrippismApi.TraveLayer;

namespace Trippism
{
    public static class ConstantsReader
    {
        //private static ICacheService _cacheService;
        private static string _expireTime = ConfigurationManager.AppSettings["RedisExpireInMin"].ToString();
        private static string TrippismCurrencySymbolsKey = "Trippism.CurrencySymbols";
        //public ConstantsReader(ICacheService cacheService)
        //{
        //    _cacheService = cacheService;
        //}

        public static CurrencySymbolsViewModel GetCurrencySymbols()
        {
            //var tripCurrencySymbols = _cacheService.GetByKey<TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel>(TrippismCurrencySymbolsKey);
            //if (tripCurrencySymbols != null)
              //  return tripCurrencySymbols;

            string jsonPath = GetFullPath(ConfigurationManager.AppSettings["CurrencySymbolsJsonPath"].ToString());            
            StreamReader readerCurrencySymbolJson = new StreamReader(jsonPath);
            string currencySymbolJsonString = readerCurrencySymbolJson.ReadToEnd();

            CurrencySymbols currencySymbols = new CurrencySymbols();
            currencySymbols = ServiceStackSerializer.DeSerialize<CurrencySymbols>(currencySymbolJsonString);
            TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel currencySymbolViewModel = Mapper.Map<CurrencySymbols, TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel>(currencySymbols);
            //_cacheService.Save<TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel>(TrippismCurrencySymbolsKey, currencySymbolViewModel);
            return currencySymbolViewModel;
        }

        public static string GetCurrencySymbolFromCode(string CurrencyCode)
        {
            CurrencySymbolsViewModel currencySymbolList = GetCurrencySymbols();
            var currency = currencySymbolList.Currency.Find(x => x.code == CurrencyCode);
            if (currency == null)
                return string.Empty;
            else
                return currency.symbol;
        }

        private static string GetFullPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath(path);
        }
    }
}