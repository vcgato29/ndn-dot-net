// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2013-2019 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.util {
	
	using ILOG.J2CsMapping.NIO;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// A SignedBlob extends Blob to keep the offsets of a signed portion (e.g., the
	/// bytes of Data packet).
	/// This inherits from Blob, including Blob.size and Blob.buf.
	/// </summary>
	///
	public class SignedBlob : Blob {
		/// <summary>
		/// Create a new SignedBlob with a null pointer and 0 for the offsets.
		/// </summary>
		///
		public SignedBlob() {
		}
	
		/// <summary>
		/// Create a new SignedBlob as a copy of the given signedBlob.
		/// </summary>
		///
		/// <param name="signedBlob">The SignedBlob to copy.</param>
		public SignedBlob(SignedBlob signedBlob) : base(signedBlob.buf(), false) {
			signedPortionBeginOffset_ = signedBlob.signedPortionBeginOffset_;
			signedPortionEndOffset_ = signedBlob.signedPortionEndOffset_;
			setSignedBuffer();
		}
	
		/// <summary>
		/// Create a new SignedBlob and take another pointer to the given blob's
		/// buffer.
		/// </summary>
		///
		/// <param name="blob">The Blob from which we take another pointer to the same buffer.</param>
		/// <param name="signedPortionBeginOffset"></param>
		/// <param name="signedPortionEndOffset"></param>
		public SignedBlob(Blob blob, int signedPortionBeginOffset,
				int signedPortionEndOffset) : base(blob) {
			signedPortionBeginOffset_ = signedPortionBeginOffset;
			signedPortionEndOffset_ = signedPortionEndOffset;
			setSignedBuffer();
		}
	
		/// <summary>
		/// Create a new SignedBlob from an existing ByteBuffer.  IMPORTANT: If copy
		/// is false,
		/// after calling this constructor, if you keep a pointer to the buffer then
		/// you must treat it as immutable and promise not to change it.
		/// </summary>
		///
		/// <param name="buffer"></param>
		/// <param name="copy"></param>
		/// <param name="signedPortionBeginOffset"></param>
		/// <param name="signedPortionEndOffset"></param>
		public SignedBlob(ByteBuffer buffer, bool copy,
				int signedPortionBeginOffset, int signedPortionEndOffset) : base(buffer, copy) {
			signedPortionBeginOffset_ = signedPortionBeginOffset;
			signedPortionEndOffset_ = signedPortionEndOffset;
			setSignedBuffer();
		}
	
		/// <summary>
		/// Create a new SignedBlob from the the byte array. IMPORTANT: If copy is false,
		/// after calling this constructor, if you keep a pointer to the buffer then
		/// you must treat it as immutable and promise not to change it.
		/// </summary>
		///
		/// <param name="value">The byte array. If copy is true, this makes a copy.</param>
		/// <param name="copy"></param>
		/// <param name="signedPortionBeginOffset"></param>
		/// <param name="signedPortionEndOffset"></param>
		public SignedBlob(byte[] value_ren, bool copy, int signedPortionBeginOffset,
				int signedPortionEndOffset) : base(value_ren, copy) {
			signedPortionBeginOffset_ = signedPortionBeginOffset;
			signedPortionEndOffset_ = signedPortionEndOffset;
			setSignedBuffer();
		}
	
		/// <summary>
		/// Get the length of the signed portion of the immutable byte buffer.
		/// </summary>
		///
		/// <returns>The length of the signed portion, or 0 if the pointer is null.</returns>
		public int signedSize() {
			if (signedBuffer_ != null)
				return signedBuffer_.limit();
			else
				return 0;
		}
	
		/// <summary>
		/// Get a new read-only ByteBuffer for the signed portion of the byte buffer.
		/// </summary>
		///
		/// <returns>The new ByteBuffer, or null if the pointer is null.</returns>
		public ByteBuffer signedBuf() {
			if (signedBuffer_ != null)
				// We call asReadOnlyBuffer each time because it is still allowed to
				//   change the position and limit on a read-only buffer, and we don't
				//   want the caller to modify our buffer_.
				return signedBuffer_.asReadOnlyBuffer();
			else
				return null;
		}
	
		/// <summary>
		/// Set up signedBuffer_ to a slice of buf() based on signedPortionBeginOffset_
		/// and signedPortionEndOffset_.
		/// </summary>
		///
		private void setSignedBuffer() {
			if (!isNull()) {
				// Note that the result of buf() is already a separate ByteBuffer, so it
				//   is OK to change the position.
				ByteBuffer tempBuffer = buf();
				tempBuffer.position(signedPortionBeginOffset_);
				tempBuffer.limit(signedPortionEndOffset_);
				// Get a slice which is limited to the signed portion.
				signedBuffer_ = tempBuffer.slice();
			} else
				signedBuffer_ = null;
		}
	
		private ByteBuffer signedBuffer_;
		private int signedPortionBeginOffset_;
		private int signedPortionEndOffset_;
	}
}
