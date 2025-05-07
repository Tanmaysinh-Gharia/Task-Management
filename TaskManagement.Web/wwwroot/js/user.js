var User = {
    _sortColumn: null,
    _sortOrder: "asc",
    _currentPage: 1,
    _hasMore: true,


    init: function () {
        User.bindTaskActions();
        $(".ass-des li").on("click", function () {
            $(".ass-des li").removeClass("active");
            $(this).addClass("active");
        });

    },

    // Filtering and Sorting Section
    applyFilter: function () {
        User._currentPage = 1;
        User._hasMore = true;
        const model = User.getFilterModel();
        model.pageNumber = User._currentPage;

        $.post("/User/Filter", model, function (html) {
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
            sortColumn: User._sortColumn,
            sortOrder: User._sortOrder,
            pageNumber: User._currentPage,
            pageSize: 10
        };
    }
    ,

    applySort: function () {
        const selectedColumn = $("input[name='sortColumn']:checked").val();
        const order = $(".ass-des li.active").data("order");

        User._sortColumn = selectedColumn;
        User._sortOrder = order;

        User.applyFilter(); // reuse filter with new sort
        Common.hideFilter("#divSorting");
    },

    resetFilter: function () {
        $("#filterForm")[0].reset();
        $("#ddlStatus, #ddlPriority").val(null).trigger("change");
        User.applyFilter();
    },

    resetSort: function () {
        $("input[name='sortColumn']").prop("checked", false);
        $(".ass-des li").removeClass("active").first().addClass("active");
        User._sortColumn = null;
        User._sortOrder = "asc";
        User.applyFilter();
    },

    bindTaskActions: function () {
        $(document).on("click", "#btnAddTask", function () {
            $("#formAddEditTask")[0].reset();
            $("#taskId").val(0);
            Common.openDrawer("#drawerAddEditTask");
        });

        $(document).on("click", "#btnSaveTask", function (e) {
            e.preventDefault();

            const id = parseInt($("#taskId").val());
            const isUpdate = id > 0;
            const url = isUpdate ? `/User/Update` : "/User/Create";
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
                User.loadTaskList();

            }).fail(function (xhr) {
                const msg = xhr.responseJSON?.Message || "Error while saving task.";
                alert(msg);
            });
        });



    },

    
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
        $.post("/User/Filter", model, function (html) {
            $("#taskListContainer").html(html);
        }).fail(function () {
            alert("Failed to load tasks.");
        });
    },


    Edit: function (id) {
            $.ajax({
                url: `/User/Edit/${id}`,
                type: "GET",
                success: function (html) {
                    $("#drawerAddEditTask").html(html);

                    Common.openDrawer("#drawerAddEditTask");
                }
            });
        
    },

    View: async function (id) {

        
            $.ajax({
                url: `/User/ViewTask/${id}`,
                type: "GET",
                success: function (html) {
                    $("#drawerViewTask").html(html);
                    Common.openDrawer("#drawerViewTask");
                    // 🔁 After drawer is open, inject assignee name from dropdown

                }
            });

        

    },

    Delete: function (id, isUser) {
        const confirmation = confirm(`Are you sure you want to delete this ${isUser ? "user" : "task"}?`);
        if (!confirmation) return;

        const url = isUser ? `/User/DeleteUser/${id}` : `/User/Delete/${id}`;

        $.ajax({
            url: url,
            type: "POST",
            success: function () {
                alert(`${isUser ? "User" : "Task"} deleted successfully.`);
                isUser ? User.loadUserList() : User.loadTaskList();
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
        Common.openDrawer("#drawerAddEditTask");
    },

    OpenUserDrawer: function () {
        $("#formAddEditUser")[0].reset();
        $("#userId").val(0);
        Common.openDrawer("#drawerAddEditUser");
    },



    openTaskHistoryDrawer: function (id) {
        $.get(`/User/History/${id}`, function (html) {
            $("#drawerTaskHistory").html(html);
            Common.openDrawer("#drawerTaskHistory");
        }).fail(function () {
            alert("Failed to load task history.");
        });
    }
};

$(document).ready(function () {
    if (typeof Admin === "undefined" && typeof User !== "undefined") {
        User.init();

        $(document).on("click", "#btnLoadMoreTasks", function () {
            if (!User._hasMore) return;

            User._currentPage++;

            const model = User.getFilterModel();

            $.post("/User/Filter", model, function (html) {
                if ($.trim(html).length === 0) {
                    User._hasMore = false;
                    $("#btnLoadMoreTasks").hide();
                } else {
                    $("#taskListContainer").append(html);
                    // Optional: Smooth scroll to new content
                    $('html, body').animate({ scrollTop: $("#btnLoadMoreTasks").offset().top }, 300);
                }
            }).fail(() => {
                alert("Failed to load more tasks.");
                User._currentPage--; // rollback if error
            });
        });
    }
});
