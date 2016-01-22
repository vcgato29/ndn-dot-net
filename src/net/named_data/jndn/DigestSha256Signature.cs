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
namespace net.named_data.jndn {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// A DigestSha256Signature extends Signature and holds the signature bits (which
	/// are only the SHA256 digest) and an empty SignatureInfo for a data packet or
	/// signed interest.
	/// </summary>
	///
	public class DigestSha256Signature : Signature {
		/// <summary>
		/// Create a new DigestSha256Signature with default values.
		/// </summary>
		///
		public DigestSha256Signature() {
			this.signature_ = new Blob();
			this.changeCount_ = 0;
		}
	
		/// <summary>
		/// Create a new DigestSha256Signature with a copy of the fields in the given
		/// signature object.
		/// </summary>
		///
		/// <param name="signature">The signature object to copy.</param>
		public DigestSha256Signature(DigestSha256Signature signature) {
			this.signature_ = new Blob();
			this.changeCount_ = 0;
			signature_ = signature.signature_;
		}
	
		/// <summary>
		/// Return a new DigestSha256Signature which is a deep copy of this
		/// DigestSha256Signature.
		/// </summary>
		///
		/// <returns>A new DigestSha256Signature.</returns>
		/// <exception cref="System.Exception"></exception>
		public override Object Clone() {
			return new DigestSha256Signature(this);
		}
	
		/// <summary>
		/// Get the signature bytes.
		/// </summary>
		///
		/// <returns>The signature bytes. If not specified, the value isNull().</returns>
		public sealed override Blob getSignature() {
			return signature_;
		}
	
		/// <summary>
		/// Set the signature bytes to the given value.
		/// </summary>
		///
		/// <param name="signature">A Blob with the signature bytes.</param>
		public sealed override void setSignature(Blob signature) {
			signature_ = ((signature == null) ? new Blob() : signature);
			++changeCount_;
		}
	
		/// <summary>
		/// Get the change count, which is incremented each time this object
		/// (or a child object) is changed.
		/// </summary>
		///
		/// <returns>The change count.</returns>
		public sealed override long getChangeCount() {
			return changeCount_;
		}
	
		private Blob signature_;
		private long changeCount_;
	}
}
