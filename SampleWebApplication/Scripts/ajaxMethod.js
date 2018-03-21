var SampleWebApplication;
(function (SampleWebApplication) {
    var Models;
    (function (Models) {
        var SampleModel = /** @class */ (function () {
            function SampleModel() {
            }
            return SampleModel;
        }());
        Models.SampleModel = SampleModel;
    })(Models = SampleWebApplication.Models || (SampleWebApplication.Models = {}));
})(SampleWebApplication || (SampleWebApplication = {}));
(function (SampleWebApplication) {
    var Controllers;
    (function (Controllers) {
        var HomeController = /** @class */ (function () {
            function HomeController() {
            }
            HomeController.Remove = function (id, then, fail, token) {
                if (token === void 0) { token = null; }
                var __arg = {
                    id: id,
                };
                this.__loadModel('./Home/Remove', __arg, then, fail, token);
            };
            HomeController.Add = function (name, note, then, fail, token) {
                if (token === void 0) { token = null; }
                var __arg = {
                    name: name,
                    note: note,
                };
                this.__loadModel('./Home/Add', __arg, then, fail, token);
            };
            HomeController.Edit = function (id, name, note, then, fail, token) {
                if (token === void 0) { token = null; }
                var __arg = {
                    id: id,
                    name: name,
                    note: note,
                };
                this.__loadModel('./Home/Edit', __arg, then, fail, token);
            };
            HomeController.__convert = function (text) {
                var dict = [];
                for (var _i = 0, dict_1 = dict; _i < dict_1.length; _i++) {
                    var d = dict_1[_i];
                    text = text.replace(d[0], d[1]);
                }
                return text;
            };
            HomeController.__loadModel = function (url, arg, then, fail, token) {
                var _this = this;
                if (token === void 0) { token = null; }
                var xhr = new XMLHttpRequest();
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4) {
                        var message = 'unkwon error.';
                        if (xhr.status == 200) {
                            try {
                                var jsonText = _this.__convert(xhr.responseText);
                                then(JSON.parse(jsonText));
                                return;
                            }
                            catch (ex) {
                                if (ex instanceof Error) {
                                    message = ex.message;
                                }
                            }
                        }
                        else {
                            fail(message);
                        }
                    }
                };
                xhr.open('POST', url, true);
                xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');
                xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
                if (token)
                    xhr.setRequestHeader('__RequestVerificationToken', token);
                xhr.send(JSON.stringify(arg));
            };
            return HomeController;
        }());
        Controllers.HomeController = HomeController;
    })(Controllers = SampleWebApplication.Controllers || (SampleWebApplication.Controllers = {}));
})(SampleWebApplication || (SampleWebApplication = {}));
//# sourceMappingURL=ajaxMethod.js.map