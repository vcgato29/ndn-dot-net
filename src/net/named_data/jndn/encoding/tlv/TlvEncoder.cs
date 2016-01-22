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
namespace net.named_data.jndn.encoding.tlv {
	
	using ILOG.J2CsMapping.NIO;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// A TlvEncoder holds an output buffer and has methods to output NDN-TLV.
	/// </summary>
	///
	public class TlvEncoder {
		/// <summary>
		/// Create a new TlvEncoder to use a DynamicByteBuffer with the initialCapacity.
		/// When done, you should call getOutput().
		/// </summary>
		///
		/// <param name="initialCapacity">The initial capacity of buffer().</param>
		public TlvEncoder(int initialCapacity) {
			output_ = new DynamicByteBuffer(initialCapacity);
			// We will start encoding from the back.
			output_.position(output_.limit());
		}
	
		/// <summary>
		/// Create a new TlvEncoder with a default DynamicByteBuffer.
		/// When done, you should call getOutput().
		/// </summary>
		///
		public TlvEncoder() {
			output_ = new DynamicByteBuffer(16);
			// We will start encoding from the back.
			output_.position(output_.limit());
		}
	
		/// <summary>
		/// Get the number of bytes that have been written to the output.  You can
		/// save this number, write sub TLVs, then subtract the new length from this
		/// to get the total length of the sub TLVs.
		/// </summary>
		///
		/// <returns>The number of bytes that have been written to the output.</returns>
		public int getLength() {
			return output_.remaining();
		}
	
		/// <summary>
		/// Encode varNumber as a VAR-NUMBER in NDN-TLV and write it to the output just
		/// before getLength() from the back.  Advance getLength().
		/// </summary>
		///
		/// <param name="varNumber"></param>
		public void writeVarNumber(int varNumber) {
			if (varNumber < 253) {
				int position = output_
						.setRemainingFromBack(output_.remaining() + 1);
				output_.buffer().put(position, (byte) (varNumber & 0xff));
			} else if (varNumber <= 0xffff) {
				int position_0 = output_
						.setRemainingFromBack(output_.remaining() + 3);
				output_.buffer().put(position_0, (byte) 253);
				output_.buffer()
						.put(position_0 + 1, (byte) ((varNumber >> 8) & 0xff));
				output_.buffer().put(position_0 + 2, (byte) (varNumber & 0xff));
			} else {
				// A Java int is 32 bits so ignore a 64-bit VAR-NUMBER.
				int position_1 = output_
						.setRemainingFromBack(output_.remaining() + 5);
				output_.buffer().put(position_1, (byte) 254);
				output_.buffer().put(position_1 + 1,
						(byte) ((varNumber >> 24) & 0xff));
				output_.buffer().put(position_1 + 2,
						(byte) ((varNumber >> 16) & 0xff));
				output_.buffer()
						.put(position_1 + 3, (byte) ((varNumber >> 8) & 0xff));
				output_.buffer().put(position_1 + 4, (byte) (varNumber & 0xff));
			}
		}
	
		/// <summary>
		/// Encode the type and length as VAR-NUMBER and write to the output just
		/// before getLength() from the back.  Advance getLength().
		/// </summary>
		///
		/// <param name="type"></param>
		/// <param name="length"></param>
		public void writeTypeAndLength(int type, int length) {
			// Write backwards.
			writeVarNumber(length);
			writeVarNumber(type);
		}
	
		/// <summary>
		/// Encode value as a non-negative integer and write it to the output just
		/// before getLength() from  the back. Advance getLength(). This does not write
		/// a type or length for the value.
		/// </summary>
		///
		/// <param name="value">a Java long is signed).</param>
		/// <exception cref="System.Exception">if the value is negative.</exception>
		public void writeNonNegativeInteger(long value_ren) {
			if (value_ren < 0)
				throw new Exception("TLV integer value may not be negative");
	
			// Write backwards.
			if (value_ren <= 0xffL) {
				int position = output_
						.setRemainingFromBack(output_.remaining() + 1);
				output_.buffer().put(position, (byte) (value_ren & 0xff));
			} else if (value_ren <= 0xffffL) {
				int position_0 = output_
						.setRemainingFromBack(output_.remaining() + 2);
				output_.buffer().put(position_0, (byte) ((value_ren >> 8) & 0xff));
				output_.buffer().put(position_0 + 1, (byte) (value_ren & 0xff));
			} else if (value_ren <= 0xffffffffL) {
				int position_1 = output_
						.setRemainingFromBack(output_.remaining() + 4);
				output_.buffer().put(position_1, (byte) ((value_ren >> 24) & 0xff));
				output_.buffer().put(position_1 + 1, (byte) ((value_ren >> 16) & 0xff));
				output_.buffer().put(position_1 + 2, (byte) ((value_ren >> 8) & 0xff));
				output_.buffer().put(position_1 + 3, (byte) (value_ren & 0xff));
			} else {
				int position_2 = output_
						.setRemainingFromBack(output_.remaining() + 8);
				output_.buffer().put(position_2, (byte) ((value_ren >> 56) & 0xff));
				output_.buffer().put(position_2 + 1, (byte) ((value_ren >> 48) & 0xff));
				output_.buffer().put(position_2 + 2, (byte) ((value_ren >> 40) & 0xff));
				output_.buffer().put(position_2 + 3, (byte) ((value_ren >> 32) & 0xff));
				output_.buffer().put(position_2 + 4, (byte) ((value_ren >> 24) & 0xff));
				output_.buffer().put(position_2 + 5, (byte) ((value_ren >> 16) & 0xff));
				output_.buffer().put(position_2 + 6, (byte) ((value_ren >> 8) & 0xff));
				output_.buffer().put(position_2 + 7, (byte) (value_ren & 0xff));
			}
		}
	
		/// <summary>
		/// Write the type, then the length of the encoded value then encode value as a
		/// non-negative integer and write it to the output just before getLength()
		/// from  the back. Advance getLength().
		/// </summary>
		///
		/// <param name="type"></param>
		/// <param name="value">a Java long is signed).</param>
		/// <exception cref="System.Exception">if the value is negative.</exception>
		public void writeNonNegativeIntegerTlv(int type, long value_ren) {
			// Write backwards.
			int saveNBytes = output_.remaining();
			writeNonNegativeInteger(value_ren);
			writeTypeAndLength(type, output_.remaining() - saveNBytes);
		}
	
		/// <summary>
		/// If value is negative or null then do nothing, otherwise call
		/// writeNonNegativeIntegerTlv.
		/// </summary>
		///
		/// <param name="type"></param>
		/// <param name="value">63-bit because a Java long is signed).</param>
		public void writeOptionalNonNegativeIntegerTlv(int type, long value_ren) {
			if (value_ren >= 0)
				writeNonNegativeIntegerTlv(type, value_ren);
		}
	
		/// <summary>
		/// If value is negative or null then do nothing, otherwise call
		/// writeNonNegativeIntegerTlv.
		/// </summary>
		///
		/// <param name="type"></param>
		/// <param name="value">If negative do nothing, otherwise use (long)Math.round(value).</param>
		public void writeOptionalNonNegativeIntegerTlvFromDouble(int type,
				double value_ren) {
			if (value_ren >= 0.0d)
				writeNonNegativeIntegerTlv(type, (long) Math.Round(value_ren,MidpointRounding.AwayFromZero));
		}
	
		/// <summary>
		/// Write the buffer from its position() to limit() to the output just
		/// before getLength() from the back. Advance getLength() of the output. This
		/// does NOT change buffer.position(). Note that this does not encode a type
		/// and length; for that see writeBlobTlv.
		/// </summary>
		///
		/// <param name="buffer"></param>
		public void writeBuffer(ByteBuffer buffer) {
			if (buffer == null)
				return;
	
			// Write backwards.
			int position = output_.setRemainingFromBack(output_.remaining()
					+ buffer.remaining());
			int saveBufferValuePosition = buffer.position();
			output_.buffer().put(buffer);
			// Restore positions after put.
			output_.position(position);
			buffer.position(saveBufferValuePosition);
		}
	
		/// <summary>
		/// Write the type, then the length of the buffer then the buffer value from
		/// its position() to limit() to the output just before getLength() from the
		/// back. Advance getLength() of the output. This does NOT change
		/// value.position().
		/// </summary>
		///
		/// <param name="type"></param>
		/// <param name="value"></param>
		public void writeBlobTlv(int type, ByteBuffer value_ren) {
			if (value_ren == null) {
				writeTypeAndLength(type, 0);
				return;
			}
	
			// Write backwards.
			writeBuffer(value_ren);
			writeTypeAndLength(type, value_ren.remaining());
		}
	
		/// <summary>
		/// If the byte buffer value is null or value.remaining() is zero then do
		/// nothing, otherwise call writeBlobTlv.
		/// </summary>
		///
		/// <param name="type"></param>
		/// <param name="value"></param>
		public void writeOptionalBlobTlv(int type, ByteBuffer value_ren) {
			if (value_ren != null && value_ren.remaining() > 0)
				writeBlobTlv(type, value_ren);
		}
	
		/// <summary>
		/// Return a slice of the output buffer up to the current length of the output
		/// encoding.
		/// </summary>
		///
		/// <returns>A ByteBuffer which shares the same underlying buffer with the
		/// output buffer.</returns>
		public ByteBuffer getOutput() {
			// The output buffer position is already at the beginning of the encoding.
			return output_.buffer().slice();
		}
	
		private readonly DynamicByteBuffer output_;
	}
}
