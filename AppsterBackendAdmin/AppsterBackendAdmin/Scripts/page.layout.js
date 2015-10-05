var pageHandler = function () {
    "use strict";
    var pageModalId = '#pageModal';

    var setModalContent = function (strTitle, isShowLoading) {
        if (strTitle) { $(pageModalLabel).html(strTitle); }
        else { $(pageModalLabel).html('Processing...'); }

        if (isShowLoading) { $(pageModalLabel).children('img').show(); }
        else { $(pageModalLabel).children('img').hide(); }
    };

    var showAjaxModal = function (strTitle) {
        setModalContent(strTitle, true);
        $(pageModalId).modal('show');
        return false;
    };

    var hideAjaxModal = function () {
        setModalContent('...', false);
        $(pageModalId).modal('hide');
        return false;
    };

    var updateModalStatus = function (str, showLoading) {
        $(pageModalId).children("div > div.modal-content").fadeOut(300, function () {
            setModalContent(str, showLoading);
            $(pageModalId).children("div > div.modal-content").fadeIn();
        });
        return false;
    };

    return {
        init: function () {
            return {
                showAjaxModal: showAjaxModal,
                hideAjaxModal: hideAjaxModal,
                updateModalStatus: updateModalStatus
            }
        }
    };
}();