(function ($) {
    $.MoverBoxes = function (element, options) {
        // plugin's default options
        // this is private property and is  accessible only from inside the plugin
        var defaults = {
            size: 15
            // if your plugin is event-driven, you may provide callback capabilities for its events.
            // execute these functions before or after events of your plugin, so that users may customize
            // those particular events without changing the plugin's code

        };

        var Moverhtml = function (dataleft, dataright, leftLabel, rightLabel) {

            //main toobar div.
            var moverDIV = document.createElement("div");
            $(moverDIV).addClass('mover-div');

            //table
            var movertable = document.createElement("table");
            var moverTableTbody = document.createElement("tbody");
            movertable.appendChild(moverTableTbody);

            var tableTR = document.createElement("tr");
            var tableTDOne = document.createElement("td");


            if (leftLabel) {

                var leftSpan = document.createElement("span");
                $(leftSpan).addClass("MoverBox-label");
                $(leftSpan).addClass("leftBox-label");
                $(leftSpan).html(leftLabel);
                tableTDOne.appendChild(leftSpan);

            }

            var tableTDSelect = document.createElement("select");
            $(tableTDSelect).attr("id", "selectorOne");
            $(tableTDSelect).attr("name", "selectorOne");
            $(tableTDSelect).addClass("MoverBoxLeft");
            $(tableTDSelect).addClass("MoverBox");
            $(tableTDSelect).attr("multiple", "multiple");
            $(tableTDSelect).attr("size", options.size);

            //add dataleft
            for (var i = 0, len = options.dataleft.length; i < len; ++i) {
                var dataObj = options.dataleft[i].split(',');
                var optionEl = document.createElement("option");
                $(optionEl).attr("value", dataObj[0]);
                $(optionEl).attr("id", "optId_" + dataObj[0]);
                $(optionEl).html(dataObj[1]);
                tableTDSelect.appendChild(optionEl);
            }

            tableTDOne.appendChild(tableTDSelect);
            tableTR.appendChild(tableTDOne);

            var tableTDTwo = document.createElement("td");
            $(tableTDTwo).attr("align", "center");
            $(tableTDTwo).addClass('buttonTD');
            $(tableTDTwo).attr("valign", "middle");


            //input buttons
            var input = document.createElement("input");
            $(input).attr("class", "ap-ui-button twoPxMargin");
            $(input).attr("type", "button");
            $(input).attr("value", " > ");
            $(input).attr("id", "moveRight");
            var br = document.createElement("br");
            tableTDTwo.appendChild(input);
            tableTDTwo.appendChild(br);

            var inputTwo = document.createElement("input");
            $(inputTwo).attr("class", "ap-ui-button twoPxMargin");
            $(inputTwo).attr("type", "button");
            $(inputTwo).attr("value", " < ");
            $(inputTwo).attr('id', 'moveLeft');
            tableTDTwo.appendChild(inputTwo);
            tableTR.appendChild(tableTDTwo);


            var brAllOne = document.createElement("br");
            tableTDTwo.appendChild(brAllOne);
            var inputTwoAll = document.createElement("input");
            $(inputTwoAll).attr("class", "ap-ui-button twoPxMargin");
            $(inputTwoAll).attr("type", "button");
            $(inputTwoAll).attr("value", " << ");
            $(inputTwoAll).attr('id', 'moveLeftAll');

            tableTDTwo.appendChild(inputTwoAll);


            var brAllTwp = document.createElement("br");
            tableTDTwo.appendChild(brAllTwp);
            var inputThreeAll = document.createElement("input");
            $(inputThreeAll).attr("class", "ap-ui-button twoPxMargin");
            $(inputThreeAll).attr("type", "button");
            $(inputThreeAll).attr("value", " >> ");
            $(inputThreeAll).attr('id', 'moveRightAll');
            tableTDTwo.appendChild(inputThreeAll);

            var tableTDThree = document.createElement("td");
            if (rightLabel) {
                var rightSpan = document.createElement("span");
                $(rightSpan).addClass("MoverBox-label");
                $(rightSpan).addClass("rightBox-label");
                $(rightSpan).html(rightLabel);
                tableTDThree.appendChild(rightSpan);
            }
            var tableTDSelectTwo = document.createElement("select");
            $(tableTDSelectTwo).attr("id", "selectorTwo");
            $(tableTDSelectTwo).addClass("MoverBoxRight");
            $(tableTDSelectTwo).addClass("MoverBox");
            $(tableTDSelectTwo).attr("name", "selectorTwo");
            $(tableTDSelectTwo).attr("multiple", "multiple");
            $(tableTDSelectTwo).attr("size", options.size);

            //add dataright
            for (var i = 0, len = options.dataright.length; i < len; ++i) {
                var dataObj = options.dataright[i].split(',');
                var optionEl2 = document.createElement("option");
                $(optionEl2).attr("value", dataObj[0]);
                $(optionEl2).attr("id", "optId_" + dataObj[0]);
                $(optionEl2).html(dataObj[1]);
                tableTDSelectTwo.appendChild(optionEl2);
            }


            tableTDThree.appendChild(tableTDSelectTwo);
            tableTR.appendChild(tableTDThree);

            moverTableTbody.appendChild(tableTR);
            movertable.appendChild(moverTableTbody);
            moverDIV.appendChild(movertable);
            this.Moverhtml = moverDIV;

        };


        // to avoid confusions, use "plugin" to reference the current instance of the object
        var plugin = this;

        // this will hold the merged default, and user-provided options
        // plugin's properties will be available through this object like:
        // plugin.settings.propertyName from inside the plugin or
        // element.data('pluginName').settings.propertyName from outside the plugin, where "element" is the
        // element the plugin is attached to;
        plugin.settings = { };

        var $element = $(element), // reference to the jQuery version of DOM element the plugin is attached to
            element = element; // reference to the actual DOM element

        // the "constructor" method that gets called when the object is created
        plugin.init = function () {

            // the plugin's final properties are the merged default and user-provided options (if any)
            plugin.settings = $.extend({}, defaults, options);
            var myMoverBox = new Moverhtml(options.dataleft, options.dataright, options.leftLabel, options.rightLabel);

            //add to dom
            $(element).html(myMoverBox.Moverhtml);

            $('#moveLeft').live('click', function () {
                moveLeftToright();
            });

            $('#moveRight').live('click', function () {
                moveRightToleft();
            });

            $('#moveRightAll').live('click', function () {
                movAllRightToleft();
            });
            
            $('#moveLeftAll').live('click', function () {
                moveAllLeftToRight();
            });
        };
        
        // public methods
        // these methods can be called like:
        // plugin.methodName(arg1, arg2, ... argn) from inside the plugin or
        // element.data('pluginName').publicMethod(arg1, arg2, ... argn) from outside the plugin, where "element"
        // is the element the plugin is attached to;

        // a public method. for demonstration purposes only - remove it!
        plugin.SelectedValues = function () {

            var values = [];
            $('#selectorTwo option').each(function (index) {
                values.push($(this).attr('value'));
            });
            return values;
        };

        plugin.NotSelectedValues = function () {

            var values = [];
            $('#selectorOne option').each(function (index) {
                values.push($(this).attr('value'));
            });
            return values;
        };

        var moveAllLeftToRight = function () {
            $('#selectorTwo option').each(function (index) {
                $(this).remove();
                var that = $(this);
                $(that).removeClass('selected');
                $('#selectorOne').append(that);
            });
        };

        var movAllRightToleft = function () {
            $('#selectorOne option').each(function (index) {
                $(this).remove();
                var that = $(this);
                $(that).removeClass('selected');
                $('#selectorTwo').append(that);
            });
        };

        var moveLeftToright = function () {
            $('#selectorTwo option').each(function (index) {
                if ($(this)[0].selected) {

                    $(this).remove();
                    var that = $(this);
                    $(that).removeClass('selected');
                    $('#selectorOne').append(that);
                }
            });
        };

        var moveRightToleft = function () {
            $('#selectorOne option').each(function (index) {
                if ($(this)[0].selected) {

                    $(this).remove();
                    var that = $(this);
                    $(that).removeClass('selected');
                    $('#selectorTwo').append(that);
                }
            });
        };
        // fire up the plugin!
        // call the "constructor" method
        plugin.init();
    };

    // add the plugin to the jQuery.fn object
    $.fn.MoverBoxes = function (options) {

        // iterate through the DOM elements we are attaching the plugin to
        return this.each(function () {
            // if plugin has not already been attached to the element
            if (undefined == $(this).data('MoverBoxes')) {
                // create a new instance of the plugin
                // pass the DOM element and the user-provided options as arguments
                var plugin = new $.MoverBoxes(this, options);

                // in the jQuery version of the element
                // store a reference to the plugin object
                // you can later access the plugin and its methods and properties like
                // element.data('pluginName').publicMethod(arg1, arg2, ... argn) or
                // element.data('pluginName').settings.propertyName
                $(this).data('MoverBoxes', plugin);

            }
        });
    };

})(jQuery);