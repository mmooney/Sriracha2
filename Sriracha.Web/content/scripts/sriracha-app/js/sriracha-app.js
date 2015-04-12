window.SrirachaApp = Ember.Application.create({
    rootElement: "#sriracha-app",
    LOG_TRANSITIONS: true
});

window.SrirachaApp.Router.map(function () {
    this.resource("project", function () {
        this.route("create", { path: "create" });
        this.route("view", {path: ":project_id"});
    });
});

SrirachaApp.Project = Ember.Model.extend({
    id: Ember.attr(),
    projectName: Ember.attr()
});
SrirachaApp.Project.adapter = Ember.RESTAdapter.create();
SrirachaApp.Project.url = "/api/project";
SrirachaApp.Project.collectionKey = "projects";
//SrirachaApp.Project.rootKey = "project";

SrirachaApp.ProjectIndexRoute = Ember.Route.extend({
    model: function () {
        return SrirachaApp.Project.find();
    }
});

SrirachaApp.ProjectCreateRoute = Ember.Route.extend({
    setupController: function (controller, model) {
        controller.set("errorMessage", "");
        controller.set("projectName", "");
    }
});

SrirachaApp.ProjectCreateController = Ember.Controller.extend({
    errorMessage: "",
    actions: {
        "saveProject": function (data) {
            var ctrl = this;
            var project = SrirachaApp.Project.create({projectName: this.get("projectName")});
            project.save()
                    .then(function (data) {
                        ctrl.transitionToRoute("project.view", data.get("id"));
                    }, function (errorData, x, y, z) {
                        ctrl.set("errorMessage", errorData.responseText);
                    });
        }
    }
});
