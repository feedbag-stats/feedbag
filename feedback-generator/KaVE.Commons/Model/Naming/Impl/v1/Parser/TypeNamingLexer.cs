﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.5.3
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from TypeNaming.g4 by ANTLR 4.5.3

// Unreachable code detected

using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;

#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace KaVE.Commons.Model.Naming.Impl.v1.Parser {

/**
 * Copyright 2016 Technische Universität Darmstadt
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
    [System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5.3")]
[System.CLSCompliant(false)]
public partial class TypeNamingLexer : Lexer {
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, T__26=27, T__27=28, UNKNOWN=29, POSNUM=30, LETTER=31, 
		SIGN=32, WS=33, EOL=34;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "T__18", "T__19", "T__20", "T__21", "T__22", "T__23", "T__24", 
		"T__25", "T__26", "T__27", "UNKNOWN", "POSNUM", "LETTER", "SIGN", "DIGIT", 
		"DIGIT_NON_ZERO", "WS", "EOL"
	};


	public TypeNamingLexer(ICharStream input)
		: base(input)
	{
		Interpreter = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, "'->'", "','", "'d:'", "'arr('", "'):'", "'n:'", "'+'", "'.'", "'e:'", 
		"'i:'", "'s:'", "'''", "'['", "']'", "'get'", "'set'", "'('", "')'", "']..ctor'", 
		"']..cctor'", "'].'", "'static'", "'params '", "'opt '", "'ref '", "'out '", 
		"'this '", "'0'", "'?'", null, null, null, null, "'\n'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, "UNKNOWN", "POSNUM", "LETTER", "SIGN", "WS", 
		"EOL"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "TypeNaming.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	private static string _serializedATN = _serializeATN();
	private static string _serializeATN()
	{
	    StringBuilder sb = new StringBuilder();
	    sb.Append("\x3\x430\xD6D1\x8206\xAD2D\x4417\xAEF1\x8D80\xAADD\x2$\xD1");
		sb.Append("\b\x1\x4\x2\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6");
		sb.Append("\x4\a\t\a\x4\b\t\b\x4\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r");
		sb.Append("\t\r\x4\xE\t\xE\x4\xF\t\xF\x4\x10\t\x10\x4\x11\t\x11\x4\x12");
		sb.Append("\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15\t\x15\x4\x16\t\x16\x4");
		sb.Append("\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x4\x1A\t\x1A\x4\x1B\t\x1B");
		sb.Append("\x4\x1C\t\x1C\x4\x1D\t\x1D\x4\x1E\t\x1E\x4\x1F\t\x1F\x4 \t ");
		sb.Append("\x4!\t!\x4\"\t\"\x4#\t#\x4$\t$\x4%\t%\x3\x2\x3\x2\x3\x2\x3\x3");
		sb.Append("\x3\x3\x3\x4\x3\x4\x3\x4\x3\x5\x3\x5\x3\x5\x3\x5\x3\x5\x3\x6");
		sb.Append("\x3\x6\x3\x6\x3\a\x3\a\x3\a\x3\b\x3\b\x3\t\x3\t\x3\n\x3\n\x3");
		sb.Append("\n\x3\v\x3\v\x3\v\x3\f\x3\f\x3\f\x3\r\x3\r\x3\xE\x3\xE\x3\xF");
		sb.Append("\x3\xF\x3\x10\x3\x10\x3\x10\x3\x10\x3\x11\x3\x11\x3\x11\x3\x11");
		sb.Append("\x3\x12\x3\x12\x3\x13\x3\x13\x3\x14\x3\x14\x3\x14\x3\x14\x3");
		sb.Append("\x14\x3\x14\x3\x14\x3\x14\x3\x15\x3\x15\x3\x15\x3\x15\x3\x15");
		sb.Append("\x3\x15\x3\x15\x3\x15\x3\x15\x3\x16\x3\x16\x3\x16\x3\x17\x3");
		sb.Append("\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x18\x3\x18\x3\x18");
		sb.Append("\x3\x18\x3\x18\x3\x18\x3\x18\x3\x18\x3\x19\x3\x19\x3\x19\x3");
		sb.Append("\x19\x3\x19\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1B\x3\x1B");
		sb.Append("\x3\x1B\x3\x1B\x3\x1B\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3");
		sb.Append("\x1C\x3\x1D\x3\x1D\x3\x1E\x3\x1E\x3\x1F\x3\x1F\a\x1F\xBC\n\x1F");
		sb.Append("\f\x1F\xE\x1F\xBF\v\x1F\x3 \x3 \x3!\x3!\x3\"\x3\"\x5\"\xC7\n");
		sb.Append("\"\x3#\x3#\x3$\x6$\xCC\n$\r$\xE$\xCD\x3%\x3%\x2\x2&\x3\x3\x5");
		sb.Append("\x4\a\x5\t\x6\v\a\r\b\xF\t\x11\n\x13\v\x15\f\x17\r\x19\xE\x1B");
		sb.Append("\xF\x1D\x10\x1F\x11!\x12#\x13%\x14\'\x15)\x16+\x17-\x18/\x19");
		sb.Append("\x31\x1A\x33\x1B\x35\x1C\x37\x1D\x39\x1E;\x1F= ?!\x41\"\x43");
		sb.Append("\x2\x45\x2G#I$\x3\x2\x5\x4\x2\x43\\\x63|\v\x2##%&,-//\x31\x31");
		sb.Append("<=??\x42\x42\x61\x61\x4\x2\v\v\"\"\xD1\x2\x3\x3\x2\x2\x2\x2");
		sb.Append("\x5\x3\x2\x2\x2\x2\a\x3\x2\x2\x2\x2\t\x3\x2\x2\x2\x2\v\x3\x2");
		sb.Append("\x2\x2\x2\r\x3\x2\x2\x2\x2\xF\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2");
		sb.Append("\x2\x13\x3\x2\x2\x2\x2\x15\x3\x2\x2\x2\x2\x17\x3\x2\x2\x2\x2");
		sb.Append("\x19\x3\x2\x2\x2\x2\x1B\x3\x2\x2\x2\x2\x1D\x3\x2\x2\x2\x2\x1F");
		sb.Append("\x3\x2\x2\x2\x2!\x3\x2\x2\x2\x2#\x3\x2\x2\x2\x2%\x3\x2\x2\x2");
		sb.Append("\x2\'\x3\x2\x2\x2\x2)\x3\x2\x2\x2\x2+\x3\x2\x2\x2\x2-\x3\x2");
		sb.Append("\x2\x2\x2/\x3\x2\x2\x2\x2\x31\x3\x2\x2\x2\x2\x33\x3\x2\x2\x2");
		sb.Append("\x2\x35\x3\x2\x2\x2\x2\x37\x3\x2\x2\x2\x2\x39\x3\x2\x2\x2\x2");
		sb.Append(";\x3\x2\x2\x2\x2=\x3\x2\x2\x2\x2?\x3\x2\x2\x2\x2\x41\x3\x2\x2");
		sb.Append("\x2\x2G\x3\x2\x2\x2\x2I\x3\x2\x2\x2\x3K\x3\x2\x2\x2\x5N\x3\x2");
		sb.Append("\x2\x2\aP\x3\x2\x2\x2\tS\x3\x2\x2\x2\vX\x3\x2\x2\x2\r[\x3\x2");
		sb.Append("\x2\x2\xF^\x3\x2\x2\x2\x11`\x3\x2\x2\x2\x13\x62\x3\x2\x2\x2");
		sb.Append("\x15\x65\x3\x2\x2\x2\x17h\x3\x2\x2\x2\x19k\x3\x2\x2\x2\x1Bm");
		sb.Append("\x3\x2\x2\x2\x1Do\x3\x2\x2\x2\x1Fq\x3\x2\x2\x2!u\x3\x2\x2\x2");
		sb.Append("#y\x3\x2\x2\x2%{\x3\x2\x2\x2\'}\x3\x2\x2\x2)\x85\x3\x2\x2\x2");
		sb.Append("+\x8E\x3\x2\x2\x2-\x91\x3\x2\x2\x2/\x98\x3\x2\x2\x2\x31\xA0");
		sb.Append("\x3\x2\x2\x2\x33\xA5\x3\x2\x2\x2\x35\xAA\x3\x2\x2\x2\x37\xAF");
		sb.Append("\x3\x2\x2\x2\x39\xB5\x3\x2\x2\x2;\xB7\x3\x2\x2\x2=\xB9\x3\x2");
		sb.Append("\x2\x2?\xC0\x3\x2\x2\x2\x41\xC2\x3\x2\x2\x2\x43\xC6\x3\x2\x2");
		sb.Append("\x2\x45\xC8\x3\x2\x2\x2G\xCB\x3\x2\x2\x2I\xCF\x3\x2\x2\x2KL");
		sb.Append("\a/\x2\x2LM\a@\x2\x2M\x4\x3\x2\x2\x2NO\a.\x2\x2O\x6\x3\x2\x2");
		sb.Append("\x2PQ\a\x66\x2\x2QR\a<\x2\x2R\b\x3\x2\x2\x2ST\a\x63\x2\x2TU");
		sb.Append("\at\x2\x2UV\at\x2\x2VW\a*\x2\x2W\n\x3\x2\x2\x2XY\a+\x2\x2YZ");
		sb.Append("\a<\x2\x2Z\f\x3\x2\x2\x2[\\\ap\x2\x2\\]\a<\x2\x2]\xE\x3\x2\x2");
		sb.Append("\x2^_\a-\x2\x2_\x10\x3\x2\x2\x2`\x61\a\x30\x2\x2\x61\x12\x3");
		sb.Append("\x2\x2\x2\x62\x63\ag\x2\x2\x63\x64\a<\x2\x2\x64\x14\x3\x2\x2");
		sb.Append("\x2\x65\x66\ak\x2\x2\x66g\a<\x2\x2g\x16\x3\x2\x2\x2hi\au\x2");
		sb.Append("\x2ij\a<\x2\x2j\x18\x3\x2\x2\x2kl\a)\x2\x2l\x1A\x3\x2\x2\x2");
		sb.Append("mn\a]\x2\x2n\x1C\x3\x2\x2\x2op\a_\x2\x2p\x1E\x3\x2\x2\x2qr\a");
		sb.Append("i\x2\x2rs\ag\x2\x2st\av\x2\x2t \x3\x2\x2\x2uv\au\x2\x2vw\ag");
		sb.Append("\x2\x2wx\av\x2\x2x\"\x3\x2\x2\x2yz\a*\x2\x2z$\x3\x2\x2\x2{|");
		sb.Append("\a+\x2\x2|&\x3\x2\x2\x2}~\a_\x2\x2~\x7F\a\x30\x2\x2\x7F\x80");
		sb.Append("\a\x30\x2\x2\x80\x81\a\x65\x2\x2\x81\x82\av\x2\x2\x82\x83\a");
		sb.Append("q\x2\x2\x83\x84\at\x2\x2\x84(\x3\x2\x2\x2\x85\x86\a_\x2\x2\x86");
		sb.Append("\x87\a\x30\x2\x2\x87\x88\a\x30\x2\x2\x88\x89\a\x65\x2\x2\x89");
		sb.Append("\x8A\a\x65\x2\x2\x8A\x8B\av\x2\x2\x8B\x8C\aq\x2\x2\x8C\x8D\a");
		sb.Append("t\x2\x2\x8D*\x3\x2\x2\x2\x8E\x8F\a_\x2\x2\x8F\x90\a\x30\x2\x2");
		sb.Append("\x90,\x3\x2\x2\x2\x91\x92\au\x2\x2\x92\x93\av\x2\x2\x93\x94");
		sb.Append("\a\x63\x2\x2\x94\x95\av\x2\x2\x95\x96\ak\x2\x2\x96\x97\a\x65");
		sb.Append("\x2\x2\x97.\x3\x2\x2\x2\x98\x99\ar\x2\x2\x99\x9A\a\x63\x2\x2");
		sb.Append("\x9A\x9B\at\x2\x2\x9B\x9C\a\x63\x2\x2\x9C\x9D\ao\x2\x2\x9D\x9E");
		sb.Append("\au\x2\x2\x9E\x9F\a\"\x2\x2\x9F\x30\x3\x2\x2\x2\xA0\xA1\aq\x2");
		sb.Append("\x2\xA1\xA2\ar\x2\x2\xA2\xA3\av\x2\x2\xA3\xA4\a\"\x2\x2\xA4");
		sb.Append("\x32\x3\x2\x2\x2\xA5\xA6\at\x2\x2\xA6\xA7\ag\x2\x2\xA7\xA8\a");
		sb.Append("h\x2\x2\xA8\xA9\a\"\x2\x2\xA9\x34\x3\x2\x2\x2\xAA\xAB\aq\x2");
		sb.Append("\x2\xAB\xAC\aw\x2\x2\xAC\xAD\av\x2\x2\xAD\xAE\a\"\x2\x2\xAE");
		sb.Append("\x36\x3\x2\x2\x2\xAF\xB0\av\x2\x2\xB0\xB1\aj\x2\x2\xB1\xB2\a");
		sb.Append("k\x2\x2\xB2\xB3\au\x2\x2\xB3\xB4\a\"\x2\x2\xB4\x38\x3\x2\x2");
		sb.Append("\x2\xB5\xB6\a\x32\x2\x2\xB6:\x3\x2\x2\x2\xB7\xB8\a\x41\x2\x2");
		sb.Append("\xB8<\x3\x2\x2\x2\xB9\xBD\x5\x45#\x2\xBA\xBC\x5\x43\"\x2\xBB");
		sb.Append("\xBA\x3\x2\x2\x2\xBC\xBF\x3\x2\x2\x2\xBD\xBB\x3\x2\x2\x2\xBD");
		sb.Append("\xBE\x3\x2\x2\x2\xBE>\x3\x2\x2\x2\xBF\xBD\x3\x2\x2\x2\xC0\xC1");
		sb.Append("\t\x2\x2\x2\xC1@\x3\x2\x2\x2\xC2\xC3\t\x3\x2\x2\xC3\x42\x3\x2");
		sb.Append("\x2\x2\xC4\xC7\a\x32\x2\x2\xC5\xC7\x5\x45#\x2\xC6\xC4\x3\x2");
		sb.Append("\x2\x2\xC6\xC5\x3\x2\x2\x2\xC7\x44\x3\x2\x2\x2\xC8\xC9\x4\x33");
		sb.Append(";\x2\xC9\x46\x3\x2\x2\x2\xCA\xCC\t\x4\x2\x2\xCB\xCA\x3\x2\x2");
		sb.Append("\x2\xCC\xCD\x3\x2\x2\x2\xCD\xCB\x3\x2\x2\x2\xCD\xCE\x3\x2\x2");
		sb.Append("\x2\xCEH\x3\x2\x2\x2\xCF\xD0\a\f\x2\x2\xD0J\x3\x2\x2\x2\x6\x2");
		sb.Append("\xBD\xC6\xCD\x2");
	    return sb.ToString();
	}

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
} // namespace KaVE.Commons.Model.Names.CSharp.Parser