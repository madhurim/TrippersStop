using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.ViewModels;

namespace BusinessLogic
{
    public class FareInsightsBusinessLayer : IBusinessLayer<FareOutput, FareOutput>
    {
        public FareOutput Process(FareOutput fareInput)
        {
            LowFareForecast forecast = fareInput.LowFareForecast;
            FareRange farerange = fareInput.FareRange;

            int lowfare = forecast.Forecast.LowestPredictedFare;
            int highestfare = forecast.Forecast.HighestPredictedFare;

            // parse fareforcast and farerange data
            // the result should parse to FareOutput : Farerange and fareforecast

            // if current lowest price is lower than lowest forecasted price : Recommend : Buy
            // if current lowest price is higher than highest forecasted price : Recommend : Wait
            // if current lowest price is between lowest and highest : Wait or buy

            //fare range
            // if current lowest price is in between the lower range : betn min and median , it's a good price
            // if lowest forecasted price is higher than median price and the current lowest price , then the trend is:  price is going higher.
            // then a buy should be recommended.


            return null;
        }
    }
}
