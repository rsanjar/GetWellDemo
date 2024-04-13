var Pagination = Pagination = Pagination || new function() {};


Pagination.Pagesize = 5;
Pagination.PageNumber = 1;
Pagination.TotalCount = 0;
Pagination.TotalPages = 1;

$(document).ready(function () {
	Pagination.ResetPagingValues();
	Pagination.InitPaginationButtons();

	//Pagination.PageSize = $('#PageSize').val();
	//Pagination.PageNumber = $('#PageNumber').val();
	//Pagination.TotalCount = $('#TotalCount').val();
	Pagination.TotalPages = Math.ceil(Pagination.TotalCount / (Pagination.PageSize * 1.00));

	$("#currentPageNumber").html(Pagination.PageNumber);
	$("#currentTotalPages").html(Pagination.TotalPages);
});

Pagination.SubmitForm = function(action, page) {
	if (action === "previous" && Pagination.HasPreviousPage()) {
		Pagination.PageNumber--;
	}
	if (action === "next" && Pagination.HasNextPage()) {
		Pagination.PageNumber++;
	}
	if (action === "set")
		Pagination.PageNumber = page;

	if (Pagination.PageNumber === $("#PageNumber").val() * 1) return false;

	//$('#pageSize').val(6); //needed if pageSize is to be set from the UI
	$("#PageNumber").val(Pagination.PageNumber);

	Pagination.InitPaginationButtons();

	$("#currentPageNumber").html(Pagination.PageNumber);
	$("#currentTotalPages").html(Pagination.TotalPages);
    $(".submit-button").closest("form").submit();

	return false;
}

Pagination.HasNextPage = function() {
	return $("#TotalCount").val() > $("#PageNumber").val() * $("#PageSize").val();
}

Pagination.HasPreviousPage = function() {
	return $("#PageNumber").val() > 1;
}

Pagination.InitPaginationButtons = function() {
	if (!Pagination.HasNextPage()) {
		$("#nextPagingButton").addClass("disabled");
	} else {
		$("#nextPagingButton").removeClass("disabled");
	}

	if (!Pagination.HasPreviousPage()) {
		$("#previousPagingButton").addClass("disabled");
	} else {
		$("#previousPagingButton").removeClass("disabled");
	}
}

Pagination.ResetPagingValues = function() {
	$("#PageNumber").val(1);
	Pagination.PageNumber = $("#PageNumber").val();
}

Pagination.SetQueryOrder = function(order) {
	var currentOrder = $("#OrderBy").val();
	
	if (order === currentOrder) {
		if ($("#IsOrderAscending").val() === "true") {
			$("#IsOrderAscending").val(false);
		} else {
			$("#IsOrderAscending").val(true);
		}
	} else {
		$("#IsOrderAscending").val(true);
	}

	$("#OrderBy").val(order);

	return Pagination.SubmitSearchForm();
}

Pagination.SetTableSortOrder = function(id) {
    var currentOrder = $("#OrderBy").val();
    var isIdDefined = !(id == undefined || id === "");
    var order = isIdDefined ? (id | "") : currentOrder;
    var sortId = order;

    $("#searchResult table thead tr th.sorting").each(function() {
        $(this).removeClass("asc");
        $(this).removeClass("desc");
    });

    if (order === currentOrder) {
        if ($("#IsOrderAscending").val() === "true") {
            if (isIdDefined)
                $("#IsOrderAscending").val(false);

            $("#" + sortId).addClass("asc");
        } else {
            if (isIdDefined)
                $("#IsOrderAscending").val(true);

            $("#" + sortId).addClass("desc");
        }
    } else {
        $("#IsOrderAscending").val(true);
    }

    $("#OrderBy").val(order);
    $("#PageNumber").val(1);

    return !isIdDefined ? false : Pagination.SubmitSearchForm();
}

Pagination.ToggleResultView = function() {
	if(Pagination.InitResultViewButtons(true))
		return Pagination.SubmitSearchForm();

	return false;
}

Pagination.InitResultViewButtons = function(isToggle) {
    var tableViewResultID = ".action-menu.row .btn-group #tableViewResultID";
	var listViewResultID = ".action-menu.row .btn-group #listViewResultID";
    
    if ($("#IsTableView").val() === "true") {
		if (isToggle)
			$("#IsTableView").val("false");

		$(tableViewResultID).addClass("btn-purple");
		$(listViewResultID).removeClass("btn-purple");
	} else {
		if (isToggle)
			$("#IsTableView").val("true");

		$(tableViewResultID).removeClass("btn-purple");
		$(listViewResultID).addClass("btn-purple");
	}

	return true;
}

Pagination.InitTableViewResult = function() {
    var sortOrderDropDown = ".action-menu #sortOrderDropDown";
    var sortByDropDown = ".action-menu #sortByDropDown";

    $(sortOrderDropDown).change(function () {
		if ($(sortByDropDown).val() === "") {
			$("#IsOrderAscending").val($(sortOrderDropDown).val() === "desc" ? false : true);
			return false;
		}
		$("#OrderBy").val($(sortByDropDown).val());
		$("#IsOrderAscending").val($(sortOrderDropDown).val() === "desc" ? false : true);

		return Pagination.SubmitSearchForm();
	});
	$(sortByDropDown).change(function () {
		$("#OrderBy").val($(sortByDropDown).val());
		$("#IsOrderAscending").val($(sortOrderDropDown).val() === "desc" ? false : true);

		return Pagination.SubmitSearchForm();
	});

	$(sortByDropDown).val($("#OrderBy").val());
	$(sortOrderDropDown).val($("#IsOrderAscending").val() === "true" ? "asc" : "desc");
}

Pagination.SubmitSearchForm = function(page) {
	if (page === "undefined" || page < 1)
		page = 1;

	$("#PageNumber").val(page);
	$(".submit-button").closest("form").submit();

	return false;
}

Pagination.BindTableSortClick = function() {
    $("#searchResult table thead tr th.sorting").click(function() {
        var clickedCol = $(this);
        clickedCol.removeClass("sorting_desc sorting_asc");
        return Pagination.SetQueryOrder(clickedCol.attr("sort-id"));
    });

    var currentOrder = $("#OrderBy").val();
    var sortClass = "sorting_desc";

    if ($("#IsOrderAscending").val() === "true") {
        sortClass = "sorting_asc";
    }
	
    $("table thead tr th.sorting[sort-id='" + currentOrder + "']").addClass(sortClass);
}