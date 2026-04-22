var TaskController = (function (CommonAjaxService) {

    var init = function () {
        loadGrid();

        $('#btnSave').on('click', function (e) {
            e.preventDefault();

            var getId = $('#Id').val();
            var status = "Save";

            if (parseInt(getId) > 0) {
                status = "Update";
            }

            var form = $("#Task_Form");

            if (form.valid()) {
                Confirmation("Are you sure? Do You Want to " + status + " Data?",
                    function (result) {
                        if (result) {
                            save(status);
                        }
                    });
            } else {
                toastr.warning("Please fill in all required fields correctly before saving.");
            }
        });

        $('#btnDelete').on('click', function () {
            deleteSelected();
        });
    };

    function loadGrid() {
        CommonAjaxService.initKendoGrid({
            gridId: "GridData",
            url: "/Setup/Task/GetGridData",
            fileName: "TaskList",
            searchFields: ["title", "description", "dueDate", "isCompleted"],
            aliasMap: {
                id: "Id",
                title: "Title",
                description: "Description",
                dueDate: "DueDate",
                isCompleted: "IsCompleted"
            },
            columns: [
                { selectable: true, width: 30 },
                {
                    title: "Action",
                    width: 100,
                    template: dataItem => `
                        <a href="/Setup/Task/Upsert/${dataItem.id}" class="btn btn-primary btn-sm mr-2" title="Edit">
                            <i class="fas fa-pencil-alt"></i>
                        </a>
                        <a href="#" class="btn btn-danger btn-sm" title="Delete" onclick="TaskController.deleteSelected('${dataItem.id}')">
                            <i class="fas fa-trash"></i>
                        </a>`
                },
                { field: "id", hidden: true },

                {
                    field: "title",
                    title: "Title",
                    width: 150
                },
                {
                    field: "description",
                    title: "Description",
                    width: 200
                },
                {
                    field: "dueDate",
                    title: "Due Date",
                    template: "#= kendo.toString(kendo.parseDate(dueDate), 'yyyy-MM-dd') #",
                    width: 150
                },
                {
                    field: "isCompleted",
                    title: "Status",
                    template: function (dataItem) {
                        return dataItem.isCompleted
                            ? '<span class="badge bg-success">Completed</span>'
                            : '<span class="badge bg-warning">Pending</span>';
                    },
                    width: 120
                }
            ]
        });
    }
    function save(status) {
        CommonAjaxService.saveData(
            "/Setup/Task/Save",
            "#Task_Form",
            function (res) {
                CommonAjaxService.handleSaveResponse(res, "#Task_Form", status);
            },
            function (err) {
                toastr.error("Failed to save task.");
            }
        );
    }

    function deleteSelected(id = null) {
        CommonAjaxService.SelectData(
            "/Setup/Task/Delete",
            "GridData",
            function (res) {
                CommonAjaxService.handleDeleteResponse(res, "GridData");
            },
            function (err) {
                toastr.error("Failed to delete task.");
            },
            id
        );
    }

    return {
        init: init,
        deleteSelected: deleteSelected
    };

})(CommonAjaxService);