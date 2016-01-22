// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
// 1/22/16 11:38 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2014-2016 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.security {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	
	/// <summary>
	/// A class implements OnVerifiedInterest if it has onVerifiedInterest which is
	/// called by verifyInterest to report a successful verification.
	/// </summary>
	///
	public interface OnVerifiedInterest {
		/// <summary>
		/// When verifyInterest succeeds, onVerifiedInterest is called.
		/// </summary>
		///
		/// <param name="interest">The interest object being verified.</param>
		void onVerifiedInterest(Interest interest);
	}
}
