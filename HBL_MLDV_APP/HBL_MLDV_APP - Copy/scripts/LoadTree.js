
function GetTree() {
        var data;
        if (localStorage.getItem("lock") === null) {
            //$.ajax(
            //               {
            //                   url: "/Menu/GetAttendanceLock",
            //                   type: "POST",
            //                   contentType: "application/json; charset=utf-8",
            //                   dataType: "json",
            //                   error: function (response) {
            //                   },
            //                   success: function (response) {
            //                       // data = JSON.parse(response.data);
            //                       var result = '';
            //                       for (var i = 0; i < response.length; i++) {
            //                           if (i == response.length - 1) {
            //                               result += response[i].month_sk;
            //                           } else {
            //                               result += response[i].month_sk + ',';
            //                           }


            //                       }
            //                       localStorage.setItem("lock", JSON.stringify(result));
            //                   }

            //               });

        }
        if (localStorage.getItem("menu") !== null) {

            $('.sidebar-menu').append(localStorage.getItem("menu"));
            return;
        }
        $.ajax(
           {
               url: "/Login/GetMenu",
               type: "POST",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               error: function (response) {
               },
               success: function (response) {
                   // data = JSON.parse(response.data);
                   loadTree(response);
               }

           });
    }

function loadTree(data)
{
    debugger;
    var kl = 0;
    var km = 0;
    var kn = 0;
    var temp = false;
    var html = "";
    var parentnode = [];
    for (var i = 0; i < data.length; i++) {
        if (data[i].ActivityParentId === 0) {
            parentnode.push(data[i])

        }
    }
    for (i = 0; i < parentnode.length; i++) {
        html +=
                                    '<li class="treeview"><a ' + parentnode[i].ActivityAttr + ' href="' + parentnode[i].ActivityUrl + '">' +
                                   '<i class="' + parentnode[i].ActivityIcon + '"></i>' +
                                   '<span>' + parentnode[i].AcivityTitle + '</span>' +
                                   '<span class="pull-right-container">'+
                                   '<i class="fa fa-angle-left pull-right"></i></span>' +
                                  "</a>";//wraper
        pid = parentnode[i].AcivityId;
        for (j = 0; j < data.length; j++) {
            ///alert(data[j].ActivityParentID);
            //fa fa-angle-left
            if (pid === data[j].ActivityParentId) {
                
                if (kl === 0) {
                    html += '<ul class="treeview-menu" style="border-top: 1px solid white;">'; kl++;
                }
                
                html += '<li>' +
//<i class="fa fa-circle-o"></i><i class="fa fa fa-asterisk pull-right"></i><i class="fa fa-star pull-right"></i>
                                 '<a ' + data[j].ActivityAttr + ' href="' + data[j].ActivityUrl + '">' +
                                 '<i class="' + data[j].ActivityIcon + '"></i>'+
                                '' + data[j].AcivityTitle + '</a>';
                level1 = data[j].AcivityId;
                for (k = 0; k < data.length; k++) {
                    if (level1 === data[k].ActivityParentId) {
                        if (km === 0) {
                            html += '<ul class="treeview-menu" style="border-top: 1px solid white;">'; km++;
                        }

                        html += '<li>' +
                       ' <a ' + data[k].ActivityAttr + ' href="' + data[k].ActivityUrl + '"><i class="' + data[k].ActivityIcon + '"></i>' + data[k].AcivityTitle + '</a>'
                        ;
                        level2 = data[k].AcivityId;
                        for (l = 0; l < data.length; l++) {
                            if (level2 === data[l].ActivityParentId) {
                                temp = true;
                                if (kn === 0) {
                                    html += '<ul class="treeview-menu" style="border-top: 1px solid white;">'; kn++;
                                }
                                html += '<li>' +
                             '<a href="' + data[l].ActivityUrl + '"> <i class="' + data[l].ActivityIcon + '"></i>' + data[l].AcivityTitle +

                             '</a>' +
                                 "</li>";

                            }

                        }
                        if (kn === 1) {
                            html += '</ul>';
                        }
                        html += '</li>';

                        kn = 0;
                    }


                }
                if (km === 1) {
                    html += '</ul>';
                }
                html += '</li>';
            }



            km = 0;
        }
        if (kl === 1) {
            html += '</ul>';
        }//close 2nd level
        html += '</li>'//parent node close i.e end module
        kl = 0;

    }

    $('.sidebar-menu').append(html);
    localStorage.setItem("menu", html);
}

