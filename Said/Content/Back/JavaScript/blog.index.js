﻿define('blog.index', ['said', 'so', 'sweetalert', 'bsTable'], function (said, so, sweetalert) {
	var templates = {
		deleteArticle: {
			text: '您确定要删除Blog《<span class="said-red said-bold"> ${0} </span>》么？<br/>该操作为<span class="said-red said-bold">逻辑删除</span>',
			ok: '删除Blog《${0}》成功<br/>该操作为逻辑删除，仍可以在数据库中查阅到'
		},
		realDeleteArticle: {
			text: '您确定要永久删除Blog《<span class="said-red said-bold"> ${0} </span>》么？<br/>该操作为<span class="said-red said-bold">物理删除，不可恢复</span>',
			ok: '删除Blog《${0}》成功<br/>该操作为物理删除，不可恢复'
		}
	},
		deleteArticle = function (isRealDelete, id, title, callback) {
			sweetalert({
				title: '确认删除',
				text: so.format(isRealDelete ? templates.realDeleteArticle.text : templates.deleteArticle.text, title),
				html: true,
				type: 'warning',
				showCancelButton: true,
				confirmButtonColor: "#DD6B55",
				confirmButtonText: '确认删除',
				closeOnConfirm: false,
				cancelButtonText: '取消',
				//配置正在加载
				showLoaderOnConfirm: true
			}, function (isConfirm) {
				if (isConfirm) {
					said.ajax(isRealDelete ? Action.realDelete : Action.delele, {
						id: id
					}).done(function (result) {
						if (result.code === 0) {
							sweetalert({
								title: '删除成功',
								text: so.format(isRealDelete ? templates.realDeleteArticle.ok : templates.deleteArticle.ok, title),
								html: true,
								type: 'success'
							});
							callback(id);
						} else {
							sweetalert('删除异常', '服务器返回异常：' + result.msg, 'error');
						}
					}).fail(function () {
						sweetalert('删除异常', '网络连接异常', 'error');
					});
				}
			});
		};
	return function (Action, blogDatas) {
		$(function () {
			var $bsTable = $('#btTable').bootstrapTable({
				columns: [
				 {
				 	field: 'BTitle',
				 	title: '名&nbsp;&nbsp;&nbsp;称',
				 	valign: 'middle',
				 	sortable: true,
				 	formatter: function (value, row, index) {
				 		return ['<a target="_blank" href="/Blog/', row.BlogId, '.html">', value, '</a>'].join('');
				 	}
				 },
				 {
				 	field: 'BPV',
				 	title: '浏&nbsp;&nbsp;&nbsp;览',
				 	valign: 'middle',
				 	sortable: true,
				 	formatter: function (value, row, index) {
				 		return value;
				 	}
				 },
				 {
				 	field: 'BDate',
				 	title: '发布时间',
				 	sortable: true,
				 	valign: 'middle',
				 	formatter: function (value, row, index) {
				 		//return so.dateFormat(value);
				 		return value;
				 	}
				 },
				 {
				 	field: 'Classify.CName',
				 	title: '分&nbsp;&nbsp;&nbsp;类',
				 	sortable: true,
				 	valign: 'middle',
				 	formatter: function (value, row, index) {
				 		return value;
				 	}
				 },
				 {
				 	field: 'Likes',
				 	title: '喜&nbsp;&nbsp;&nbsp;欢',
				 	valign: 'middle',
				 	formatter: function (value, row, index) {
				 		return value;
				 	}
				 },
				 {
				 	field: 'BPV',
				 	title: '浏&nbsp;&nbsp;&nbsp;览',
				 	valign: 'middle',
				 	formatter: function (value, row, index) {
				 		return value;
				 	}
				 },
				 {
				 	field: 'BComment',
				 	title: '评&nbsp;&nbsp;&nbsp;论',
				 	valign: 'middle',
				 	formatter: function (value, row, index) {
				 		return value;
				 	}
				 },
				 {
				 	field: 'Likes',
				 	title: '喜&nbsp;&nbsp;&nbsp;欢',
				 	valign: 'middle',
				 	formatter: function (value, row, index) {
				 		return value;
				 	}
				 },
				 {
				 	field: 'BlogId',
				 	title: '操&nbsp;&nbsp;&nbsp;作',
				 	formatter: function (value, row, index) {
				 		return ['<a class="btn btn-info fa fa-edit" data-id="', value, '" href="/Back/Blog/Edit/', value, '" title="编辑"></a>&nbsp;&nbsp;&nbsp;<a class="btn btn-default fa fa-trash-o btn-delete" data-title="', row.BTitle, '" data-id="', value, '" title="逻辑删除"></a>&nbsp;&nbsp;&nbsp;<a class="btn btn-danger fa fa-trash-o btn-delete" data-is-real="1" data-title="', row.BTitle, '" data-id="', value, '" title="物理删除（永久删除）"></a>'].join('');
				 	}
				 }],
				pageSize: 20,
				pageList: [20, 50, 100, 150, 200, 400, 600, 1000],
				data: blogDatas
			}).on('click', '.btn-delete', function () {
				var $elem = $(this), dataId = $elem.data('id'), dataTitle = $elem.data('title'), isRealDelete = $elem.data('isReal');
				deleteArticle(isRealDelete, dataId, dataTitle, function () {
					$bsTable.bootstrapTable('remove', {
						field: 'BlogId',
						values: [dataId + '']//bootStrapTable的bug，因为dataId是被jQuery转成了数字，导致bootStrapTable检测BlogId字段是否相等的时候发生了bug
					});
				});
			});
		});
	}


});