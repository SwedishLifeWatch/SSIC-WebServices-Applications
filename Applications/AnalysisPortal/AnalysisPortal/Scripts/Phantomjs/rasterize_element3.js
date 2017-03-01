var page = require('webpage').create(),
    system = require('system'),
    address, output, size, strScale, strBodyWidth;

address = system.args[1];
output = system.args[2];
selector = system.args[3];
page.zoomFactor = system.args[4];
//page.zoomFactor = 1.5;
page.viewportSize = { width: 600, height: 600 };    

phantom.addCookie({
    'name': 'ASP.NET_SessionId',
    'value': system.args[6],
    'domain': system.args[5]
});
    
page.open(address, function (status) {
	if (status !== 'success') {
	    console.log('Unable to load the address!');
	} else {
	    window.setTimeout(function () {	        
	        strScale = "\"scale(" + system.args[4] + ")\"";
	        console.log("scale: " + strScale);
	        strBodyWidth = "\"" + (100 / parseFloat(system.args[4])) + "%\"";	        
	        var s = [];
	        s.push(strScale);
	        s.push(strBodyWidth);
	        //page.zoomFactor = 2.0;
	        var clipRect = page.evaluate(function (s) {
	            var cr = document.querySelector(s).getBoundingClientRect();
	            return cr;
	        }, selector);

	        page.clipRect = {
	            top: clipRect.top * page.zoomFactor,
	            left: clipRect.left,
	            width: clipRect.width * page.zoomFactor,
	            height: clipRect.height * page.zoomFactor
	        };	        
	        
	        page.render(output);
	        phantom.exit();
	    }, 1500);
	}
});
