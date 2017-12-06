// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2017 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.security.v2 {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	
	public abstract class CertificateContainerInterface {
		/// <summary>
		/// Add the certificate to the container.
		/// </summary>
		///
		/// <param name="certificate">The certificate to add, which is copied.</param>
		public abstract void add(CertificateV2 certificate);
	
		/// <summary>
		/// Remove the certificate with the given name. If the name does not exist,
		/// do nothing.
		/// </summary>
		///
		/// <param name="certificateName">The name of the certificate.</param>
		public abstract void remove(Name certificateName);
	}
}
