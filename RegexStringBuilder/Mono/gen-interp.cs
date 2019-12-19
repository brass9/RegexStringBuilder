//
// This file is not part of the build. It is used to generate the character related
// opcodes in RxInterpreter.cs.
//

using System;

class Op {
	public string name, body, cond, len;

	public Op (string name, string body, string cond, string len) {
		this.name = name;
		this.body = body;
		this.cond = cond;
		this.len = len;
	}
}

public class GenInterp
{
	public static int base_indent;

	public static void write (int indent, string format, params string[] args) {
		for (int i = 0; i < base_indent + indent; ++i)
			Console.Write ("\t");
		Console.WriteLine (format, args);
	}

	public static void Main () {
		base_indent = 4;

		write (0, "// GENERATED BY gen-interp.cs, DO NOT MODIFY");

		Op[] ops = new Op [] {

			new Op ("Char", "", "(c == program [pc + 1])", "2"),
			new Op ("Range", "", "(c >= program [pc + 1] && c <= program [pc + 2])", "3"),
			new Op ("UnicodeRange", "", "(c >= (program [pc + 1] | ((int)program [pc + 2] << 8))) && (c <= (program [pc + 3] | ((int)program [pc + 4] << 8)))", "5"),
			new Op ("UnicodeChar", "", "(c == (program [pc + 1] | ((int)program [pc + 2] << 8)))", "3"),
			new Op ("CategoryAny", "", @"(c != '\n')", "1"),
			new Op ("CategoryAnySingleline", "", "true", "1"),
			new Op ("CategoryWord", "", "(Char.IsLetterOrDigit (c) || Char.GetUnicodeCategory (c) == UnicodeCategory.ConnectorPunctuation)", "1"),
			new Op ("CategoryDigit", "", "(Char.IsDigit (c))", "1"),
			new Op ("CategoryWhiteSpace", "", "(Char.IsWhiteSpace (c))", "1"),
			new Op ("CategoryEcmaWord", "", "('a' <= c && c <= 'z' || 'A' <= c && c <= 'Z' || '0' <= c && c <= '9' || c == '_')", "1"),
			new Op ("CategoryEcmaWhiteSpace", "", @"(c == ' ' || c == '\t' || c == '\n' || c == '\r' || c == '\f' || c == '\v')", "1"),
			new Op ("CategoryUnicodeSpecials", "", @"('\uFEFF' <= c && c <= '\uFEFF' || '\uFFF0' <= c && c <= '\uFFFD')", "1"),
			new Op ("CategoryUnicode", "", "(Char.GetUnicodeCategory (c) == (UnicodeCategory)program [pc + 1])", "2"),
			new Op ("CategoryGeneral", "", "(CategoryUtils.IsCategory ((Category)program [pc + 1], c))", "2"),
			new Op ("Bitmap", "int c2 = (int)c; c2 -= program [pc + 1]; length = program [pc + 2];", "(c2 >= 0 && c2 < (length << 3) && (program [pc + 3 + (c2 >> 3)] & (1 << (c2 & 0x7))) != 0)", "3 + program [pc + 2]"),
			new Op ("UnicodeBitmap", "int c2 = (int)c; c2 -= (program [pc + 1] | ((int)program [pc + 2] << 8)); length = (program [pc + 3] | ((int)program [pc + 4] << 8));", "(c2 >= 0 && c2 < (length << 3) && (program [pc + 5 + (c2 >> 3)] & (1 << (c2 & 0x7))) != 0)", "5 + (program [pc + 3] | ((int)program [pc + 4] << 8))")
		};

		for (int i1 = 0; i1 < 2; ++i1) {
			for (int i2 = 0; i2 < 2; ++i2) {
				for (int i3 = 0; i3 < 2; ++i3) {
					bool reverse = (i1 == 1);
					bool revert = (i2 == 1);
					bool ignore = (i3 == 1);

					foreach (Op op in ops) {
						if (op.name.StartsWith ("Category") && ignore)
							// These have no IgnoreCase versions
							continue;

						if (i1 == 0 && i2 == 0 && i3 == 0) {
							write (0, "");
							write (0, "/* {0} */", op.name);
							write (0, "");
						}						

						write (0, "case RxOp.{0}{1}{2}{3}:", revert ? "No" : "", op.name, ignore ? "IgnoreCase" : "", reverse ? "Reverse" : "");
						if (reverse)
							write (1, "if (strpos > 0) {{");
						else
							write (1, "if (strpos < string_end) {{");
						if (!ignore) {
							if (reverse)
								write (2, "char c = str [strpos - 1];");
							else
								write (2, "char c = str [strpos];");
						} else {
							if (reverse)
								write (2, "char c = Char.ToLower (str [strpos - 1]);");
							else
								write (2, "char c = Char.ToLower (str [strpos]);");
						}
						if (op.body != String.Empty)
							write (2, op.body);
						write (2, "if ({0}({1})) {{", revert ? "!" : "", op.cond);
						// TRUE case
						if (!revert) {
							if (!reverse)
								write (3, "strpos ++;");
							else
								write (3, "strpos --;");
							write (3, "if (char_group_end != 0)");
							write (4, "goto test_char_group_passed;");
							write (3, "pc += {0};", "" + op.len);
							write (3, "continue;");
						} else {
							/*
							 * If we are inside a char group, the cases are ANDed 
							 * together, so we have to continue checking the
							 * other cases, and we need to increase strpos after 
							 * the final check.
							 * The char group is termined by a True, hence the
							 * + 1 below.
							 */
							write (3, "pc += {0};", "" + op.len);
							write (3, "if (char_group_end == 0 || (pc + 1 == char_group_end)) {{");
							if (!reverse)
								write (4, "strpos ++;");
							else
								write (4, "strpos --;");
							write (3, "if (pc + 1 == char_group_end)");
							write (4, "goto test_char_group_passed;");
							write (3, "}}");
							write (3, "continue;");
						}
						write (2, "}}");
						write (1, "}}");

						// FALSE case							
						if (!revert) {
							write (1, "if (char_group_end == 0)");
							write (2, "return false;");
							write (1, "pc += {0};", "" + op.len);
							write (1, "continue;");
						} else {
							/* Fail both inside and outside a char group */
							write (1, "return false;");
						}

#if FALSE
							if (strpos < string_end && (COND (str [strpos]))) {
							if (!revert) {
								strpos ++;
								if (char_group_end != 0)
									goto test_char_group_passed;
								pc += ins_len;
								continue;
							} else {
								/*
								 * If we are inside a char group, the cases are ANDed 
								 * together, so we have to continue checking the
								 * other cases, and we need to increase strpos after 
								 * the final check.
								 * The char group is termined by a True, hence the
								 * + 1 below.
								 * FIXME: Optimize this.
								 */
								pc += ins_len;
								if (char_group_end == 0 || (pc + 1 == char_group_end))
									strpos ++;
								continue;
							}
						} else {
							if (!revert) {
								if (char_group_end == 0)
									return false;
								pc += ins_len;
								continue;
							} else {
								/* Fail both inside and outside a char group */
								return false;
							}
						}
					} else {
						// Same as above, but use:
						// - strpos > 0 instead of strpos < string_len
						// - COND (str [strpos - 1]) instead of COND (str [strpos])
						// - strpos -- instead of strpos ++
					}
#endif
					}
				}
			}
		}

	write (0, "");
	write (0, "// END OF GENERATED CODE");
	}
}
