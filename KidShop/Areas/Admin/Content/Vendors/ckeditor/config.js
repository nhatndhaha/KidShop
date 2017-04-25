CKEDITOR.editorConfig = function (config) {
    config.toolbarGroups = [
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'styles', groups: ['styles'] },
		'/',
		'/',
		{ name: 'others', groups: ['others'] },
		{ name: 'about', groups: ['about'] }
    ];

    config.removeButtons = 'Save,PasteText,PasteFromWord,Redo,Undo,SelectAll,Scayt,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,Checkbox,Form,CopyFormatting,RemoveFormat,BidiLtr,BidiRtl,Language,Unlink,Anchor,Flash,Iframe,ShowBlocks,About,Print,Cut,Copy,Paste,CreateDiv';
};