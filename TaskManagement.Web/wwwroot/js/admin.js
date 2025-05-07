var Admin = {
    init: function () {
        Admin.bindTaskActions();
        Admin.bindUserActions();
    },

    bindTaskActions: function () {
        $(document).on("click", "#btnAddTask", function () {
            $("#formAddEditTask")[0].reset();
            $("#taskId").val(0);
            Common.openDrawer("#drawerAddEditTask");
        });


        //$("#btnLogin").on("click", function () {
        //    const email = $("#txtEmail").val().trim();
        //    const password = $("#txtPassword").val().trim();

        //    if (!email || !password) {
        //        alert("Email and Password are required.");
        //        return;
        //    }

        //    Common.showLoader();

        //    $.ajax({
        //        url: Url.ApiBase + Url.Login,
        //        type: "POST",
        //        contentType: "application/json",
        //        data: JSON.stringify({ Email: email, Password: password }),
        //        success: function () {
        //            Common.hideLoader();
        //            window.location.href = Url.HomePage;
        //        },
        //        error: function () {
        //            Common.hideLoader();
        //            alert("Invalid credentials.");
        //        }
        //    });
        //});

        $(document).on("click", "#btnSaveTask", function (e) {
            e.preventDefault();
            const model = {
                Id: parseInt($("#taskId").val()),
                Title: $("#txtTitle").val().trim(),
                Description: $("#txtDescription").val().trim(),
                AssigneeId: $("#ddlAssigneeId").val(),
                Priority: $("#ddlPriority").val(),
                Status: $("#ddlStatus").val(),
                DueDate: $("#txtDueDate").val()
            };

            const url = model.Id > 0 ? "/Admin/Update" : "/Admin/Create";

            $.ajax({
                url: url,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(model),
                success: function () {
                    Common.closeDrawer("#drawerAddEditTask");
                    alert("Task saved successfully.");
                    Admin.loadTaskList();
                },
                error: function () {
                    alert("Error while saving task.");
                }
            });
        });
    },

    loadTaskList: function () {
        $.ajax({
            url: "/Admin/TaskList",
            type: "GET",
            success: function (html) {
                $("#taskListContainer").html(html);
            }
        });
    },


    loadAssignees: function () {
        $.ajax({
            url: "/Admin/GetAllUsers",
            type: "GET",
            success: function (html) {
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, 'text/html');
                const users = $(doc).find(".alerts-content-single");

                const $ddl = $("#ddlAssigneeId");
                $ddl.empty().append('<option value="">Select Assignee</option>');

                users.each(function () {
                    const id = $(this).find(".link").first().attr("onclick").match(/\d+/)[0];
                    const name = $(this).find(".content-title span").text().trim();
                    $ddl.append(`<option value="${id}">${name}</option>`);
                });
            },
            error: function () {
                alert("Failed to load users.");
            }
        });
    },



    bindUserActions: function () {
        $(document).on("click", "#btnAddUser", function () {
            $("#formAddEditUser")[0].reset();
            $("#userId").val(0);
            loadAssignees();
            Common.openDrawer("#drawerAddUser");
        });

        $(document).on("click", "#btnSaveUser", function (e) {
            e.preventDefault();
            const model = {
                Id: parseInt($("#userId").val()),
                UserName: $("#txtUserName").val().trim(),
                Email: $("#txtEmail").val().trim(),
                FirstName: $("#txtFirstName").val().trim(),
                LastName: $("#txtLastName").val().trim(),
                PhoneNumber: $("#txtPhoneNumber").val().trim(),
                IsActive: $("#ddlStatus").val() === "true"
            };

            const url = model.Id > 0 ? "/Admin/UpdateUser" : "/Admin/CreateUser";

            $.ajax({
                url: url,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(model),
                success: function () {
                    Common.closeDrawer("#drawerAddUser");
                    alert("User saved successfully.");
                    Admin.loadUserList();
                },
                error: function () {
                    alert("Error while saving user.");
                }
            });
        });
    },

    loadUserList: function () {
        $.ajax({
            url: "/Admin/GetAllUsers",
            type: "GET",
            success: function (html) {
                $("#divUserList").html(html);
            }
        });
    },

    Edit: function (id, isUser) {
        if (isUser) {
            $.ajax({
                url: `/Admin/EditUser/${id}`,
                type: "GET",
                success: function (html) {
                    $("#drawerAddUser").html(html);
                    Common.openDrawer("#drawerAddUser");
                }
            });
        } else {
            $.ajax({
                url: `/Admin/Edit/${id}`,
                type: "GET",
                success: function (html) {
                    $("#drawerAddEditTask").html(html);
                    Common.openDrawer("#drawerAddEditTask");
                }
            });
        }
    },

    View: function (id, isUser) {
        if (isUser) {
            $.ajax({
                url: `/Admin/ViewUser/${id}`,
                type: "GET",
                success: function (html) {
                    $("#drawerViewUser").html(html);
                    Common.openDrawer("#drawerViewUser");
                }
            });
        } else {
            $.ajax({
                url: `/Admin/ViewTask/${id}`,
                type: "GET",
                success: function (html) {
                    $("#drawerViewTask").html(html);
                    Common.openDrawer("#drawerViewTask");
                }
            });
        }
    },

    Delete: function (id, isUser) {
        const confirmation = confirm(`Are you sure you want to delete this ${isUser ? "user" : "task"}?`);
        if (!confirmation) return;

        const url = isUser ? `/Admin/DeleteUser/${id}` : `/Admin/Delete/${id}`;

        $.ajax({
            url: url,
            type: "POST",
            success: function () {
                alert(`${isUser ? "User" : "Task"} deleted successfully.`);
                isUser ? Admin.loadUserList() : Admin.loadTaskList();
            },
            error: function () {
                alert("Error deleting record.");
            }
        });
    },

    ShowEditOption: function (el) {
        $(el).next(".edit-name").show();
    },

    CloseEditOption: function (el) {
        $(el).closest(".edit-name").hide();
    },

    openTaskDrawer: function () {
        //$("#formAddEditTask")[0].reset();
        //$("#taskId").val(0);
        Admin.loadAssignees();
        Common.openDrawer("#drawerAddEditTask");
    },

    openTaskHistoryDrawer: function (id) {
        $.ajax({
            url: `/Admin/History/${id}`,
            type: "GET",
            success: function (html) {
                $("#drawerTaskHistory").html(html);
                Common.openDrawer("#drawerTaskHistory");
            }
        });
    }
};

$(document).ready(function () {
    Admin.init();
});
