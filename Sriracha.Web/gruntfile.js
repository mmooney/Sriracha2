module.exports = function (grunt) {
    grunt.initConfig({
        emberTemplates: {
            compile: {
                options: {
                    templateCompilerPath: 'content/scripts/vendor/ember/1.11/ember-template-compiler.js',
                    handlebarsPath: 'content/scripts/vendor/ember/1.11/handlebars.js',
                    templateNamespace: 'HTMLBars',
                    templateBasePath: "content/scripts/sriracha-app/templates/",
                    templateName: function (sourceFile) {
                        return sourceFile.replace("content/scripts/sriracha-app/templates/", "");
                    }
                },
                files: {
                    "content/scripts/sriracha-app/templates/sriracha-app-templates.js": "content/scripts/sriracha-app/templates/**/*.hbs"
                }
            }
        },
        watch: {
            files: ["content/scripts/sriracha-app/templates/**/*.hbs"],
            tasks: ["emberTemplates"]
        }
    });

    grunt.loadNpmTasks('grunt-ember-templates');
    grunt.loadNpmTasks("grunt-contrib-watch");
    grunt.registerTask('default', 'emberTemplates');
};