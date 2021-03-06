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
namespace net.named_data.jndn.encoding.der {
	
	using ILOG.J2CsMapping.NIO;
	using ILOG.J2CsMapping.Util;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn.encoding;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// DerNode implements the DER node types used in encoding/decoding DER-formatted
	/// data.
	/// </summary>
	///
	public class DerNode {
		/// <summary>
		/// Create a generic DER node with the given nodeType. This is a private
		/// constructor used by one of the public DerNode subclasses defined below.
		/// </summary>
		///
		/// <param name="nodeType">The DER node type, a value from DerNodeType.</param>
		private DerNode(int nodeType) {
			this.parent_ = null;
			this.header_ = ILOG.J2CsMapping.NIO.ByteBuffer.allocate(0);
			this.payload_ = new DynamicByteBuffer(0);
			nodeType_ = nodeType;
		}
	
		public virtual int getSize() {
			// payload_ is not flipped yet.
			return header_.remaining() + payload_.position();
		}
	
		/// <summary>
		/// Encode the given size and update the header.
		/// </summary>
		///
		/// <param name="size">The payload size to encode.</param>
		protected internal void encodeHeader(int size) {
			DynamicByteBuffer buffer = new DynamicByteBuffer(10);
			buffer.ensuredPut((byte) nodeType_);
			if (size < 0)
				// We don't expect this to happen since this is a protected method and
				// always called with the non-negative size() of some buffer.
				throw new Exception("encodeHeader: DER object has negative length");
			else if (size <= 127)
				buffer.ensuredPut((byte) (size & 0xff));
			else {
				DynamicByteBuffer tempBuf = new DynamicByteBuffer(10);
				// We encode backwards from the back.
				tempBuf.position(tempBuf.limit());
	
				int val = size;
				int n = 0;
				while (val != 0) {
					tempBuf.ensuredPutFromBack((byte) (val & 0xff));
					val >>= 8;
					n += 1;
				}
				tempBuf.ensuredPutFromBack((byte) (((1 << 7) | n) & 0xff));
	
				buffer.ensuredPut(tempBuf.buffer());
			}
	
			header_ = buffer.flippedBuffer();
		}
	
		/// <summary>
		/// Extract the header from an input buffer and return the size.
		/// </summary>
		///
		/// <param name="inputBuf">position.</param>
		/// <param name="startIdx">The offset into the buffer.</param>
		/// <returns>The parsed size in the header.</returns>
		protected internal int decodeHeader(ByteBuffer inputBuf, int startIdx) {
			int idx = startIdx;
	
			int nodeType = ((int) inputBuf.get(idx)) & 0xff;
			idx += 1;
	
			nodeType_ = nodeType;
	
			int sizeLen = ((int) inputBuf.get(idx)) & 0xff;
			idx += 1;
	
			DynamicByteBuffer header = new DynamicByteBuffer(10);
			header.ensuredPut((byte) nodeType);
			header.ensuredPut((byte) sizeLen);
	
			int size = sizeLen;
			bool isLongFormat = (sizeLen & (1 << 7)) != 0;
			if (isLongFormat) {
				int lenCount = sizeLen & ((1 << 7) - 1);
				size = 0;
				while (lenCount > 0) {
					if (inputBuf.limit() <= idx)
						throw new DerDecodingException(
								"DerNode.parse: The input length is too small");
					byte b = inputBuf.get(idx);
					idx += 1;
					header.ensuredPut(b);
					size = 256 * size + (((int) b) & 0xff);
					lenCount -= 1;
				}
			}
	
			header_ = header.flippedBuffer();
			return size;
		}
	
		/// <summary>
		/// Get the raw data encoding for this node.
		/// </summary>
		///
		/// <returns>The raw data encoding.</returns>
		public virtual Blob encode() {
			DynamicByteBuffer buffer = new DynamicByteBuffer(getSize());
	
			buffer.ensuredPut(header_);
			buffer.ensuredPut(payload_.flippedBuffer());
	
			return new Blob(buffer.flippedBuffer(), false);
		}
	
		/// <summary>
		/// Decode and store the data from an input buffer.
		/// </summary>
		///
		/// <param name="inputBuf">position.</param>
		/// <param name="startIdx">The offset into the buffer.</param>
		protected internal virtual void decode(ByteBuffer inputBuf, int startIdx) {
			int idx = startIdx;
			int payloadSize = decodeHeader(inputBuf, idx);
			int skipBytes = header_.remaining();
			if (payloadSize > 0) {
				idx += skipBytes;
				payload_.ensuredPut(inputBuf, idx, idx + payloadSize);
			}
		}
	
		/// <summary>
		/// Parse the data from the input buffer recursively and return the root as an
		/// object of a subclass of DerNode.
		/// </summary>
		///
		/// <param name="inputBuf">position.</param>
		/// <param name="startIdx">The offset into the buffer.</param>
		/// <returns>An object of a subclass of DerNode.</returns>
		public static DerNode parse(ByteBuffer inputBuf, int startIdx) {
			if (inputBuf.limit() <= startIdx)
				throw new DerDecodingException(
						"DerNode.parse: The input length is too small");
			int nodeType = ((int) inputBuf.get(startIdx)) & 0xff;
			// Don't increment idx. We're just peeking.
	
			DerNode newNode;
			if (nodeType == net.named_data.jndn.encoding.der.DerNodeType.Boolean)
				newNode = new DerNode.DerBoolean ();
			else if (nodeType == net.named_data.jndn.encoding.der.DerNodeType.Integer)
				newNode = new DerNode.DerInteger ();
			else if (nodeType == net.named_data.jndn.encoding.der.DerNodeType.BitString)
				newNode = new DerNode.DerBitString ();
			else if (nodeType == net.named_data.jndn.encoding.der.DerNodeType.OctetString)
				newNode = new DerNode.DerOctetString ();
			else if (nodeType == net.named_data.jndn.encoding.der.DerNodeType.Null)
				newNode = new DerNode.DerNull ();
			else if (nodeType == net.named_data.jndn.encoding.der.DerNodeType.ObjectIdentifier)
				newNode = new DerNode.DerOid ();
			else if (nodeType == net.named_data.jndn.encoding.der.DerNodeType.Sequence)
				newNode = new DerNode.DerSequence ();
			else if (nodeType == net.named_data.jndn.encoding.der.DerNodeType.PrintableString)
				newNode = new DerNode.DerPrintableString ();
			else if (nodeType == net.named_data.jndn.encoding.der.DerNodeType.GeneralizedTime)
				newNode = new DerNode.DerGeneralizedTime ();
			else if ((nodeType & 0xe0) == net.named_data.jndn.encoding.der.DerNodeType.ExplicitlyTagged)
				newNode = new DerNode.DerExplicitlyTagged (nodeType & 0x1f);
			else
				throw new DerDecodingException("Unimplemented DER type " + nodeType);
	
			newNode.decode(inputBuf, startIdx);
			return newNode;
		}
	
		/// <summary>
		/// Parse the data from the input buffer recursively and return the root as an
		/// object of a subclass of DerNode.
		/// </summary>
		///
		/// <param name="inputBuf"></param>
		/// <returns>An object of a subclass of DerNode.</returns>
		public static DerNode parse(ByteBuffer inputBuf) {
			return parse(inputBuf, inputBuf.position());
		}
	
		/// <summary>
		/// Convert the encoded data to a standard representation. Overridden by some
		/// subclasses (e.g. DerBoolean).
		/// </summary>
		///
		/// <returns>The encoded data as a Blob.</returns>
		public virtual Object toVal() {
			return encode();
		}
	
		/// <summary>
		/// Get a copy of the payload bytes.
		/// </summary>
		///
		/// <returns>A copy of the payload.</returns>
		public Blob getPayload() {
			return new Blob(payload_.flippedBuffer(), true);
		}
	
		/// <summary>
		/// If this object is a DerSequence, get the children of this node. Otherwise,
		/// throw an exception. (DerSequence overrides to implement this method.)
		/// </summary>
		///
		/// <returns>The children as a List of DerNode.</returns>
		/// <exception cref="DerDecodingException">if this object is not a DerSequence.</exception>
		public virtual IList getChildren() {
			throw new DerDecodingException(
					"getChildren: This DerNode is not DerSequence");
		}
	
		/// <summary>
		/// Check that index is in bounds for the children list, cast
		/// children.get(index) to DerSequence and return it.
		/// </summary>
		///
		/// <param name="children"></param>
		/// <param name="index">The index of the children.</param>
		/// <returns>children.get(index) cast to DerSequence.</returns>
		/// <exception cref="DerDecodingException">if index is out of bounds or ifchildren.get(index) is not a DerSequence.</exception>
		public static DerNode.DerSequence  getSequence(IList children, int index) {
			if (index < 0 || index >= children.Count)
				throw new DerDecodingException(
						"getSequence: Child index is out of bounds");
	
			if (!(children[index]   is  DerNode.DerSequence ))
				throw new DerDecodingException(
						"getSequence: Child DerNode is not DerSequence");
	
			return (DerNode.DerSequence ) children[index];
		}
	
		/// <summary>
		/// A DerStructure extends DerNode to hold other DerNodes.
		/// </summary>
		///
		public class DerStructure : DerNode {
			/// <summary>
			/// Create a DerStructure with the given nodeType. This is a private
			/// constructor. To create an object, use DerSequence.
			/// </summary>
			///
			/// <param name="nodeType">The DER node type, a value from DerNodeType.</param>
			public DerStructure(int nodeType) : base(nodeType) {
				this.childChanged_ = false;
				this.nodeList_ = new ArrayList<DerNode>();
				this.size_ = 0;
			}
	
			/// <summary>
			/// Get the total length of the encoding, including children.
			/// </summary>
			///
			/// <returns>The total (header + payload) length.</returns>
			public override int getSize() {
				if (childChanged_) {
					updateSize();
					childChanged_ = false;
				}
	
				encodeHeader(size_);
				return size_ + header_.remaining();
			}
	
			/// <summary>
			/// Get the children of this node.
			/// </summary>
			///
			/// <returns>The children as a List of DerNode.</returns>
			public sealed override IList getChildren() {
				return nodeList_;
			}
	
			public void updateSize() {
				int newSize = 0;
	
				for (int i = 0; i < nodeList_.Count; ++i) {
					DerNode n = nodeList_[i];
					newSize += n.getSize();
				}
	
				size_ = newSize;
				childChanged_ = false;
			}
	
			/// <summary>
			/// Add a child to this node.
			/// </summary>
			///
			/// <param name="node">The child node to add.</param>
			/// <param name="notifyParent"></param>
			public void addChild(DerNode node, bool notifyParent) {
				node.parent_ = this;
				ILOG.J2CsMapping.Collections.Collections.Add(nodeList_,node);
	
				if (notifyParent) {
					if (parent_ != null)
						parent_.setChildChanged();
				}
	
				childChanged_ = true;
			}
	
			public void addChild(DerNode node) {
				addChild(node, false);
			}
	
			/// <summary>
			/// Mark the child list as dirty, so that we update size when necessary.
			/// </summary>
			///
			public void setChildChanged() {
				if (parent_ != null)
					parent_.setChildChanged();
				childChanged_ = true;
			}
	
			/// <summary>
			/// Override the base encode to return raw data encoding for this node and
			/// its children
			/// </summary>
			///
			/// <returns>The raw data encoding.</returns>
			public override Blob encode() {
				DynamicByteBuffer temp = new DynamicByteBuffer(10);
				updateSize();
				encodeHeader(size_);
				temp.ensuredPut(header_);
	
				for (int i = 0; i < nodeList_.Count; ++i) {
					DerNode n = nodeList_[i];
					Blob encodedChild = n.encode();
					temp.ensuredPut(encodedChild.buf());
				}
	
				return new Blob(temp.flippedBuffer(), false);
			}
	
			/// <summary>
			/// Override the base decode to decode and store the data from an input
			/// buffer. Recursively populates child nodes.
			/// </summary>
			///
			/// <param name="inputBuf">position.</param>
			/// <param name="startIdx">The offset into the buffer.</param>
			protected internal override void decode(ByteBuffer inputBuf, int startIdx) {
				int idx = startIdx;
				size_ = decodeHeader(inputBuf, idx);
				idx += header_.remaining();
	
				int accSize = 0;
				while (accSize < size_) {
					DerNode node = net.named_data.jndn.encoding.der.DerNode.parse(inputBuf, idx);
					int size = node.getSize();
					idx += size;
					accSize += size;
					addChild(node, false);
				}
			}
	
			private bool childChanged_;
			private readonly ArrayList<DerNode> nodeList_;
			private int size_;
		}
	
		////////
		// Now for all the node types...
		////////
	
		/// <summary>
		/// A DerByteString extends DerNode to handle byte strings.
		/// </summary>
		///
		public class DerByteString : DerNode {
			/// <summary>
			/// Create a DerByteString with the given inputData and nodeType. This is a
			/// private constructor used by one of the public subclasses such as
			/// DerOctetString or DerPrintableString.
			/// </summary>
			///
			/// <param name="inputData"></param>
			/// <param name="nodeType">The specific DER node type, a value from DerNodeType.</param>
			public DerByteString(ByteBuffer inputData, int nodeType) : base(nodeType) {
				if (inputData != null) {
					payload_.ensuredPut(inputData);
					encodeHeader(inputData.remaining());
				}
			}
	
			/// <summary>
			/// Override to return just the byte string.
			/// </summary>
			///
			/// <returns>The byte string as a copy of the payload ByteBuffer.</returns>
			public override Object toVal() {
				return getPayload();
			}
		}
	
		/// <summary>
		/// DerBoolean extends DerNode to encode a boolean value.
		/// </summary>
		///
		public class DerBoolean : DerNode {
			/// <summary>
			/// Create a new DerBoolean for the value.
			/// </summary>
			///
			/// <param name="value">The value to encode.</param>
			public DerBoolean(bool value_ren) : base(net.named_data.jndn.encoding.der.DerNodeType.Boolean) {
				byte val = (value_ren) ? (byte) 0xff : (byte) 0x00;
				payload_.ensuredPut(val);
				encodeHeader(1);
			}
	
			public DerBoolean() : base(net.named_data.jndn.encoding.der.DerNodeType.Boolean) {
			}
	
			public override Object toVal() {
				byte val = payload_.buffer().get(0);
				return val != 0x00;
			}
		}
	
		/// <summary>
		/// DerInteger extends DerNode to encode an integer value.
		/// </summary>
		///
		public class DerInteger : DerNode {
			/// <summary>
			/// Create a new DerInteger for the value.
			/// </summary>
			///
			/// <param name="integer">The value to encode.</param>
			public DerInteger(int integer) : base(net.named_data.jndn.encoding.der.DerNodeType.Integer) {
				if (integer < 0)
					throw new DerEncodingException(
							"DerInteger: Negative integers are not currently supported");
	
				// Convert the integer to bytes the easy/slow way.
				DynamicByteBuffer temp = new DynamicByteBuffer(10);
				// We encode backwards from the back.
				temp.position(temp.limit());
				while (true) {
					temp.ensuredPutFromBack((byte) (integer & 0xff));
					integer >>= 8;
	
					if (integer <= 0)
						// We check for 0 at the end so we encode one byte if it is 0.
						break;
				}
	
				if ((((int) temp.buffer().get(temp.position())) & 0xff) >= 0x80)
					// Make it a non-negative integer.
					temp.ensuredPutFromBack((byte) 0);
	
				payload_.ensuredPut(temp.buffer().slice());
				encodeHeader(payload_.position());
			}
	
			/// <summary>
			/// Create a new DerInteger from the bytes in the buffer. If bytes represent
			/// a positive integer, you must ensure that the first byte is less than 0x80.
			/// </summary>
			///
			/// <param name="buffer"></param>
			/// <exception cref="DerEncodingException">if the first byte is not less than 0x80.</exception>
			public DerInteger(ByteBuffer buffer) : base(net.named_data.jndn.encoding.der.DerNodeType.Integer) {
				if (buffer.remaining() > 0
						&& (((int) buffer.get(buffer.position())) & 0xff) >= 0x80)
					throw new DerEncodingException(
							"DerInteger: Negative integers are not currently supported");
	
				if (buffer.remaining() == 0)
					payload_.ensuredPut((byte) 0);
				else
					payload_.ensuredPut(buffer);
	
				encodeHeader(payload_.position());
			}
	
			public DerInteger() : base(net.named_data.jndn.encoding.der.DerNodeType.Integer) {
			}
	
			public override Object toVal() {
				if (payload_.buffer().position() > 0
						&& (((int) payload_.buffer().get(0)) & 0xff) >= 0x80)
					throw new DerDecodingException(
							"DerInteger: Negative integers are not currently supported");
	
				int result = 0;
				// payload_ is not flipped yet.
				for (int i = 0; i < payload_.buffer().position(); ++i) {
					result <<= 8;
					// Use & 0xff in case byte was in the range -128 to -1.
					result += ((int) payload_.buffer().get(i)) & 0xff;
				}
	
				return result;
			}
		}
	
		/// <summary>
		/// A DerBitString extends DerNode to handle a bit string.
		/// </summary>
		///
		public class DerBitString : DerNode {
			/// <summary>
			/// Create a DerBitString with the given padding and inputBuf.
			/// </summary>
			///
			/// <param name="inputBuf"></param>
			/// <param name="paddingLen"></param>
			public DerBitString(ByteBuffer inputBuf, int paddingLen) : base(net.named_data.jndn.encoding.der.DerNodeType.BitString) {
				if (inputBuf != null) {
					payload_.ensuredPut((byte) (paddingLen & 0xff));
					payload_.ensuredPut(inputBuf);
					encodeHeader(payload_.position());
				}
			}
	
			public DerBitString() : base(net.named_data.jndn.encoding.der.DerNodeType.BitString) {
			}
		}
	
		/// <summary>
		/// DerOctetString extends DerByteString to encode a string of bytes.
		/// </summary>
		///
		public class DerOctetString : DerNode.DerByteString  {
			/// <summary>
			/// Create a new DerOctetString for the inputData.
			/// </summary>
			///
			/// <param name="inputData"></param>
			public DerOctetString(ByteBuffer inputData) : base(inputData, net.named_data.jndn.encoding.der.DerNodeType.OctetString) {
			}
	
			public DerOctetString() : base(null, net.named_data.jndn.encoding.der.DerNodeType.OctetString) {
			}
		}
	
		/// <summary>
		/// A DerNull extends DerNode to encode a null value.
		/// </summary>
		///
		public class DerNull : DerNode {
			/// <summary>
			/// Create a DerNull.
			/// </summary>
			///
			public DerNull() : base(net.named_data.jndn.encoding.der.DerNodeType.Null) {
				encodeHeader(0);
			}
		}
	
		/// <summary>
		/// A DerOid extends DerNode to represent an object identifier
		/// </summary>
		///
		public class DerOid : DerNode {
			/// <summary>
			/// Create a DerOid with the given object identifier. The object identifier
			/// string must begin with 0,1, or 2 and must contain at least 2 digits.
			/// </summary>
			///
			/// <param name="oidStr">The OID string to encode.</param>
			public DerOid(String oidStr) : base(net.named_data.jndn.encoding.der.DerNodeType.ObjectIdentifier) {
				String[] splitString = ILOG.J2CsMapping.Text.RegExUtil.Split(oidStr, "\\.");
				int[] parts = new int[splitString.Length];
				for (int i = 0; i < parts.Length; ++i)
					parts[i] = Int32.Parse(splitString[i]);
	
				prepareEncoding(parts);
			}
	
			/// <summary>
			/// Create a DerOid with the given object identifier. The object identifier
			/// must begin with 0,1, or 2 and must contain at least 2 digits.
			/// </summary>
			///
			/// <param name="oid">The OID to encode.</param>
			public DerOid(OID oid) : base(net.named_data.jndn.encoding.der.DerNodeType.ObjectIdentifier) {
				prepareEncoding(oid.getIntegerList());
			}
	
			public DerOid() : base(net.named_data.jndn.encoding.der.DerNodeType.ObjectIdentifier) {
			}
	
			/// <summary>
			/// Encode a sequence of integers into an OID object and set the payload.
			/// </summary>
			///
			/// <param name="value">The array of integers.</param>
			public void prepareEncoding(int[] value_ren) {
				int firstNumber = 0;
				if (value_ren.Length == 0)
					throw new DerEncodingException("No integer in OID");
				else {
					if (value_ren[0] >= 0 && value_ren[0] <= 2)
						firstNumber = value_ren[0] * 40;
					else
						throw new DerEncodingException(
								"First integer in OID is out of range");
				}
	
				if (value_ren.Length >= 2) {
					if (value_ren[1] >= 0 && value_ren[1] <= 39)
						firstNumber += value_ren[1];
					else
						throw new DerEncodingException(
								"Second integer in OID is out of range");
				}
	
				DynamicByteBuffer encodedBuffer = new DynamicByteBuffer(10);
				encodedBuffer.ensuredPut(encode128(firstNumber));
	
				if (value_ren.Length > 2) {
					for (int i = 2; i < value_ren.Length; ++i)
						encodedBuffer.ensuredPut(encode128(value_ren[i]));
				}
	
				encodeHeader(encodedBuffer.position());
				payload_.ensuredPut(encodedBuffer.flippedBuffer());
			}
	
			/// <summary>
			/// Compute the encoding for one part of an OID, where values greater than 128 must be encoded as multiple bytes.
			/// </summary>
			///
			/// <param name="value">A component of an OID.</param>
			/// <returns>The encoded buffer.</returns>
			public static ByteBuffer encode128(int value_ren) {
				int mask = (1 << 7) - 1;
				DynamicByteBuffer outBytes = new DynamicByteBuffer(10);
				// We encode backwards from the back.
				outBytes.position(outBytes.limit());
	
				if (value_ren < 128)
					outBytes.ensuredPutFromBack((byte) (value_ren & mask));
				else {
					outBytes.ensuredPutFromBack((byte) (value_ren & mask));
					value_ren >>= 7;
					while (value_ren != 0) {
						outBytes.ensuredPutFromBack((byte) ((value_ren & mask) | (1 << 7)));
						value_ren >>= 7;
					}
				}
	
				return outBytes.buffer().slice();
			}
	
			/// <summary>
			/// Convert an encoded component of the encoded OID to the original integer.
			/// </summary>
			///
			/// <param name="offset">The offset into this node's payload.</param>
			/// <param name="skip">Set skip[0] to the number of payload bytes to skip.</param>
			/// <returns>The original integer.</returns>
			public int decode128(int offset, int[] skip) {
				int flagMask = 0x80;
				int result = 0;
				int oldOffset = offset;
	
				while ((payload_.buffer().get(offset) & flagMask) != 0) {
					result = 128 * result
							+ ((int) payload_.buffer().get(offset) & 0xff) - 128;
					offset += 1;
				}
	
				result = result * 128
						+ ((int) payload_.buffer().get(offset) & 0xff);
	
				skip[0] = offset - oldOffset + 1;
				return result;
			}
	
			/// <summary>
			/// Override to return the string representation of the OID.
			/// </summary>
			///
			/// <returns>The string representation of the OID.</returns>
			public override Object toVal() {
				int offset = 0;
				ArrayList components = new ArrayList(); // of Integer.
	
				while (offset < payload_.position()) {
					int[] skip = new int[1];
					int nextVal = decode128(offset, skip);
					offset += skip[0];
					ILOG.J2CsMapping.Collections.Collections.Add(components,nextVal);
				}
	
				// for some odd reason, the first digits are represented in one byte
				int firstByte = ((Int32)components[0]);
				int firstDigit = firstByte / 40;
				int secondDigit = firstByte % 40;
	
				String result = firstDigit + "." + secondDigit;
				for (int i = 1; i < components.Count; ++i)
					result += "." + (Int32) components[i];
	
				return result;
			}
		}
	
		/// <summary>
		/// A DerSequence extends DerStructure to contains an ordered sequence of other
		/// nodes.
		/// </summary>
		///
		public class DerSequence : DerNode.DerStructure  {
			/// <summary>
			/// Create a DerSequence.
			/// </summary>
			///
			public DerSequence() : base(net.named_data.jndn.encoding.der.DerNodeType.Sequence) {
			}
		}
	
		/// <summary>
		/// A DerPrintableString extends DerByteString to handle a a printable string. No
		/// escaping or other modification is done to the string
		/// </summary>
		///
		public class DerPrintableString : DerNode.DerByteString  {
			/// <summary>
			/// Create a DerPrintableString with the given inputData.
			/// </summary>
			///
			/// <param name="inputData"></param>
			public DerPrintableString(ByteBuffer inputData) : base(inputData, net.named_data.jndn.encoding.der.DerNodeType.PrintableString) {
			}
	
			public DerPrintableString() : base(null, net.named_data.jndn.encoding.der.DerNodeType.PrintableString) {
			}
		}
	
		/// <summary>
		/// A DerGeneralizedTime extends DerNode to represent a date and time, with
		/// millisecond accuracy.
		/// </summary>
		///
		public class DerGeneralizedTime : DerNode {
			/// <summary>
			/// Create a DerGeneralizedTime with the given milliseconds since 1970.
			/// </summary>
			///
			/// <param name="msSince1970">The timestamp as milliseconds since Jan 1, 1970.</param>
			public DerGeneralizedTime(double msSince1970) : base(net.named_data.jndn.encoding.der.DerNodeType.GeneralizedTime) {
				String derTime = toDerTimeString(msSince1970);
				// Use Blob to convert to a ByteBuffer.
				payload_.ensuredPut(new Blob(derTime).buf());
				encodeHeader(payload_.position());
			}
	
			public DerGeneralizedTime() : base(net.named_data.jndn.encoding.der.DerNodeType.GeneralizedTime) {
			}
	
			/// <summary>
			/// Convert a UNIX timestamp to the internal string representation.
			/// </summary>
			///
			/// <param name="msSince1970">Timestamp as milliseconds since Jan 1, 1970.</param>
			/// <returns>The string representation.</returns>
			public static String toDerTimeString(double msSince1970) {
				DateTime utcTime = net.named_data.jndn.util.Common.millisecondsSince1970ToDate((long) Math.Round(msSince1970,MidpointRounding.AwayFromZero));
				return dateFormat_.format(utcTime);
			}
	
			/// <summary>
			/// Compute the date format for storing in the static variable dateFormat_.
			/// </summary>
			///
			public static SimpleDateFormat getDateFormat() {
				SimpleDateFormat dateFormat = new SimpleDateFormat(
						"yyyyMMddHHmmss'Z'");
				dateFormat.setTimeZone(System.Collections.TimeZone.getTimeZone("UTC"));
				return dateFormat;
			}
	
			/// <summary>
			/// Override to return the milliseconds since 1970.
			/// </summary>
			///
			/// <returns>The timestamp value as milliseconds since 1970 as a Double value.</returns>
			public override Object toVal() {
				// Use Blob to convert to a string.
				String timeStr = "" + new Blob(payload_.flippedBuffer(), false);
				try {
					DateTime date = dateFormat_.parse(timeStr);
					return (double) net.named_data.jndn.util.Common.dateToMillisecondsSince1970(date);
				} catch (ParseException ex) {
					throw new DerDecodingException(
							"DerGeneralizedTime: Error decoding the date string: "
									+ ex);
				}
			}
	
			private static readonly SimpleDateFormat dateFormat_ = getDateFormat();
		}
	
		/// <summary>
		/// A DerExplicitlyTagged extends DerNode to represent an explicitly-tagged
		/// type which wraps another DerNode.
		/// </summary>
		///
		public class DerExplicitlyTagged : DerNode {
			/// <summary>
			/// Create a DerExplicitlyTagged with the given tag number.
			/// </summary>
			///
			/// <param name="tagNumber">The explicit tag number from 0x00 to 0x1f.</param>
			public DerExplicitlyTagged(int tagNumber) : base(net.named_data.jndn.encoding.der.DerNodeType.ExplicitlyTagged) {
				this.innerNode_ = null;
				tagNumber_ = tagNumber;
			}
	
			/// <summary>
			/// Override the base encode to return raw data encoding for the explicit tag
			/// and encoded inner node.
			/// </summary>
			///
			/// <returns>The raw data encoding.</returns>
			public override Blob encode() {
				throw new NotSupportedException(
						"DerExplicitlyTagged.encode is not implemented");
			}
	
			/// <summary>
			/// Override the base decode to decode and store the inner DerNode.
			/// </summary>
			///
			/// <param name="inputBuf">position.</param>
			/// <param name="startIdx">The offset into the buffer.</param>
			protected internal override void decode(ByteBuffer inputBuf, int startIdx) {
				base.decode(inputBuf,startIdx);
				innerNode_ = net.named_data.jndn.encoding.der.DerNode.parse(getPayload().buf());
			}
	
			/// <summary>
			/// Get the tag number.
			/// </summary>
			///
			/// <returns>The tag number.</returns>
			public int getTagNumber() {
				return tagNumber_;
			}
	
			/// <summary>
			/// Get the inner node that is wrapped by the explicit tag.
			/// </summary>
			///
			/// <returns>The inner node, or null if node specified.</returns>
			public DerNode getInnerNode() {
				return innerNode_;
			}
	
			private readonly int tagNumber_;
			private DerNode innerNode_;
		}
	
		protected internal DerNode.DerStructure  parent_;
		// A value from DerNodeType.
		private int nodeType_;
		protected internal ByteBuffer header_;
		// NOTE: We never "flip" the internal buffer.  Its data is from 0 to position().
		protected internal DynamicByteBuffer payload_;
	}
}
