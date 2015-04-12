var index = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, helperMissing=helpers.helperMissing, buffer = "        <tr>\r\n            <td>";
  stack1 = ((helpers['link-to'] || (depth0 && depth0['link-to']) || helperMissing).call(depth0, "project.view", ((stack1 = (depth0 != null ? depth0.project : depth0)) != null ? stack1.id : stack1), {"name":"link-to","hash":{},"fn":this.program(2, data),"inverse":this.noop,"data":data}));
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</td>\r\n        </tr>\r\n";
},"2":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.project : depth0)) != null ? stack1.projectName : stack1), depth0));
  },"4":function(depth0,helpers,partials,data) {
  return "Create Project";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helperMissing=helpers.helperMissing, buffer = "﻿<h3>Project List</h3>\r\n<table class=\"table table-striped\">\r\n    <tbody>\r\n        <tr>\r\n            <th>Project Name</th>\r\n        </tr>\r\n";
  stack1 = helpers.each.call(depth0, (depth0 != null ? depth0.project : depth0), (depth0 != null ? depth0['in'] : depth0), depth0, {"name":"each","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "    </tbody>\r\n</table>\r\n";
  stack1 = ((helpers['link-to'] || (depth0 && depth0['link-to']) || helperMissing).call(depth0, "project.create", {"name":"link-to","hash":{},"fn":this.program(4, data),"inverse":this.noop,"data":data}));
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n";
},"useData":true});