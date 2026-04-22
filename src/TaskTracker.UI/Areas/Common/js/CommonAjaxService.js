var CommonAjaxService = (function () {

   
    function ajaxCall(options) {
        return $.ajax({
            url: options.url,
            method: options.method || 'POST',
            data: options.data || {},
            processData: options.processData !== false,
            contentType: options.contentType !== false ? options.contentType || 'application/x-www-form-urlencoded; charset=UTF-8' : false,
            timeout: options.timeout || 60000,
            beforeSend: function () {
                if (typeof options.beforeSend === 'function') {
                    options.beforeSend();
                }
            }
        })
            .done(function (res) {
                if (typeof options.done === 'function') options.done(res);
            })
            .fail(function (err) {
                if (typeof options.fail === 'function') options.fail(err);
                else ShowNotification(3, "Server request failed!");
            });
    }

    function finalSave(url, masterObj, done, fail) {
        ajaxCall({ url: url, data: masterObj, done: done, fail: fail });
    }

    function finalImageSave(url, masterObj, done, fail) {
        ajaxCall({
            url: url,
            data: masterObj,
            processData: false,
            contentType: false,
            done: done,
            fail: fail
        });
    }

    function deleteData(url, masterObj, done, fail) {
        ajaxCall({ url: url, data: masterObj, done: done, fail: fail });
    }

    function multiplePost(url, masterObj, done, fail) {
        ajaxCall({ url: url, data: masterObj, done: done, fail: fail });
    }

    function ImportExcel(url, masterObj, done, fail) {
        ajaxCall({
            url: url,
            data: masterObj,
            processData: false,
            contentType: false,
            beforeSend: function () {
                console.log("Uploading Excel file...");
            },
            done: done,
            fail: fail
        });
    }

    return {
        finalSave,
        finalImageSave,
        deleteData,
        multiplePost,
        ImportExcel
    };

})();
