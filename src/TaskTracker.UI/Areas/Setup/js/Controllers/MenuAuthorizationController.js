var MenuAuthorizationController = (function (CommonAjaxService) {
    var IsRoleMenuView = $("#IsRoleMenuView").val();
    var init = function () {
        loadRoleIndexGrid();
        var $table = $('#MenuAccessLists');


        function updateRowMainCheckbox($row) {
            var $mainCheckbox = $row.find('.mainCheckbox');
            var $ops = $row.find('input[type="checkbox"]').not('.mainCheckbox');

            var total = $ops.length;
            var checkedCount = $ops.filter(':checked').length;

            if (checkedCount === 0) {
                $mainCheckbox.prop('checked', false).prop('indeterminate', false);
            } else if (checkedCount === total) {
                $mainCheckbox.prop('checked', true).prop('indeterminate', false);
            } else {
                $mainCheckbox.prop('checked', false).prop('indeterminate', true);
            }
        }

        function updateHeaderCheckbox() {
            var $allRows = $('#MenuAccessLists .mainCheckbox');
            var $header = $('#MenuAccessLists .chkAll');

            var total = $allRows.length;
            var checkedCount = $allRows.filter(':checked').length;
            var indeterminateCount = $allRows.filter(function () { return this.indeterminate; }).length;

            if (checkedCount === total) {
                $header.prop('checked', true).prop('indeterminate', false);
            } else if (checkedCount === 0 && indeterminateCount === 0) {
                $header.prop('checked', false).prop('indeterminate', false);
            } else {
                $header.prop('checked', false).prop('indeterminate', true);
            }
        }

        $('#MenuAccessLists').on('change', '.chkAll', function () {
            var isChecked = $(this).is(':checked');
            $('#MenuAccessLists .mainCheckbox').prop('checked', isChecked).prop('indeterminate', false).trigger('change');
        });

        $('#MenuAccessLists').on('change', '.mainCheckbox', function () {
            var $row = $(this).closest('tr');
            var isChecked = $(this).is(':checked');
            $row.find('input[type="checkbox"]').not('.mainCheckbox').prop('checked', isChecked);
            $(this).prop('indeterminate', false);

            updateHeaderCheckbox();
        });

        $('#MenuAccessLists').on('change', 'input[type="checkbox"]:not(.mainCheckbox, .chkAll)', function () {
            var $row = $(this).closest('tr');
            updateRowMainCheckbox($row);
            updateHeaderCheckbox();
        });

        $('#MenuAccessLists tbody tr').each(function () {
            updateRowMainCheckbox($(this));
        });
        updateHeaderCheckbox();




        $('#btnSave').on('click', function () {
            console.log("Save button clicked");
            var getId = $('#Id').val();
            var status = "Save";
            if (parseInt(getId) > 0) {
                status = "Update";
            }

            Confirmation("Are you sure? Do You Want to " + status + " Data?",
                function (result) {
                    if (result) {
                        save();
                    }
                });
        });

        $('#roleMenuSave').on('click', function () {


            Confirmation("Are you sure? Do You Want to " + "set " + " Data?",
                function (result) {
                    if (result) {
                        RoleMenuSave($table);
                    }
                });
        });
    };

    function loadRoleIndexGrid() {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '/Setup/MenuAuthorization/RoleIndex',
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json"
                },
                parameterMap: function (options) {
                    return kendo.stringify(options);
                }
            },
            schema: {
                data: "items",
                total: "totalCount",
                model: {
                    id: "id",
                    fields: { id: { type: "number" }, name: { type: "string" } }
                }
            },
            error: function (e) {
                console.error("Grid Load Error:", e);
            }
        });

        $("#RoleIndexDataList").kendoGrid({
            dataSource: gridDataSource,
            pageable: { refresh: true, pageSizes: [10, 20, 50, "all"] },
            sortable: true,
            filterable: true,
            resizable: true,
            toolbar: ["excel", "pdf"],
            columns: [
                {
                    title: "Action",
                    width: 100,
                    template: function (dataItem) {
                        return IsRoleMenuView ? `<a href="/SetUp/MenuAuthorization/RoleMenuEdit/${dataItem.id}?roleName=${encodeURIComponent(dataItem.name)}" class="btn btn-primary btn-sm"><i class="fas fa-pencil-alt"></i></a>` : `<a href="/SetUp/MenuAuthorization/Edit/${dataItem.id}" class="btn btn-primary btn-sm"><i class="fas fa-pencil-alt"></i></a>`;
                    }
                },
                { field: "id", hidden: true },
                { field: "name", title: "Role Name" }
            ]
        });
    }



    function save() {
        var operation = $("#Operation").val();
        console.log(operation)
        CommonAjaxService.saveData(
            "/SetUp/MenuAuthorization/UpsertRole",
            "#frm_role",
            function (res) { CommonAjaxService.handleSaveResponse(res, "#frm_role", operation); },
            function (err) { toastr.error("Failed to save."); }
        );
    }
    function RoleMenuSave() {
        var operation = $("#Operation").val();
        console.log(operation)
        CommonAjaxService.saveData(
            "/SetUp/MenuAuthorization/UpsertRoleMenu",
            "#frm_RoleMenu",
            function (res) { CommonAjaxService.handleSaveResponse(res, "#frm_RoleMenu", operation); },
            function (err) { toastr.error("Failed to save."); }
        );
    }



    function RoleMenuSave($table) {

        var validator = $("#frm_RoleMenu").validate();
        var model = serializeInputs("frm_RoleMenu");

        var details = serializeTablesData($table);


        var isCheckedOverall = details.some(item => item.IsChecked === true);
        if (isCheckedOverall === false) {
            ShowNotification(3, "Please Select CheckBox First!");
            return;
        }


        var result = validator.form();
        if (!result) {
            validator.focusInvalid();
            return;
        }

        model.RoleMenuList = details;
        CommonAjaxService.saveJson(
            "/SetUp/MenuAuthorization/UpsertRoleMenu",
            model,

            function () {
                toastr.error("Failed to save.");
            }
        );
    };

    function serializeTablesData($table) {
        var data = [];
        $table.find('tbody tr').each(function () {
            var row = {};
            //$(this).find('input[type="checkbox"], input[type="text"], select').each(function () {
            //    if (this.type === 'checkbox') {
            //        row[$(this).attr('name')] = this.checked;
            //    } else {
            //        row[$(this).attr('name')] = $(this).val();
            //    }
            //});
            var menuIdValue = $(this).find('td:nth-child(3)').text().trim();
            row['MenuId'] = menuIdValue;


            var mainCheckbox = $(this).find('.mainCheckbox');
        row['IsChecked'] = mainCheckbox.is(':checked') || mainCheckbox.prop('indeterminate');


            // Loop only operation checkboxes
            $(this).find('input[type="checkbox"]').not('.mainCheckbox').each(function () {
                var name = $(this).attr('name'); // should be "List", "Insert", "Delete"
                row[name] = $(this).is(':checked');
            });

            data.push(row);
        });

        return data;
    };

    return { init: init };
})(CommonAjaxService);
