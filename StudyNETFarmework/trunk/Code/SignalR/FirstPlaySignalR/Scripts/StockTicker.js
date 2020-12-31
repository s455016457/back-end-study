// 一个简单的模板方法，用于替换用大括号括起来的占位符。
if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

$(function () {
    var StockTicker = {
        ticker: $.connection.stockTickerMini, // 生成的客户端集线器代理
        up: '▲',
        down: '▼',
        stockTable: $('#stockTable'),
        stockTicker: $("#stockTicker"),
        stockTableBody: $('#stockTable').find('tbody'),
        stockTickerUl: $("#stockTicker ul"),
        rowTemplate: '<tr data-symbol="{Symbol}"><td>{Symbol}</td><td>{Price}</td><td>{DayOpen}</td><td>{High}</td><td>{Low}</td><td>{Direction} {Change}</td><td>{PercentChange}</td></tr>',
        liTemplate:'<li data-symbol="{Symbol}"><span class="symbol">{Symbol}</span><span class="price">{Price}</span><span class="change">{Direction}{Change}({PercentChange})</span>)</li>',
        init: function () {
            StockTicker.ticker.server.getAllStocks().done(function (stocks) {
                StockTicker.stockTableBody.empty();
                StockTicker.stockTickerUl.empty();
                $.each(stocks, function () {
                    var stock = formatStock(this);
                    console.log(stock);
                    var $li = CreateLi(stock),
                        $row = CreateRow(stock);
                    StockTicker.stockTableBody.append($row);
                    StockTicker.stockTickerUl.append($li);
                });
            });
        }
    };

    // 添加服务器将调用的客户端hub方法
    StockTicker.ticker.client.updateStockPrice = function (stock) {
        var displayStock = formatStock(stock),
            $li = CreateLi(displayStock),
            $row = CreateRow(displayStock);

        StockTicker.stockTableBody.find('tr[data-symbol=' + stock.Symbol + ']').replaceWith($row);
        StockTicker.stockTickerUl.find('li[data-symbol=' + stock.Symbol + ']').replaceWith($li);
    };

    StockTicker.ticker.client.marketOpened = function () {
        $("#openMarket").prop("disabled", true);
        $("#closeMarket").prop("disabled", false);
        $("#resetMarket").prop("disabled", true);
        scrollTicker();
    };
    StockTicker.ticker.client.marketClosed = function () {
        $("#openMarket").prop("disabled", false);
        $("#closeMarket").prop("disabled", true);
        $("#resetMarket").prop("disabled", false);
        StockTicker.stockTickerUl.stop();
    };
    StockTicker.ticker.client.marketReset = function () {
        return StockTicker.init();
    };

    function formatStock(stock) {
        return $.extend(stock, {
            Price: stock.Price.toFixed(2),
            PercentChange: (stock.PercentChange * 100).toFixed(2) + '%',
            Direction: stock.Change === 0 ? '' : stock.Change >= 0 ? StockTicker.up : StockTicker.down
        });
    }

    function CreateRow(stock) {
        var $row = $(StockTicker.rowTemplate.supplant(stock));
        setDocStyle(stock.Change, $row);
        return $row;
    }

    function CreateLi(stock) {
        var $li = $(StockTicker.liTemplate.supplant(stock));
        setDocStyle(stock.Change, $li);
        return $li;
    }

    function setDocStyle(change,$doc) {
        if (change > 0) {
            $doc.addClass("up");
            $doc.removeClass("down");
            $doc.removeClass("noChange");
        }
        else if (change < 0) {
            $doc.addClass("down");
            $doc.removeClass("up");
            $doc.removeClass("noChange");
        } else {
            $doc.removeClass("up");
            $doc.removeClass("down");
            $doc.addClass("noChange");
        }
    }

    function scrollTicker() {
        var w = StockTicker.stockTicker.width();
        StockTicker.stockTickerUl.css({ marginLeft: w });
        StockTicker.stockTickerUl.animate({ marginLeft: -StockTicker.stockTickerUl.width() }, 15000, 'linear', scrollTicker);
    }

    // 客户端 启用日志
    $.connection.hub.logging = true;
    // 开始连接
    $.connection.hub.start()
        .pipe(StockTicker.init)
        .pipe(function () {
            return StockTicker.ticker.server.getMarketState();
        })
        .done(function (state) {
            if (state === 'Open') {
                StockTicker.ticker.client.marketOpened();
            } else {
                StockTicker.ticker.client.marketClosed();
            }

            // Wire up the buttons
            $("#openMarket").click(function () {
                StockTicker.ticker.server.openMarket();
            });

            $("#closeMarket").click(function () {
                StockTicker.ticker.server.closeMarket();
            });

            $("#resetMarket").click(function () {
                StockTicker.ticker.server.reset();
            });
        });

});