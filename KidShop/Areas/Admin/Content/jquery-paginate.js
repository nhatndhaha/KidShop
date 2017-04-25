

(function ($) {
    $.fn.zPaging = function (options) {

        var defaults = {
            'paging'        : '#paging',
            'rows'          : '#rows',
            'pages'         : '#pages',
            'btnPrevious'   : '#goPrevious',
            'btnNext'       : '#goNext',
            'txtCurrentPage': '#currentPage',
            'pageInfo'      : '#pageInfo',
            'pagination'    : '#pagination',
            'items'         : 10,
            'height'        : 43.2,
            'currentPage'   : 1,
            'total'         : 0,
        };

        options = $.extend(defaults, options);

        var paging          = $(options.paging);
        var rows            = $(options.rows);
        var pages           = $(options.pages);
        var btnPrevious     = $(options.btnPrevious);
        var btnNext         = $(options.btnNext);
        var txtCurrentPage  = $(options.txtCurrentPage);
        var pageInfo        = $(options.pageInfo);
        var pagination      = $(options.pagination)

        init();

        function init(isLoadData) {
            // Lấy tổng số trang
            $.ajax({
                url: '/Admin/Product/Count?items='+options.items,
                type: 'GET',
                dataType: 'json'
            }).done(function (data) {
                options.total = data.total;
                setLblPageInfo();
                setPagination();
                if (isLoadData != false) { loadData() };
            });

            // Set trang hiện tại
            setCurrentPage(options.currentPage);

            // Set Even cho nút Previous
            btnPrevious.on('click', function (e) {
                goPrevious();
                e.stopImmediatePropagation();
            });

            // Set Even cho nút Next
            btnNext.on('click', function (e) {
                goNext();
                e.stopImmediatePropagation();
            });

            // Set Even cho khi nhập số trang
            txtCurrentPage.on('keyup', function (e) {
                if (e.keyCode == 13) {
                    var currentPageValue = parseInt($(this).val());
                    if (isNaN(currentPageValue) || currentPageValue <= 0 || currentPageValue > options.total) {
                        alert("So nhap vao ko hop le");
                        e.stopImmediatePropagation();
                    } else {
                        options.currentPage = currentPageValue;
                        loadData();
                        setLblPageInfo();
                    }
                }
            });
        }

        function setPagination(e) {
            $('#pagination .canChange').remove();

            var start = (options.currentPage - 3 <= 1) ? 1 : options.currentPage - 3;
            var end = (options.currentPage + 3 > options.total) ? options.total : options.currentPage + 3;
                        
            for (var i = start; i <= end; i++) {
                var item = '<li id="p' + i + '" class="canChange"><a href="#' + i + '">' + i + '</a></li>';
                pagination.children(':last').before(item);
            }
            pagination.children().on('click', function (e) {
                options.currentPage = parseInt($(this).text());
                loadData();
            });
            $('#pagination #p' + options.currentPage).addClass('active');
        }

        function goPrevious() {
            if (options.currentPage > 1) {
                options.currentPage--;
                loadData();
            }
        }

        function goNext() {
            if (options.currentPage < options.total) {
                options.currentPage++;
                loadData();
            }
        }

        function setRowsHeight() {
            var pagingHeight = (options.items * options.height) + 'px';
            var rowsHeight = options.height + 'px';
            paging.css('height', pagingHeight);
            paging.css('display', 'fixed');
            paging.children().css('height', rowsHeight);
        }

        function setCurrentPage(value) {
            txtCurrentPage.val(value);
        }

        function setLblPageInfo() {
            pageInfo.text('Trang ' + options.currentPage + ' trên tổng số ' + options.total);
        }

        function loadData() {
            $.ajax({
                url: '/Admin/Product/GetAllProduct',
                type: 'POST',
                dataType: 'json',
                cache: false,
                data: {
                    page    : options.currentPage,
                    pageSize: options.items
                }
            }).done(function (data) {
                if (data.Data.length > 0) {
                    paging.empty();
                    setLblPageInfo();
                    setCurrentPage(options.currentPage);
                    setPagination();
                    $.each(data.Data, function (index, value) {
                        var str = '';
                        str += '<tr class="odd pointer" item-id="'+value.ProductId+'">';
                        str += '<td>' + value.ProductName + '</td>';
                        str += '<td>' + value.ProductId + '</td>';
                        str += '<td>' + value.CategoryName + '</td>';
                        str += '<td>' + value.Price + '</td>';
                        str += '<td>' + value.Qty + '</td>';
                        str += '<td><img src="/Areas/Admin/Content/Images/ProductImages/' + value.Image + '" id="Image" style="width: 20px; height: 20px; border-radius: 3px;"/></td>';
                        str += '<td><input type = "checkbox" disabled = "" '+((value.Status==true)?'checked=""':'')+'/></td>';
                        str += '<td class=" last">';
                        str += '<a class="btn btn-primary btn-xs" href="/Admin/Product/Edit/'+value.ProductId+'"><i class="fa fa-edit"></i> Sửa</a>';
                        str += '<a class="btn btn-danger btn-xs" id="'+value.ProductId+'"><i class="fa fa-trash"></i> Xóa</a>';
                        str += '</td>';
                        str += '</tr>';
                        paging.append(str);
                    });
                    setRowsHeight();
                    $('#paging tr td a:last-child').on('click', function () {
                        var obj = this;
                        $.confirm({
                            title: 'Xác nhận xóa!',
                            content: 'Bạn có chắc chắn muốn xóa?',
                            buttons: {
                                OK: {
                                    text: 'Xóa',
                                    btnClass: 'btn-red',
                                    keys: ['enter', 'shift'],
                                    action: function () {
                                        deleteItem(obj);
                                    }
                                },
                                cancel: {
                                    text: 'Hủy',
                                    keys: ['esc']
                                }
                            }
                        });
                    });
                }
            });
        }

        function deleteItem(obj) {
            var parent = $(obj).parent().parent();
            var itemId = $(obj).attr('id');
            var lastId = $(parent.parent()).children(':last').attr('item-id');
            $(parent).fadeOut(100, function () {
                $(this).remove();
                $.ajax({
                    url: '/Admin/Product/Delete',
                    type: 'POST',
                    data: { id: itemId }
                });
                if (itemId == lastId && paging.children().length == 0) {
                    options.currentPage--;
                    init();
                }
                init(false);
            });

            $.ajax({
                url: '/Admin/Product/GetNextProduct',
                type: 'POST',
                cache: false,
                data: {
                    lastId : lastId
                }
            }).done(function (data) {
                if (data.Data.length > 0) {
                    var value = data.Data[0];
                    var str = '';
                    str += '<tr class="odd pointer" item-id="' + value.ProductId + '">';
                    str += '<td>' + value.ProductName + '</td>';
                    str += '<td>' + value.ProductId + '</td>';
                    str += '<td>' + value.CategoryName + '</td>';
                    str += '<td>' + value.Price + '</td>';
                    str += '<td>' + value.Qty + '</td>';
                    str += '<td><img src="/Areas/Admin/Content/Images/ProductImages/' + value.Image + '" id="Image" style="width: 20px; height: 20px; border-radius: 3px;" /></td>';
                    str += '<td><input type = "checkbox" disabled = "" ' + ((value.Status == true) ? 'checked=""' : '') + '/></td>';
                    str += '<td class=" last">';
                    str += '<a class="btn btn-primary btn-xs" href="/Admin/Product/Edit/' + value.ProductId + '"><i class="fa fa-edit"></i> Sửa</a>';
                    str += '<a class="btn btn-danger btn-xs" id="' + value.ProductId + '"><i class="fa fa-trash"></i> Xóa</a>';
                    str += '</td>';
                    str += '</tr>';
                    str = $(str).hide().appendTo(paging);
                    $(str).fadeIn(900);
                    $(str).children(':last').children(':last').on('click', function () {
                        var obj = $(str).children(':last').children(':last');
                        $.confirm({
                            title: 'Xác nhận xóa!',
                            content: 'Bạn có chắc chắn muốn xóa?',
                            buttons: {
                                OK: {
                                    text: 'Xóa',
                                    btnClass: 'btn-red',
                                    keys: ['enter', 'shift'],
                                    action: function () {
                                        deleteItem(obj);
                                    }
                                },
                                cancel: {
                                    text: 'Hủy',
                                    keys: ['esc']
                                }
                            }
                        });
                    });
                }
            });
        }

    };
})(jQuery);

$(document).ready(function () {
    var obj = { currentPage : 1, items: 10 };
    $('#paging').zPaging(obj);
});