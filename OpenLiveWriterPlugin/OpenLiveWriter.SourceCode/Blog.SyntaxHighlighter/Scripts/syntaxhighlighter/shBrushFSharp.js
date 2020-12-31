/**
 * SyntaxHighlighter
 * http://alexgorbatchev.com/SyntaxHighlighter
 *
 * SyntaxHighlighter is donationware. If you are using it, please donate.
 * http://alexgorbatchev.com/SyntaxHighlighter/donate.html
 *
 * @version
 * 3.0.83 (July 02 2010)
 * 
 * @copyright
 * Copyright (C) 2004-2010 Alex Gorbatchev.
 *
 * @license
 * Dual licensed under the MIT and GPL licenses.
 */
;(function()
{
	// CommonJS
	typeof(require) != 'undefined' ? SyntaxHighlighter = require('shCore').SyntaxHighlighter : null;

	function Brush()
	{
		// Contributes by B.v.Zanten, Getronics
		// http://confluence.atlassian.com/display/CONFEXT/New+Code+Macro

	var keywords =	'abstract and as assert begin class default delegate do done downcast downto else end enum ' +
	                'exception extern false finally for fun function if in inherit interface land lazy let match ' + 
	                'member module mutable namespace new null of open or override rec sig static struct then to ' + 
	                'true try type val when inline upcast while with void asr land lor lsl lsr lxor mod async ' + 
	                'atomic break checked component const constraint constructor continue decimal eager event ' +
	                'external fixed functor include method mixin object process property protected public pure ' +
	                'readonly return sealed virtual volatile';

	var operators =	'land lor lsl lsr lxor mod';

	this.regexList = [
		{ regex: SyntaxHighlighter.regexLib.singleLineCComments,	css: 'comments' },			// one line comments
		{ regex: SyntaxHighlighter.regexLib.doubleQuotedString,		css: 'string' },			// strings
		{ regex: SyntaxHighlighter.regexLib.singleQuotedString,		css: 'string' },			// strings
		{ regex: /^ *#.*/gm,						css: 'preprocessor' },
		{ regex: new RegExp(this.getKeywords(keywords), 'gm'),		css: 'keyword' },			// keyword
		{ regex: new RegExp(this.getKeywords(operators), 'gm'),		css: 'functions' }
		];

	};

	Brush.prototype	= new SyntaxHighlighter.Highlighter();
	Brush.aliases	= ['fsharp', 'fs'];

	SyntaxHighlighter.brushes.FSharp = Brush;

	// CommonJS
	typeof(exports) != 'undefined' ? exports.Brush = Brush : null;
})();
