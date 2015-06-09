﻿$(document).ready(function () {
    $("#lsBelonger").change(function () {
        if ($("#lsBelonger").val() != "") {
            $.getJSON("/Admin/GetTypeByBelonger", { "belonger": $("#lsBelonger").val() }, function (data) {
                var str = "<option value='0'>无上级类型</option>";
                for (var i = 0; i < data.length; i++) {
                    str += "<option value='" + data[i].ID + "'>" + data[i].TypeValue + "</option>";
                }
                $("#lsFatherID").html(str);
            });
        }
    });


    $("#lsOneType").change(function () {
        if ($("#lsOneType").val() != 0) {
            $.getJSON("/Admin/GetChildrenTypeByFather", { "father": $("#lsOneType ").val() }, function (data) {
                var str = "";
                for (var i = 0; i < data.length; i++) {
                    str += "<option value='" + data[i].ID + "'>" + data[i].TypeValue + "</option>";
                }
                $("#lsSecondType").html(str);
            });
        }
    });
});