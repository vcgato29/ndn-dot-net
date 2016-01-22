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
	
	using ILOG.J2CsMapping.NIO;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	using net.named_data.jndn.encoding;
	using net.named_data.jndn.encoding.tlv;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// A Name holds an array of Name.Component and represents an NDN name.
	/// </summary>
	///
	public class Name : ChangeCountable, IComparable {
		/// <summary>
		/// A Name.Component holds a read-only name component value.
		/// </summary>
		///
		public class Component : IComparable {
			/// <summary>
			/// Create a new Name.Component with a zero-length value.
			/// </summary>
			///
			public Component() {
				value_ = new Blob(ILOG.J2CsMapping.NIO.ByteBuffer.allocate(0), false);
			}
	
			/// <summary>
			/// Create a new Name.Component, using the existing the Blob value.
			/// </summary>
			///
			/// <param name="value"></param>
			public Component(Blob value_ren) {
				if (value_ren == null)
					throw new NullReferenceException(
							"Component: Blob value may not be null");
				value_ = value_ren;
			}
	
			/// <summary>
			/// Create a new Name.Component, taking another pointer to the component's
			/// read-only value.
			/// </summary>
			///
			/// <param name="component">The component to copy.</param>
			public Component(Name.Component  component) {
				value_ = component.value_;
			}
	
			/// <summary>
			/// Create a new Name.Component, copying the given value.
			/// </summary>
			///
			/// <param name="value">The value byte array.</param>
			public Component(byte[] value_ren) {
				value_ = new Blob(value_ren);
			}
	
			/// <summary>
			/// Create a new Name.Component, converting the value to UTF8 bytes.
			/// Note, this does not escape %XX values.  If you need to escape, use
			/// Name.fromEscapedString.
			/// </summary>
			///
			/// <param name="value">The string to convert to UTF8.</param>
			public Component(String value_ren) {
				value_ = new Blob(value_ren);
			}
	
			/// <summary>
			/// Get the component value.
			/// </summary>
			///
			/// <returns>The component value.</returns>
			public Blob getValue() {
				return value_;
			}
	
			/// <summary>
			/// Write this component value to result, escaping characters according to
			/// the NDN URI Scheme. This also adds "..." to a value with zero or more ".".
			/// </summary>
			///
			/// <param name="result">The StringBuffer to write to.</param>
			public void toEscapedString(StringBuilder result) {
				net.named_data.jndn.Name.toEscapedString(value_.buf(), result);
			}
	
			/// <summary>
			/// Convert this component value by escaping characters according to the
			/// NDN URI Scheme. This also adds "..." to a value with zero or more ".".
			/// </summary>
			///
			/// <returns>The escaped string.</returns>
			public String toEscapedString() {
				return net.named_data.jndn.Name.toEscapedString(value_.buf());
			}
	
			/// <summary>
			/// Interpret this name component as a network-ordered number and return an
			/// integer.
			/// </summary>
			///
			/// <returns>The integer number.</returns>
			public long toNumber() {
				ByteBuffer buffer = value_.buf();
				if (buffer == null)
					return 0;
	
				long result = 0;
				for (int i = buffer.position(); i < buffer.limit(); ++i) {
					result *= 256;
					result += (long) ((int) buffer.get(i) & 0xff);
				}
	
				return result;
			}
	
			/// <summary>
			/// Interpret this name component as a network-ordered number with a marker
			/// and return an integer.
			/// </summary>
			///
			/// <param name="marker">The required first byte of the component.</param>
			/// <returns>The integer number.</returns>
			/// <exception cref="EncodingException">If the first byte of the component does notequal the marker.</exception>
			public long toNumberWithMarker(int marker) {
				ByteBuffer buffer = value_.buf();
				if (buffer == null || buffer.remaining() <= 0
						|| buffer.get(0) != (byte) marker)
					throw new EncodingException(
							"Name component does not begin with the expected marker.");
	
				long result = 0;
				for (int i = buffer.position() + 1; i < buffer.limit(); ++i) {
					result *= 256;
					result += (long) ((int) buffer.get(i) & 0xff);
				}
	
				return result;
			}
	
			/// <summary>
			/// Interpret this name component as a segment number according to NDN naming
			/// conventions for "Segment number" (marker 0x00).
			/// http://named-data.net/doc/tech-memos/naming-conventions.pdf
			/// </summary>
			///
			/// <returns>The integer segment number.</returns>
			/// <exception cref="EncodingException">If the first byte of the component is not theexpected marker.</exception>
			public long toSegment() {
				return toNumberWithMarker(0x00);
			}
	
			/// <summary>
			/// Interpret this name component as a segment byte offset according to NDN
			/// naming conventions for segment "Byte offset" (marker 0xFB).
			/// http://named-data.net/doc/tech-memos/naming-conventions.pdf
			/// </summary>
			///
			/// <returns>The integer segment byte offset.</returns>
			/// <exception cref="EncodingException">If the first byte of the component is not theexpected marker.</exception>
			public long toSegmentOffset() {
				return toNumberWithMarker(0xFB);
			}
	
			/// <summary>
			/// Interpret this name component as a version number  according to NDN naming
			/// conventions for "Versioning" (marker 0xFD). Note that this returns
			/// the exact number from the component without converting it to a time
			/// representation.
			/// </summary>
			///
			/// <returns>The integer version number.</returns>
			/// <exception cref="EncodingException">If the first byte of the component is not theexpected marker.</exception>
			public long toVersion() {
				return toNumberWithMarker(0xFD);
			}
	
			/// <summary>
			/// Interpret this name component as a timestamp  according to NDN naming
			/// conventions for "Timestamp" (marker 0xFC).
			/// http://named-data.net/doc/tech-memos/naming-conventions.pdf
			/// </summary>
			///
			/// <returns>The number of microseconds since the UNIX epoch (Thursday,
			/// 1 January 1970) not counting leap seconds.</returns>
			/// <exception cref="EncodingException">If the first byte of the component is not theexpected marker.</exception>
			public long toTimestamp() {
				return toNumberWithMarker(0xFC);
			}
	
			/// <summary>
			/// Interpret this name component as a sequence number according to NDN naming
			/// conventions for "Sequencing" (marker 0xFE).
			/// http://named-data.net/doc/tech-memos/naming-conventions.pdf
			/// </summary>
			///
			/// <returns>The integer sequence number.</returns>
			/// <exception cref="EncodingException">If the first byte of the component is not theexpected marker.</exception>
			public long toSequenceNumber() {
				return toNumberWithMarker(0xFE);
			}
	
			/// <summary>
			/// Create a component whose value is the nonNegativeInteger encoding of the
			/// number.
			/// </summary>
			///
			/// <param name="number">The number to be encoded.</param>
			/// <returns>The component value.</returns>
			public static Name.Component  fromNumber(long number) {
				if (number < 0)
					number = 0;
	
				TlvEncoder encoder = new TlvEncoder(8);
				encoder.writeNonNegativeInteger(number);
				return new Name.Component (new Blob(encoder.getOutput(), false));
			}
	
			/// <summary>
			/// Create a component whose value is the marker appended with the
			/// nonNegativeInteger encoding of the number.
			/// </summary>
			///
			/// <param name="number">The number to be encoded.</param>
			/// <param name="marker">The marker to use as the first byte of the component.</param>
			/// <returns>The component value.</returns>
			public static Name.Component  fromNumberWithMarker(long number, int marker) {
				if (number < 0)
					number = 0;
	
				TlvEncoder encoder = new TlvEncoder(9);
				// Encode backwards.
				encoder.writeNonNegativeInteger(number);
				encoder.writeNonNegativeInteger((long) marker);
				return new Name.Component (new Blob(encoder.getOutput(), false));
			}
	
			/// <summary>
			/// Check if this is the same component as other.
			/// </summary>
			///
			/// <param name="other">The other Component to compare with.</param>
			/// <returns>True if the components are equal, otherwise false.</returns>
			public bool equals(Name.Component  other) {
				return value_.equals(other.value_);
			}
	
			public override bool Equals(Object other) {
				if (!(other  is  Name.Component ))
					return false;
	
				return equals((Name.Component ) other);
			}
	
			public override int GetHashCode() {
				return value_.GetHashCode();
			}
	
			/// <summary>
			/// Compare this to the other Component using NDN canonical ordering.
			/// </summary>
			///
			/// <param name="other">The other Component to compare with.</param>
			/// <returns>0 If they compare equal, -1 if this comes before other in the
			/// canonical ordering, or 1 if this comes after other in the canonical
			/// ordering.</returns>
			public int compare(Name.Component  other) {
				if (value_.size() < other.value_.size())
					return -1;
				if (value_.size() > other.value_.size())
					return 1;
	
				// The components are equal length. Just do a byte compare.
				return value_.compare(other.value_);
			}
	
			public int compareTo(Object o) {
				return this.compare((Name.Component ) o);
			}
	
			// Also include this version for portability.
			public int CompareTo(Object o) {
				return this.compare((Name.Component ) o);
			}
	
			/// <summary>
			/// Reverse the bytes in buffer starting at position, up to but not including
			/// limit.
			/// </summary>
			///
			/// <param name="buffer"></param>
			/// <param name="position"></param>
			/// <param name="limit"></param>
			public static void reverse(ByteBuffer buffer, int position, int limit) {
				int from = position;
				int to = limit - 1;
				while (from < to) {
					// swap
					byte temp = buffer.get(from);
					buffer.put(from, buffer.get(to));
					buffer.put(to, temp);
	
					--to;
					++from;
				}
			}
	
			private readonly Blob value_;
		}
	
		/// <summary>
		/// Create a new Name with no components.
		/// </summary>
		///
		public Name() {
			this.changeCount_ = 0;
			this.haveHashCode_ = false;
			this.hashCodeChangeCount_ = 0;
			components_ = new ArrayList();
		}
	
		/// <summary>
		/// Create a new Name with the components in the given name.
		/// </summary>
		///
		/// <param name="name">The name with components to copy from.</param>
		public Name(Name name) {
			this.changeCount_ = 0;
			this.haveHashCode_ = false;
			this.hashCodeChangeCount_ = 0;
			components_ = new ArrayList(name.components_);
		}
	
		/// <summary>
		/// Create a new Name, copying the components.
		/// </summary>
		///
		/// <param name="components">The components to copy.</param>
		public Name(ArrayList components) {
			this.changeCount_ = 0;
			this.haveHashCode_ = false;
			this.hashCodeChangeCount_ = 0;
			// Don't need to deep-copy Component elements because they are read-only.
			components_ = new ArrayList(components);
		}
	
		/// <summary>
		/// Create a new Name, copying the components.
		/// </summary>
		///
		/// <param name="components">The components to copy.</param>
		public Name(Name.Component [] components) {
			this.changeCount_ = 0;
			this.haveHashCode_ = false;
			this.hashCodeChangeCount_ = 0;
			components_ = new ArrayList();
			for (int i = 0; i < components.Length; ++i)
				ILOG.J2CsMapping.Collections.Collections.Add(components_,components[i]);
		}
	
		/// <summary>
		/// Parse the uri according to the NDN URI Scheme and create the name with the
		/// components.
		/// </summary>
		///
		/// <param name="uri">The URI string.</param>
		public Name(String uri) {
			this.changeCount_ = 0;
			this.haveHashCode_ = false;
			this.hashCodeChangeCount_ = 0;
			components_ = new ArrayList();
			set(uri);
		}
	
		/// <summary>
		/// Get the number of components.
		/// </summary>
		///
		/// <returns>The number of components.</returns>
		public int size() {
			return components_.Count;
		}
	
		/// <summary>
		/// Get the component at the given index.
		/// </summary>
		///
		/// <param name="i"></param>
		/// <returns>The name component at the index.</returns>
		public Name.Component  get(int i) {
			if (i >= 0)
				return (Name.Component ) components_[i];
			else
				return (Name.Component ) components_[components_.Count - (-i)];
		}
	
		public void set(String uri) {
			clear();
	
			uri = uri.trim();
			if (uri.Length == 0)
				return;
	
			int iColon = uri.indexOf(':');
			if (iColon >= 0) {
				// Make sure the colon came before a '/'.
				int iFirstSlash = uri.indexOf('/');
				if (iFirstSlash < 0 || iColon < iFirstSlash)
					// Omit the leading protocol such as ndn:
					uri = uri.Substring(iColon + 1).trim();
			}
	
			// Trim the leading slash and possibly the authority.
			if (uri[0] == '/') {
				if (uri.Length >= 2 && uri[1] == '/') {
					// Strip the authority following "//".
					int iAfterAuthority = uri.indexOf('/', 2);
					if (iAfterAuthority < 0)
						// Unusual case: there was only an authority.
						return;
					else
						uri = uri.Substring(iAfterAuthority + 1).trim();
				} else
					uri = uri.Substring(1).trim();
			}
	
			int iComponentStart = 0;
	
			// Unescape the components.
			while (iComponentStart < uri.Length) {
				int iComponentEnd = ILOG.J2CsMapping.Util.StringUtil.IndexOf(uri,"/",iComponentStart);
				if (iComponentEnd < 0)
					iComponentEnd = uri.Length;
	
				Name.Component  component = new Name.Component (fromEscapedString(uri,
						iComponentStart, iComponentEnd));
				// Ignore illegal components.  This also gets rid of a trailing '/'.
				if (!component.getValue().isNull())
					append(component);
	
				iComponentStart = iComponentEnd + 1;
			}
		}
	
		/// <summary>
		/// Clear all the components.
		/// </summary>
		///
		public void clear() {
			ILOG.J2CsMapping.Collections.Collections.Clear(components_);
			++changeCount_;
		}
	
		/// <summary>
		/// Append a new component, copying from value.
		/// </summary>
		///
		/// <param name="value">The component value.</param>
		/// <returns>This name so that you can chain calls to append.</returns>
		public Name append(byte[] value_ren) {
			return append(new Name.Component (value_ren));
		}
	
		/// <summary>
		/// Append a new component, using the existing Blob value.
		/// </summary>
		///
		/// <param name="value">The component value.</param>
		/// <returns>This name so that you can chain calls to append.</returns>
		public Name append(Blob value_ren) {
			return append(new Name.Component (value_ren));
		}
	
		/// <summary>
		/// Append the component to this name.
		/// </summary>
		///
		/// <param name="component">The component to append.</param>
		/// <returns>This name so that you can chain calls to append.</returns>
		public Name append(Name.Component  component) {
			ILOG.J2CsMapping.Collections.Collections.Add(components_,component);
			++changeCount_;
			return this;
		}
	
		public Name append(Name name) {
			if (name == this)
				// Copying from this name, so need to make a copy first.
				return append(new Name(name));
	
			for (int i = 0; i < name.components_.Count; ++i)
				append(name.get(i));
	
			return this;
		}
	
		/// <summary>
		/// Convert the value to UTF8 bytes and append a Name.Component.
		/// Note, this does not escape %XX values.  If you need to escape, use
		/// Name.fromEscapedString.  Also, if the string has "/", this does not split
		/// into separate components.  If you need that then use
		/// append(new Name(value)).
		/// </summary>
		///
		/// <param name="value">The string to convert to UTF8.</param>
		/// <returns>This name so that you can chain calls to append.</returns>
		public Name append(String value_ren) {
			return append(new Name.Component (value_ren));
		}
	
		/// <summary>
		/// Get a new name, constructed as a subset of components.
		/// </summary>
		///
		/// <param name="iStartComponent">name.size() - N.</param>
		/// <param name="nComponents">The number of components starting at iStartComponent.</param>
		/// <returns>A new name.</returns>
		public Name getSubName(int iStartComponent, int nComponents) {
			if (iStartComponent < 0)
				iStartComponent = components_.Count - (-iStartComponent);
	
			Name result = new Name();
	
			int iEnd = iStartComponent + nComponents;
			for (int i = iStartComponent; i < iEnd && i < components_.Count; ++i)
				ILOG.J2CsMapping.Collections.Collections.Add(result.components_,components_[i]);
	
			return result;
		}
	
		/// <summary>
		/// Get a new name, constructed as a subset of components starting at
		/// iStartComponent until the end of the name.
		/// </summary>
		///
		/// <param name="iStartComponent">name.size() - N.</param>
		/// <returns>A new name.</returns>
		public Name getSubName(int iStartComponent) {
			if (iStartComponent < 0)
				iStartComponent = components_.Count - (-iStartComponent);
	
			Name result = new Name();
	
			for (int i = iStartComponent; i < components_.Count; ++i)
				ILOG.J2CsMapping.Collections.Collections.Add(result.components_,components_[i]);
	
			return result;
		}
	
		/// <summary>
		/// Return a new Name with the first nComponents components of this Name.
		/// </summary>
		///
		/// <param name="nComponents">returns the name without the final component.</param>
		/// <returns>A new Name.</returns>
		public Name getPrefix(int nComponents) {
			if (nComponents < 0)
				return getSubName(0, components_.Count + nComponents);
			else
				return getSubName(0, nComponents);
		}
	
		/// <summary>
		/// Encode this name as a URI according to the NDN URI Scheme.
		/// </summary>
		///
		/// <param name="includeScheme">which is normally the case where toUri() is used for display.</param>
		/// <returns>The URI string.</returns>
		public String toUri(bool includeScheme) {
			if ((components_.Count==0))
				return (includeScheme) ? "ndn:/" : "/";
	
			StringBuilder result = new StringBuilder();
			if (includeScheme)
				result.append("ndn:");
			for (int i = 0; i < components_.Count; ++i) {
				result.append("/");
				toEscapedString(get(i).getValue().buf(), result);
			}
	
			return result.toString();
		}
	
		/// <summary>
		/// Encode this name as a URI according to the NDN URI Scheme. Just return the
		/// path, e.g. "/example/name" which is the default case where toUri() is used
		/// for display.
		/// </summary>
		///
		/// <returns>The URI string.</returns>
		public String toUri() {
			return toUri(false);
		}
	
		public override String ToString() {
			return toUri();
		}
	
		/// <summary>
		/// Append a component with the encoded segment number according to NDN
		/// naming conventions for "Segment number" (marker 0x00).
		/// http://named-data.net/doc/tech-memos/naming-conventions.pdf
		/// </summary>
		///
		/// <param name="segment">The segment number.</param>
		/// <returns>This name so that you can chain calls to append.</returns>
		public Name appendSegment(long segment) {
			return append(net.named_data.jndn.Name.Component.fromNumberWithMarker(segment, 0x00));
		}
	
		/// <summary>
		/// Append a component with the encoded segment byte offset according to NDN
		/// naming conventions for segment "Byte offset" (marker 0xFB).
		/// http://named-data.net/doc/tech-memos/naming-conventions.pdf
		/// </summary>
		///
		/// <param name="segmentOffset">The segment byte offset.</param>
		/// <returns>This name so that you can chain calls to append.</returns>
		public Name appendSegmentOffset(long segmentOffset) {
			return append(net.named_data.jndn.Name.Component.fromNumberWithMarker(segmentOffset, 0xFB));
		}
	
		/// <summary>
		/// Append a component with the encoded version number according to NDN
		/// naming conventions for "Versioning" (marker 0xFD).
		/// http://named-data.net/doc/tech-memos/naming-conventions.pdf
		/// Note that this encodes the exact value of version without converting from a
		/// time representation.
		/// </summary>
		///
		/// <param name="version">The version number.</param>
		/// <returns>This name so that you can chain calls to append.</returns>
		public Name appendVersion(long version) {
			return append(net.named_data.jndn.Name.Component.fromNumberWithMarker(version, 0xFD));
		}
	
		/// <summary>
		/// Append a component with the encoded timestamp according to NDN naming
		/// conventions for "Timestamp" (marker 0xFC).
		/// http://named-data.net/doc/tech-memos/naming-conventions.pdf
		/// </summary>
		///
		/// <param name="timestamp"></param>
		/// <returns>This name so that you can chain calls to append.</returns>
		public Name appendTimestamp(long timestamp) {
			return append(net.named_data.jndn.Name.Component.fromNumberWithMarker(timestamp, 0xFC));
		}
	
		/// <summary>
		/// Append a component with the encoded sequence number according to NDN naming
		/// conventions for "Sequencing" (marker 0xFE).
		/// http://named-data.net/doc/tech-memos/naming-conventions.pdf
		/// </summary>
		///
		/// <param name="sequenceNumber">The sequence number.</param>
		/// <returns>This name so that you can chain calls to append.</returns>
		public Name appendSequenceNumber(long sequenceNumber) {
			return append(net.named_data.jndn.Name.Component.fromNumberWithMarker(sequenceNumber, 0xFE));
		}
	
		/// <summary>
		/// Check if this name has the same component count and components as the given
		/// name.
		/// </summary>
		///
		/// <param name="name">The Name to check.</param>
		/// <returns>true if the names are equal, otherwise false.</returns>
		public bool equals(Name name) {
			if (components_.Count != name.components_.Count)
				return false;
	
			// Check from last to first since the last components are more likely to differ.
			for (int i = components_.Count - 1; i >= 0; --i) {
				if (!get(i).getValue().equals(name.get(i).getValue()))
					return false;
			}
	
			return true;
		}
	
		public override bool Equals(Object other) {
			if (!(other  is  Name))
				return false;
	
			return equals((Name) other);
		}
	
		public override int GetHashCode() {
			if (hashCodeChangeCount_ != getChangeCount()) {
				// The values have changed, so the previous hash code is invalidated.
				haveHashCode_ = false;
				hashCodeChangeCount_ = getChangeCount();
			}
	
			if (!haveHashCode_) {
				int hashCode = 0;
				// Use a similar hash code algorithm as String.
				for (int i = 0; i < components_.Count; ++i)
					hashCode = 37 * hashCode
							+ ((Name.Component ) components_[i]).GetHashCode();
	
				hashCode_ = hashCode;
				haveHashCode_ = true;
			}
	
			return hashCode_;
		}
	
		/// <summary>
		/// Check if the N components of this name are the same as the first N
		/// components of the given name.
		/// </summary>
		///
		/// <param name="name">The Name to check.</param>
		/// <returns>true if this matches the given name, otherwise false.  This always
		/// returns true if this name is empty.</returns>
		public bool match(Name name) {
			// This name is longer than the name we are checking it against.
			if (components_.Count > name.components_.Count)
				return false;
	
			// Check if at least one of given components doesn't match. Check from last
			// to first since the last components are more likely to differ.
			for (int i = components_.Count - 1; i >= 0; --i) {
				if (!get(i).getValue().equals(name.get(i).getValue()))
					return false;
			}
	
			return true;
		}
	
		/// <summary>
		/// Encode this Name for a particular wire format.
		/// </summary>
		///
		/// <param name="wireFormat">A WireFormat object used to encode this Name.</param>
		/// <returns>The encoded buffer.</returns>
		public Blob wireEncode(WireFormat wireFormat) {
			return wireFormat.encodeName(this);
		}
	
		/// <summary>
		/// Encode this Name for the default wire format
		/// WireFormat.getDefaultWireFormat().
		/// </summary>
		///
		/// <returns>The encoded buffer.</returns>
		public Blob wireEncode() {
			return wireEncode(net.named_data.jndn.encoding.WireFormat.getDefaultWireFormat());
		}
	
		/// <summary>
		/// Decode the input using a particular wire format and update this Name.
		/// </summary>
		///
		/// <param name="input"></param>
		/// <param name="wireFormat">A WireFormat object used to decode the input.</param>
		/// <exception cref="EncodingException">For invalid encoding.</exception>
		public void wireDecode(ByteBuffer input, WireFormat wireFormat) {
			wireFormat.decodeName(this, input);
		}
	
		/// <summary>
		/// Decode the input using the default wire format
		/// WireFormat.getDefaultWireFormat() and update this Name.
		/// </summary>
		///
		/// <param name="input"></param>
		/// <exception cref="EncodingException">For invalid encoding.</exception>
		public void wireDecode(ByteBuffer input) {
			wireDecode(input, net.named_data.jndn.encoding.WireFormat.getDefaultWireFormat());
		}
	
		/// <summary>
		/// Decode the input using a particular wire format and update this Name.
		/// </summary>
		///
		/// <param name="input">The input blob to decode.</param>
		/// <param name="wireFormat">A WireFormat object used to decode the input.</param>
		/// <exception cref="EncodingException">For invalid encoding.</exception>
		public void wireDecode(Blob input, WireFormat wireFormat) {
			wireDecode(input.buf(), wireFormat);
		}
	
		/// <summary>
		/// Decode the input using the default wire format
		/// WireFormat.getDefaultWireFormat() and update this Name.
		/// </summary>
		///
		/// <param name="input">The input blob to decode.</param>
		/// <exception cref="EncodingException">For invalid encoding.</exception>
		public void wireDecode(Blob input) {
			wireDecode(input, net.named_data.jndn.encoding.WireFormat.getDefaultWireFormat());
		}
	
		/// <summary>
		/// Compare this to the other Name using NDN canonical ordering.  If the first
		/// components of each name are not equal, this returns -1 if the first comes
		/// before the second using the NDN canonical ordering for name components, or
		/// 1 if it comes after. If they are equal, this compares the second components
		/// of each name, etc.  If both names are the same up to the size of the
		/// shorter name, this returns -1 if the first name is shorter than the second
		/// or 1 if it is longer.  For example, sorted gives:
		/// /a/b/d /a/b/cc /c /c/a /bb .  This is intuitive because all names with the
		/// prefix /a are next to each other.  But it may be also be counter-intuitive
		/// because /c comes before /bb according to NDN canonical ordering since it is
		/// shorter.
		/// </summary>
		///
		/// <param name="other">The other Name to compare with.</param>
		/// <returns>0 If they compare equal, -1 if this Name comes before other in the
		/// canonical ordering, or 1 if this Name comes after other in the canonical
		/// ordering.
		/// See http://named-data.net/doc/0.2/technical/CanonicalOrder.html</returns>
		public int compare(Name other) {
			for (int i = 0; i < size() && i < other.size(); ++i) {
				int comparison = ((Name.Component ) components_[i])
						.compare((Name.Component ) other.components_[i]);
				if (comparison == 0)
					// The components at this index are equal, so check the next components.
					continue;
	
				// Otherwise, the result is based on the components at this index.
				return comparison;
			}
	
			// The components up to min(this.size(), other.size()) are equal, so the
			//   shorter name is less.
			if (size() < other.size())
				return -1;
			else if (size() > other.size())
				return 1;
			else
				return 0;
		}
	
		public int compareTo(Object o) {
			return this.compare((Name) o);
		}
	
		// Also include this version for portability.
		public int CompareTo(Object o) {
			return this.compare((Name) o);
		}
	
		/// <summary>
		/// Get the change count, which is incremented each time this object is changed.
		/// </summary>
		///
		/// <returns>The change count.</returns>
		public long getChangeCount() {
			return changeCount_;
		}
	
		/// <summary>
		/// Make a Blob value by decoding the escapedString between beginOffset and
		/// endOffset according to the NDN URI Scheme. If the escaped string is
		/// "", "." or ".." then return a Blob with a null pointer, which means the
		/// component should be skipped in a URI name.
		/// </summary>
		///
		/// <param name="escapedString">The escaped string</param>
		/// <param name="beginOffset"></param>
		/// <param name="endOffset"></param>
		/// <returns>The Blob value. If the escapedString is not a valid escaped
		/// component, then the Blob has a null pointer.</returns>
		public static Blob fromEscapedString(String escapedString, int beginOffset,
				int endOffset) {
			String trimmedString = escapedString.Substring(beginOffset,(endOffset)-(beginOffset))
					.trim();
			ByteBuffer value_ren = unescape(trimmedString);
	
			// Check for all dots.
			bool gotNonDot = false;
			for (int i = value_ren.position(); i < value_ren.limit(); ++i) {
				if (value_ren.get(i) != '.') {
					gotNonDot = true;
					break;
				}
			}
	
			if (!gotNonDot) {
				// Special case for component of only periods.
				if (value_ren.remaining() <= 2)
					// Zero, one or two periods is illegal.  Ignore this component.
					return new Blob();
				else {
					// Remove 3 periods.
					value_ren.position(value_ren.position() + 3);
					return new Blob(value_ren, false);
				}
			} else
				return new Blob(value_ren, false);
		}
	
		/// <summary>
		/// Make a Blob value by decoding the escapedString according to the NDN URI
		/// Scheme.
		/// If the escaped string is "", "." or ".." then return a Blob with a null
		/// pointer, which means the component should be skipped in a URI name.
		/// </summary>
		///
		/// <param name="escapedString">The escaped string.</param>
		/// <returns>The Blob value. If the escapedString is not a valid escaped
		/// component, then the Blob has a null pointer.</returns>
		public static Blob fromEscapedString(String escapedString) {
			return fromEscapedString(escapedString, 0, escapedString.Length);
		}
	
		/// <summary>
		/// Write the value to result, escaping characters according to the NDN URI
		/// Scheme.
		/// This also adds "..." to a value with zero or more ".".
		/// </summary>
		///
		/// <param name="value"></param>
		/// <param name="result">The StringBuffer to write to.</param>
		public static void toEscapedString(ByteBuffer value_ren, StringBuilder result) {
			bool gotNonDot = false;
			for (int i = value_ren.position(); i < value_ren.limit(); ++i) {
				if (value_ren.get(i) != 0x2e) {
					gotNonDot = true;
					break;
				}
			}
			if (!gotNonDot) {
				// Special case for component of zero or more periods.  Add 3 periods.
				result.append("...");
				for (int i_0 = value_ren.position(); i_0 < value_ren.limit(); ++i_0)
					result.append('.');
			} else {
				for (int i_1 = value_ren.position(); i_1 < value_ren.limit(); ++i_1) {
					int x = ((int) value_ren.get(i_1) & 0xff);
					// Check for 0-9, A-Z, a-z, (+), (-), (.), (_)
					if (x >= 0x30 && x <= 0x39 || x >= 0x41 && x <= 0x5a
							|| x >= 0x61 && x <= 0x7a || x == 0x2b || x == 0x2d
							|| x == 0x2e || x == 0x5f)
						result.append((char) x);
					else {
						result.append('%');
						if (x < 16)
							result.append('0');
						result.append(ILOG.J2CsMapping.Util.IlNumber.ToString(x,16).ToUpper());
					}
				}
			}
		}
	
		/// <summary>
		/// Convert the value by escaping characters according to the NDN URI Scheme.
		/// This also adds "..." to a value with zero or more ".".
		/// </summary>
		///
		/// <param name="value"></param>
		/// <returns>The escaped string.</returns>
		public static String toEscapedString(ByteBuffer value_ren) {
			StringBuilder result = new StringBuilder(value_ren.remaining());
			toEscapedString(value_ren, result);
			return result.toString();
		}
	
		/// <summary>
		/// Convert the hex character to an integer from 0 to 15.
		/// </summary>
		///
		/// <param name="c">The hex character.</param>
		/// <returns>The hex value, or -1 if not a hex character.</returns>
		private static int fromHexChar(char c) {
			if (c >= '0' && c <= '9')
				return (int) c - (int) '0';
			else if (c >= 'A' && c <= 'F')
				return (int) c - (int) 'A' + 10;
			else if (c >= 'a' && c <= 'f')
				return (int) c - (int) 'a' + 10;
			else
				return -1;
		}
	
		/// <summary>
		/// Return a copy of str, converting each escaped "%XX" to the char value.
		/// </summary>
		///
		/// <param name="str">The escaped string.</param>
		/// <returns>The unescaped string as a ByteBuffer with position and limit set.</returns>
		private static ByteBuffer unescape(String str) {
			// We know the result will be shorter than the input str.
			ByteBuffer result = ILOG.J2CsMapping.NIO.ByteBuffer.allocate(str.Length);
	
			for (int i = 0; i < str.Length; ++i) {
				if (str[i] == '%' && i + 2 < str.Length) {
					int hi = fromHexChar(str[i + 1]);
					int lo = fromHexChar(str[i + 2]);
	
					if (hi < 0 || lo < 0)
						// Invalid hex characters, so just keep the escaped string.
						result.put((byte) str[i])
								.put((byte) str[i + 1])
								.put((byte) str[i + 2]);
					else
						result.put((byte) (16 * hi + lo));
	
					// Skip ahead past the escaped value.
					i += 2;
				} else
					// Just copy through.
					result.put((byte) str[i]);
			}
	
			result.flip();
			return result;
		}
	
		// Use ArrayList without generics so it works with older Java compilers.
		private readonly ArrayList components_;
		private long changeCount_;
		private bool haveHashCode_;
		private int hashCode_;
		private long hashCodeChangeCount_;
	}
}
