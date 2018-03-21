/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="./ajaxMethod.ts" />
var model = SampleWebApplication.Models.SampleModel;
var controller = SampleWebApplication.Controllers.HomeController;
var _tableId = "";
function init(tableId, models) {
    _tableId = tableId;
    listup(models);
    $("#SampleModel-save").click(function () {
        if ($("#SampleModel-dialog form").valid()) {
            $('#SampleModel-dialog').modal('hide');
            var id = Number($("#SampleModel-dialog input[name='Id']").val());
            var name_1 = $("#SampleModel-dialog input[name='Name']").val();
            var note = $("#SampleModel-dialog input[name='Note']").val();
            if (id < 1) {
                controller.Add(name_1, note, listup, function (message) { alert(message); });
            }
            else {
                controller.Edit(id, name_1, note, listup, function (message) { alert(message); });
            }
        }
    });
    $("#SampleModel-remove").click(function () {
        if ($("#SampleModel-dialog form").valid()) {
            $('#SampleModel-dialog').modal('hide');
            var id = Number($("#SampleModel-dialog input[name='Id']").val());
            if (id > 0) {
                controller.Remove(id, listup, function (message) { alert(message); });
            }
        }
    });
}
function listup(models) {
    var table = document.getElementById(_tableId);
    if (!table)
        throw new Error("tableId is disable.");
    $(table).children().remove();
    $("#SampleModel-dialog input[name='Id']").val(0);
    $("#SampleModel-dialog input[name='Name']").val("");
    $("#SampleModel-dialog input[name='Note']").val("");
    var row = null;
    var th = null;
    var td = null;
    row = table.insertRow(-1);
    th = document.createElement('th');
    row.appendChild(th);
    th.innerText = "Name";
    th = document.createElement('th');
    row.appendChild(th);
    th.innerText = "Note";
    if (!models || models.length < 1)
        return;
    for (var _i = 0, models_1 = models; _i < models_1.length; _i++) {
        var item = models_1[_i];
        row = table.insertRow(-1);
        row.dataset["id"] = item.Id.toString();
        td = row.insertCell(-1);
        td.innerText = item.Name.toString();
        td = row.insertCell(-1);
        td.innerText = item.Note.toString();
        row.onclick = function (ev) {
            var row = ev.currentTarget;
            var id = row.dataset["id"];
            var name = row.cells[0].innerText;
            var note = row.cells[1].innerText;
            $("#SampleModel-dialog input[name='Id']").val(id);
            $("#SampleModel-dialog input[name='Name']").val(name);
            $("#SampleModel-dialog input[name='Note']").val(note);
            $('#SampleModel-dialog').modal();
        };
    }
}
//# sourceMappingURL=index.js.map