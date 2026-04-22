// ValidationHelper.js
(function ($) {

    function splitCamelCase(name) {
        if (!name) return "";
        if (name.indexOf('.') !== -1) {
            var parts = name.split('.');
            parts.shift();
            name = parts.join('.');
        }

        name = name.replace(/\./g, ' ');

        // Split camel case
        return name.replace(/([a-z])([A-Z])/g, '$1 $2');
    }

    function applyFriendlyValidation() {
        $("form [data-val-required]").each(function () {

            var $input = $(this);

            var name = $input.attr("name");

            var friendlyName = splitCamelCase(name);

            $input.attr(
                "data-val-required",
                "The " + friendlyName + " field is required."
            );

            if (name) {

                var label = $("label[for='" + $input.attr("id") + "']");

                if (label.length && label.find(".req-star").length === 0) {
                    label.append(' <span class="text-danger req-star">*</span>');
                }
            }
        });
        $("form").each(function () {
            $(this).removeData("validator").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(this);
        });
    }

    function requiredDropdown(hiddenId, comboId, message) {

        var value = $("#" + hiddenId).val();
        var combo = $("#" + comboId).data("kendoMultiColumnComboBox");

        var isValid =
            value !== null &&
            value !== "" &&
            value !== "0" &&
            value !== "00000000-0000-0000-0000-000000000000";

        let element = combo ? combo.wrapper : $("#" + comboId);

        if (!isValid) {

            element.addClass("k-combo-invalid");

            element.find(".k-dropdown-wrap")
                .css("border-bottom", "2px solid red");

            element.find(".k-multiselect-wrap")
                .css("border-bottom", "2px solid red");

            showFieldError(comboId, message);

            return false;
        }

        element.removeClass("k-combo-invalid");

        element.find(".k-dropdown-wrap").css("border-bottom", "");
        element.find(".k-multiselect-wrap").css("border-bottom", "");

        clearFieldError(comboId);

        return true;
    }




    function showFieldError(comboId, message) {

        let element = $("#" + comboId);

        element.next(".field-error").remove();

        element.after(`<span class="field-error text-danger">${message}</span>`);
    }

    function clearFieldError(comboId) {

        let element = $("#" + comboId);

        element.next(".field-error").remove();
    }


    function requiredDropdowns(list) {

        var isValid = true;

        list.forEach(x => {

            var value = $("#" + x.hiddenId).val();

            var combo = $("#" + x.comboId).data("kendoMultiColumnComboBox");

            var invalid = false;

            if (x.type === "multiselect") {
                invalid = !value || value.length === 0;
            } else {
                invalid =
                    value === null ||
                    value === "" ||
                    value === "0" ||
                    value === "00000000-0000-0000-0000-000000000000";
            }

            let element;

            if (x.type === "multiselect") {
                element = $("#" + x.comboId).closest(".k-multiselect");
            } else if (combo) {
                element = combo.wrapper;
            } else {
                element = $("#" + x.comboId);
            }
            if (!invalid) {

                element.removeClass("k-combo-invalid");
                element.find(".k-dropdown-wrap").css("border-bottom", "");
                element.find(".k-multiselect-wrap").css("border-bottom", "");

                clearFieldError(x.comboId);

                return;
            }
            isValid = false;

            element.addClass("k-combo-invalid");

            element.find(".k-dropdown-wrap")
                .css("border-bottom", "2px solid red");

            element.find(".k-multiselect-wrap")
                .css("border-bottom", "2px solid red");

            showFieldError(x.comboId, x.message);
        });

        return isValid;
    }

    window.ValidationHelper = {
        apply: applyFriendlyValidation,
        requiredDropdown: requiredDropdown,
        requiredDropdowns: requiredDropdowns
    };

})(jQuery);