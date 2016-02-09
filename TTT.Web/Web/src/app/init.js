(function () {
	'use strict';
	var ttt = angular.module('ttt', ['ngResource']);

	ttt.factory('BoardMove', ['$resource', function ($resource) {
		return $resource('/api/boardMove/:id', null,
			{
				'makeMove': { method: 'POST' },
				'resetBoard': { method: 'DELETE' }
			});
	}]);
})();