
/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />

function restoreH2States(subkey) {
    var cookieName = "H2ToggleStatus";
    var cookieValue = $.subcookie(cookieName, subkey);
    if (cookieValue == null)
        return;
    //var bitString = unpackBitString(cookieValue);
    var bitString = cookieValue;
    var bitArray = stringToBitArray(bitString);
    //var panels = $('fieldset h2.open, fieldset h2.closed');
    var panels = $('fieldset h2.savestate');    
    if (bitArray != null) {
        for (var c = 0; c < panels.length && c < bitArray.length; c++) {
            if (bitArray[c])
                $(panels).eq(c).removeClass('closed').addClass('open');
            else                
                $(panels).eq(c).removeClass('open').addClass('closed');
        }
    }
}

function saveH2States(subkey) {
    var panels = $('fieldset h2.savestate');
    if (panels.length == 0)
        return;
    
    var cookieName = "H2ToggleStatus";
    var bitArray = [];
    for (var c = 0; c < panels.length; c++) {
        if ($(panels).eq(c).hasClass('closed')) {
            bitArray.push(0);
        } else {
            bitArray.push(1);
        }
    };    
    var bitString = arrayToString(bitArray);
    //var packed = packBitString(bitString);
    var options = { path: '/', expires: 90 }; // 90 days
    $.subcookie(cookieName, subkey, bitString, options);
    //$.subcookie(cookieName, subkey, packed, options);
}



function initToggleFieldsetH2() {
    //$('fieldset h2').click(function () {
    $('fieldset h2.open, fieldset h2.closed').click(function () {        
        $(this)
                    .toggleClass("closed")
                    .toggleClass("open")
                    .next("div.fieldsetContent").slideToggle('fast', function () {
                        // Animation complete.                        
                        
                    });
    });
    $('fieldset h2.closed').closest("fieldset").find("div.fieldsetContent").hide();
}

function initToggleFieldsetH3() {
    $('div.fieldsetContent h3').click(function () {
        $(this)
                    .toggleClass("closed")
                    .toggleClass("open")
                    .next("div.fieldsetSubContent").slideToggle('fast', function () {
                        // Animation complete.
                    });
    });
    $('div.fieldsetContent h3.closed').closest("div.fieldsetContent").find("div.fieldsetSubContent").hide();
}


function packBitString(/* string */values) {
    var chunks = values.match(/.{1,16}/g), packed = ''; 
    for (var i = 0; i < chunks.length; i++) {
         packed += String.fromCharCode(parseInt(chunks[i], 2));
    } return packed;
 } 

function unpackBitString(/* string */packed) {
    var values = ''; 
    for (var i = 0; i < packed.length; i++) {
         values += packed.charCodeAt(i).toString(2);
     } 
    return values;
}

function arrayToString(array) {
    var result = "";
    for (var i = 0; i < array.length; i++) {
        result += array[i];
    }
    return result;
}

function stringToBitArray(str) {
    var result = [];
    for (var i = 0; i < str.length; i++) {
        result.push(parseInt(str[i]));
    }
    return result;
}


//var detectFormCatcher = function () {
//    var changed = false;
//    $('form').each(function () {
//        if ($(this).data('initialForm') != $(this).serialize()) {
//            changed = true;
//            $(this).addClass('changed');
//        } else {
//            $(this).removeClass('changed');
//        }
//    });
//    if (changed) {
//        return 'One or more forms have changed!';
//    }
//};

function detectFormCatcher() {
    var changed = false;
    $('form').each(function () {
        if ($(this).data('initialForm') != $(this).serialize()) {
            changed = true;
            $(this).addClass('changed');
        } else {
            $(this).removeClass('changed');
        }
    });
    if (changed) {
        return 'One or more forms have changed!';
    }
}

function initDetectFormChanges() {
    $('form').each(function() {
        $(this).data('initialForm', $(this).serialize());
    });
}

function isAnyFormChanged() {
    var changed = false;
    $('form').each(function () {
        if ($(this).data('initialForm') != $(this).serialize()) {
            changed = true;
            $(this).addClass('changed');
        } else {
            $(this).removeClass('changed');
        }
    });
    return changed;
}


function initDetectFormChangesLeavePage() {
    $('form').each(function () {
        $(this).data('initialForm', $(this).serialize());
    }).submit(function (e) {
        var formEl = this;
        var changed = false;
        $('form').each(function () {
            if (this != formEl && $(this).data('initialForm') != $(this).serialize()) {
                changed = true;
                $(this).addClass('changed');
            } else {
                $(this).removeClass('changed');
            }
        });
        if (changed && !confirm('Another form has been changed. Continue with submission?')) {
            e.preventDefault();
        } else {
            $(window).unbind('beforeunload', detectFormCatcher);
        }
    });
    $(window).bind('beforeunload', detectFormCatcher);
};





function showDialog(title, text, confirmText, cancelText, inputId) {
    var $dialog = $('<div></div>')
        .html(text)
        .dialog({
            autoOpen: false,
            title: title,
            modal: true,
            zIndex: 999999,                
            buttons:
                [
                {
                    text: confirmText, 
                    click: function () {
                    $('input[type=submit]#'+inputId).click();                    
                    $(this).dialog("close");
                }},
                  {  
                    text: cancelText, 
                    click: function () { $(this).dialog("close"); }
                 }            
            ]
            });

    $dialog.dialog('open');
    return false;
}

//function showInfoDialog(title, text, confirmText, inputId) {
//    var $dialog = $('<div></div>')
//        .html(text)
//        .dialog({
//            autoOpen: false,
//            title: title,
//            modal: true,
//            zIndex: 999999,                
//            buttons: [
//                {
//                    text: confirmText,
//                    click: function () {
//                        $('input[type=submit]#' + inputId).click();                        
//                        $(this).dialog("close");
//                    }
//                }
//            ]
//        });
//    $dialog.dialog('open');
//    return false;
//}



function showDialog(title, text, confirmText, cancelText, okFunction, cancelFunction) {
    var $dialog = $('<div></div>')
        .html(text)
        .dialog({
            autoOpen: false,
            title: title,
            modal: true,
            zIndex: 999999,                
            buttons:
                [{
                    text: confirmText,
                    click: function () {
                        $(this).dialog("close");
                        if (typeof okFunction == 'function') {
                            okFunction();
                        }
                    }
                }, {
                    text: cancelText,
                    click: function () {
                        $(this).dialog("close");
                        if (typeof cancelFunction == 'function') {
                            cancelFunction();
                        }
                    }
                }]
        });

    $dialog.dialog('open');
    return false;
    }

    function showInfoDialog(title, text, confirmText, okFunction) {
        var $dialog = $('<div></div>')
        .html(text)
        .dialog({
            autoOpen: false,
            title: title,
            modal: true,
            zIndex: 999999,                
            buttons:
                [{
                    text: confirmText,
                    click: function () {
                        $(this).dialog("close");
                        if (typeof okFunction == 'function') {
                            okFunction();
                        }
                    }

                }]
        });

        $dialog.dialog('open');
        return false;
    }

    function showYesNoDialog(title, text, yesText, yesFunction, noText, noFunction) {
        var $dialog = $('<div></div>')
        .html(text)
        .dialog({
            autoOpen: false,
            title: title,
            modal: true,
            zIndex: 999999,
            buttons:
                [{
                    text: yesText,
                    click: function () {
                        $(this).dialog("close");
                        if (typeof yesFunction == 'function') {
                            yesFunction();
                        }
                    }
                },
                {
                    text: noText,
                    click: function () {
                        $(this).dialog("close");
                        if (typeof noFunction == 'function') {
                            noFunction();
                        }
                    }
                }]
        });

        $dialog.dialog('open');
        return false;
    }


//var DyntaxaReferences;
//if (!DyntaxaReferences) {
//    DyntaxaReferences = {};
//}

//(function () {
//    'use strict';

//    function f(n) {
//        // Format integers to have at least two digits.
//        return n < 10 ? '0' + n : n;
//    }    
//    
//    if (typeof DyntaxaReferences.stringify !== 'function') {
//        DyntaxaReferences.stringify = function (value, replacer, space) {
//            
//            
//        };
//    }            

//}());