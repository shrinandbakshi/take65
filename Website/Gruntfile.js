

module.exports = function (grunt) {
    grunt.initConfig({
        angular: [
            'Js/Angular/services.js',
            'Js/Angular/hello.js',
            //'bower_components/hello/src/modules/yahoo.js',
            'Js/Angular/controllers.js',
            'Js/Angular/filters.js',
            'Js/Angular/animate.js',
            'Js/Angular/app.js'
        ],
        js: [
            'Js/Plugin/*.js',
            // 'Js/XSockets/XSockets.latest.js',
            'Js/Lib/Jquery.browser.js',
            'Js/Lib/angular.min.js',
            'Js/Angular/Lib/*.js',
            'Js/Angular/Services/*.js',
            'Js/Angular/Directives/*.js',
            'Js/Main.js',
            'Js/App.js',
            'Js/Modules/*.js',
            '<%= angular %>'
        ],
        concat: {
            my_target: {
                files: {
                    'Js/Min.Debug.js': ['<%= js %>']
                }
            }
        },
        uglify: {
            my_target: {
                files: {
                    'Js/Min.js': ['<%= js %>']
                }
            }
        },
        compass: {                  // Task
            dist: {                   // Target
                options: {              // Target options
                    trace: true,
                    config: 'config.rb',
                    noLineComments: true
                }
            }
        },
        "tfs-unlock": {
            checkout: {
                options: {
                    tfsPath: ["vs2012", "bit32"],
                    action: 'checkout'
                },
                files: {
                    src: [
                        'Services/Add-New-Widget.aspx'
                    ]
                }
            }
        },
        plato: {
            your_task: {
                files: {
                    'Js/output/': '<%= js %>',
                }
            },
        },
        watch: {
            options: {
                livereload: true
            },
            sass: {
                files: ['Css/sass/*.{scss,sass}'],
                tasks: ['compass']
            },
            css: {
                files: ['*.css']
            },
            js: {
                files: [
                    '<%= js %>'
                ],
                tasks: ['concat'/*, 'uglify'*/]
            }
        }// watch
    });

    // carrega plugins
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-compass');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-tfs-unlock');
    grunt.loadNpmTasks('grunt-plato');
    grunt.loadNpmTasks('grunt-contrib-watch');


    grunt.registerTask('default', ['compass', 'concat', 'uglify', 'watch']);
    grunt.registerTask('unlock', ['tfs-unlock']);
    grunt.registerTask('js', ['concat', 'uglify']);
    grunt.registerTask('plt', ['plato']);
};