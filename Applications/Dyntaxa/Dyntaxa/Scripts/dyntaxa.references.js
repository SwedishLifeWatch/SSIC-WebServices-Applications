/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />
/// <reference path="Dynatree/src/jquery.dynatree.js" />



$.fn.dataTableExt.oApi.fnReloadAjax = function (oSettings, sNewSource, fnCallback, bStandingRedraw) {
    if (typeof sNewSource != 'undefined' && sNewSource != null) {
        oSettings.sAjaxSource = sNewSource;
    }
    this.oApi._fnProcessingDisplay(oSettings, true);
    var that = this;
    var iStart = oSettings._iDisplayStart;

    oSettings.fnServerData(oSettings.sAjaxSource, [], function (json) {
        /* Clear the old information from the table */
        that.oApi._fnClearTable(oSettings); /* Got the data - add it to the table */
        for (var i = 0; i < json.aaData.length; i++) {
            that.oApi._fnAddData(oSettings, json.aaData[i]);
        } oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
        that.fnDraw(that);

        if (typeof bStandingRedraw != 'undefined' && bStandingRedraw === true) {


            oSettings._iDisplayStart = iStart;


            that.fnDraw(false);


        }

        that.oApi._fnProcessingDisplay(oSettings, false);

        /* Callback user function - for event handlers etc */
        if (typeof fnCallback == 'function' && fnCallback != null) {
            fnCallback(oSettings);
        }
    }, oSettings);
};


jQuery.fn.dataTableExt.oApi.fnFilterOnButton = function (oSettings, searchButtonId, searchDivId) {
    /*
    * Usage:       $('#example').dataTable().fnFilterOnButton();
    * Author:      Jcodecowboy
    * License:     GPL v2 or BSD 3 point style
 
    */
    var _that = this;

    this.each(function (i) {
        $.fn.dataTableExt.iApiIndex = i;
        var $this = this;
        var anControl = $('input', _that.fnSettings().aanFeatures.f);
        anControl.unbind('keyup');
        var searchButton = $('#referenceSearchButton').bind('click', function (e) {
            _that.fnFilter(anControl.val());
        });

        $('#searchDiv input').bind('keypress', function (e) {
            if (e.keyCode == 13) {
                _that.fnFilter(anControl.val());
            }
        });


        return this;
    });
    return this;
};


function wireUpReferenceForm(dialog) {
    $('form', dialog).submit(function () {

        // Do not submit if the form
        // does not pass client side validation
        if (!$(this).valid())
            return false;

        // Client side validation passed, submit the form
        // using the jQuery.ajax form
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                // Check whether the post was successful
                if (result.success) {
                    $(dialog).dialog('close');
                    reloadTable();
                } else {
                    // Reload the dialog to show model errors                    
                    $(dialog).html(result);

                    // Enable client side validation
                    $.validator.unobtrusive.parse(dialog);

                    // Setup the ajax submit logic
                    wireUpReferenceForm(dialog);
                }
            }
        });
        return false;
    });
}    


