var newUsers = {
    labels: ['1d ago', '2d ago', '3d ago', '4d ago', '5d ago'],
    series: [[15, 10, 6, 18, 11]]
};

var services = {
    labels: ['Facebook', 'Twitter', 'Google Plus', 'Email'],
    series: [212, 104, 32, 71]
};

var newArticles = {
    labels: ['1d ago', '2d ago', '3d ago', '4d ago', '5d ago'],
    series: [[110, 98, 121, 72, 89]]
};


var sources = {
    labels: ['Hello Fashion', 'Popjustice', 'Nerd Fitness', '/Film', 'Other'],
    series: [253, 231, 180, 156, 400]
};

var frequence = {
    labels: ['00:00', '04:00', '08:00', '12:00', '16:00', '20:00'],
    series: [[9, 1, 7, 19, 26, 22]]
};

var categories = {
    labels: ['Movie', 'Music', 'Fashion', 'Fitness', 'Economy', 'Other'],
    series: [[10, 15, 13, 8, 7, 28]]
};

$(function(){

    new Chartist.Line('#newUsers', newUsers);

    new Chartist.Pie('#registered', services, {
        labelInterpolationFnc: function(value) {
            return value
        }
    });

    new Chartist.Bar('#newArticles', newArticles, {axisY: {
        position: 'end'
    }});

    new Chartist.Pie('#sources', sources, {
        labelInterpolationFnc: function(value) {
            return value
        }
    });

    new Chartist.Line('#frequence', frequence);

    new Chartist.Bar('#categories', categories, {axisY: {
        position: 'end'
    }});

});
