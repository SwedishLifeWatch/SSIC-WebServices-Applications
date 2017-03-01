var page = require('webpage').create(),
    system = require('system'),
    address, output, size, strScale, strBodyWidth;

address = system.args[1];
output = system.args[2];
selector = system.args[3];
page.viewportSize = { width: 600, height: 600 };    
    
/*if (system.args.length > 4) {
    page.zoomFactor = system.args[4];
}*/
console.log("Loading page...");
page.open(address, function (status) {
	if (status !== 'success') {
	    console.log('Unable to load the address!');
	} else {
	    window.setTimeout(function () {
	        console.log("Getting element clipRect...");
	        strScale = "\"scale(" + system.args[4] + ")\"";
	        console.log("scale: " + strScale);
	        strBodyWidth = "\"" + (100 / parseFloat(system.args[4])) + "%\"";
	        console.log("body width: " + strBodyWidth);
	        var s = [];
	        s.push(strScale);
	        s.push(strBodyWidth);
	        var clipRect = page.evaluate(function (s) {

	            /* scale the whole body */
	            /*document.body.style.webkitTransform = "scale(2.0)";	                
	            document.body.style.webkitTransformOrigin = "0% 0%";*/
	            /* fix the body width that overflows out of the viewport */
	            /*document.body.style.width = "50%";*/


	            /* scale the whole body */
//	            document.body.style.webkitTransform = s[0];
//	            document.body.style.webkitTransformOrigin = "0% 0%";
	            /* fix the body width that overflows out of the viewport */
//	            document.body.style.width = s[1];

	            var cr = document.querySelector(s).getBoundingClientRect();
	            return cr;
	        }, selector);

	        page.clipRect = {
	            top: clipRect.top,
	            left: clipRect.left,
	            width: clipRect.width,
	            height: clipRect.height
	        };
	        console.log("Rendering to file...");
	        page.render(output);
	        phantom.exit();
	    }, 1500);
	}
});
