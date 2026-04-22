var CommonAjaxService = (function () {

    var baseApiUrl = "https://localhost:7025";

    function ajaxCall(options) {
        return $.ajax({
            url: options.url,
            method: options.method || 'POST',
            data: options.data || {},
            processData: options.processData !== false,
            contentType: options.contentType !== false ? options.contentType || 'application/x-www-form-urlencoded; charset=UTF-8' : false,
            timeout: options.timeout || 60000,
        })
            .done(options.done || function (res) { console.log("Success:", res); })
            .fail(options.fail || function (err) { console.error("Ajax Error:", err); ShowNotification(3, "Server request failed!"); });
    }

    // Generic save (with/without file)
    function saveData(url, formSelector, done, fail, hasFile = false) {
        var formData = hasFile ? new FormData($(formSelector)[0]) : $(formSelector).serialize();
        ajaxCall({
            url: url,
            data: formData,
            processData: !hasFile ? true : false,
            contentType: !hasFile ? 'application/x-www-form-urlencoded; charset=UTF-8' : false,
            done: done,
            fail: fail
        });
    }


    // ✅ NEW: Save JSON Model (for complex objects / grids)
    function saveJson(url, model, fail) {
        $.ajax({
            url: url,
            method: 'POST',
            data: model

        })
            .done(function (res) {
                // centralized success handler
                toastr.success("Saved successfully!");
                console.log("Server response:", res);

                if (model.Operation) {
                    console.log("Operation:", model.Operation);
                }
            })
            .fail(fail || function (err) {
                toastr.error("Failed to save.");
                console.error(err);
            });
    }

    function SelectData(url, gridId, done, fail, singleId = null) {
        var IDs = [];

        // ✅ If a single id is passed (for row-level delete)
        if (singleId) {
            IDs.push(singleId);
        } else {
            var grid = $("#" + gridId).data("kendoGrid");
            var selectedRows = grid.select();

            if (selectedRows.length === 0) {
                ShowNotification(3, "Please select at least one record!");
                return;
            }

            selectedRows.each(function () {
                var dataItem = grid.dataItem(this);
                IDs.push(dataItem.id);
            });
        }

        // ✅ Confirmation before deleting
        Confirmation("Are you sure? Do you want to delete the selected data?", function (result) {
            if (result) {
                ajaxCall({
                    url: url,
                    data: { IDs: IDs },
                    done: done,
                    fail: fail
                });
            }
        });
    }





    function handleSaveResponse(response, formSelector, status) {
        if (response.id && response.id !== "00000000-0000-0000-0000-000000000000") {
            toastr.success(response.message || "Saved successfully!");
            if (status.toLowerCase() === "save") $(formSelector)[0].reset();
        } else {
            toastr.warning(response.message || "Something went wrong!");
        }
    }
    function handleDeleteResponse(result, gridId) {
        var grid = $("#" + gridId).data("kendoGrid");
        if (grid) grid.dataSource.read();

        var status = result.Status || result.status;
        var message = result.Message || result.message || "No message";

        if (status?.toLowerCase() === "success") {
            ShowNotification(1, message);
        } else if (status == 400) {
            ShowNotification(3, message);
        } else {
            ShowNotification(2, message);
        }
    }

    function getImageUrl(imagePath) {
        if (!imagePath) return "";
        return `${baseApiUrl}${imagePath.startsWith("/") ? "" : "/"}${imagePath}`;
    }


    function initImagePreview(imgId, fileInputId, existingImagePath, defaultImagePath) {
        const img = $("#" + imgId);
        const input = $("#" + fileInputId);

        // Set initial image
        if (existingImagePath) {
            img.attr("src", `${baseApiUrl}${existingImagePath.startsWith("/") ? "" : "/"}${existingImagePath}`);
        } else {
            img.attr("src", defaultImagePath);
        }

        // On file change, show preview
        input.on("change", function (e) {
            const file = e.target.files[0];
            if (!file) return;

            const reader = new FileReader();
            reader.onload = function (e) {
                img.attr("src", e.target.result);
            };
            reader.readAsDataURL(file);
        });
    }


    function initKendoGrid(options) {
        const gridId = options.gridId || "GridData";
        const url = options.url;
        const columns = options.columns || [];
        const fileName = options.fileName || "GridList";
        const searchFields = options.searchFields || [];
        const aliasMap = options.aliasMap || {};
        const extraFilter = options.extraFilter || null;

        const gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            autoSync: true,
            pageSize: 10,
            transport: {
                read: {
                    url: url,
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    cache: false,
                    complete: function (xhr) {
                        try {
                            var response = JSON.parse(xhr.responseText);
                            console.log(response); // <-- fixed typo
                            if (response.redirect) {
                                window.location.href = response.redirect;
                            }
                        } catch (e) {
                            // Not JSON or normal data
                        }
                    }
                },
                parameterMap: function (options) {

                    const allowedFields = searchFields || [];

                    if (options.sort) {
                        options.sort.forEach(function (param) {
                            if (allowedFields.includes(param.field) && aliasMap[param.field]) {
                                param.field = aliasMap[param.field];
                            }
                        });
                    }

                    if (options.filter && options.filter.filters) {

                        options.filter.filters = options.filter.filters.filter(f =>
                            allowedFields.includes(f.field)
                        );

                        options.filter.filters.forEach(function (param) {
                            if (aliasMap[param.field]) {
                                param.field = aliasMap[param.field];
                            }
                        });
                    }

                    if (extraFilter) {
                        options.extraFilter = extraFilter;
                    }

                    return kendo.stringify(options);
                }


            },
            schema: {
                data: "items",
                total: "totalCount",
                model: {
                    id: "id"
                }
            },
            error: function (e) {
                console.error(`${fileName} Grid Load Error:`, e);
                ShowNotification(3, "Grid data load failed!");
            }
        });

        $("#" + gridId).kendoGrid({
            dataSource: gridDataSource,
            pageable: {
                refresh: true,
                pageSizes: [10, 20, 50, "all"]
            },
            noRecords: true,
            messages: { noRecords: "No Record Found!" },
            scrollable: true,
            filterable: {
                extra: true,
                operators: {
                    string: {
                        startswith: "Starts with",
                        contains: "Contains",
                        doesnotcontain: "Does not contain",
                        eq: "Is equal to",
                        neq: "Is not equal to"
                    }
                }
            },
            sortable: true,
            resizable: true,
            reorderable: true,
            groupable: true,
            toolbar: ["excel", "pdf", "search"],
            search: searchFields,
            excel: {
                fileName: `${fileName}.xlsx`,
                filterable: true
            },
            pdf: {
                fileName: `${fileName}_${new Date().toISOString().split('T')[0]}_${new Date().getMilliseconds()}.pdf`,
                allPages: true,
                avoidLinks: true,
                paperSize: "A3",
                landscape: true,
                margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" }
            },
            pdfExport: function (e) {
                $(".k-grid-toolbar").hide();
                $(".k-grouping-header").hide();
                $(".k-floatwrap").hide();
                const grid = e.sender;
                const actionColumnIndex = grid.columns.findIndex(col => col.title === "Action");
                if (actionColumnIndex >= 0) grid.hideColumn(actionColumnIndex);
                setTimeout(() => window.location.reload(), 1000);
            },
            columns: columns,
            editable: false,
            selectable: "multiple row",
            navigatable: true,
            columnMenu: true
        });
    }


    function generateColumns(data, columnMap = {}, hiddenFields = []) {

        if (!Array.isArray(data) || data.length === 0) {
            return [{
                field: "name",
                title: columnMap.name || "Name",
                width: 200
            }];
        }

        const sample = data[0];
        const hidden = new Set(hiddenFields);

        const valueFields = [
            "valueOne", "valueTwo", "valueThree",
            "valueFour", "valueFive", "valueSix", "valueSeven"
        ];

        const columns = [
            {
                field: "name",
                title: columnMap.name || sample?.nameLabel || "Name",
                width: 200
            }
        ];

        for (const field of valueFields) {

            if (hidden.has(field)) continue;

            const value = sample?.[field];
            if (value == null) continue;

            columns.push({
                field,
                title: columnMap[field] || sample?.[field + "Label"] || field,
                width: 120
            });
        }

        return columns;
    }


    function loadMultiColumnCombo(config) {

        const $el = $("#" + config.comboId);

        const existing = $el.data("kendoMultiColumnComboBox");
        if (existing) {
            existing.destroy();
            $el.empty();
        }

        let raw = [];

        try {
            raw = JSON.parse($(config.dataHolder).attr("data-json") || "[]");
        } catch (e) {
            console.error("Invalid JSON in dataHolder", e);
        }

        const data = config.dataKey ? raw?.[config.dataKey] : raw;

        if (!Array.isArray(data)) return null;

        const combo = $el.kendoMultiColumnComboBox({
            dataTextField: config.textField || "name",
            dataValueField: config.valueField || "id",
            filter: "contains",
            autoBind: true,
            valuePrimitive: true,
            dataSource: data,

            columns: config.columns ||
                generateColumns(data, config.columnMap, config.hiddenFields),

            placeholder: config.placeholder || "Select",

            change: function () {
                const value = this.value();
                const item = this.dataItem();

                $(config.hiddenId).val(value);

                config.onChange?.(value, item);
            }
        }).data("kendoMultiColumnComboBox");

        const hiddenVal = $(config.hiddenId).val();
        if (hiddenVal && hiddenVal !== "0" && hiddenVal !== "00000000-0000-0000-0000-000000000000") {
            combo.value(hiddenVal);
        }

        $el.closest(".k-multicolumncombobox")
            .css("width", "100%");

        return combo;
    }



    function loadMultiSelectCombo(config) {

        const $el = $("#" + config.comboId);

        const existing = $el.data("kendoMultiSelect");
        if (existing) {
            existing.destroy();
            $el.empty();
        }


        let raw = [];

        try {
            raw = JSON.parse($(config.dataHolder).attr("data-json") || "[]");
        } catch (e) {
            console.error("Invalid JSON in dataHolder", e);
        }

        const data = config.dataKey ? raw?.[config.dataKey] : raw;

        if (!Array.isArray(data)) return null;

        const selectedValues =
            JSON.parse($("#dataHolder").attr("data-selected") || "[]")
                .map(x => x.toString());

        const combo = $el.kendoMultiSelect({
            dataTextField: config.textField || "name",
            dataValueField: config.valueField || "id",
            filter: "contains",
            autoBind: true,
            valuePrimitive: true,
            dataSource: data,
            placeholder: config.placeholder || "Select",

            dataBound: function () {
                if (selectedValues.length > 0) {
                    this.value(selectedValues);
                }
            },

            change: function () {
                const value = this.value();
                config.onChange?.(value);
            }
        }).data("kendoMultiSelect");

        $el.closest(".k-multiselect")
            .css("width", "100%");

        return combo;
    }

    return {
        saveData,
        saveJson,
        SelectData,
        handleSaveResponse,
        handleDeleteResponse,
        ajaxCall,
        getImageUrl,
        initImagePreview,
        initKendoGrid,
        loadMultiColumnCombo,
        loadMultiSelectCombo,
        generateColumns
    };

})();

