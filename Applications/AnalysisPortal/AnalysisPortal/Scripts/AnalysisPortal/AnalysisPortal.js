/// <reference path="../jquery-2.1.0-vsdoc.js" />
/// <reference path="AnalysisPortal.Resources-vsdoc.js" />
/// <reference path="../extjs-4.2.1/ext-all.js" />
/// <reference path="../OpenLayers/OpenLayers.js" />

// Info about JavaScript namespace: http://javascriptweblog.wordpress.com/2010/12/07/namespacing-in-javascript/

$(document).ready(function () {
    AnalysisPortal.initialize();
});

var AnalysisPortal = {};http://localhost:48423/../
(function (context) {
    /*
     * Public properties are written as:   context.VariableName =
     * Private variables are written as:   var variableName =  
     * Public functions are written as:    context.functionName = function(args) {
     * Private functions are written as:   function functionName(args) {
     */    
    
        
    context.ApplicationPath = "/";    
    context.SessionTimeoutInSeconds = 0;    
    context.SessionTimeoutDate = null;    
    context.SessionTimeOutIntervalTimerId = 0;
    context.CurrentUser = null;
    context.Language = "en";
    context.CurrentSelectedObservationId = null;
    context.ObservationDetailsSettings = null;
    //context.ObservationDetailsShowEmptyFields = true;
    //context.ObservationDetailsImportance = 1;
    //context.ObservationDetailsShowDwcTitle = false;
    //context.IsMobile = false;
    //context.IsTablet = false;
    //context.IsDesktop = false;
    context.IsTouchDevice = false;
    context.DefaultContentWidth = 787;



    context.initialize = function() {
        /// <summary>
        /// Initializes common JavaScript things.
        /// </summary>
                
        fixConsole();
        context.IsTouchDevice = 'ontouchstart' in document.documentElement;
        //detectDevice();
        initAjaxSettings();        
        initExtJS();
        initOpenLayers();
        preventDisabledLinksToBeClickable();
        Ext.resetElement = Ext.getBody();
        initTooltip();
        initPopover();
        console.log('AnalysisPortal JavaScript initialized');

        $('#MySettingsSummary').on('click', 'input.toggleSetting', toggleSettings);
    };

    function initExtJS() {
        /// <summary>
        /// Init ExtJS
        /// </summary>
        
        Ext.QuickTips.init();
        Ext.Ajax.timeout = 20 * 60 * 1000; // 20 minutes
        Ext.override(Ext.data.proxy.Ajax, { timeout: 20 * 60 * 1000 });
        Ext.apply(Ext.util.Format, {
            thousandSeparator: ' ',
            decimalSeparator: ','
        });        
        Ext.Ajax.on('requestcomplete', function (conn, response, options, eOpts) {
            context.restartSessionTimeoutTimer();
        });
    }
    
    function initAjaxSettings() {
        /// <summary>
        /// Init Ajax Settings        
        /// </summary>
                
        // globally turn off all ajax cache.
        $.ajaxSetup({
            cache: false,
            timeout: 20 * 60 * 1000 // 20 minutes
        });
        $(document).ajaxComplete(function () {
            context.restartSessionTimeoutTimer();
        });
    }    
    
    function initOpenLayers() {
        if(! (typeof OpenLayers === 'undefined')){ 
            OpenLayers.ProxyHost = context.ApplicationPath + '/Proxy.ashx?url=';
            OpenLayers.ImgPath = '/Content/images/OpenLayers/';
        };
    }

    function preventDisabledLinksToBeClickable() {
        /// <summary>
        /// Prevent links that is disabled to be clickable
        /// </summary>        
        
        $('a.disabled, a[disabled]').click(function (event) {
            event.preventDefault();
            return false;
        });        
    }

    context.initSessionTimeoutTimer = function (sessionTimeoutInSeconds) {
        context.SessionTimeoutInSeconds = sessionTimeoutInSeconds;
        context.SessionTimeoutDate = addSecondsToDate(new Date(), sessionTimeoutInSeconds);
        //context.SessionTimeoutDate = getSessionTimeoutDate(sessionTimeoutInSeconds);
        context.SessionTimeOutIntervalTimerId = setInterval(checkSessionTimeout, 1000);               
    };

    function getSessionTimeoutDate(nrSeconds) {
        var date = addSecondsToDate(new Date(), nrSeconds);
        //var date = new Date();
        //date.setSeconds(date.getSeconds() + nrSeconds);
        return date;
    }

    function addMinutesToDate(date, minutes) {
        return new Date(date.getTime() + minutes * 60000);
    }

    function addSecondsToDate(date, seconds) {
        return new Date(date.getTime() + seconds * 1000);
    }

    function checkSessionTimeout() {
        var now = new Date();
        if (now > context.SessionTimeoutDate) {
            clearInterval(context.SessionTimeOutIntervalTimerId);
            context.showSessionTimeoutDialog();            
        }
    }

    context.restartSessionTimeoutTimer = function () {
        context.SessionTimeoutDate = getSessionTimeoutDate(context.SessionTimeoutInSeconds);
    };

    context.showSessionTimeoutDialog = function () {        
        showSaveSettingsMessageOnLeave = false; // on save pages this needs to be set.                
        $('#sessionTimeoutModal').modal({ 
            backdrop: true,
            keyboard: false,
            show: true
        });        
    };

    function initTooltip() {
        if (!$.browser.mobile) {
            //Init bootstrap tooltip
            $('[title][title!=""]').not('.noTooltip').tooltip({ container: 'body' });

            //make sure tooltip is hidden when hovering drop down menu or leaving button
            $('.dropdown-menu').on('mouseover', function () { $(this).siblings('.btn').tooltip('hide'); });

            $('.btn-group').on('mouseout', function () { $(this).children('.btn').tooltip('hide'); });
        }
    };

    function initPopover () {
        //Init bootstrap popover
       $('[data-toggle="popover"]').popover({
            container: 'body',
            html: true,
            content: function () {
                var content = null;
                var type = $(this).data('type');
                var identifier = $(this).data('identifier');

                if (identifier != null) {
                    var url = null;
                    var data = {};

                    switch (type) {
                        case 'summaryitem':

                            context.saveCurrentPageSettings();
                            url = '/MySettings/GetSummaryItemSettings';
                            data = { identifier: identifier };

                            break;
                    }

                    $.ajax({
                        type: 'GET',
                        url: context.ApplicationPath + url,
                        data: data,
                        success: function (html) {
                            content = html;
                        },
                        async: false
                    });
                }

                return content;
            }
        });

       //Fix to hide popup trigged by click when clicking outside the popup
       $(document).on('mouseup', function (e) {
            $('[data-toggle="popover"][aria-describedby]').each(function() {
                if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0
                    && $(e.target).parents('.popover.in').length === 0) {
                    $(this).popover('hide');

                    //When the popup is manually hidden we need to click twice to open popup again, this uggly fix prevent that
                    $(this).click();
                }
            });
       });
    };

    function toggleSettings(e) {
        /// <summary>
        /// Toggles the check state using an Ajax call. The check state is stored in session.
        /// This function is called when the user clicks a checkbox in one of our state buttons.
        /// </summary>
        
        context.saveCurrentPageSettings();

        var $checkBox = $(e.target);
        context.makeAjaxCall({
            url: AnalysisPortal.ApplicationPath + '/MySettings/ActivateDeactivateSetting',
            params: {
                identifier: $checkBox.data('identifier'),
                checkValue: $checkBox.prop('checked')
            },
            showWaitMessage: false
        },
            function (result) {
                context.updateMySettingsSummary();
            }
        );
    };

    context.isStringNullOrEmpty = function(str) {
        return (!str || 0 === str.length);
    };    

    context.saveCurrentMySettings = function() {
        /// <summary>
        /// Save current settings to disk. The current settings
        /// is stored in session so no parameters has to be sent.
        /// </summary>        
        AnalysisPortal.makeAjaxCall({
                url: AnalysisPortal.ApplicationPath + "/MySettings/SaveMySettings",
                method: "POST",
                waitMessage: AnalysisPortal.Resources.SharedSaving                
            },
            function(result) {
                AnalysisPortal.showMsg(AnalysisPortal.Resources.MySettingsSaveSuccess);
            }
        );        
    };

    context.checkBoxClick = function (button, identifier) {
        /// <summary>
        /// Toggles the check state using an Ajax call. The check state is stored in session.
        /// This function is called when the user clicks a checkbox in one of our state buttons.
        /// </summary>
        /// <param name="button">The button clicked (DOM element)</param>
        /// <param name="identifier">An identifier (integer) that identifies which button was pressed.</param>        

        var checkValue = false;
        if ($(button).children('i').hasClass("icon-check-empty")) {
            checkValue = true;
        }
        context.saveCurrentPageSettings();
        AnalysisPortal.makeAjaxCall({
            url: AnalysisPortal.ApplicationPath + '/MySettings/ChangeButtonCheckStateAjax',
            params: {
                identifier: identifier,
                checkValue: checkValue
            },
            showWaitMessage: false
        },
            function (result) {
                if (result.data.IsChecked) {
                    $(button).children('i').removeClass("icon-check-empty").addClass("icon-check"); // check
                    $(button).attr('title', AnalysisPortal.Resources.SharedButtonUnCheckToolTip);
                } else {
                    $(button).children('i').removeClass("icon-check").addClass("icon-check-empty"); // uncheck                
                    $(button).attr('title', AnalysisPortal.Resources.SharedButtonCheckToolTip);
                }
                context.updateMySettingsSummary();
            }
        );
    };

    context.showToasterMsg = function (msg, title, options) {        
        /// <summary>
        /// Shows a message at the bottom of the screen that fades out after 3 seconds        
        /// </summary>
        /// <param name="options">
        ///     messageType {String} "info", "warning", "success" or "error".
        ///     positionClass {String} decides where the toastr message will appear.
        ///     timeout {Int} the number of milliseconds the message will be displayed.
        /// </param>     
        var settings = $.extend(true, {
            messageType: "success",
            positionClass: "toast-bottom-left",
            timeout: 6000            
        }, options);   

        toastr.options = {
            debug: false,
            positionClass: settings.positionClass,
            //positionClass: "toast-bottom-full-width",
            fadeIn: 300,
            extendedTimeOut: 1000,
            fadeOut: 1000,
            timeOut: settings.timeout            
        };
        
        toastr[settings.messageType](msg, title);        
    };

    context.saveCurrentPageSettings = function () {
        /// <summary>
        /// Calls the function updateMySettingsOnServer() if it has been
        /// defined on the current page.
        ///</summary>
        //if (typeof (updateMySettingsOnServer) != 'function') {
        //    return;
        //}
        //updateMySettingsOnServer(); 
    };
   
    context.updateStateButtonGroup = function(selector, buttonGroup) {
        /// <summary>
        /// Loads a specific state button group
        /// </summary>
        /// <param name="selector">jQuery selector</param>
        /// <param name="buttonGroup">Enum name of the button group</param>
        
        $(selector).load(
            context.ApplicationPath + '/Navigation/StateButtonGroup', {
                buttonGroup: buttonGroup
            });        
    };    
    
    context.updateMySettingsSummary = function() {
        /// <summary>
        /// Loads the MySettingsSummary panel
        /// </summary>

        $('#MySettingsSummary').load(context.ApplicationPath + '/MySettings/MySettingsSummary', function() {
            initPopover();
        });
    };


    context.showWaitMsg = function(msg, divId) {
        /// <summary>
        /// Shows a wait message.
        /// </summary>
        /// <param name="msg">The message to display.</param>
        /// <param name="divId">
        /// The name of the div that the message box will centered over. 
        /// If divId is null or 'body', the message will be centered over the page.
        /// </param>

        if (divId != null && divId != 'body' && divId != 'mainContent') {
            Ext.get(divId).mask(msg);
            return;
        }
        
        try {
            var windowWidth = $(window).width();
            var containerWidth = $("#mainContent").width();
            if (containerWidth != null) {
                if (windowWidth < containerWidth) {
                    Ext.getBody().mask(msg);
                }
                else {
                    Ext.get('mainContent').mask(msg);
                }
            }
            else {
                Ext.getBody().mask(msg);
            }
        }
        catch(err) {
            console.log("showWaitMsg() ERROR!");   
        }
        
    };
     
    
    context.hideWaitMsg = function(divId) {
        /// <summary>
        /// Hides a wait message
        /// </summary>        
        /// <param name="divId">
        /// The name of the div that the message box is centered over. 
        /// If divId is null or 'body', the message is centered over the page.
        /// </param>
        
        if (divId != null && divId != 'body' && divId != 'mainContent') {
            Ext.get(divId).unmask();
        } else {
            Ext.getBody().unmask();
            var containerDiv = Ext.get('mainContent');
            if (containerDiv != null) {
                containerDiv.unmask();
            }
        }
    };    

    context.showTimeoutMsg = function() {
        /// <summary>
        /// Shows a timeout message box
        /// </summary>
        
        var msgBox = Ext.MessageBox.alert(AnalysisPortal.Resources.SharedError, AnalysisPortal.Resources.SharedTimeoutError);
        //centerMessageBox(msgBox);
    };
    
    context.showErrorMsg = function(msg) {
        /// <summary>
        /// Shows an error message.
        /// </summary>
        /// <param name="msg">The error message to display.</param>        
        
        if (!msg) {
            msg = AnalysisPortal.Resources.SharedErrorOccurred;
        }
        var msgBox = Ext.MessageBox.alert(AnalysisPortal.Resources.SharedError, msg);
        //centerMessageBox(msgBox);
    };    
    
    context.showMsg = function(msg) {
        /// <summary>
        /// Shows a message.
        /// </summary>
        /// <param name="msg">The message to display.</param>        
        
        var msgBox = Ext.MessageBox.alert(AnalysisPortal.Resources.SharedAnalysisPortalMainTitle, msg);
        //centerMessageBox(msgBox);
    };    
    // Private functions

    context.showMessageBox = function(options, callback) {

        var settings = $.extend(true, {
            title: AnalysisPortal.Resources.SharedAnalysisPortalMainTitle,
            msg: "",
            buttons: Ext.MessageBox.YES,
            icon: Ext.MessageBox.INFO            
        }, options);

        var msgBoxSettings = {
            title: settings.title,
            msg: settings.msg,
            buttons: settings.buttons,
            icon: settings.icon
        };
        if (callback)
            msgBoxSettings.fn = callback;

        var msgBox = Ext.Msg.show(msgBoxSettings);

        //centerMessageBox(msgBox);

    };
    
    function centerMessageBox(msgBox) {
        /// <summary>
        /// Centers a message box over the container div.
        /// </summary>
        /// <param name="msgBox">The message box to center.</param>
        
        //try {            
        //    var windowWidth = $(window).width();
        //    var containerWidth = $("#mainContent").width();            
        //    if (containerWidth != null && windowWidth > containerWidth) {
        //        var currentPos = msgBox.getPosition();
        //        var y = currentPos[1];
        //        var width = msgBox.width;
        //        var x = (containerWidth - width) / 2.0;            
        //        msgBox.setPosition(x, y, false);
        //    }   
        //}
        //catch(err) {
        //    console.log("centerMessageBox() ERROR!");   
        //}        
        

    }

    context.fullScreen = function(mainPanel, containerPanel, showCallback, closeCallback) {
        $('html').addClass('x-viewport');
        mainPanel.remove(containerPanel, false);
        Ext.create('Ext.window.Window', {
            maximized: true,
            height: 800,
            width: 1200,
            resizable: false,
            modal: true,
            draggable: false,
            layout: 'fit',
            items: [containerPanel],
            listeners: {
                close: function (p, eOpts) {
                    $('html').removeClass('x-viewport');
                    mainPanel.add(containerPanel);
                    if (closeCallback && typeof (closeCallback) === "function") {
                        closeCallback();
                    }
                },
                show: function(p, eOpts) {
                    if (showCallback && typeof(showCallback) === "function") {
                        showCallback();
                    }
                }
            }
        }).show();       
    };
    
    context.makeAjaxCall = function(options, callback) {
    	/// <summary>
    	/// Makes a Ajax request and returns the request object
    	/// </summary>
    	/// <param name="options">
    	///     url {String} A string containing the URL to which the request is sent.
    	///     method {String} HTTP method: 'GET' or 'POST'. Default is 'GET'.
    	///     params {Object} Data to be sent to the server.
    	///     showWaitMessage {Bool} Indicates if a wait message should be shown while the ajax request is made. Defaults to true.
        ///     waitMessage {String} The wait message to show if showWaitMessage is set to true.
        ///     waitMessageDivId {String} The div to center the wait message over.
        ///     manualHideWaitMessage {Bool} If false, this function doesn't hide the wait message on successful call and a callback is passed as parameter.
        ///     allowMultipleRequests {Bool} If you only want one request to be active simultaneously set this to false. Defaults to true.
        ///     requestObject: {Object} If you set allowMultipleRequests to false you must use the requestObject.
        ///     async {Bool} If you need synchronous requests, set this option to false. Defaults to true.
        ///     errorCallback {Function} A function that will be called if the Ajax call fails.
        ///     timeout: {Number} Timeout in milliseconds
        ///     headers: {Object} Specify HTTP headers. Default is: { 'Accept': 'application/json' }
        ///     test {String} A string that triggers an error or a wait in testing purpose. Possible values are: "timeout", "failure", "wait"        
        /// </param>
    	/// <param name="callback">A function that will be called if the Ajax call is successful</param>

        var settings = $.extend(true, {
            url: '',                        
            showWaitMessage: false,
            waitMessage: AnalysisPortal.Resources.SharedWait,
            waitMessageDivId: null,
            allowMultipleRequests: true,
            requestObject: null,
            async: true,
            jsonModel: true,
            manualHideWaitMessage: false,
            headers: { 'Accept': 'application/json' },
            errorCallback: null,
            test: null,
            timeout: 20 * 60 * 1000 // 20 minutes
        }, options);

        var requestSettings = {
            url: settings.url,
            async: settings.async,
            headers: settings.headers,
            callback: function(opts, success, response) {                
                if (settings.requestObject) {
                    settings.requestObject = null;
                }
                //if (settings.showWaitMessage) {
                //    AnalysisPortal.hideWaitMsg(settings.waitMessageDivId);
                //}
                if (success) { // The server responded  
                    if (settings.jsonModel == false)
                        return response.responseText;
                    var result = Ext.JSON.decode(response.responseText); // deserialize the JSON response from server
                    if (settings.test == "failure")
                        result.success = false;                    
                    if (result.success) {
                        if (callback && typeof (callback) === "function") {
                            if (settings.showWaitMessage && !settings.manualHideWaitMessage) {
                                AnalysisPortal.hideWaitMsg(settings.waitMessageDivId);
                            }
                            callback(result);
                        } else {
                            if (settings.showWaitMessage) {
                                AnalysisPortal.hideWaitMsg(settings.waitMessageDivId);
                            }
                        }
                    } else {
                        if (settings.showWaitMessage) {
                            AnalysisPortal.hideWaitMsg(settings.waitMessageDivId);
                        }
                        AnalysisPortal.showErrorMsg(result.msg);
                        if (settings.errorCallback && typeof(settings.errorCallback) === "function") {
                            settings.errorCallback(result);
                        }
                    }
                } else {
                    if (settings.showWaitMessage) {
                        AnalysisPortal.hideWaitMsg(settings.waitMessageDivId);
                    }
                    if (!response.aborted) { // No response from server (timout) or the request was aborted by user
                        setTimeout(function() {
                            AnalysisPortal.showTimeoutMsg();
                        }, 3000);
                    }
                }
            }
        };
        if (options.method)
            requestSettings.method = options.method;        
        if (options.params) 
            requestSettings.params = options.params;
        if (options.timeout)
            requestSettings.timeout = options.timeout;
        if (settings.test == "timeout")
            requestSettings.timeout = 1;
        
        // abort existing request
        if (!settings.allowMultipleRequests && settings.requestObject != null) {
            Ext.Ajax.abort(settings.requestObject);
        }

        // show wait message
        if (settings.showWaitMessage) {
            AnalysisPortal.showWaitMsg(settings.waitMessage, settings.waitMessageDivId);
        }       
        
        // make ajax call
        if (settings.test == "wait") {
            setTimeout(function() {
                 return Ext.Ajax.request(requestSettings);
            }, 5000);
        } else {
            return Ext.Ajax.request(requestSettings);
        }
        
    };
   

    context.createJsonStore = function(modelName, url, options) {
        var settings = $.extend(true, {
            api: null,
            writer: null,
            root: 'data',            
        }, options);


        var proxySettings = {
            type: 'ajax',
            url: url,
            reader: {
                type: 'json',
                root: settings.root,
                successProperty: 'success'
            },            
            listeners: {
                exception: function(proxy, response, operation, eOpts) {
                    if (response.timedout) {
                        AnalysisPortal.showTimeoutMsg();
                    } else {
                        var result = Ext.JSON.decode(response.responseText);
                        AnalysisPortal.showErrorMsg(result.msg);
                    }
                }
            }
        };
        if (settings.api) {
            proxySettings.api = settings.api;
        }
        if (settings.writer) {
            proxySettings.writer = settings.writer;
        }
        if (settings.method) {
            proxySettings.actionMethods = settings.method;
        }

        var storeSettings = {
            model: modelName,
            autoLoad: false,
            proxy: proxySettings            
        };
        if (settings.groupField)
            storeSettings.groupField = settings.groupField;
        if (settings.pageSize)
            storeSettings.pageSize = settings.pageSize;
        if (settings.listeners)
            storeSettings.listeners = settings.listeners;
        var store = Ext.create('Ext.data.Store', storeSettings);
        return store;
    };
    
    context.createJsonTreeStore = function (modelName, url, options) {
        var settings = $.extend(true, {
            api: {
                read: 'GetTaxaTreeNodes'
            },
            writer: null,
            root: 'data',
        }, options);


        var proxySettings = {
            type: 'ajax',
            url: url,
            reader: {
                type: 'json',
                root: settings.root,
                successProperty: 'success'
            },
            listeners: {
                exception: function (proxy, response, operation, eOpts) {
                    if (response.timedout) {
                        AnalysisPortal.showTimeoutMsg();
                    } else {
                        var result = Ext.JSON.decode(response.responseText);
                        AnalysisPortal.showErrorMsg(result.msg);
                    }
                }
            }
        };
        if (settings.api) {
            proxySettings.api = settings.api;
        }
        if (settings.writer) {
            proxySettings.writer = settings.writer;
        }
        if (settings.method) {
            proxySettings.actionMethods = settings.method;
        }

        var storeSettings = {
            model: modelName,
            autoLoad: false,
            proxy: proxySettings
        };
        if (settings.groupField)
            storeSettings.groupField = settings.groupField;
        if (settings.pageSize)
            storeSettings.pageSize = settings.pageSize;
        if (settings.listeners)
            storeSettings.listeners = settings.listeners;
        var store = Ext.create('Ext.data.TreeStore', storeSettings);
        return store;
    };

    context.createLayoutContainer = function(items, options) {
        var settings = $.extend(true, {
            title: "",
            width: null,
            height: null,
            layout: "fit"
        }, options);        

        var panelSettings = {
            layout: settings.layout,
            title: settings.title,
            width: settings.width,
            height: settings.height,
            items: items
        };
        if (settings.region)
            panelSettings.region = settings.region;
        if (settings.split)
            panelSettings.split = settings.split;
        if (settings.collapsible)
            panelSettings.collapsible = settings.collapsible;
        if (settings.resizable)
            panelSettings.resizable = settings.resizable;
        if (settings.border)
            panelSettings.border = settings.border;
        if (settings.titleCollapse)
            panelSettings.titleCollapse = settings.titleCollapse;
        if (settings.collapsed)
            panelSettings.collapsed = settings.collapsed;


        var container = Ext.create('Ext.Container', panelSettings);
        return container;
    };

    context.createLayoutPanel = function (items, options) {
        var settings = $.extend(true, {
            title: "",
            width: null,
            height: null,
            layout: "fit"
        }, options);

        var panelSettings = {
            layout: settings.layout,
            title: settings.title,
            width: settings.width,
            height: settings.height,
            items: items
        };
        if (settings.region)
            panelSettings.region = settings.region;
        if (settings.split)
            panelSettings.split = settings.split;
        if (settings.collapsible)
            panelSettings.collapsible = settings.collapsible;
        if (settings.resizable)
            panelSettings.resizable = settings.resizable;
        if (settings.border)
            panelSettings.border = settings.border;
        if (settings.titleCollapse)
            panelSettings.titleCollapse = settings.titleCollapse;
        if (settings.collapsed)
            panelSettings.collapsed = settings.collapsed;


        var container = Ext.create('Ext.Panel', panelSettings);
        return container;
    };

    context.createHtmlPanel = function(id, options) {
        var settings = $.extend(true, {
            title: "",
            width: null,
            height: null,
            layout: "fit"
        }, options);
        

        var panelSettings = {
            contentEl: id,
            cls: 'empty',
            layout: settings.layout,
            title: settings.title,
            width: settings.width,
            height: settings.height
        };
        if (settings.region)
            panelSettings.region = settings.region;
        if (settings.split)
            panelSettings.split = settings.split;
        if (settings.collapsible)
            panelSettings.collapsible = settings.collapsible;
        if (settings.resizable)
            panelSettings.resizable = settings.resizable;
        if (settings.autoScroll)
            panelSettings.autoScroll = settings.autoScroll;

        var panel = Ext.create('Ext.Panel', panelSettings);
        return panel;
    };

    context.createHtmlContainer = function(id, options) {
        var settings = $.extend(true, {            
            width: null,
            height: null,
            layout: "fit"
        }, options);

        var panel = Ext.create('Ext.Container', {
            contentEl: id,
            cls: 'empty',
            layout: settings.layout,            
            width: settings.width,
            height: settings.height
        });
        return panel;
    };

    context.createMainPanel = function(div, options) {
        var settings = $.extend(true, {
            title: "",
            width: null,
            height: null,
            layout: "fit",
            collapsible: true,
            collapsed: false,
            titleCollapse: true,
            doLayout: true,
            items: [],
            listeners: []
        }, options);

        var panelSettings = {
            renderTo: div,
            layout: settings.layout,
            width: settings.width,
            height: settings.height,
            collapsible: settings.collapsible,
            collapsed: settings.collapsed,
            titleCollapse: settings.titleCollapse,
            title: settings.title,
            items: settings.items,
            listeners: settings.listeners
        };
        if (options.autoDestroy)
            panelSettings.autoDestroy = options.autoDestroy;

        var mainPanel = Ext.create('Ext.Panel', panelSettings);

        if (settings.doLayout) {
            setTimeout(function() {
                mainPanel.doLayout();
            }, 500);
        }
        return mainPanel;
    };
    
    context.QueryComplexityDialog = {
        continueCallback: null,
        cancelCallback: null
    };

    context.showQueryComplexityDialog = function(continueCallback) {
        $('#queryComplexityModal').modal({         
            backdrop: true,
            keyboard: false,
            show: true
        });
        context.QueryComplexityDialog.continueCallback = continueCallback;

        //if (continueCallback && typeof (continueCallback) === "function") {
        //    continueCallback();
        //}
    };

    context.objectToString = function(object) {
    	/// <summary>
    	/// Use in debug purpose to write out an object as a string.
    	/// </summary>    	
        var string = "";
        var name;
        for (name in object) {
            if (typeof object[name] !== 'function') {
                string += "{\"" + name + "\": \"" + object[name] + "\"}<br />";
            }
        }
        return string;
    };

    context.integerWithThousandSeparator = function(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
    };

    context.floatWithThousandSeparator = function(x) {
        var parts = x.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parts.join(".");
    };

    context.deleteWhiteSpaces = function(x) {
        return x.replace(/\s/g, '');
    };

    context.blockWhileServerWorking = function()
    {
        context.showWaitMsg(AnalysisPortal.Resources.SharedLoadingData);
        Ext.util.Cookies.set('ServerDone', 'false');
        var timerId = setInterval(function () {
            var done = Ext.util.Cookies.get('ServerDone');
            if (done == 'true') {
                clearInterval(timerId);
                context.hideWaitMsg();
            }
        }, 1000);
    };

    function fixConsole () {
        /// <summary>
        /// Fix Console.log() issue - Console.log() is used by FireBug to log in JS but it is not defined in IE.
        /// If console.log() is used IE will throw an error - the solution is to always define an own console-wrapper if doesn't exist already
        /// </summary>        
        
        var alertFallback = false;
        if (typeof console === "undefined") {
            console = {}; // define it if it doesn't exist already
        }
        if (typeof console.log === "undefined") {
            if (alertFallback) { console.log = function (msg) { alert(msg); }; }
            else { console.log = function () { }; }
        }
        if (typeof console.dir === "undefined") {
            if (alertFallback) {
                // THIS COULD BE IMPROVED… maybe list all the object properties?
                console.dir = function (obj) { alert("DIR: " + obj); };
            }
            else { console.dir = function () { }; }
        }
        
        // console.time implementation for IE
        if (window.console && typeof (window.console.time) == "undefined") {
            console.time = function (name, reset) {
                if (!name) { return; }
                var time = new Date().getTime();
                if (!console.timeCounters) { console.timeCounters = {}; }
                var key = "KEY" + name.toString();
                if (!reset && console.timeCounters[key]) { return; }
                console.timeCounters[key] = time;
            };

            console.timeEnd = function (name) {
                var time = new Date().getTime();
                if (!console.timeCounters) { return; }
                var key = "KEY" + name.toString();
                var timeCounter = console.timeCounters[key];
                var diff;
                if (timeCounter) {
                    diff = time - timeCounter;
                    var label = name + ": " + diff + "ms";
                    console.info(label);
                    delete console.timeCounters[key];
                }
                return diff;
            };
        }


    }    
    
    context.getAlertStatusClassInGrid = function(v, metadata, rec) {
        /// <summary>
        /// Get the Taxon Alert Status Css class and sets tooltip text
        /// Used in Ext JS grid Action columns.
        /// </summary>        
        
        var val = rec.get('TaxonStatus');        
        switch (val) {
            case 0:
                //this.items[0].tooltip = AnalysisPortal.Resources.TaxonSharedAlertStatusGreen;
                return 'taxon-alert-status-green';
            case 1:
                this.items[0].tooltip = AnalysisPortal.Resources.TaxonSharedAlertStatusYellow;
                return 'taxon-alert-status-yellow';
            case 2:
                this.items[0].tooltip = AnalysisPortal.Resources.TaxonSharedAlertStatusRed;
                return 'taxon-alert-status-red';
        }
        return '';
    };

    context.formatDecimalNumber = function (num) {
        /// <summary>
        /// Formats a decimal number.        
        /// </summary>    
        if (context.Language == "sv-SE") {
            return num.toString().replace('.', ',');
        } else {
            return num.toString();
        }
    };

    context.autolink = function (str, attributes) {
        /// <summary>
        /// Looks for properly formatted url:s inside a text and converts them to  clickable links        
        /// </summary>        
        attributes = attributes || {};
        var attrs = "";
        for (name in attributes)
            attrs += " " + name + '="' + attributes[name] + '"';
        var reg = new RegExp("(\\s?)((http|https|ftp)://[^\\s<]+[^\\s<\.)])", "gim");
        str = str.toString().replace(reg, '$1<a href="$2"' + attrs + '>$2</a>');
        return str;
    };

    context.showDialog = function (options) {
        /// <summary>
        /// Shows a dialog using Twitter Bootstrap-modal
        /// </summary>        
        
        var settings = $.extend(true, {
            modalDiv: 'ModalDialog',
            url: null,
            content: '',
            sizeClass: 'modal-md',            
            show: true,
            backdrop: true,
            keyboard: true,
            title: null,
            showFooter: false,
            buttons: '<button class="btn btn-primary" data-dismiss="modal">Ok</button>',
            afterLoadCallback: null
        }, options);

        //Make sure the modal not exists in DOM
        $('#' + settings.modalDiv).remove();

        var $modalHeader = $('<div/>')
            .addClass('modal-header')
            .append($('<button/>')
                .attr({ type: 'button', 'aria-label': 'Close', 'data-dismiss': 'modal' })
                .addClass('close')
                .append($('<span/>')
                    .attr('aria-hidden', 'true')
                    .html('&times;')
                )
            )
            .append($('<h4/>')
                .addClass('modal-title')
                .html(settings.title)
            );

        var $modalBody = $('<div/>')
            .addClass('modal-body')
            .html(settings.content);

        var $modalFooter = null;
        if (settings.showFooter) {
            $modalFooter = $('<div/>')
                .addClass('modal-footer')
                .append(settings.buttons);
        }
        
        var $modalContent = $('<div/>')
            .addClass('modal-content')
            .append($modalHeader)
            .append($modalBody)
            .append($modalFooter);

        var $modalDialog = $('<div/>')
            .addClass('modal-dialog ' + settings.sizeClass)
            .append($modalContent);

        var $modal = $('<div/>')
            .attr({
                id: settings.modalDiv,
                tabindex: -1,
                role: 'dialog'
            })
            .addClass('modal fade')
            .css('z-index', 10000003)
            .append($modalDialog);

        $modal.appendTo('body');

        if (settings.url) {
            $('.modal-body', $modal).load(settings.url, settings.data, settings.afterLoadCallback);
        } else if (settings.afterLoadCallback != null) {
            $modal.on('shown.bs.modal', settings.afterLoadCallback);
        }

        $modal.modal({
            show: settings.show,
            backdrop: settings.backdrop,
            keyboard: settings.keyboard
        });
    };

    context.hideDialog = function() {
        $('#ModalDialog').modal('hide');
    }

    context.InitMultiSelect = function(selector, options) {
        var settings = $.extend(true,
            {
                includeSelectAllOption: false,
                numberDisplayed: 1,
                selectAllText: AnalysisPortal.Resources.MultiSelectSelectAll,
                nonSelectedText: AnalysisPortal.Resources.MultiSelectNonSelected,
                allSelectedText: AnalysisPortal.Resources.MultiSelectAllSelected,
                nSelectedText: AnalysisPortal.Resources.MultiSelectNSelected
            },
            options);

        $(selector).multiselect(settings);
    };

    context.getMaxZ = function (selector) {
        return Math.max.apply(null, $(selector).map(function () {
            var z;
            return isNaN(z = parseInt($(this).css("z-index"), 10)) ? 0 : z;
        }));
    };

    context.base64Encode = function(input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

       // input = utf8_decode(input);
       
        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            var keyStr = 'ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäö0123456789+/=';
            output = output +
                keyStr.charAt(enc1) +
                keyStr.charAt(enc2) +
                keyStr.charAt(enc3) +
                keyStr.charAt(enc4);

        }

        return output;
    };

    context.base64Decode = function(input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Öa-ö0-9\+\/\=]/g, "");

        var keyStr = 'ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäö0123456789+/=';
        while (i < input.length) {

            enc1 = keyStr.indexOf(input.charAt(i++));
            enc2 = keyStr.indexOf(input.charAt(i++));
            enc3 = keyStr.indexOf(input.charAt(i++));
            enc4 = keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }
        }

        return utf8_decode(output);
    };

    function utf8_decode(utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }

        return string;
    }

})(AnalysisPortal);    
    