'use strict';

angular.module('ttt').
controller('mainCtrl', ['$scope', 'BoardMove',
	function ($scope, BoardMove) {
		$scope.message = { header: "", body: "" };
		var getBoard = function () {
			BoardMove.get().$promise.then(function (board) {
				$scope.board = board;
			});
		};

		getBoard();

		$scope.send = function (index) {
			if ($scope.board.Moves[index] == 0 && $scope.board.Status == 0) {
				BoardMove.makeMove(index).$promise.then(function () {
					getBoard();
				});
			}
		};

		$scope.clear = function () {
			BoardMove.resetBoard().$promise.then(function () {
				getBoard();
			});
		};

		$scope.popupStatus = function () {
			if ($scope.board) {
				switch ($scope.board.Status) {
					case 1:
						return "win";
						break;
					case 2:
						return "loose";
						break;
				}
			}
		};

		$scope.$watch("board.Status", function (value) {
			switch (value) {
				case 1:
					$scope.message.header = "Congrats!";
					$scope.message.body = "You WON! Could you try again?";
					break;
				case 2:
					$scope.message.header = "Apologise...";
					$scope.message.body = "But you loose =( Could you try again?";
					break;
				case 3:
					$scope.message.header = "Hmm..";
					$scope.message.body = "There is no more moves... Could you try again?";
					break;
			}
		});
	}]).
directive('ttt', function () {
	return {
		restrict: 'E',
		templateUrl: 'src/app/main/ttt.html'
	};
});