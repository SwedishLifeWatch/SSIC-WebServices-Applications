/**
* @author Elzo Valugi
* Cookie plugin with subcookie functionality
* Based on $.cookie plugin by Klaus Hartl (stilbuero.de) and YUI.Cookie
*
* Dual licensed under the MIT and GPL licenses:
* http://www.opensource.org/licenses/mit-license.php
* http://www.gnu.org/licenses/gpl.html
*
* http://developer.yahoo.com/yui/docs/YAHOO.util.Cookie.html
* http://stilbuero.de
*/
/**
* Create a cookie with the given name, part and value and other optional parameters.
* If the part parameter is passed the value will be asigned to the part key
*
*
* @example $.cookie('the_cookie', 'part', 'the_value');
* @desc Set the value of a cookie. If part parameter is present it will create the value in serialized form
* @example: $.cookie('the_cookie', null, 'the_value') // creates a cookie with value "the_value"
* @example: $.cookie('the_cookie, 'name', 'the_name') // creates a cookie with value "name=the_name"
* @example: $.cookie('the_cookie, 'prename','second_name') // will add info to the cookie value "name=the_name&prename=second_name"
* @example $.cookie('the_cookie', null, 'the_value', {expires: 7, path: '/', domain: 'jquery.com', secure: true});
* @desc Create a cookie with all available options.
* @example $.cookie('the_cookie', null, 'the_value');
* @desc Create a session cookie.
* @example $.cookie('the_cookie','name', null);
* @desc Delete the name value
* @example $.cookie('the_cookie',null, null);
* @desc Delete a cookie by passing null as value.
*
* @param String name The name of the cookie.
* @param String part The key for the array.
* @param String value The value of the cookie or of the element.
* @param Object options An object literal containing key/value pairs to provide optional cookie attributes.
* @option Number|Date expires Either an integer specifying the expiration date from now on in days or a Date object.
* If a negative value is specified (e.g. a date in the past), the cookie will be deleted.
* If set to null or omitted, the cookie will be a session cookie and will not be retained
* when the the browser exits.
* @option String path The value of the path atribute of the cookie (default: path of page that created the cookie).
* @option String domain The value of the domain attribute of the cookie (default: domain of page that created the cookie).
* @option Boolean secure If true, the secure attribute of the cookie will be set and the cookie transmission will
* require a secure protocol (like HTTPS).
*
*
*/
jQuery.subcookie = function (name, part, value, options) {
    var helper = {
        set: function (name, part, value, options) {
            options = options || {};
            if (value === null) {
                value = '';
                if (!part) {
                    options.expires = -1;
                }
            }
            if (part) {
                var full = {};
                var existing = this.get(name);
                if (existing) {
                    full = this.parseCookieHash(existing);
                }
                full[part] = value;
                value = this.createCookieHashString(full);
            }
            var expires = '';
            if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                var date;
                if (typeof options.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                } else {
                    date = options.expires;
                }
                expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
            }
            var path = options.path ? '; path=' + options.path : '';
            var domain = options.domain ? '; domain=' + options.domain : '';
            var secure = options.secure ? '; secure' : '';
            document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        },
        get: function (name, part) {
            var cookieValue = null;
            if (document.cookie && document.cookie != '') {
                var cookies = document.cookie.split(';');
                for (var i = 0; i < cookies.length; i++) {
                    var cookie = jQuery.trim(cookies[i]);
                    if (cookie.substring(0, name.length + 1) == (name + '=')) {
                        var temp = decodeURIComponent(cookie.substring(name.length + 1));
                        if (!part) {
                            cookieValue = temp;
                        } else {
                            var cookieElements = this.parseCookieHash(temp);
                            cookieValue = cookieElements[part];
                        }
                        break;
                    }
                }
            }
            return cookieValue;
        },
        /**
        * Parses a cookie hash string into an object.
        * @param {String} text
        * @return {Object} hash
        */
        parseCookieHash: function (text) {
            var hashParts = text.split("&"), hashPart = null, hash = {};
            if (text.length > 0) {
                for (var i = 0, len = hashParts.length; i < len; i++) {
                    hashPart = hashParts[i].split("=");
                    hash[decodeURIComponent(hashPart[0])] = decodeURIComponent(hashPart[1]);
                }
            }
            return hash;
        },
        /**
        * Transformes the hash into string
        * @param {Object} hash
        * @return {String} text
        */
        createCookieHashString: function (hash) {
            var text = [];
            for (var key in hash) {
                text.push(key + "=" + String(hash[key]));
            }
            return text.join("&");
        }
    };
    if (typeof value != 'undefined') {
        helper.set(name, part, value, options);
    } else {
        return helper.get(name, part);
    }
};
