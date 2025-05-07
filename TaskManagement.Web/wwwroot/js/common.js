var Common = {
    init: function () {
        $("#divFilters").hide();
        $("#divSorting").hide();
        $(".overlay").hide();
        //$('.select2').select2();

        //Common.bindLogin();
    },

    bindLogin: function () {
        $("#btnLogin").on("click", function (e) {
            e.preventDefault();
            console.log("Login button clicked");
            const email = $("#txtEmail").val().trim();
            const password = $("#txtPassword").val().trim();

            if (!email || !password) {
                alert("Email and Password are required.");
                return;
            }

            Common.showLoader();

            $.ajax({
                url: Url.ApiBase + Url.Login,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ Email: email, Password: password }),
                success: function () {
                    Common.hideLoader();
                    //window.location.href = Url.HomePage;
                },
                error: function () {
                    Common.hideLoader();
                    alert("Invalid credentials.");
                }
            });
        });
    },

    openDrawer: function (selector) {
        Common.showLoader();
        $(`${selector} .drawer-inner`).animate({ right: "0" });
        $("body").addClass("overflow-hidden");
        $(".overlay", self).css('display', 'block');
        Common.hideLoader();
    },

    closeDrawer: function (selector) {
        Common.showLoader();
        $(`${selector} .drawer-inner`).animate({ right: "-100%" });
        $("body").removeClass("overflow-hidden");
        $(".overlay", self).css('display', 'none');
        Common.hideLoader();
        try {
            Admin.loadTaskList();
        }
        catch {
            Admin.loadUserList();
        }

    },

    showLoader: function () {
        $(".loading-wrapper").show();
    },

    hideLoader: function () {
        $(".loading-wrapper").hide();
    },

    showFilter: function (trigger, target) {
        $(target).toggle();
        $(target).on("click", function (e) {
            e.stopPropagation();
        });
        $(trigger).toggleClass("active");
    },

    hideFilter: function (target) {
        $(target).hide();
        $(".nt-sort, .nt-filter").removeClass("active");
    },

    ShowEditOption: function (element) {
        $(element).next(".edit-name").show();
    },

    CloseEditOption: function (element) {
        $(element).closest(".edit-name").hide();
    }
};

$(document).ready(function () {
    Common.init();
});
