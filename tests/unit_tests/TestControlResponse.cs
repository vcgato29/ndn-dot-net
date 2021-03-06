/*
 * jndn
 * Copyright (c) 2015-2016, Intel Corporation.
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms and conditions of the GNU Lesser General Public License,
 * version 3, as published by the Free Software Foundation.
 *
 * This program is distributed in the hope it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for
 * more details.
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace net.named_data.jndn.tests.unit_tests {
	
	using ILOG.J2CsMapping.NIO;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// Test encoding/decoding of ControlResponses
	/// </summary>
	///
	public class TestControlResponse {
		// Convert the int array to a ByteBuffer.
		private static ByteBuffer toBuffer(int[] array) {
			ByteBuffer result = ILOG.J2CsMapping.NIO.ByteBuffer.allocate(array.Length);
			for (int i = 0; i < array.Length; ++i)
				result.put((byte) (array[i] & 0xff));
	
			result.flip();
			return result;
		}
	
		static internal readonly ByteBuffer TestControlResponse1 = toBuffer(new int[] {
				0x65,
				0x1c, // ControlResponse
				0x66, 0x02, 0x01,
				0x94, // StatusCode
				0x67,
				0x11, // StatusText
				0x4e, 0x6f, 0x74, 0x68, 0x69, 0x6e, 0x67, 0x20, 0x6e, 0x6f, 0x74,
				0x20, 0x66, 0x6f, 0x75, 0x6e, 0x64, 0x68, 0x03, // ControlParameters
				0x69, 0x01, 0x0a // FaceId
		});
	
		public void Encode() {
			ControlResponse response = new ControlResponse();
			response.setStatusCode(404);
			response.setStatusText("Nothing not found");
			response.setBodyAsControlParameters(new ControlParameters());
			response.getBodyAsControlParameters().setFaceId(10);
			Blob wire = response.wireEncode();
	
			Assert.AssertEquals(wire.buf(), TestControlResponse1);
		}
	
		public void Decode() {
			ControlResponse response = new ControlResponse();
			response.wireDecode(TestControlResponse1);
	
			Assert.AssertEquals(response.getStatusCode(), 404);
			Assert.AssertEquals(response.getStatusText(), "Nothing not found");
			Assert.AssertEquals(response.getBodyAsControlParameters().getFaceId(), 10);
		}
	}
}
