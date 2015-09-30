var dashboardJs =
{
    init: function () {
        $('a.suspend-user').unbind('click').click(function () {
            var id = $(this).attr('rel').replace('data-user-suspend-', '');
            var currentSuspended = $('td[rel="data-user-status-' + id + '-0"]').attr('rel');
            var userName = $('td#data-user-name-' + id).attr("title");
            var status = currentSuspended ? 0 : 1;
            var message = status === 0 ? "Do you want to activate user: " + userName + "?" : "Do you want to suspend user: " + userName + " ?";
            if (confirm(message))
            {
                var suspenUrl = appBaseUrl + 'users/suspenduser';
                var isSuspend = status === 1 ? true : false;
                var onError = function () {
                    alert("Can not perform action. Refresh and try again or contact your web master for advice.");
                    return false;
                };
                var onSuccess = function (data) {
                    $('td[rel="data-user-status-' + id + '-' + status + '"]').html(data.userStatusText)
                                                                .attr('rel', 'data-user-status-' + id + '-' + data.userStatus);
                    if (data.userStatus === 0) {
                        alert(userName + " has been suspended");
                    } else {
                        alert(userName + " has been activated");
                    }                    
                    return false;
                }

                dashboardJs.handleAjaxCall(suspenUrl, 'POST', { id: id, suspend : isSuspend }, onSuccess, onError);
            }
            else
            {
                return false;
            }           
        });

        $('a.suspend-gift').unbind('click').click(function () {
            var id = $(this).attr('rel').replace('data-gift-suspend-', '');
            var giftName = $('td#data-gift-name-' + id).attr('title');
            var currentSuspended = $('td[rel="data-gift-status-' + id + '-0"]').attr('rel');
            var status = currentSuspended ? 0 : 1;
            var message = status === 0 ? "Do you want to activate gift: " + giftName + "?" : "Do you want to suspend gift: " + giftName + "?";
            if (confirm(message)) {
                var suspenUrl = appBaseUrl + 'gift/suspendgift';
                var isSuspend = status === 1 ? true : false;
                var onError = function () {
                    alert("Can not perform action. Refresh and try again or contact your web master for advice.");
                    return false;
                };
                var onSuccess = function (data) {
                    $('td[rel="data-gift-status-' + id + '-' + status + '"]').html(data.giftStatusText)
                                                                    .attr('rel', 'data-gift-status-' + id + '-' + data.giftStatus);
                    if (data.giftStatus === 0) {
                        alert(giftName + " has been suspended");
                    } else {
                        alert(giftName + " has been activated");
                    }
                    return false;
                }

                dashboardJs.handleAjaxCall(suspenUrl, 'POST', { id: id , suspend : isSuspend }, onSuccess, onError);
            }
            else {
                return false;
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