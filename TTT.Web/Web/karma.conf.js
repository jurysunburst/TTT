'use strict';

module.exports = function(config) {
    config.set({

        basePath : '',

        files : [
            { pattern: './src/assets/lib/angular.min.js'},
            { pattern: './src/assets/lib/angular-resource.min.js'},
            { pattern: './src/assets/lib/angular-mocks.js'},
            { pattern: './src/app/*.js'},
            { pattern: './src/app/main/*.js'},
            { pattern: './test/*.js'},
        ],

        frameworks: ['jasmine' ],

        plugins : [
            require('karma-jasmine')
        ],

        autoWatch : true,
        singleRun: true,
        exclude: []
    });
};