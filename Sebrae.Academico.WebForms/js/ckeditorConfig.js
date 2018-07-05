/*
Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

/*
CKEDITOR.editorConfig = function( config )
{
// Define changes to default configuration here. For example:
// config.language = 'fr';
// config.uiColor = '#AADC6E';
};
*/


CKEDITOR.editorConfig = function (config) {
    config.width = '110%';
    config.height = '400px';
    config.contentsLangDirection = 'ltr';
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;

    config.fillEmptyBlocks = false;

    config.forcePasteAsPlainText = true;

    config.toolbar = 'MyToolbar';

    config.toolbar_MyToolbar =
	[
		{ name: 'document', items: ['NewPage', 'Preview'] },
		{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
		{ name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll'] },
		{ name: 'insert', items: ['Table', 'HorizontalRule', 'SpecialChar'] },
		{ name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
                '/',
		{ name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
		{ name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', '-', 'RemoveFormat', '-', 'TextColor', 'BGColor'] },
		        '/',
		{ name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'] },
		{ name: 'tools', items: ['Maximize'] }
	];
};