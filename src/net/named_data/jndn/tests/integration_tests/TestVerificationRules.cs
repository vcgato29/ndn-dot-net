// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2015-2017 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.tests.integration_tests {
	
	using ILOG.J2CsMapping.NIO;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.security;
	using net.named_data.jndn.security.identity;
	using net.named_data.jndn.security.policy;
	using net.named_data.jndn.util;
	
	public class TestVerificationRules : ConfigPolicyManager.Friend {
		// Convert the int array to a ByteBuffer.
		public static ByteBuffer toBuffer(int[] array) {
			ByteBuffer result = ILOG.J2CsMapping.NIO.ByteBuffer.allocate(array.Length);
			for (int i = 0; i < array.Length; ++i)
				result.put((byte) (array[i] & 0xff));
	
			result.flip();
			return result;
		}
	
		private static readonly ByteBuffer DEFAULT_RSA_PUBLIC_KEY_DER = toBuffer(new int[] {
				0x30, 0x82, 0x01, 0x22, 0x30, 0x0d, 0x06, 0x09, 0x2a, 0x86, 0x48,
				0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01, 0x05, 0x00, 0x03, 0x82, 0x01,
				0x0f, 0x00, 0x30, 0x82, 0x01, 0x0a, 0x02, 0x82, 0x01, 0x01, 0x00,
				0xb8, 0x09, 0xa7, 0x59, 0x82, 0x84, 0xec, 0x4f, 0x06, 0xfa, 0x1c,
				0xb2, 0xe1, 0x38, 0x93, 0x53, 0xbb, 0x7d, 0xd4, 0xac, 0x88, 0x1a,
				0xf8, 0x25, 0x11, 0xe4, 0xfa, 0x1d, 0x61, 0x24, 0x5b, 0x82, 0xca,
				0xcd, 0x72, 0xce, 0xdb, 0x66, 0xb5, 0x8d, 0x54, 0xbd, 0xfb, 0x23,
				0xfd, 0xe8, 0x8e, 0xaf, 0xa7, 0xb3, 0x79, 0xbe, 0x94, 0xb5, 0xb7,
				0xba, 0x17, 0xb6, 0x05, 0xae, 0xce, 0x43, 0xbe, 0x3b, 0xce, 0x6e,
				0xea, 0x07, 0xdb, 0xbf, 0x0a, 0x7e, 0xeb, 0xbc, 0xc9, 0x7b, 0x62,
				0x3c, 0xf5, 0xe1, 0xce, 0xe1, 0xd9, 0x8d, 0x9c, 0xfe, 0x1f, 0xc7,
				0xf8, 0xfb, 0x59, 0xc0, 0x94, 0x0b, 0x2c, 0xd9, 0x7d, 0xbc, 0x96,
				0xeb, 0xb8, 0x79, 0x22, 0x8a, 0x2e, 0xa0, 0x12, 0x1d, 0x42, 0x07,
				0xb6, 0x5d, 0xdb, 0xe1, 0xf6, 0xb1, 0x5d, 0x7b, 0x1f, 0x54, 0x52,
				0x1c, 0xa3, 0x11, 0x9b, 0xf9, 0xeb, 0xbe, 0xb3, 0x95, 0xca, 0xa5,
				0x87, 0x3f, 0x31, 0x18, 0x1a, 0xc9, 0x99, 0x01, 0xec, 0xaa, 0x90,
				0xfd, 0x8a, 0x36, 0x35, 0x5e, 0x12, 0x81, 0xbe, 0x84, 0x88, 0xa1,
				0x0d, 0x19, 0x2a, 0x4a, 0x66, 0xc1, 0x59, 0x3c, 0x41, 0x83, 0x3d,
				0x3d, 0xb8, 0xd4, 0xab, 0x34, 0x90, 0x06, 0x3e, 0x1a, 0x61, 0x74,
				0xbe, 0x04, 0xf5, 0x7a, 0x69, 0x1b, 0x9d, 0x56, 0xfc, 0x83, 0xb7,
				0x60, 0xc1, 0x5e, 0x9d, 0x85, 0x34, 0xfd, 0x02, 0x1a, 0xba, 0x2c,
				0x09, 0x72, 0xa7, 0x4a, 0x5e, 0x18, 0xbf, 0xc0, 0x58, 0xa7, 0x49,
				0x34, 0x46, 0x61, 0x59, 0x0e, 0xe2, 0x6e, 0x9e, 0xd2, 0xdb, 0xfd,
				0x72, 0x2f, 0x3c, 0x47, 0xcc, 0x5f, 0x99, 0x62, 0xee, 0x0d, 0xf3,
				0x1f, 0x30, 0x25, 0x20, 0x92, 0x15, 0x4b, 0x04, 0xfe, 0x15, 0x19,
				0x1d, 0xdc, 0x7e, 0x5c, 0x10, 0x21, 0x52, 0x21, 0x91, 0x54, 0x60,
				0x8b, 0x92, 0x41, 0x02, 0x03, 0x01, 0x00, 0x01 });
	
		// Java uses an unencrypted PKCS #8 PrivateKeyInfo, not a PKCS #1 RSAPrivateKey.
		private static readonly ByteBuffer DEFAULT_RSA_PRIVATE_KEY_DER = toBuffer(new int[] {
				0x30, 0x82, 0x04, 0xbf, 0x02, 0x01, 0x00, 0x30, 0x0d, 0x06, 0x09,
				0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01, 0x05, 0x00,
				0x04, 0x82, 0x04, 0xa9, 0x30, 0x82, 0x04, 0xa5, 0x02, 0x01, 0x00,
				0x02, 0x82, 0x01, 0x01, 0x00, 0xb8, 0x09, 0xa7, 0x59, 0x82, 0x84,
				0xec, 0x4f, 0x06, 0xfa, 0x1c, 0xb2, 0xe1, 0x38, 0x93, 0x53, 0xbb,
				0x7d, 0xd4, 0xac, 0x88, 0x1a, 0xf8, 0x25, 0x11, 0xe4, 0xfa, 0x1d,
				0x61, 0x24, 0x5b, 0x82, 0xca, 0xcd, 0x72, 0xce, 0xdb, 0x66, 0xb5,
				0x8d, 0x54, 0xbd, 0xfb, 0x23, 0xfd, 0xe8, 0x8e, 0xaf, 0xa7, 0xb3,
				0x79, 0xbe, 0x94, 0xb5, 0xb7, 0xba, 0x17, 0xb6, 0x05, 0xae, 0xce,
				0x43, 0xbe, 0x3b, 0xce, 0x6e, 0xea, 0x07, 0xdb, 0xbf, 0x0a, 0x7e,
				0xeb, 0xbc, 0xc9, 0x7b, 0x62, 0x3c, 0xf5, 0xe1, 0xce, 0xe1, 0xd9,
				0x8d, 0x9c, 0xfe, 0x1f, 0xc7, 0xf8, 0xfb, 0x59, 0xc0, 0x94, 0x0b,
				0x2c, 0xd9, 0x7d, 0xbc, 0x96, 0xeb, 0xb8, 0x79, 0x22, 0x8a, 0x2e,
				0xa0, 0x12, 0x1d, 0x42, 0x07, 0xb6, 0x5d, 0xdb, 0xe1, 0xf6, 0xb1,
				0x5d, 0x7b, 0x1f, 0x54, 0x52, 0x1c, 0xa3, 0x11, 0x9b, 0xf9, 0xeb,
				0xbe, 0xb3, 0x95, 0xca, 0xa5, 0x87, 0x3f, 0x31, 0x18, 0x1a, 0xc9,
				0x99, 0x01, 0xec, 0xaa, 0x90, 0xfd, 0x8a, 0x36, 0x35, 0x5e, 0x12,
				0x81, 0xbe, 0x84, 0x88, 0xa1, 0x0d, 0x19, 0x2a, 0x4a, 0x66, 0xc1,
				0x59, 0x3c, 0x41, 0x83, 0x3d, 0x3d, 0xb8, 0xd4, 0xab, 0x34, 0x90,
				0x06, 0x3e, 0x1a, 0x61, 0x74, 0xbe, 0x04, 0xf5, 0x7a, 0x69, 0x1b,
				0x9d, 0x56, 0xfc, 0x83, 0xb7, 0x60, 0xc1, 0x5e, 0x9d, 0x85, 0x34,
				0xfd, 0x02, 0x1a, 0xba, 0x2c, 0x09, 0x72, 0xa7, 0x4a, 0x5e, 0x18,
				0xbf, 0xc0, 0x58, 0xa7, 0x49, 0x34, 0x46, 0x61, 0x59, 0x0e, 0xe2,
				0x6e, 0x9e, 0xd2, 0xdb, 0xfd, 0x72, 0x2f, 0x3c, 0x47, 0xcc, 0x5f,
				0x99, 0x62, 0xee, 0x0d, 0xf3, 0x1f, 0x30, 0x25, 0x20, 0x92, 0x15,
				0x4b, 0x04, 0xfe, 0x15, 0x19, 0x1d, 0xdc, 0x7e, 0x5c, 0x10, 0x21,
				0x52, 0x21, 0x91, 0x54, 0x60, 0x8b, 0x92, 0x41, 0x02, 0x03, 0x01,
				0x00, 0x01, 0x02, 0x82, 0x01, 0x01, 0x00, 0x8a, 0x05, 0xfb, 0x73,
				0x7f, 0x16, 0xaf, 0x9f, 0xa9, 0x4c, 0xe5, 0x3f, 0x26, 0xf8, 0x66,
				0x4d, 0xd2, 0xfc, 0xd1, 0x06, 0xc0, 0x60, 0xf1, 0x9f, 0xe3, 0xa6,
				0xc6, 0x0a, 0x48, 0xb3, 0x9a, 0xca, 0x21, 0xcd, 0x29, 0x80, 0x88,
				0x3d, 0xa4, 0x85, 0xa5, 0x7b, 0x82, 0x21, 0x81, 0x28, 0xeb, 0xf2,
				0x43, 0x24, 0xb0, 0x76, 0xc5, 0x52, 0xef, 0xc2, 0xea, 0x4b, 0x82,
				0x41, 0x92, 0xc2, 0x6d, 0xa6, 0xae, 0xf0, 0xb2, 0x26, 0x48, 0xa1,
				0x23, 0x7f, 0x02, 0xcf, 0xa8, 0x90, 0x17, 0xa2, 0x3e, 0x8a, 0x26,
				0xbd, 0x6d, 0x8a, 0xee, 0xa6, 0x0c, 0x31, 0xce, 0xc2, 0xbb, 0x92,
				0x59, 0xb5, 0x73, 0xe2, 0x7d, 0x91, 0x75, 0xe2, 0xbd, 0x8c, 0x63,
				0xe2, 0x1c, 0x8b, 0xc2, 0x6a, 0x1c, 0xfe, 0x69, 0xc0, 0x44, 0xcb,
				0x58, 0x57, 0xb7, 0x13, 0x42, 0xf0, 0xdb, 0x50, 0x4c, 0xe0, 0x45,
				0x09, 0x8f, 0xca, 0x45, 0x8a, 0x06, 0xfe, 0x98, 0xd1, 0x22, 0xf5,
				0x5a, 0x9a, 0xdf, 0x89, 0x17, 0xca, 0x20, 0xcc, 0x12, 0xa9, 0x09,
				0x3d, 0xd5, 0xf7, 0xe3, 0xeb, 0x08, 0x4a, 0xc4, 0x12, 0xc0, 0xb9,
				0x47, 0x6c, 0x79, 0x50, 0x66, 0xa3, 0xf8, 0xaf, 0x2c, 0xfa, 0xb4,
				0x6b, 0xec, 0x03, 0xad, 0xcb, 0xda, 0x24, 0x0c, 0x52, 0x07, 0x87,
				0x88, 0xc0, 0x21, 0xf3, 0x02, 0xe8, 0x24, 0x44, 0x0f, 0xcd, 0xa0,
				0xad, 0x2f, 0x1b, 0x79, 0xab, 0x6b, 0x49, 0x4a, 0xe6, 0x3b, 0xd0,
				0xad, 0xc3, 0x48, 0xb9, 0xf7, 0xf1, 0x34, 0x09, 0xeb, 0x7a, 0xc0,
				0xd5, 0x0d, 0x39, 0xd8, 0x45, 0xce, 0x36, 0x7a, 0xd8, 0xde, 0x3c,
				0xb0, 0x21, 0x96, 0x97, 0x8a, 0xff, 0x8b, 0x23, 0x60, 0x4f, 0xf0,
				0x3d, 0xd7, 0x8f, 0xf3, 0x2c, 0xcb, 0x1d, 0x48, 0x3f, 0x86, 0xc4,
				0xa9, 0x00, 0xf2, 0x23, 0x2d, 0x72, 0x4d, 0x66, 0xa5, 0x01, 0x02,
				0x81, 0x81, 0x00, 0xdc, 0x4f, 0x99, 0x44, 0x0d, 0x7f, 0x59, 0x46,
				0x1e, 0x8f, 0xe7, 0x2d, 0x8d, 0xdd, 0x54, 0xc0, 0xf7, 0xfa, 0x46,
				0x0d, 0x9d, 0x35, 0x03, 0xf1, 0x7c, 0x12, 0xf3, 0x5a, 0x9d, 0x83,
				0xcf, 0xdd, 0x37, 0x21, 0x7c, 0xb7, 0xee, 0xc3, 0x39, 0xd2, 0x75,
				0x8f, 0xb2, 0x2d, 0x6f, 0xec, 0xc6, 0x03, 0x55, 0xd7, 0x00, 0x67,
				0xd3, 0x9b, 0xa2, 0x68, 0x50, 0x6f, 0x9e, 0x28, 0xa4, 0x76, 0x39,
				0x2b, 0xb2, 0x65, 0xcc, 0x72, 0x82, 0x93, 0xa0, 0xcf, 0x10, 0x05,
				0x6a, 0x75, 0xca, 0x85, 0x35, 0x99, 0xb0, 0xa6, 0xc6, 0xef, 0x4c,
				0x4d, 0x99, 0x7d, 0x2c, 0x38, 0x01, 0x21, 0xb5, 0x31, 0xac, 0x80,
				0x54, 0xc4, 0x18, 0x4b, 0xfd, 0xef, 0xb3, 0x30, 0x22, 0x51, 0x5a,
				0xea, 0x7d, 0x9b, 0xb2, 0x9d, 0xcb, 0xba, 0x3f, 0xc0, 0x1a, 0x6b,
				0xcd, 0xb0, 0xe6, 0x2f, 0x04, 0x33, 0xd7, 0x3a, 0x49, 0x71, 0x02,
				0x81, 0x81, 0x00, 0xd5, 0xd9, 0xc9, 0x70, 0x1a, 0x13, 0xb3, 0x39,
				0x24, 0x02, 0xee, 0xb0, 0xbb, 0x84, 0x17, 0x12, 0xc6, 0xbd, 0x65,
				0x73, 0xe9, 0x34, 0x5d, 0x43, 0xff, 0xdc, 0xf8, 0x55, 0xaf, 0x2a,
				0xb9, 0xe1, 0xfa, 0x71, 0x65, 0x4e, 0x50, 0x0f, 0xa4, 0x3b, 0xe5,
				0x68, 0xf2, 0x49, 0x71, 0xaf, 0x15, 0x88, 0xd7, 0xaf, 0xc4, 0x9d,
				0x94, 0x84, 0x6b, 0x5b, 0x10, 0xd5, 0xc0, 0xaa, 0x0c, 0x13, 0x62,
				0x99, 0xc0, 0x8b, 0xfc, 0x90, 0x0f, 0x87, 0x40, 0x4d, 0x58, 0x88,
				0xbd, 0xe2, 0xba, 0x3e, 0x7e, 0x2d, 0xd7, 0x69, 0xa9, 0x3c, 0x09,
				0x64, 0x31, 0xb6, 0xcc, 0x4d, 0x1f, 0x23, 0xb6, 0x9e, 0x65, 0xd6,
				0x81, 0xdc, 0x85, 0xcc, 0x1e, 0xf1, 0x0b, 0x84, 0x38, 0xab, 0x93,
				0x5f, 0x9f, 0x92, 0x4e, 0x93, 0x46, 0x95, 0x6b, 0x3e, 0xb6, 0xc3,
				0x1b, 0xd7, 0x69, 0xa1, 0x0a, 0x97, 0x37, 0x78, 0xed, 0xd1, 0x02,
				0x81, 0x80, 0x33, 0x18, 0xc3, 0x13, 0x65, 0x8e, 0x03, 0xc6, 0x9f,
				0x90, 0x00, 0xae, 0x30, 0x19, 0x05, 0x6f, 0x3c, 0x14, 0x6f, 0xea,
				0xf8, 0x6b, 0x33, 0x5e, 0xee, 0xc7, 0xf6, 0x69, 0x2d, 0xdf, 0x44,
				0x76, 0xaa, 0x32, 0xba, 0x1a, 0x6e, 0xe6, 0x18, 0xa3, 0x17, 0x61,
				0x1c, 0x92, 0x2d, 0x43, 0x5d, 0x29, 0xa8, 0xdf, 0x14, 0xd8, 0xff,
				0xdb, 0x38, 0xef, 0xb8, 0xb8, 0x2a, 0x96, 0x82, 0x8e, 0x68, 0xf4,
				0x19, 0x8c, 0x42, 0xbe, 0xcc, 0x4a, 0x31, 0x21, 0xd5, 0x35, 0x6c,
				0x5b, 0xa5, 0x7c, 0xff, 0xd1, 0x85, 0x87, 0x28, 0xdc, 0x97, 0x75,
				0xe8, 0x03, 0x80, 0x1d, 0xfd, 0x25, 0x34, 0x41, 0x31, 0x21, 0x12,
				0x87, 0xe8, 0x9a, 0xb7, 0x6a, 0xc0, 0xc4, 0x89, 0x31, 0x15, 0x45,
				0x0d, 0x9c, 0xee, 0xf0, 0x6a, 0x2f, 0xe8, 0x59, 0x45, 0xc7, 0x7b,
				0x0d, 0x6c, 0x55, 0xbb, 0x43, 0xca, 0xc7, 0x5a, 0x01, 0x02, 0x81,
				0x81, 0x00, 0xab, 0xf4, 0xd5, 0xcf, 0x78, 0x88, 0x82, 0xc2, 0xdd,
				0xbc, 0x25, 0xe6, 0xa2, 0xc1, 0xd2, 0x33, 0xdc, 0xef, 0x0a, 0x97,
				0x2b, 0xdc, 0x59, 0x6a, 0x86, 0x61, 0x4e, 0xa6, 0xc7, 0x95, 0x99,
				0xa6, 0xa6, 0x55, 0x6c, 0x5a, 0x8e, 0x72, 0x25, 0x63, 0xac, 0x52,
				0xb9, 0x10, 0x69, 0x83, 0x99, 0xd3, 0x51, 0x6c, 0x1a, 0xb3, 0x83,
				0x6a, 0xff, 0x50, 0x58, 0xb7, 0x28, 0x97, 0x13, 0xe2, 0xba, 0x94,
				0x5b, 0x89, 0xb4, 0xea, 0xba, 0x31, 0xcd, 0x78, 0xe4, 0x4a, 0x00,
				0x36, 0x42, 0x00, 0x62, 0x41, 0xc6, 0x47, 0x46, 0x37, 0xea, 0x6d,
				0x50, 0xb4, 0x66, 0x8f, 0x55, 0x0c, 0xc8, 0x99, 0x91, 0xd5, 0xec,
				0xd2, 0x40, 0x1c, 0x24, 0x7d, 0x3a, 0xff, 0x74, 0xfa, 0x32, 0x24,
				0xe0, 0x11, 0x2b, 0x71, 0xad, 0x7e, 0x14, 0xa0, 0x77, 0x21, 0x68,
				0x4f, 0xcc, 0xb6, 0x1b, 0xe8, 0x00, 0x49, 0x13, 0x21, 0x02, 0x81,
				0x81, 0x00, 0xb6, 0x18, 0x73, 0x59, 0x2c, 0x4f, 0x92, 0xac, 0xa2,
				0x2e, 0x5f, 0xb6, 0xbe, 0x78, 0x5d, 0x47, 0x71, 0x04, 0x92, 0xf0,
				0xd7, 0xe8, 0xc5, 0x7a, 0x84, 0x6b, 0xb8, 0xb4, 0x30, 0x1f, 0xd8,
				0x0d, 0x58, 0xd0, 0x64, 0x80, 0xa7, 0x21, 0x1a, 0x48, 0x00, 0x37,
				0xd6, 0x19, 0x71, 0xbb, 0x91, 0x20, 0x9d, 0xe2, 0xc3, 0xec, 0xdb,
				0x36, 0x1c, 0xca, 0x48, 0x7d, 0x03, 0x32, 0x74, 0x1e, 0x65, 0x73,
				0x02, 0x90, 0x73, 0xd8, 0x3f, 0xb5, 0x52, 0x35, 0x79, 0x1c, 0xee,
				0x93, 0xa3, 0x32, 0x8b, 0xed, 0x89, 0x98, 0xf1, 0x0c, 0xd8, 0x12,
				0xf2, 0x89, 0x7f, 0x32, 0x23, 0xec, 0x67, 0x66, 0x52, 0x83, 0x89,
				0x99, 0x5e, 0x42, 0x2b, 0x42, 0x4b, 0x84, 0x50, 0x1b, 0x3e, 0x47,
				0x6d, 0x74, 0xfb, 0xd1, 0xa6, 0x10, 0x20, 0x6c, 0x6e, 0xbe, 0x44,
				0x3f, 0xb9, 0xfe, 0xbc, 0x8d, 0xda, 0xcb, 0xea, 0x8f });
	
		public void setUp() {
			policyConfigDirectory = net.named_data.jndn.tests.integration_tests.IntegrationTestsCommon
					.getPolicyConfigDirectory();
	
			identityStorage = new MemoryIdentityStorage();
			privateKeyStorage = new MemoryPrivateKeyStorage();
			// Not using keychain for verification so we don't need to set the
			//   policy manager.
			keyChain = new KeyChain(new IdentityManager(identityStorage,
					privateKeyStorage));
			identityName = new Name("/SecurityTestSecRule/Basic/Longer");
	
			Name keyName = new Name(identityName).append("ksk-2439872");
			defaultCertName = certNameFromKeyName(keyName);
			identityStorage.addKey(keyName, net.named_data.jndn.security.KeyType.RSA, new Blob(
					DEFAULT_RSA_PUBLIC_KEY_DER, false));
			privateKeyStorage.setKeyPairForKeyName(keyName, net.named_data.jndn.security.KeyType.RSA,
					DEFAULT_RSA_PUBLIC_KEY_DER, DEFAULT_RSA_PRIVATE_KEY_DER);
	
			keyName = new Name("/SecurityTestSecRule/Basic/ksk-0923489");
			identityStorage.addKey(keyName, net.named_data.jndn.security.KeyType.RSA, new Blob(
					DEFAULT_RSA_PUBLIC_KEY_DER, false));
			privateKeyStorage.setKeyPairForKeyName(keyName, net.named_data.jndn.security.KeyType.RSA,
					DEFAULT_RSA_PUBLIC_KEY_DER, DEFAULT_RSA_PRIVATE_KEY_DER);
	
			shortCertName = certNameFromKeyName(keyName, -2);
	
			net.named_data.jndn.security.policy.ConfigPolicyManager.setFriendAccess(this);
		}
	
		public virtual void setConfigPolicyManagerFriendAccess(
				ConfigPolicyManager.FriendAccess friendAccess) {
			this.friendAccess = friendAccess;
		}
	
		private static Name certNameFromKeyName(Name keyName, int keyIdx) {
			if (keyIdx < 0)
				keyIdx = keyName.size() + keyIdx;
			return keyName.getPrefix(keyIdx).append("KEY")
					.append(keyName.getSubName(keyIdx)).append("ID-CERT")
					.append("0");
		}
	
		private static Name certNameFromKeyName(Name keyName) {
			return certNameFromKeyName(keyName, -1);
		}
	
		internal FileInfo policyConfigDirectory;
		internal MemoryIdentityStorage identityStorage;
		internal MemoryPrivateKeyStorage privateKeyStorage;
		internal KeyChain keyChain;
		internal Name identityName;
		internal Name defaultCertName;
		internal Name shortCertName;
		internal ConfigPolicyManager.FriendAccess friendAccess;
	
		public void testNameRelation() {
			ConfigPolicyManager policyManagerPrefix = new ConfigPolicyManager(
					new FileInfo(System.IO.Path.Combine(policyConfigDirectory.FullName,"relation_ruleset_prefix.conf")).FullName);
			ConfigPolicyManager policyManagerStrict = new ConfigPolicyManager(
					new FileInfo(System.IO.Path.Combine(policyConfigDirectory.FullName,"relation_ruleset_strict.conf")).FullName);
			ConfigPolicyManager policyManagerEqual = new ConfigPolicyManager(
					new FileInfo(System.IO.Path.Combine(policyConfigDirectory.FullName,"relation_ruleset_equal.conf")).FullName);
	
			Name dataName = new Name("/TestRule1");
			Assert.AssertNotNull("Prefix relation should match prefix name",
					friendAccess.findMatchingRule(policyManagerPrefix, dataName,
							"data"));
			Assert.AssertNotNull("Equal relation should match prefix name",
					friendAccess.findMatchingRule(policyManagerEqual, dataName,
							"data"));
			AssertNull("Strict-prefix relation should not match prefix name",
					friendAccess.findMatchingRule(policyManagerStrict, dataName,
							"data"));
	
			dataName = new Name("/TestRule1/hi");
			Assert.AssertNotNull("Prefix relation should match longer name",
					friendAccess.findMatchingRule(policyManagerPrefix, dataName,
							"data"));
			AssertNull("Equal relation should not match longer name",
					friendAccess.findMatchingRule(policyManagerEqual, dataName,
							"data"));
			Assert.AssertNotNull("Strict-prefix relation should match longer name",
					friendAccess.findMatchingRule(policyManagerStrict, dataName,
							"data"));
	
			dataName = new Name("/Bad/TestRule1/");
			AssertNull("Prefix relation should not match inner components",
					friendAccess.findMatchingRule(policyManagerPrefix, dataName,
							"data"));
			AssertNull("Equal relation should not match inner components",
					friendAccess.findMatchingRule(policyManagerEqual, dataName,
							"data"));
			AssertNull("Strict-prefix relation should  not match inner components",
					friendAccess.findMatchingRule(policyManagerStrict, dataName,
							"data"));
		}
	
		public void testSimpleRegex() {
			ConfigPolicyManager policyManager = new ConfigPolicyManager(new FileInfo(System.IO.Path.Combine(policyConfigDirectory.FullName,"regex_ruleset.conf")).FullName);
	
			Name dataName1 = new Name("/SecurityTestSecRule/Basic");
			Name dataName2 = new Name("/SecurityTestSecRule/Basic/More");
			Name dataName3 = new Name("/SecurityTestSecRule/");
			Name dataName4 = new Name("/SecurityTestSecRule/Other/TestData");
			Name dataName5 = new Name("/Basic/Data");
	
			BoostInfoTree matchedRule1 = friendAccess.findMatchingRule(
					policyManager, dataName1, "data");
			BoostInfoTree matchedRule2 = friendAccess.findMatchingRule(
					policyManager, dataName2, "data");
			BoostInfoTree matchedRule3 = friendAccess.findMatchingRule(
					policyManager, dataName3, "data");
			BoostInfoTree matchedRule4 = friendAccess.findMatchingRule(
					policyManager, dataName4, "data");
			BoostInfoTree matchedRule5 = friendAccess.findMatchingRule(
					policyManager, dataName5, "data");
	
			Assert.AssertNotNull(matchedRule1);
			AssertNull(matchedRule2);
			Assert.AssertNotNull(matchedRule3);
			AssertNotSame("Rule regex matched extra components", matchedRule3,
					matchedRule1);
			Assert.AssertNotNull(matchedRule4);
			AssertNotSame("Rule regex matched with missing component",
					matchedRule4, matchedRule1);
	
			AssertNull(matchedRule5);
		}
	
		public void testHierarchical() {
			ConfigPolicyManager policyManager = new ConfigPolicyManager(
					new FileInfo(System.IO.Path.Combine(policyConfigDirectory.FullName,"/hierarchical_ruleset.conf")).FullName);
	
			Name dataName1 = new Name("/SecurityTestSecRule/Basic/Data1");
			Name dataName2 = new Name("/SecurityTestSecRule/Basic/Longer/Data2");
	
			Data data1 = new Data(dataName1);
			Data data2 = new Data(dataName2);
	
			BoostInfoTree matchedRule = friendAccess.findMatchingRule(
					policyManager, dataName1, "data");
			AssertSame(matchedRule,
					friendAccess.findMatchingRule(policyManager, dataName2, "data"));
	
			keyChain.sign(data1, defaultCertName);
			keyChain.sign(data2, defaultCertName);
	
			Name signatureName1 = ((Sha256WithRsaSignature) data1.getSignature())
					.getKeyLocator().getKeyName();
			Name signatureName2 = ((Sha256WithRsaSignature) data2.getSignature())
					.getKeyLocator().getKeyName();
	
			String[] failureReason = new String[] { "unknown" };
			Assert.AssertFalse(
					"Hierarchical matcher matched short data name to long key name",
					friendAccess.checkSignatureMatch(policyManager, signatureName1,
							dataName1, matchedRule, failureReason));
			Assert.AssertTrue(friendAccess.checkSignatureMatch(policyManager,
					signatureName2, dataName2, matchedRule, failureReason));
	
			keyChain.sign(data1, shortCertName);
			keyChain.sign(data2, shortCertName);
	
			signatureName1 = ((Sha256WithRsaSignature) data1.getSignature())
					.getKeyLocator().getKeyName();
			signatureName2 = ((Sha256WithRsaSignature) data1.getSignature())
					.getKeyLocator().getKeyName();
	
			Assert.AssertTrue(friendAccess.checkSignatureMatch(policyManager,
					signatureName1, dataName1, matchedRule, failureReason));
			Assert.AssertTrue(friendAccess.checkSignatureMatch(policyManager,
					signatureName2, dataName2, matchedRule, failureReason));
		}
	
		public void testHyperRelation() {
			ConfigPolicyManager policyManager = new ConfigPolicyManager(
					new FileInfo(System.IO.Path.Combine(policyConfigDirectory.FullName,"/hyperrelation_ruleset.conf")).FullName);
	
			Name dataName = new Name("/SecurityTestSecRule/Basic/Longer/Data2");
			Data data1 = new Data(dataName);
			Data data2 = new Data(dataName);
	
			BoostInfoTree matchedRule = friendAccess.findMatchingRule(
					policyManager, dataName, "data");
			keyChain.sign(data1, defaultCertName);
			keyChain.sign(data2, shortCertName);
	
			Name signatureName1 = ((Sha256WithRsaSignature) data1.getSignature())
					.getKeyLocator().getKeyName();
			Name signatureName2 = ((Sha256WithRsaSignature) data2.getSignature())
					.getKeyLocator().getKeyName();
	
			String[] failureReason = new String[] { "unknown" };
			Assert.AssertTrue(friendAccess.checkSignatureMatch(policyManager,
					signatureName1, dataName, matchedRule, failureReason));
			Assert.AssertFalse(friendAccess.checkSignatureMatch(policyManager,
					signatureName2, dataName, matchedRule, failureReason));
	
			dataName = new Name("/SecurityTestSecRule/Basic/Other/Data1");
			data1 = new Data(dataName);
			data2 = new Data(dataName);
	
			matchedRule = friendAccess.findMatchingRule(policyManager, dataName,
					"data");
			keyChain.sign(data1, defaultCertName);
			keyChain.sign(data2, shortCertName);
	
			signatureName1 = ((Sha256WithRsaSignature) data1.getSignature())
					.getKeyLocator().getKeyName();
			signatureName2 = ((Sha256WithRsaSignature) data2.getSignature())
					.getKeyLocator().getKeyName();
	
			Assert.AssertFalse(friendAccess.checkSignatureMatch(policyManager,
					signatureName1, dataName, matchedRule, failureReason));
			Assert.AssertTrue(friendAccess.checkSignatureMatch(policyManager,
					signatureName2, dataName, matchedRule, failureReason));
		}
	}
}
