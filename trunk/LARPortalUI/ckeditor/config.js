/**
 * @license Copyright (c) 2003-2016, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.toolbarGroups = [
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'others', groups: ['others'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'about', groups: ['about'] }
    ];

    config.removeButtons = 'Subscript,Superscript,RemoveFormat,Styles,Format,About,Maximize,Image,Anchor,Source';
};
