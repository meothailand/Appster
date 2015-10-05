"use strict";

var backendHandler = function () {
    var baseUrl = window.tattooControlPanel.baseUrl.replace(/\/+$/, '') + '/controlpanel';
    var apiUrl = {
        deleteArtist: baseUrl + '/artistpanel/artist',
        deleteArtwork: baseUrl + '/artistpanel/deleteartwork',
        deleteAllArtwork: baseUrl + '/artistpanel/deleteallartworks',
        getArtworks: baseUrl + '/artistpanel/artworksajax',
        editArtwork: baseUrl + '/artistpanel/updateartwork',
        addArtworks: baseUrl + '/artistpanel/addartworks',
        updateProfile: baseUrl + '/profilepanel/companyprofile',
        addSocialConnection: baseUrl + '/profilepanel/addsocialconnection',
        updateSocialConnection: baseUrl + '/profilepanel/updatesocialconnection',
        deleteSocialConnection: baseUrl + '/profilepanel/deletesocialconnection',
        openFeedback: baseUrl + '',
        deleteFeedback: baseUrl + ''
    };

    var maxlengthHadler = function () {
        $('.maxlengthCheck').maxlength({
            threshold: 20,
            warningClass: "label label-success",
            limitReachedClass: "label label-danger",
            separator: ' out of ',
            preText: 'You typed ',
            postText: ' chars available.',
            placement: 'top',
            validate: true
        });
    }

    var artworksUpload = function () {
        $('#fileupload').fileupload({url: apiUrl.addArtworks, singleFileUploads: false});
    }
    
    var artistValidation = function () {
        var name = $('#FullName').val();

        if (name.replace(/\s+/gi, '') == '') {
            $('#FullName').css('borderColor', '#B94A48');
            $('#FullName').after('<span class="help-block" style="color:Red;">Required</span>');
            showError('Full name is required!');
            return false;
        }
    }

    var reloadArtworks = function (artistId, pageNo) {
        var el = $('#artwork-table-container');
        var tableEl = $('#artwork-table');
        var message = '';
        App.blockUI(el);
        $.post(apiUrl.getArtworks, { id: artistId, page: pageNo }, function (data) {
            tableEl.html(data);
        }).fail(function (jqXHR) {
            switch (jqXHR.status) {
                case 404:
                    message = 'This artist does not or no longer exist.';
                    break;
                default:
                    message = 'Unknown error. Can not load artwork. Try to refresh the page. Please contact your administrator for help.'
            }
            showError(message);
        }).always(function () {            
            App.unblockUI(el);                     
        });
    }

    var deleteArtist = function (id, name) {
        setShowProcessing('Deleting...');
        var title = '';
        var message = '';
        var footerButtons = '';
        $.post(apiUrl.deleteArtist, { id: id }, function (data) {
            title = 'Deleted';
            message = 'Artist named ' + name + ' has been deleted';
            footerButtons = '<button type="button" data-dismiss="modal" class="btn green" onclick="javascript:handler.reload();">Reload</button>';
        }).fail(function (jqXHR) {
            title = 'Unable to delete artist ' + name;
            footerButtons = '<button type="button" data-dismiss="modal" class="btn green">Close</button>';
            switch (jqXHR.status) {
                case 404:
                    message = 'This artist does not or no longer exist.';
                    break;
                case 403:
                    message = 'The artist is linked with one or more artworks. Delete these artworks before delete the artist.';
                    break;
                default:
                    message = 'Unknown error. Can not delete artist. Please contact your administrator for help.';
            }

        }).always(function () {
            setHideProcessing(title, message, footerButtons);
        });
      
    }
    
    var editArtwork = function () {
        var selector = '#editForm';
        setShowProcessingForm(selector, 'Saving...');
        var art = {
            Id: null,
            ArtImage: null,
            Thumbnail: null,
            ArtImageUrl: '',
            ThumbnailUrl: '',
            ArtDesc: '',
            ShowOnIntroScreen: false
        };

        art.Id = $('#edit-form-id').val();
        art.ArtDesc = $('#edit-form-desc').val();
        art.ShowOnIntroScreen = $('#edit-form-show input').prop('checked');
        art.ThumbnailUrl = $('#edit-form-thumb').attr('src');
        var title = '';
        var message = '';
        var footerButtons = '';

        $.post(apiUrl.editArtwork, { art: art }, function (data) {
            setArtUpdated(data.Id, data.ShowOnIntroScreen, data.ArtDesc);
            title = '<span style="color:Blue; font-weight:bold">Update successful</span>';
            footerButtons = '<button type="button" data-dismiss="modal" class="btn green">Close</button>';
            message = '<span style="color:Blue; font-weight:bold">Artwork has been updated.';                              
            message += '<br/><br/><img src="' + data.ThumbnailUrl + '" />';
            message += '<br/><br/><span>' + data.ArtDesc + '</span>';
        }).fail(function (jqXHR) {
            title = '<span style="color:Red; font-weight:bold">Unable to update artwork</span>';
            footerButtons = '<button type="button" data-dismiss="modal" class="btn green">Close</button>';
            message = '<span style="color:Red; font-weight:bold">Unknown error. Can not update artwork. ' +
                              'Please contact your administrator for help.</span>';
            message += '<br/><br/><img src="' + art.ThumbnailUrl + '" />';
        }).always(function () {
            setHideProcessingForm(selector, title, message, footerButtons);
        });
    }

    var deleteArtwork = function (id, thumbUrl) {
        setShowProcessing('Deleting...');
        var title = '';
        var message = '';
        var footerButtons = '';
        $.post(apiUrl.deleteArtwork, { artId: id}, function (data) {
            title = '<span style="color:Blue; font-weight:bold">Delete successfully</span>';
            message = 'The artwork has been deleted.';
            footerButtons = '<button type="button" data-dismiss="modal" class="btn green" onclick="javascript:hanlder.removeDeletedArt(' + id + ')">Close</button>';
        }).fail(function (jqXHR) {
            title = '<span style="color:Red; font-weight:bold">Unable to delete artwork</span>';
            footerButtons = '<button type="button" data-dismiss="modal" class="btn green">Close</button>';
            switch (jqXHR.status) {                
                case 404:
                    message = '<span style="color:Red; font-weight:bold">This artwork does not or no longer exist.</span>';
                    break;
                default:
                    message = '<span style="color:Red; font-weight:bold">Unknown error. Can not delete artwork. ' +
                              'Please contact your administrator for help.</span>';
            }

            message += '<br/><br/><img src="' + thumbUrl + '" />';

        }).always(function () {
            setHideProcessing(title, message, footerButtons);
        });
    }

    var deleteAllArtworks = function (id, name) {
        setShowProcessing('Deleting...');
        var title = '';
        var message = '';
        var footerButtons = '';
        $.post(apiUrl.deleteAllArtwork, { artistId: id }, function (data) {
            title = '<span style="color:Blue; font-weight:bold">Delete successfully</span>';
            message = data + ' artworks of ' + name + ' have been deleted.';
            footerButtons = '<button type="button" data-dismiss="modal" class="btn green" onclick="javascript:handler.reloadArtworks('+ id +',1)">';
            footerButtons += 'Reload</button>';
        }).fail(function () {
            title = '<span style="color:Red; font-weight:bold">Unable to delete artworks</span>';
            footerButtons = '<button type="button" data-dismiss="modal" class="btn green">Close</button>';
            message = '<span style="color:Red; font-weight:bold">Unknown error. Can not delete artworks. ' +
                             'Please contact your administrator for help.</span>';
        }).always(function () {
            setHideProcessing(title, message, footerButtons);
        });
    }

    var deleteConnection = function (id) {
        setShowProcessing('Deleting...');
        var title = '';
        var message = '';
        var footerButtons = '<button type="button" data-dismiss="modal" class="btn green">Close</button>';;
        var con = getConnInfo(id);
        $.post(apiUrl.deleteSocialConnection, { id: id }, function (data) {
            removeDeletedCon(id);
            title = '<span style="color:Blue; font-weight:bold">Delete successfully</span>';
            message = con.name + ' connection has been deleted.';
        }).fail(function () {
            title = '<span style="color:Red; font-weight:bold">Unable to delete ' + con.name + ' connection</span>';            
            message = '<span style="color:Red; font-weight:bold">Unknown error. Can not delete connection ' + con.name +
                             ' .Please contact your administrator for help.</span>';
        }).always(function () {
            setHideProcessing(title, message, footerButtons);
        });
    }

    var updateConnection = function (id) {
        var name = $('#con-name').val();
        var link = $('#con-link').val();
        if (name.replace(/\s+/gi, '') == '' || link.replace(/\s+/gi, '') == '') {
            $('#con-error').html('Please fill in all information');
        } else {
            setShowProcessing('Updating...');
            var title = '';
            var message = '';
            var footerButtons = '<button type="button" data-dismiss="modal" class="btn green">Close</button>';;
            $.post(apiUrl.updateSocialConnection,
                  {
                      Id: id,
                      NetworkName: name,
                      ProfileLink: link,
                      HoverImage: null,
                      MainImage: null
                  },
                  function (data) {
                      title = '<span style="color:Blue; font-weight:bold">Update successfully</span>';
                      message = data.NetworkName + ' connection has been update.';
                      $('#name-' + id).text(data.NetworkName);
                      $('#link-' + id).attr('href', data.ProfileLink);
                      $('#link-' + id).text(data.ProfileLink);
                  }).fail(function () {
                      title = '<span style="color:Red; font-weight:bold">Unable to update ' + name + ' connection</span>';
                      message = '<span style="color:Red; font-weight:bold">Unknown error. Can not update connection ' + name +
                                       ' .Please contact your administrator for help.</span>';
                  }).always(function () {
                      setHideProcessing(title, message, footerButtons);
                  });
        }
    }

    var enableEdit = function (id) {
        id = '#' + id;
        $(id).removeClass('hidden');
    }

    var openDeleteArtworkModal = function (id, thumbUrl) {
        $('#static h4.modal-title').text('Do you really want to delete this artwork?');
        $('#modal-message').html('<img src="'+ thumbUrl +'" />');
        var modalFooterButtons = '<button type="button" data-dismiss="modal" class="btn default">Cancel</button>' +
                                  '<button id="button-confirm" type="button" class="btn green" ' +
                                  'onclick="javascript:handler.deleteArtwork(' + id + ',\'' + thumbUrl +'\');">Continue</button>';
        $('#modal-buttons').html(modalFooterButtons);
        return false;
    }

    var openDeleteArtistModal = function (id, name) {
        $('#static h4.modal-title').text('Confirm delete');
        $('#modal-message').text('Do you really want to delete ' + name + ' artist?');
        var modalFooterButtons = '<button type="button" data-dismiss="modal" class="btn default">Cancel</button>' +
                                  '<button type="button" class="btn green" ' +
                                  'onclick="javascript:handler.deleteArtist(' + id + ',\'' + name + '\');">Continue</button>';
        $('#modal-buttons').html(modalFooterButtons);
        return false;
    }

    var openDeleteAllArtworkModal = function (id, name) {
        $('#static h4.modal-title').text('Confirm delete');
        var message = '<span style="color:Blue;font-size:bold;">This action will delete all artworks off ' + name + ' artist?</span>';
        message += '<br/><span style="color:Red;font-size:bold;">Do you really want to proceed it?</span>';
        $('#modal-message').html(message);
        var modalFooterButtons = '<button type="button" data-dismiss="modal" class="btn default">Cancel</button>' +
                                  '<button type="button" class="btn green" ' +
                                  'onclick="javascript:handler.deleteAllArtworks(' + id + ',\'' + name + '\');">Continue</button>';
        $('#modal-buttons').html(modalFooterButtons);
        return false;
    }

    var openEditArtworkModal = function (id) {
        $('#proccessing-form').addClass('hide');
        if ($('#editForm form').hasClass('hide')) {
            $('#editForm form').removeClass('hide');
        }
        var artwork = getArtInformation(id);
        $('#edit-form-id').val(id);
        $('#edit-form-desc').val(artwork.description);
        if (artwork.showOnScreen) {
            $('#edit-form-show').html('<input type="checkbox" class="form-control" checked="checked" />');
        } else {
            $('#edit-form-show').html('<input type="checkbox" class="form-control" />');
        }
        
        $('#edit-form-thumb').attr('src', artwork.thumbnail);
    }

    var openEditConnnectionModal = function (id) {
        var con = getConnInfo(id);
        $('#static h4.modal-title').text('Update social connection');
        var message = '<div class="row" style="margin-bottom:5px;">';
        message += '<label class="col-md-3">Network Name</label>';
        message += '<div class="col-md-9"><input type="text" id="con-name" class="form-control" value="' + con.name + '" /></div></div>';                                                                   
        message += '<div class="row" style="margin-bottom:5px;"><label class="col-md-3">Connection Link</label>';
        message += '<div class="col-md-9"><input type="text" id="con-link" class="form-control" value="' + con.link + '" /></div></div>';
        message += '<div class="row" style="margin-bottom:5px;"><label class="col-md-12" style="color:Red;" id="con-error"></label></div>'
        $('#modal-message').html(message);
        var modalFooterButtons = '<button type="button" data-dismiss="modal" class="btn default">Cancel</button>' +
                                  '<button type="button" class="btn green" ' +
                                  'onclick="javascript:handler.updateConnection(' + id + ');">Continue</button>';
        $('#modal-buttons').html(modalFooterButtons);
        return false;
    }

    var openDeleteConnnectionModal = function (id) {
        $('#static h4.modal-title').text('Confirm delete');
        var con = getConnInfo(id);
        var message = '<span style="color:Blue;font-size:bold;">Do you really want to delete ' + con.name + ' connection?</span>';
        $('#modal-message').html(message);
        var modalFooterButtons = '<button type="button" data-dismiss="modal" class="btn default">Cancel</button>' +
                                  '<button type="button" class="btn green" ' +
                                  'onclick="javascript:handler.deleteConnection(' + id + ');">Continue</button>';
        $('#modal-buttons').html(modalFooterButtons);
        return false;
    }

    //private function
    function showError(errorMessage){
        $('div.messageBox').removeClass('hideMessage');
        $('span.messageBox').text(errorMessage);
        return false;
    }

    function reload() {
        window.location.reload();
    }

    function setShowProcessing(title) {
        $('#static h4.modal-title').html(title);
        $('#modal-message').html('<img src="/Content/images/loading.gif" />');
        $('#modal-buttons').addClass('hide');
        return false;
    }

    function setHideProcessing(title, resultMessage, footerButtons) {
        $('#static h4.modal-title').html(title);
        $('#modal-message').html(resultMessage);
        $('#modal-buttons').html(footerButtons);
        $('#modal-buttons').removeClass('hide');
        return false;
    }

    function setShowProcessingForm(selector, title) {
        $(selector + ' form').addClass('hide');        ;
        $(selector + ' h4.modal-title').html(title);
        $('#proccessing-form .modal-body').html('<img src="/Content/images/loading.gif" />');
        $('#proccessing-form .modal-footer').addClass('hide');
        $('#proccessing-form').removeClass('hide');
        return false;
    }

    function setHideProcessingForm(selector, title, resultMessage, footerButtons) {
        $(selector + ' h4.modal-title').html(title);
        $('#proccessing-form .modal-body').html(resultMessage);
        $('#proccessing-form .modal-footer').html(footerButtons);
        $('#proccessing-form .modal-footer').removeClass('hide');
        return false;
    }

    function getArtInformation(id) {
        var thumb = $('#thumb-' + id).attr('src');
        var isShow = $('#show-' + id).hasClass('label-success');
        var desc = $('#desc-' + id).text();
        return {
            id: id,
            thumbnail: thumb,
            description: desc,
            showOnScreen: isShow
          };
    }

    function setArtUpdated(id, isShow, desc) {
        var showEl = $('#show-' + id);
        if (isShow) {
            (showEl).text('Yes');
            if (!showEl.hasClass('label-success')) {
                showEl.addClass('label-success')
            }
            if (showEl.hasClass('label-default')) {
                showEl.removeClass('label-default')
            }
        } else {
            showEl.text('No');
            if (showEl.hasClass('label-success')) {
                showEl.removeClass('label-success')
            }
            if (!showEl.hasClass('label-default')) {
                showEl.addClass('label-default')
            }
        }
        $('#desc-' + id).text(desc);
    }

    function removeDeletedArt(id) {
        var trEl = $('#art-' + id);
        if (trEl) {
            trEl.remove();
        }
        return false;
    }

    function getConnInfo(id) {
        var name = !!$('#name-' + id) ? $('#name-' + id).text() : '';
        var link = !!$('#link-' + id) ? $('#link-' + id).attr('href') : '';
        var image = !!$('img#image-' + id) ? $('img#image-' + id).attr('src') : '';
        var hover = !!$('img#hover-' + id) ? $('img#hover-' + id).attr('src') : '';

        return {
            id: id,
            name: name,
            link: link,
            mainImage: image,
            hoverImage: hover
        };
    }

    function removeDeletedCon(id) {
        if ($('tr#con-' + id)) {
            $('tr#con-' + id).remove();
        }
        return false;
    }
    

    return {
        init: function () {
            maxlengthHadler();
            artworksUpload();
            return {
                artistValidation: artistValidation,
                reloadArtworks: reloadArtworks,
                editArtwork:editArtwork,
                deleteArtist: deleteArtist,
                deleteArtwork: deleteArtwork,
                deleteAllArtworks: deleteAllArtworks,
                deleteConnection: deleteConnection,
                updateConnection:updateConnection,
                openDeleteArtistModal: openDeleteArtistModal,
                openDeleteArtworkModal: openDeleteArtworkModal,
                openDeleteAllArtworkModal: openDeleteAllArtworkModal,
                openEditArtworkModal: openEditArtworkModal,
                openEditConnnectionModal: openEditConnnectionModal,
                openDeleteConnnectionModal:openDeleteConnnectionModal,
                removeDeletedArt: removeDeletedArt,
                enableEdit:enableEdit,
                reload: reload
            };
        }
    };
}();
