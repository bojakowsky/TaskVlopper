﻿app.service('TaskService', ['$http', function ($http) {
        
        this.getAll = function (projectId) {
            return $http.get('/Task?projectId=' + projectId)
            .then(function (response) {
                if (response.data.HttpCode != undefined) {
                    console.log(response.data.HttpCode + " " + response.data.Message);
                }
                return response.data;
            })
            .catch(function (error) {
                console.log('[TaskService.getAll] Unable to load data: ' + error.data.message);
            });
        };

        this.getAllWithStats = function (projectId) {
            return $http.get('/Task/GetAllWithStats?projectId=' + projectId)
            .then(function (response) {
                if (response.data.HttpCode != undefined) {
                    console.log(response.data.HttpCode + " " + response.data.Message);
                }
                return response.data;
            })
            .catch(function (error) {
                console.log('[TaskService.getAllWithStats] Unable to load data: ' + error.data.message);
            });
        };

        this.get = function (taskId, projectId) {
            if (taskId === undefined || isNaN(taskId)) {
                console.log("[TaskService.get] TaskID is invalid!");
                return null;
            }
            if (projectId === undefined || isNaN(projectId)) {
                console.log("[TaskService.get] ProjectID is invalid!");
                return null;
            }
            return $http.get('/Task/Details/' + taskId + "?projectId=" + projectId)
            .then(function (response) {
                if (response.data.HttpCode != undefined) {
                    console.log(response.data.HttpCode + " " + response.data.Message);
                }
                return response.data.Task;
            })
            .catch(function (error) {
                console.log('[TaskService.get] Unable to load data: ' + error.message);
            });
        };

        this.create = function (model, projectId) {
            var newmodel = JSON.stringify(model);
            return $http({
                method: 'POST',
                url: '/Task/Create?projectId=' + projectId,
                accept: 'application/json',
                data: newmodel
            })
            .then(function (response) {
                if (response.data.HttpCode != undefined) {
                    console.log(response.data.HttpCode + " " + response.data.Message);
                }
                return response;
            })
            .catch(function (error) {
                $scope.status = '[TaskService.create] Unable to load data: ' + error.message;
                console.log($scope.status);
            });
        };

        this.update = function (model, projectId) {
            return $http({
                method: 'POST',
                url: '/Task/Edit/' + model.ID + "?projectId=" + projectId,
                accept: 'application/json',
                data: model
            })
            .then(function (response) {
                if (response.data.HttpCode != undefined) {
                    console.log(response.data.HttpCode + " " + response.data.Message);
                }
                return response;
            })
            .catch(function (error) {
                $scope.status = '[TaskService.update] Unable to load data: ' + error.message;
                console.log($scope.status);
            });
        }

        this.delete = function (taskId, projectId) {
            if (taskId === undefined || isNaN(taskId)) {
                console.log("[TaskService.delete] TaskID is invalid!");
                return null;
            }
            if (projectId === undefined || isNaN(projectId)) {
                console.log("[TaskService.delete] ProjectID is invalid!");
                return null;
            }
            if (projectId !== undefined || !isNaN(projectId))
            return $http.post('/Task/Delete/' + taskId + "?projectId=" + projectId)
            .then(function (response) {
                if (response.data.HttpCode != undefined) {
                    console.log(response.data.HttpCode + " " + response.data.Message);
                }
                return response;
            })
            .catch(function (error) {
                $scope.status = '[TaskService.delete] Unable to load data: ' + error.message;
                console.log($scope.status);
            });
        };
        this.getUsers = function (taskId, projectId) {
            if (taskId !== undefined && !isNaN(taskId) && projectId !== undefined && !isNaN(projectId)) {
                return $http.get('/Task/Users/' + taskId + '?projectId=' + projectId)
                .then(function (response) {
                    if (response.data.HttpCode != undefined) {
                        console.log(response.data.HttpCode + " " + response.data.Message);
                    }
                    return response.data;
                })
                .catch(function (error) {
                    console.log('[TaskService.getUsers] Unable to load data: ' + error.message);
                });
            }
            else {
                console.log("[TaskService.getUsers] ProjectID or TaskID is invalid!");
                return null;
            }
        }

        this.bindUser = function (taskId, projectId, userId) {
            if (taskId !== undefined && !isNaN(taskId) && projectId !== undefined && !isNaN(projectId)) {
                return $http.post('/Task/Users/' + taskId + "?projectId=" + projectId + "&userId=" + userId)
                .then(function (response) {
                    if (response.data.HttpCode != undefined) {
                        console.log(response.data.HttpCode + " " + response.data.Message);
                    }
                    return response;
                })
                .catch(function (error) {
                    console.log('[TaskService.bindUser] Unable to load data: ' + error.message);
                });
            }
            else {
                console.log("[TaskService.bindUser] ProjectID or TaskID is invalid!");
                return null;
            }
        }

        this.unbindUser = function (taskId, projectId, userId) {
            if (taskId !== undefined && !isNaN(taskId) && projectId !== undefined && !isNaN(projectId)) {
                return $http.post('/Task/UnbindUser/' + taskId + "?projectId=" + projectId + "&userId=" + userId)
                .then(function (response) {
                    if (response.data.HttpCode != undefined) {
                        console.log(response.data.HttpCode + " " + response.data.Message);
                    }
                    return response;
                })
                .catch(function (error) {
                    console.log('[TaskService.unbindUser] Unable to load data: ' + error.message);
                });
            }
            else {
                console.log("[TaskService.unbindUser] ProjectID or TaskId is invalid!");
                return null;
            }
        }

    }
]);