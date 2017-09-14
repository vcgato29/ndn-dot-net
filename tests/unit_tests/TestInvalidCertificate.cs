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
namespace net.named_data.jndn.tests.unit_tests {
	
	using ILOG.J2CsMapping.NIO;
	using ILOG.J2CsMapping.Util;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.encoding;
	using net.named_data.jndn.security;
	using net.named_data.jndn.security.v2;
	using net.named_data.jndn.util;
	
	public class TestInvalidCertificate {
		// Convert the int array to a ByteBuffer.
		public static ByteBuffer toBuffer(int[] array) {
			ByteBuffer result = ILOG.J2CsMapping.NIO.ByteBuffer.allocate(array.Length);
			for (int i = 0; i < array.Length; ++i)
				result.put((byte) (array[i] & 0xff));
	
			result.flip();
			return result;
		}
	
		private static readonly ByteBuffer SIG_VALUE = toBuffer(new int[] {
				0x17,
				0x80, // SignatureValue
				0x2f, 0xd6, 0xf1, 0x6e, 0x80, 0x6f, 0x10, 0xbe, 0xb1, 0x6f, 0x3e,
				0x31, 0xec, 0xe3, 0xb9, 0xea, 0x83, 0x30, 0x40, 0x03, 0xfc, 0xa0,
				0x13, 0xd9, 0xb3, 0xc6, 0x25, 0x16, 0x2d, 0xa6, 0x58, 0x41, 0x69,
				0x62, 0x56, 0xd8, 0xb3, 0x6a, 0x38, 0x76, 0x56, 0xea, 0x61, 0xb2,
				0x32, 0x70, 0x1c, 0xb6, 0x4d, 0x10, 0x1d, 0xdc, 0x92, 0x8e, 0x52,
				0xa5, 0x8a, 0x1d, 0xd9, 0x96, 0x5e, 0xc0, 0x62, 0x0b, 0xcf, 0x3a,
				0x9d, 0x7f, 0xca, 0xbe, 0xa1, 0x41, 0x71, 0x85, 0x7a, 0x8b, 0x5d,
				0xa9, 0x64, 0xd6, 0x66, 0xb4, 0xe9, 0x8d, 0x0c, 0x28, 0x43, 0xee,
				0xa6, 0x64, 0xe8, 0x55, 0xf6, 0x1c, 0x19, 0x0b, 0xef, 0x99, 0x25,
				0x1e, 0xdc, 0x78, 0xb3, 0xa7, 0xaa, 0x0d, 0x14, 0x58, 0x30, 0xe5,
				0x37, 0x6a, 0x6d, 0xdb, 0x56, 0xac, 0xa3, 0xfc, 0x90, 0x7a, 0xb8,
				0x66, 0x9c, 0x0e, 0xf6, 0xb7, 0x64, 0xd1 });
	
		private static readonly ByteBuffer CERT = toBuffer(new int[] {
				0x06,
				0xFD,
				0x01,
				0xBB, // Data
				0x07,
				0x33, // Name /ndn/site1/KEY/ksk-1416425377094/0123/%FD%00%00%01I%C9%8B
				0x08,
				0x03,
				0x6E,
				0x64,
				0x6E,
				0x08,
				0x05,
				0x73,
				0x69,
				0x74,
				0x65,
				0x31,
				0x08,
				0x03,
				0x4B,
				0x45,
				0x59,
				0x08,
				0x11,
				0x6B,
				0x73,
				0x6B,
				0x2D,
				0x31,
				0x34,
				0x31,
				0x36,
				0x34,
				0x32,
				0x35,
				0x33,
				0x37,
				0x37,
				0x30,
				0x39,
				0x34,
				0x08,
				0x04,
				0x30,
				0x31,
				0x32,
				0x33,
				0x08,
				0x07,
				0xFD,
				0x00,
				0x00,
				0x01,
				0x49,
				0xC9,
				0x8B,
				0x14,
				0x09, // MetaInfo
				0x18,
				0x01,
				0x02, // ContentType = Key
				0x19,
				0x04,
				0x00,
				0x36,
				0xEE,
				0x80, // FreshnessPeriod = 3600000 ms
				0x15,
				0xA0, // Content
				0x30, 0x81, 0x9D, 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86,
				0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00, 0x03, 0x81, 0x8B, 0x00,
				0x30, 0x81, 0x87, 0x02, 0x81, 0x81, 0x00, 0x9E, 0x06, 0x3E, 0x47,
				0x85, 0xB2, 0x34, 0x37, 0xAA, 0x85, 0x47, 0xAC, 0x03, 0x24, 0x83,
				0xB5, 0x9C, 0xA8, 0x05, 0x3A, 0x24, 0x1E, 0xEB, 0x89, 0x01, 0xBB,
				0xE9, 0x9B, 0xB2, 0xC3, 0x22, 0xAC, 0x68, 0xE3, 0xF0, 0x6C, 0x02,
				0xCE, 0x68, 0xA6, 0xC4, 0xD0, 0xA7, 0x06, 0x90, 0x9C, 0xAA, 0x1B,
				0x08, 0x1D, 0x8B, 0x43, 0x9A, 0x33, 0x67, 0x44, 0x6D, 0x21, 0xA3,
				0x1B, 0x88, 0x9A, 0x97, 0x5E, 0x59, 0xC4, 0x15, 0x0B, 0xD9, 0x2C,
				0xBD, 0x51, 0x07, 0x61, 0x82, 0xAD, 0xC1, 0xB8, 0xD7, 0xBF, 0x9B,
				0xCF, 0x7D,
				0x24,
				0xC2,
				0x63,
				0xF3,
				0x97,
				0x17,
				0xEB,
				0xFE,
				0x62,
				0x25,
				0xBA,
				0x5B,
				0x4D,
				0x8A,
				0xC2,
				0x7A,
				0xBD,
				0x43,
				0x8A,
				0x8F,
				0xB8,
				0xF2,
				0xF1,
				0xC5,
				0x6A,
				0x30,
				0xD3,
				0x50,
				0x8C,
				0xC8,
				0x9A,
				0xDF,
				0xEF,
				0xED,
				0x35,
				0xE7,
				0x7A,
				0x62,
				0xEA,
				0x76,
				0x7C,
				0xBB,
				0x08,
				0x26,
				0xC7,
				0x02,
				0x01,
				0x11,
				0x16,
				0x55, // SignatureInfo
				0x1B,
				0x01,
				0x01, // SignatureType
				0x1C,
				0x26, // KeyLocator: /ndn/site1/KEY/ksk-2516425377094
				0x07, 0x24, 0x08, 0x03, 0x6E, 0x64, 0x6E, 0x08, 0x05, 0x73, 0x69,
				0x74, 0x65, 0x31, 0x08, 0x03, 0x4B, 0x45, 0x59, 0x08, 0x11, 0x6B,
				0x73, 0x6B, 0x2D, 0x32, 0x35,
				0x31,
				0x36,
				0x34,
				0x32,
				0x35,
				0x33,
				0x37,
				0x37,
				0x30,
				0x39,
				0x34,
				0xFD,
				0x00,
				0xFD,
				0x26, // ValidityPeriod: (20150814T223739, 20150818T223738)
				0xFD, 0x00, 0xFE, 0x0F, 0x32, 0x30, 0x31, 0x35, 0x30, 0x38, 0x31,
				0x34, 0x54, 0x32, 0x32, 0x33, 0x37, 0x33, 0x39, 0xFD, 0x00, 0xFF,
				0x0F, 0x32, 0x30, 0x31, 0x35, 0x30,
				0x38,
				0x31,
				0x38,
				0x54,
				0x32,
				0x32,
				0x33,
				0x37,
				0x33,
				0x38,
				0x17,
				0x80, // SignatureValue
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
	
		private static Sha256WithRsaSignature generateFakeSignature() {
			Sha256WithRsaSignature signatureInfo = new Sha256WithRsaSignature();
	
			Name keyLocatorName = new Name("/ndn/site1/KEY/ksk-2516425377094");
			KeyLocator keyLocator = new KeyLocator();
			keyLocator.setType(net.named_data.jndn.KeyLocatorType.KEYNAME);
			keyLocator.setKeyName(keyLocatorName);
			signatureInfo.setKeyLocator(keyLocator);
	
			ValidityPeriod period = new ValidityPeriod();
			period.setPeriod(net.named_data.jndn.tests.unit_tests.UnitTestsCommon.fromIsoString("20141111T050000"),
					net.named_data.jndn.tests.unit_tests.UnitTestsCommon.fromIsoString("20141111T060000"));
			signatureInfo.setValidityPeriod(period);
	
			Blob block2 = new Blob(SIG_VALUE, false);
			signatureInfo.setSignature(block2);
	
			return signatureInfo;
		}
	
		internal Data certificateBase_;
	
		public void setUp() {
			CertificateV2 certificateBase = new CertificateV2();
			certificateBase.wireDecode(new Blob(CERT, false));
			// Check no throw.
			CertificateV2 temp1 = new CertificateV2(certificateBase);
	
			certificateBase_ = new Data(certificateBase);
			certificateBase_.setSignature(generateFakeSignature());
	
			// Check no throw.
			CertificateV2 temp2 = new CertificateV2(certificateBase_);
		}
	
		public void testInvalidName() {
			Data data = new Data(certificateBase_);
			data.setName(new Name(
					"/ndn/site1/ksk-1416425377094/0123/%FD%00%00%01I%C9%8B"));
			data.setSignature(generateFakeSignature());
	
			try {
				new CertificateV2(data);
				Assert.Fail("The CertificateV2 constructor did not throw an exception");
			} catch (CertificateV2.Error ex) {
			} catch (Exception ex_0) {
				Assert.Fail("The CertificateV2 constructor did not throw an exception");
			}
		}
	
		public void testInvalidType() {
			Data data = new Data(certificateBase_);
			data.getMetaInfo().setType(net.named_data.jndn.ContentType.BLOB);
			data.setSignature(generateFakeSignature());
	
			try {
				new CertificateV2(data);
				Assert.Fail("The CertificateV2 constructor did not throw an exception");
			} catch (CertificateV2.Error ex) {
			} catch (Exception ex_0) {
				Assert.Fail("The CertificateV2 constructor did not throw an exception");
			}
		}
	
		public void testEmptyContent() {
			Data data = new Data(certificateBase_);
			data.setContent(new Blob());
			data.setSignature(generateFakeSignature());
	
			try {
				new CertificateV2(data);
				Assert.Fail("The CertificateV2 constructor did not throw an exception");
			} catch (CertificateV2.Error ex) {
			} catch (Exception ex_0) {
				Assert.Fail("The CertificateV2 constructor did not throw an exception");
			}
	
			CertificateV2 certificate = new CertificateV2(certificateBase_);
			certificate.setContent(new Blob());
			certificate.setSignature(generateFakeSignature());
			try {
				certificate.getPublicKey();
				Assert.Fail("getPublicKey did not throw an exception");
			} catch (CertificateV2.Error ex_1) {
			} catch (Exception ex_2) {
				Assert.Fail("getPublicKey did not throw an exception");
			}
		}
	}
}