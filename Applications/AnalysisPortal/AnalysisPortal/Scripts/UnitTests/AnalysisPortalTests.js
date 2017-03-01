/// <reference path="qunit.js" />
/// <reference path="../jquery-2.1.4.js" />
/// <reference path="../AnalysisPortal/AnalysisPortal.js" />



test("ApplicationPath_SetPath_GetThePath", function () {
    AnalysisPortal.ApplicationPath = "analysisportal/test";

    equals(AnalysisPortal.ApplicationPath, "analysisportal/test", "Passed!");    
});

test("example test", 1, function () {
    var $body = $("body");

    $body.on("click", function () {
        ok(true, "body was clicked!");
    });

    $body.trigger("click");
});



