function ConvertToRequiredDate(dt) {
    dt = new Date(dt);
    var curr_date = ('0' + dt.getDate()).slice(-2);
    var curr_month = ('0' + (dt.getMonth() + 1)).slice(-2);
    var curr_year = dt.getFullYear();
    var _date = curr_year + "-" + curr_month + "-" + curr_date;
    return _date;
}


function daydiff(first, second) {
    return Math.round((second - first) / (1000 * 60 * 60 * 24));
}

function Dateformat()
{
    var format = ['yyyy-MM-dd', 'dd-MM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
    return format;
}

function AvailableTheme() {
    var theme = [
                                    { id: "BEACH", value: "BEACH" },
                                    { id: "CARIBBEAN", value: "CARIBBEAN" },
                                    { id: "DISNEY", value: "DISNEY" },
                                    { id: "GAMBLING", value: "GAMBLING" },
                                    { id: "HISTORIC", value: "HISTORIC" },
                                    { id: "MOUNTAINS", value: "MOUNTAINS" },
                                    { id: "NATIONAL-PARKS", value: "NATIONAL-PARKS" },
                                    { id: "OUTDOORS", value: "OUTDOORS" },
                                    { id: "ROMANTIC", value: "ROMANTIC" },
                                    { id: "SHOPPING", value: "SHOPPING" },
                                    { id: "SKIING", value: "SKIING" },
                                    { id: "THEME-PARK", value: "THEME-PARK" }
    ];
    return theme;
}

function AvailableRegions() {
    var region = [
                                    { id: 'Africa', value: 'Africa' },
                                    { id: 'Asia Pacific', value: 'Asia Pacific' },
                                    { id: 'Europe', value: 'Europe' },
                                    { id: 'Latin America', value: 'Latin America' },
                                    { id: 'Middle East', value: 'Middle East' },
                                    { id: 'North America', value: 'North America' },
    ];

    return region;
}