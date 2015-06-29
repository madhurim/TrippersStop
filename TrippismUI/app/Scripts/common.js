var common = {};

common.addDays = function (dt, noOfDays) {
    var newDate = new Date(dt);
    newDate.setDate(newDate.getDate() + noOfDays);
    return newDate;
}



