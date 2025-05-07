var Admin = {
    _sortColumn: null,
_sortOrder: "asc",
_currentPage: 1,
_hasMore: true,


    init: function () {
        Admin.bindTaskActions();
        Admin.bindUserActions();
        $(".ass-des li").on("click", function () {
    $(".ass-des li").removeClass("active");
    $(this).addClass("active");
});

    },

    // Filtering and Sorting Section
    applyFilter: function () {
        Admin._currentPage = 1;
    Admin._hasMore = true;
    const model = Admin.getFilterModel();
    model.pageNumber = Admin._currentPage;

    $.post("/Admin/Filter", model, function (html) {
        $("#taskListContainer").html(html);
        Common.hideFilter("#divFilters");
    }).fail(() => alert("Failed to apply filters."));
},
getFilterModel: function () {
    return {
        searchTerm: $("#txtSearchTerm").val().trim() || null,
        status: $("#ddlFilterStatus").val() || null,
        priority: $("#ddlFilterPriority").val() || null,
        dueBefore: $("#txtDueBefore").val() || null,
        sortColumn: Admin._sortColumn,
        sortOrder: Admin._sortOrder,
        pageNumber: Admin._currentPage,
        pageSize: 10
    };
}
,

applySort: function () {
    const selectedColumn = $("input[name='sortColumn']:checked").val();
    const order = $(".ass-des li.active").data("order");

    Admin._sortColumn = selectedColumn;
    Admin._sortOrder = order;

    Admin.applyFilter(); // reuse filter with new sort
    Common.hideFilter("#divSorting");
},

resetFilter: function () {
    $("#filterForm")[0].reset();
    $("#ddlStatus, #ddlPriority").val(null).trigger("change");
    Admin.applyFilter();
},

resetSort: function () {
    $("input[name='sortColumn']").prop("checked", false);
    $(".ass-des li").removeClass("active").first().addClass("active");
    Admin._sortColumn = null;
    Admin._sortOrder = "asc";
    Admin.applyFilter();
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

        //$(document).on("click", "#btnSaveTask", function (e) {
        //    e.preventDefault();
        //    var id = parseInt($("#taskId").val());
        //    const model = {
        //        title: $("#txtTitle").val().trim(),
        //        description: $("#txtDescription").val().trim(),
        //        assigneeId: $("#ddlAssigneeId").val(),
        //        priority: $("#ddlPriority").val(),
        //        status: $("#ddlStatus").val(),
        //        dueDate: $("#txtDueDate").val()
        //    };

        //    const url = id > 0 ? "/Admin/Update" : "/Admin/Create";

        //    $.ajax({
        //        url: url,
        //        type: 'POST',
        //        contentType: 'application/json',
        //        data: model,
        //        success: function () {
        //            Common.closeDrawer("#drawerAddEditTask");
        //            alert("Task saved successfully.");
        //            Admin.loadTaskList();
        //        },
        //        error: function () {
        //            alert("Error while saving task.");
        //        }
        //    });
        //});
        $(document).on("click", "#btnSaveTask", function (e) {
            e.preventDefault();

            const id = parseInt($("#taskId").val());
            const isUpdate = id > 0;
            const url = isUpdate ? `/Admin/Update` : "/Admin/Create";
            const formData = {
                Title: $("#txtTitle").val().trim(),
                Description: $("#txtDescription").val().trim(),
                AssigneeId: $("#ddlAssigneeId").val(),
                Priority: $("#ddlPriority").val(),
                Status: $("#ddlStatus").val(),
                DueDate: $("#txtDueDate").val()
            };
            if (isUpdate) {
                    formData.Id = id;
                }
            $.post(url, formData, function () {
                Common.closeDrawer("#drawerAddEditTask");
                alert("Task saved successfully.");
                Admin.loadTaskList();

            }).fail(function (xhr) {
                const msg = xhr.responseJSON?.Message || "Error while saving task.";
                alert(msg);
            });
        });



    },

    //loadTaskList: function () {
    //    $.ajax({
    //        url: "/Admin/Filter",
    //        type: "POST",
    //        success: function (html) {
    //            $("#taskListContainer").html(html);
    //        }
    //    });
    //    //url = URL.ApiBase + URL.GetAllTasks;

    //},
    loadTaskList: function () {
        const model = {
            searchTerm: null,
            status: null,
            priority: null,
            sortColumn: null,
            sortOrder: null,
            pageNumber: 1,
            pageSize: 10
        };
        //url = URL.ApiBase + URL.GetAllTasks;
        $.post("/Admin/Filter", model, function (html) {
            $("#taskListContainer").html(html);
        }).fail(function () {
            alert("Failed to load tasks.");
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

    //loadUserList: function () {
    //    $.ajax({
    //        url: "/Admin/GetAllUsers",
    //        type: "GET",
    //        success: function (html) {
    //            $("#divUserList").html(html);
    //        }
    //    });
    //},
    loadAssignees: function () {
        $.ajax({
            url: "/Admin/GetAllUsers",
            type: "GET",
            success: function (html) {
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, 'text/html');
                const users = $(doc).find(".alerts-content-single");

                const $ddl = $("#ddlAssigneeId");
                const selectedAssigneeId = $ddl.data("assignee-id");

                $ddl.empty().append('<option value="">Select Assignee</option>');

                users.each(function () {
                    const id = $(this).find(".link").first().attr("onclick").match(/\d+/)[0];
                    const name = $(this).find(".content-title span").text().trim();
                    const selected = id == selectedAssigneeId ? "selected" : "";
                    $ddl.append(`<option value="${id}" ${selected}>${name}</option>`);
                });
            },
            error: function () {
                alert("Failed to load users.");
            }
        });
    },


    Edit: function (id) {

        $.ajax({
            url: `/Admin/Edit/${id}`,
            type: "GET",
            success: function (html) {
                $("#drawerAddEditTask").html(html);
                
                Admin.loadAssignees();
                Common.openDrawer("#drawerAddEditTask");
            }
        });
    },

    View: async function (id) {

                await Admin.loadAssignees();
    $.ajax({
            url: `/Admin/ViewTask/${id}`,
            type: "GET",
            success: function (html) {
                $("#drawerViewTask").html(html);
                Common.openDrawer("#drawerViewTask");
                // 🔁 After drawer is open, inject assignee name from dropdown
                
                    const assigneeId = $("#hiddenAssigneeId").val();
                    const $ddl = $("#ddlAssigneeId");
                    const name = $ddl.find(`option[value='${assigneeId}']`).text();
                    $("#assigneeNamePlaceholder").text(name || "Unknown");
                
            }
        });

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
    $.get(`/Admin/History/${id}`, function (html) {
        $("#drawerTaskHistory").html(html);
        Common.openDrawer("#drawerTaskHistory");
    }).fail(function () {
        alert("Failed to load task history.");
    });
}
};

$(document).ready(function () {
    Admin.init();

    $(document).on("click", "#btnLoadMoreTasks", function () {
        if (!Admin._hasMore) return;

        Admin._currentPage++;

        const model = Admin.getFilterModel();

        $.post("/Admin/Filter", model, function (html) {
            if ($.trim(html).length === 0) {
                Admin._hasMore = false;
                $("#btnLoadMoreTasks").hide();
            } else {
                $("#taskListContainer").append(html);
                // Optional: Smooth scroll to new content
                $('html, body').animate({ scrollTop: $("#btnLoadMoreTasks").offset().top }, 300);
            }
        }).fail(() => {
            alert("Failed to load more tasks.");
            Admin._currentPage--; // rollback if error
        });
    });

});
