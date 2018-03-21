/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="./ajaxMethod.ts" />


import model = SampleWebApplication.Models.SampleModel;
import controller = SampleWebApplication.Controllers.HomeController;

let _tableId = "";

function init(tableId: string, models: model[]) {
    _tableId = tableId;
    listup(models);

    $("#SampleModel-save").click(function () {
        if ($("#SampleModel-dialog form").valid()) {
            $('#SampleModel-dialog').modal('hide');

            let id = Number($("#SampleModel-dialog input[name='Id']").val());
            let name = $("#SampleModel-dialog input[name='Name']").val();
            let note = $("#SampleModel-dialog input[name='Note']").val();

            if (id < 1) {
                controller.Add(name, note, listup, (message: string) => { alert(message); });
            }
            else {
                controller.Edit(id, name, note, listup, (message: string) => { alert(message); });
            }
        }
    });

    $("#SampleModel-remove").click(function () {
        if ($("#SampleModel-dialog form").valid()) {
            $('#SampleModel-dialog').modal('hide');

            let id = Number($("#SampleModel-dialog input[name='Id']").val());
            if (id > 0) {
                controller.Remove(id, listup, (message: string) => { alert(message); });
            }
        }
    });

}


function listup(models: model[]) {
    let table = document.getElementById(_tableId) as HTMLTableElement;

    if (!table)
        throw new Error("tableId is disable.");

    $(table).children().remove();

    $("#SampleModel-dialog input[name='Id']").val(0);
    $("#SampleModel-dialog input[name='Name']").val("");
    $("#SampleModel-dialog input[name='Note']").val("");

    let row: HTMLTableRowElement = null;
    let th: HTMLTableHeaderCellElement = null;
    let td: HTMLTableCellElement = null;

    row = table.insertRow(-1);

    th = document.createElement('th');
    row.appendChild(th);
    th.innerText = "Name";

    th = document.createElement('th');
    row.appendChild(th);
    th.innerText = "Note";

    if (!models || models.length < 1)
        return;

    for (let item of models) {
        row = table.insertRow(-1);
        row.dataset["id"] = item.Id.toString();

        td = row.insertCell(-1);
        td.innerText = item.Name.toString();

        td = row.insertCell(-1);
        td.innerText = item.Note.toString();

        row.onclick = (ev: MouseEvent) => {
            let row = ev.currentTarget as HTMLTableRowElement;
            let id = row.dataset["id"];
            let name = row.cells[0].innerText;
            let note = row.cells[1].innerText;

            $("#SampleModel-dialog input[name='Id']").val(id);
            $("#SampleModel-dialog input[name='Name']").val(name);
            $("#SampleModel-dialog input[name='Note']").val(note);

            $('#SampleModel-dialog').modal();
        };
    }

}
