var dashboardJs =
{
    init: function () {
        $('a.suspend-user').unbind('click').click(function () {
            var id = $(this).attr('rel').replace('data-suspend-user-', '');
            var currentStatus = $('td[rel="data-user-status-' + id + '-0"]').attr('rel');

            if (currentStatus)
            {
                alert("This user is suspended already.");
            }
            else
            {
                var suspenUrl = appBaseUrl + 'users/suspenduser';
                var onError = function () {
                    alert("Can not suspend user. Refresh and try again or contact your web master for advice.");
                    return false;
                };
                var onSuccess = function (data) {
                    $('td[rel="data-user-status-' + id + '-1"]').html(data.userStatusText)
                                                                    .attr('rel', 'data-user-status-' + id + '-' + data.userStatus);
                    alert("User has been suspended");
                    return false;
                }

                dashboardJs.handleAjaxCall(suspenUrl, 'POST', { id: id }, onSuccess, onError);     
            }           
        });
    },
    handleAjaxCall: function (url, method, data, successCallback, errorCallback) {
        $.ajax({
            url: url,
            type: method,
            data: data
        }).success(function (data) {
            if (data.status === 200) {
                if(successCallback) successCallback(data);
            } else {
                if (errorCallback) errorCallback();
            }
        });
    }
};

$(document).ready(function () {
    dashboardJs.init();
});