var create = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "        <div class=\"alert alert-danger\">"
    + escapeExpression(((helper = (helper = helpers.errorMessage || (depth0 != null ? depth0.errorMessage : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"errorMessage","hash":{},"data":data}) : helper)))
    + "</div>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, buffer = "﻿<h3>Create Project</h3>\r\n<form class=\"form-horizontal\" "
    + escapeExpression(((helpers.action || (depth0 && depth0.action) || helperMissing).call(depth0, "saveProject", {"name":"action","hash":{
    'on': ("submit")
  },"data":data})))
    + ">\r\n    <div class=\"form-group\">\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.errorMessage : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "        <label class=\"col-md-2 control-label\" for=\"UserName\">Project Name</label>\r\n        <div class=\"col-md-10\">\r\n            "
    + escapeExpression(((helpers.input || (depth0 && depth0.input) || helperMissing).call(depth0, {"name":"input","hash":{
    'required': ("required"),
    'class': ("form-control"),
    'value': ((depth0 != null ? depth0.projectName : depth0))
  },"data":data})))
    + "\r\n        </div>\r\n    </div>\r\n    <div class=\"form-group\">\r\n        <input type=\"submit\" value=\"save\" class=\"btn btn-success\" />\r\n    </div>\r\n</form>\r\n";
},"useData":true});