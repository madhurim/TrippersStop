// *** Service Calling Proxy Class
function serviceProxyPost(serviceUrl) {
    var _I = this;
    this.serviceUrl = serviceUrl;

    // *** Call a wrapped object
    this.invoke = function (method, data, callback, error, bare) {
        // *** Convert input data into JSON - REQUIRES Json2.js
        var json = JSON2.stringify(data);

        // *** The service endpoint URL        
        var url = _I.serviceUrl + method;

        $.ajax({
            url: url,
            data: json,
            type: "POST",
            processData: false,
            contentType: "application/json",
            timeout: 10000,
            dataType: "text",  // not "json" we'll parse
            success:
            function (res) {
                if (!callback) return;

                // *** Use json library so we can fix up MS AJAX dates
                var result = JSON2.parse(res);

                // *** Bare message IS result
                if (bare)
                { callback(result); return; }

                // *** Wrapped message contains top level object node
                // *** strip it off
                for (var property in result) {
                    callback(result[property]);
                    break;
                }
            },
            error: function (xhr) {
                if (!error) return;
                if (xhr.responseText) {
                    var err = JSON2.parse(xhr.responseText);
                    if (err)
                        error(err);
                    else
                        error({ Message: "Unknown server error." })
                }
                return;
            }
        });
    }
}
function serviceProxyGet(serviceUrl) {
    var _I = this;
    this.serviceUrl = serviceUrl;

    // *** Call a wrapped object
    this.invoke = function (method, callback, error, bare) {
       
        // *** The service endpoint URL        
        var url = _I.serviceUrl + method;

        $.ajax({
            url: url,
            type: "GET",
            processData: false,
            contentType: "application/json",
            timeout: 10000,
            dataType: "text",  // not "json" we'll parse
            success:
            function (res) {
                if (!callback) return;

                // *** Use json library so we can fix up MS AJAX dates
                var result = JSON2.parse(res);

                // *** Bare message IS result
                if (bare)
                { callback(result); return; }

                // *** Wrapped message contains top level object node
                // *** strip it off
                for (var property in result) {
                    callback(result[property]);
                    break;
                }
            },
            error: function (xhr) {
                if (!error) return;
                if (xhr.responseText) {
                    var err = JSON2.parse(xhr.responseText);
                    if (err)
                        error(err);
                    else
                        error({ Message: "Unknown server error." })
                }
                return;
            }
        });
    }
}
// *** Create a static instance
var ProxyGet = new serviceProxyGet("/sabre/api/");

var origin = $("#txtOrigin").val();
ProxyGet.invoke("destinations",
    function (result) {
        //var result = serviceResponse.GetStockQuoteResult;

        $("#StockName").text(result.Company + " (" + result.Symbol + ")");
        $("#LastPrice").text(result.LastPrice.toFixed(2));
        $("#OpenPrice").text(result.OpenPrice.toFixed(2));
        $("#QuoteTime").text(result.LastQuoteTimeString);
        $("#NetChange").text(result.NetChange.toFixed(2));

        // *** if hidden make visible
        var sr = $("#divStockQuoteResult:hidden").slideDown("slow");

        // *** Also graph it
        var stocks = [];
        stocks.push(result.Symbol);
        var url = GetStockGraphUrl(stocks, result.Company, 350, 150, 2);
        $("#imgStockQuoteGraph").attr("src", url);
    },
    onPageError);

var ProxyPost = new serviceProxyPost("/sabre/api/");
var origin = $("#txtOrigin").val();
ProxyPost.invoke("BargainFinder", { origin: origin},
    function (result) {
        //var result = serviceResponse.GetStockQuoteResult;

        $("#StockName").text(result.Company + " (" + result.Symbol + ")");
        $("#LastPrice").text(result.LastPrice.toFixed(2));
        $("#OpenPrice").text(result.OpenPrice.toFixed(2));
        $("#QuoteTime").text(result.LastQuoteTimeString);
        $("#NetChange").text(result.NetChange.toFixed(2));

        // *** if hidden make visible
        var sr = $("#divStockQuoteResult:hidden").slideDown("slow");

        // *** Also graph it
        var stocks = [];
        stocks.push(result.Symbol);
        var url = GetStockGraphUrl(stocks, result.Company, 350, 150, 2);
        $("#imgStockQuoteGraph").attr("src", url);
    },
    onPageError);


