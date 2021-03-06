// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2017-2019 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.util.regex {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	
	public class NdnRegexComponentSetMatcher : NdnRegexMatcherBase {
		/// <summary>
		/// Create an NdnRegexComponentSetMatcher matcher from expr.
		/// </summary>
		///
		/// <param name="expr">The standard regular expression to match a component.</param>
		/// <param name="backrefManager">A back-reference manager.</param>
		public NdnRegexComponentSetMatcher(String expr,
				NdnRegexBackrefManager backrefManager) : base(expr, net.named_data.jndn.util.regex.NdnRegexMatcherBase.NdnRegexExprType.COMPONENT_SET, backrefManager) {
			this.components_ = new ArrayList<NdnRegexComponentMatcher>();
			this.isInclusion_ = true;
	
			compile();
		}
	
		public override bool match(Name name, int offset, int len) {
			bool isMatched = false;
	
			// ComponentSet only matches one component.
			if (len != 1)
				return false;
	
			/* foreach */
			foreach (NdnRegexComponentMatcher matcher  in  components_) {
				if (matcher.match(name, offset, len)) {
					isMatched = true;
					break;
				}
			}
	
			ILOG.J2CsMapping.Collections.Collections.Clear(matchResult_);
	
			if ((isInclusion_) ? isMatched : !isMatched) {
				ILOG.J2CsMapping.Collections.Collections.Add(matchResult_,name.get(offset));
				return true;
			} else
				return false;
		}
	
		/// <summary>
		/// Compile the regular expression to generate more matchers when necessary.
		/// </summary>
		///
		protected internal override void compile() {
			if (expr_.Length < 2)
				throw new NdnRegexMatcherBase.Error(
						"Regexp compile error (cannot parse " + expr_ + ")");
	
			if (expr_[0] == '<')
				compileSingleComponent();
			else if (expr_[0] == '[') {
				int lastIndex = expr_.Length - 1;
				if (']' != expr_[lastIndex])
					throw new NdnRegexMatcherBase.Error(
							"Regexp compile error (no matching ']' in " + expr_
									+ ")");
	
				if ('^' == expr_[1]) {
					isInclusion_ = false;
					compileMultipleComponents(2, lastIndex);
				} else
					compileMultipleComponents(1, lastIndex);
			} else
				throw new NdnRegexMatcherBase.Error(
						"Regexp compile error (cannot parse " + expr_ + ")");
		}
	
		private int extractComponent(int index) {
			int lcount = 1;
			int rcount = 0;
	
			while (lcount > rcount) {
				if (index >= expr_.Length)
					throw new NdnRegexMatcherBase.Error(
							"Error: angle brackets mismatch");
	
				if (expr_[index] == '<')
					++lcount;
				else if (expr_[index] == '>')
					++rcount;
	
				++index;
			}
	
			return index;
		}
	
		private void compileSingleComponent() {
			int end = extractComponent(1);
	
			if (expr_.Length != end)
				throw new NdnRegexMatcherBase.Error("Component expr error " + expr_);
			else {
				NdnRegexComponentMatcher component = new NdnRegexComponentMatcher(
						expr_.Substring(1,(end - 1)-(1)), backrefManager_);
	
				ILOG.J2CsMapping.Collections.Collections.Add(components_,component);
			}
		}
	
		private void compileMultipleComponents(int start, int lastIndex) {
			int index = start;
			int tempIndex = start;
	
			while (index < lastIndex) {
				if ('<' != expr_[index])
					throw new NdnRegexMatcherBase.Error("Component expr error "
							+ expr_);
	
				tempIndex = index + 1;
				index = extractComponent(tempIndex);
	
				NdnRegexComponentMatcher component = new NdnRegexComponentMatcher(
						expr_.Substring(tempIndex,(index - 1)-(tempIndex)), backrefManager_);
	
				ILOG.J2CsMapping.Collections.Collections.Add(components_,component);
			}
	
			if (index != lastIndex)
				throw new NdnRegexMatcherBase.Error("Not sufficient expr to parse "
						+ expr_);
		}
	
		private readonly ArrayList<NdnRegexComponentMatcher> components_;
		internal bool isInclusion_;
	}
}
