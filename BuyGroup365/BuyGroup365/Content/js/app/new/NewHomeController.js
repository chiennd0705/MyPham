angular.module('app', []).controller('NewHomeController', function ($scope) {
    $scope.News = {
        Id: '',
        Title: '',
        FriendlyUrl: '',
        ImageSource: '',
        Summary: '',
        Descriptions: '',
        NewsGroupId: '',
        ModifyDate: ''
    };

    $scope.GetNewHome = function (id) {
        $.ajax({
            url: '/Ajax/GetNewHome',
            data: {
                'id': id
            },
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //$scope.News.Id = data.Id;
                //$scope.News.Title = data.Title;
                //$scope.News.FriendlyUrl = data.FriendlyUrl;
                //$scope.News.Summary = data.Summary;
                //$scope.News.Descriptions = data.Descriptions;
                //$scope.News.NewsGroupId = data.NewsGroupId;
                //$scope.News.ModifyDate = data.ModifyDate;
                //$scope.$apply();
                $scope.ModelNews = data;
            },
            error: function () {
            }
        });
    };
})