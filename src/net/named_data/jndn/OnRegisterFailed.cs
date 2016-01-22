// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
// 1/22/16 11:38 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2013-2016 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// A class implements OnRegisterFailed if it has onRegisterFailed, used to pass
	/// a callback to Face.registerPrefix.
	/// </summary>
	///
	public interface OnRegisterFailed {
		/// <summary>
		/// If failed to retrieve the connected hub's ID or failed to register the
		/// prefix, onRegisterFailed is called.
		/// </summary>
		///
		/// <param name="prefix">The prefix given to registerPrefix.</param>
		void onRegisterFailed(Name prefix);
	}
}
