// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2014-2017 Regents of the University of California.
/// </summary>
///
namespace src.net.named_data.jndn.tests.integration_tests {
	
	using ILOG.J2CsMapping.NIO;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.security;
	using net.named_data.jndn.security.certificate;
	using net.named_data.jndn.security.identity;
	using net.named_data.jndn.util;
	
	public class TestFilePrivateKeyStorage {
	
		/// <summary>
		/// Keep a reference to the key storage folder
		/// </summary>
		///
		private static readonly FileInfo ndnFolder_ = new FileInfo(System.IO.Path.Combine(new FileInfo(System.Environment.GetEnvironmentVariable("user.home")+".ndn").FullName,"ndnsec-tpm-file")); 
	
		/// <summary>
		/// Create a few keys before testing
		/// </summary>
		///
		/// <exception cref="System.Exception"></exception>
		public static void setUpClass() {
			// create some test key files to use in tests
			FilePrivateKeyStorage instance = new FilePrivateKeyStorage();
			instance.generateKeyPair(new Name("/test/KEY/123"), new RsaKeyParams(
					2048));
		}
	
		/// <summary>
		/// Delete the keys we created
		/// </summary>
		///
		public static void tearDownClass() {
			// delete all keys when done
			FilePrivateKeyStorage instance = new FilePrivateKeyStorage();
			try {
				instance.deleteKey(new Name("/test/KEY/123"));
				instance.deleteKey(new Name("/test/KEY/temp1"));
			} catch (Exception e) {
				System.Console.Error.WriteLine("Failed to clean up generated keys");
			}
		}
	
		/// <summary>
		/// Convert the int array to a ByteBuffer.
		/// </summary>
		///
		/// <param name="array"></param>
		/// <returns></returns>
		private static ByteBuffer toBuffer(int[] array) {
			ByteBuffer result = ILOG.J2CsMapping.NIO.ByteBuffer.allocate(array.Length);
			for (int i = 0; i < array.Length; ++i)
				result.put((byte) (array[i] & 0xff));
	
			result.flip();
			return result;
		}
	
		/// <summary>
		/// Test of generateKeyPair method, of class FilePrivateKeyStorage.
		/// </summary>
		///
		public void testGenerateAndDeleteKeys() {
			// create some more key files
			FilePrivateKeyStorage instance = new FilePrivateKeyStorage();
			instance.generateKeyPair(new Name("/test/KEY/temp1"), new RsaKeyParams(
					2048));
			// check if files created
			FileInfo[] files = ndnFolder_.listFiles();
			int createdFileCount = files.Length;
			AssertTrue(createdFileCount >= 2); // 2 pre-created + 2 created now + some created by NFD
			// delete these keys
			instance.deleteKey(new Name("/test/KEY/temp1"));
			files = ndnFolder_.listFiles();
			int deletedfileCount = files.Length;
			AssertTrue(createdFileCount - 2 == deletedfileCount);
		}
	
		/// <summary>
		/// Test of doesKeyExist method, of class FilePrivateKeyStorage.
		/// </summary>
		///
		public void testDoesKeyExist() {
			FilePrivateKeyStorage instance = new FilePrivateKeyStorage();
			AssertTrue(instance.doesKeyExist(new Name("/test/KEY/123"),
					net.named_data.jndn.security.KeyClass.PRIVATE));
			AssertFalse(instance.doesKeyExist(new Name("/unknown"),
					net.named_data.jndn.security.KeyClass.PRIVATE));
		}
	
		/// <summary>
		/// Test of getPublicKey method, of class FilePrivateKeyStorage.
		/// </summary>
		///
		public void testGetPublicKey() {
			FilePrivateKeyStorage instance = new FilePrivateKeyStorage();
			PublicKey result = instance.getPublicKey(new Name("/test/KEY/123"));
			AssertNotNull(result);
		}
	
		/// <summary>
		/// Test of sign method, of class FilePrivateKeyStorage.
		/// </summary>
		///
		public void testSign() {
			int[] data = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
			FilePrivateKeyStorage instance = new FilePrivateKeyStorage();
			Blob result = instance.sign(toBuffer(data), new Name("/test/KEY/123"),
					net.named_data.jndn.security.DigestAlgorithm.SHA256);
			AssertNotNull(result);
		}
	}
}
