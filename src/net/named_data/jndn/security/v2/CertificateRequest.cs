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
namespace net.named_data.jndn.security.v2 {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	
	/// <summary>
	/// A CertificateRequest represents a request for a certificate, associated with
	/// the number of retries left. The interest_ and nRetriesLeft_ fields are public
	/// so that you can modify them.
	/// </summary>
	///
	public class CertificateRequest {
		/// <summary>
		/// Create a CertificateRequest with a default Interest and 0 retries left.
		/// </summary>
		///
		public CertificateRequest() {
			interest_ = new Interest();
			nRetriesLeft_ = 0;
		}
	
		/// <summary>
		/// Create  a CertificateRequest for the Interest and 3 retries left.
		/// </summary>
		///
		/// <param name="interest">The Interest which is copied.</param>
		public CertificateRequest(Interest interest) {
			// Copy the Interest.
			interest_ = new Interest(interest);
			nRetriesLeft_ = 3;
		}
	
		/// <summary>
		/// The Interest for the requested Data packet or Certificate.
		/// </summary>
		///
		public Interest interest_;
		/// <summary>
		/// The number of remaining retries after time out or NACK.
		/// </summary>
		///
		public int nRetriesLeft_;
	}
}
