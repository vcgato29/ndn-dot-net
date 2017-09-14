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
	
	using ILOG.J2CsMapping.Util.Logging;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.security;
	using net.named_data.jndn.security.certificate;
	using net.named_data.jndn.security.identity;
	using net.named_data.jndn.security.policy;
	using net.named_data.jndn.util;
	
	public class TestIdentityMethods {
		private static double getNowSeconds() {
			return net.named_data.jndn.util.Common.getNowMilliseconds() / 1000.0d;
		}
	
		private static String RSA_DER = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAuFoDcNtffwbfFix64fw0"
				+ "hI2tKMkFrc6Ex7yw0YLMK9vGE8lXOyBl/qXabow6RCz+GldmFN6E2Qhm1+AX3Zm5"
				+ "sj3H53/HPtzMefvMQ9X7U+lK8eNMWawpRzvBh4/36VrK/awlkNIVIQ9aXj6q6BVe"
				+ "zL+zWT/WYemLq/8A1/hHWiwCtfOH1xQhGqWHJzeSgwIgOOrzxTbRaCjhAb1u2TeV"
				+ "yx/I9H/DV+AqSHCaYbB92HDcDN0kqwSnUf5H1+osE9MR5DLBLhXdSiULSgxT3Or/"
				+ "y2QgsgUK59WrjhlVMPEiHHRs15NZJbL1uQFXjgScdEarohcY3dilqotineFZCeN8"
				+ "DwIDAQAB";
	
		public void setUp() {
			// Don't show INFO log messages.
			ILOG.J2CsMapping.Util.Logging.Logger.getLogger("").setLevel(ILOG.J2CsMapping.Util.Logging.Level.WARNING);
	
			FileInfo policyConfigDirectory = net.named_data.jndn.tests.integration_tests.IntegrationTestsCommon
					.getPolicyConfigDirectory();
	
			databaseFilePath = new FileInfo(System.IO.Path.Combine(policyConfigDirectory.FullName,"test-public-info.db"));
			databaseFilePath.delete();
	
			identityStorage = new BasicIdentityStorage(
					databaseFilePath.FullName);
			identityManager = new IdentityManager(identityStorage,
					new FilePrivateKeyStorage());
			policyManager = new SelfVerifyPolicyManager(identityStorage);
			keyChain = new KeyChain(identityManager, policyManager);
		}
	
		public void tearDown() {
			databaseFilePath.delete();
		}
	
		public void testIdentityCreateDelete() {
			Name identityName = new Name("/TestIdentityStorage/Identity")
					.appendVersion((long) getNowSeconds());
	
			Name certificateName = keyChain
					.createIdentityAndCertificate(identityName);
			Name keyName = net.named_data.jndn.security.certificate.IdentityCertificate
					.certificateNameToPublicKeyName(certificateName);
	
			Assert.AssertTrue("Identity was not added to IdentityStorage",
					identityStorage.doesIdentityExist(identityName));
			Assert.AssertTrue("Key was not added to IdentityStorage",
					identityStorage.doesKeyExist(keyName));
	
			keyChain.deleteIdentity(identityName);
			Assert.AssertFalse(
					"Identity still in IdentityStorage after identity was deleted",
					identityStorage.doesIdentityExist(identityName));
			Assert.AssertFalse("Key still in IdentityStorage after identity was deleted",
					identityStorage.doesKeyExist(keyName));
			Assert.AssertFalse(
					"Certificate still in IdentityStorage after identity was deleted",
					identityStorage.doesCertificateExist(certificateName));
	
			try {
				identityManager.getDefaultCertificateNameForIdentity(identityName);
				Assert.Fail("The default certificate name for the identity was not deleted");
			} catch (SecurityException ex) {
			}
		}
	
		public void testKeyCreateDelete() {
			Name identityName = new Name("/TestIdentityStorage/Identity")
					.appendVersion((long) getNowSeconds());
	
			Name keyName1 = keyChain.generateRSAKeyPair(identityName, true);
			keyChain.getIdentityManager().setDefaultKeyForIdentity(keyName1);
	
			Name keyName2 = keyChain.generateRSAKeyPair(identityName, false);
	
			Assert.AssertTrue("Default key name was changed without explicit request",
					identityManager.getDefaultKeyNameForIdentity(identityName)
							.equals(keyName1));
			Assert.AssertFalse(
					"Newly created key replaced default key without explicit request",
					identityManager.getDefaultKeyNameForIdentity(identityName)
							.equals(keyName2));
	
			identityStorage.deletePublicKeyInfo(keyName2);
	
			Assert.AssertFalse(identityStorage.doesKeyExist(keyName2));
			identityStorage.deleteIdentityInfo(identityName);
		}
	
		public void testAutoCreateIdentity() {
			Name keyName1 = new Name(
					"/TestSqlIdentityStorage/KeyType/RSA/ksk-12345");
			Name identityName = keyName1.getPrefix(-1);
	
			byte[] decodedKey = net.named_data.jndn.util.Common.base64Decode(RSA_DER);
			identityStorage.addKey(keyName1, net.named_data.jndn.security.KeyType.RSA, new Blob(decodedKey,
					false));
			identityStorage.setDefaultKeyNameForIdentity(keyName1);
	
			Assert.AssertTrue("Key was not added", identityStorage.doesKeyExist(keyName1));
			Assert.AssertTrue("Identity for key was not automatically created",
					identityStorage.doesIdentityExist(identityName));
	
			Assert.AssertTrue("Default key was not set on identity creation",
					identityManager.getDefaultKeyNameForIdentity(identityName)
							.equals(keyName1));
	
			try {
				identityStorage.getDefaultCertificateNameForKey(keyName1);
				Assert.Fail();
			} catch (SecurityException ex) {
			}
	
			// We have no private key for signing.
			try {
				identityManager.selfSign(keyName1);
				Assert.Fail();
			} catch (SecurityException ex_0) {
			}
	
			try {
				identityStorage.getDefaultCertificateNameForKey(keyName1);
				Assert.Fail();
			} catch (SecurityException ex_1) {
			}
	
			try {
				identityManager.getDefaultCertificateNameForIdentity(identityName);
				Assert.Fail();
			} catch (SecurityException ex_2) {
			}
	
			Name keyName2 = identityManager
					.generateRSAKeyPairAsDefault(identityName);
			IdentityCertificate cert = identityManager.selfSign(keyName2);
			identityManager.addCertificateAsIdentityDefault(cert);
	
			Name certName1 = identityManager
					.getDefaultCertificateNameForIdentity(identityName);
			Name certName2 = identityStorage
					.getDefaultCertificateNameForKey(keyName2);
	
			Assert.AssertTrue(
					"Key-certificate mapping and identity-certificate mapping are not consistent",
					certName1.equals(certName2));
	
			keyChain.deleteIdentity(identityName);
			Assert.AssertFalse(identityStorage.doesKeyExist(keyName1));
		}
	
		public void testCertificateAddDelete() {
			Name identityName = new Name("/TestIdentityStorage/Identity")
					.appendVersion((long) getNowSeconds());
	
			identityManager.createIdentityAndCertificate(identityName,
					net.named_data.jndn.security.KeyChain.getDefaultKeyParams());
			Name keyName1 = identityManager
					.getDefaultKeyNameForIdentity(identityName);
			IdentityCertificate cert2 = identityManager.selfSign(keyName1);
			identityStorage.addCertificate(cert2);
			Name certName2 = cert2.getName();
	
			Name certName1 = identityManager
					.getDefaultCertificateNameForIdentity(identityName);
			Assert.AssertFalse(
					"New certificate was set as default without explicit request",
					certName1.equals(certName2));
	
			identityStorage.deleteCertificateInfo(certName1);
			Assert.AssertTrue(identityStorage.doesCertificateExist(certName2));
			Assert.AssertFalse(identityStorage.doesCertificateExist(certName1));
	
			keyChain.deleteIdentity(identityName);
			Assert.AssertFalse(identityStorage.doesCertificateExist(certName2));
		}
	
		public void testStress() {
			Name identityName = new Name("/TestSecPublicInfoSqlite3/Delete")
					.appendVersion((long) getNowSeconds());
	
			// ndn-cxx returns the cert name, but the IndentityManager docstring
			// specifies a key.
			Name certName1 = keyChain.createIdentityAndCertificate(identityName);
			Name keyName1 = net.named_data.jndn.security.certificate.IdentityCertificate
					.certificateNameToPublicKeyName(certName1);
			Name keyName2 = keyChain.generateRSAKeyPairAsDefault(identityName);
	
			IdentityCertificate cert2 = identityManager.selfSign(keyName2);
			Name certName2 = cert2.getName();
			identityManager.addCertificateAsDefault(cert2);
	
			Name keyName3 = keyChain.generateRSAKeyPairAsDefault(identityName);
			IdentityCertificate cert3 = identityManager.selfSign(keyName3);
			Name certName3 = cert3.getName();
			identityManager.addCertificateAsDefault(cert3);
	
			IdentityCertificate cert4 = identityManager.selfSign(keyName3);
			identityManager.addCertificateAsDefault(cert4);
			Name certName4 = cert4.getName();
	
			IdentityCertificate cert5 = identityManager.selfSign(keyName3);
			identityManager.addCertificateAsDefault(cert5);
			Name certName5 = cert5.getName();
	
			Assert.AssertTrue(identityStorage.doesIdentityExist(identityName));
			Assert.AssertTrue(identityStorage.doesKeyExist(keyName1));
			Assert.AssertTrue(identityStorage.doesKeyExist(keyName2));
			Assert.AssertTrue(identityStorage.doesKeyExist(keyName3));
			Assert.AssertTrue(identityStorage.doesCertificateExist(certName1));
			Assert.AssertTrue(identityStorage.doesCertificateExist(certName2));
			Assert.AssertTrue(identityStorage.doesCertificateExist(certName3));
			Assert.AssertTrue(identityStorage.doesCertificateExist(certName4));
			Assert.AssertTrue(identityStorage.doesCertificateExist(certName5));
	
			identityStorage.deleteCertificateInfo(certName5);
			Assert.AssertFalse(identityStorage.doesCertificateExist(certName5));
			Assert.AssertTrue(identityStorage.doesCertificateExist(certName4));
			Assert.AssertTrue(identityStorage.doesCertificateExist(certName3));
			Assert.AssertTrue(identityStorage.doesKeyExist(keyName2));
	
			identityStorage.deletePublicKeyInfo(keyName3);
			Assert.AssertFalse(identityStorage.doesCertificateExist(certName4));
			Assert.AssertFalse(identityStorage.doesCertificateExist(certName3));
			Assert.AssertFalse(identityStorage.doesKeyExist(keyName3));
			Assert.AssertTrue(identityStorage.doesKeyExist(keyName2));
			Assert.AssertTrue(identityStorage.doesKeyExist(keyName1));
			Assert.AssertTrue(identityStorage.doesIdentityExist(identityName));
	
			keyChain.deleteIdentity(identityName);
			Assert.AssertFalse(identityStorage.doesCertificateExist(certName2));
			Assert.AssertFalse(identityStorage.doesKeyExist(keyName2));
			Assert.AssertFalse(identityStorage.doesCertificateExist(certName1));
			Assert.AssertFalse(identityStorage.doesKeyExist(keyName1));
			Assert.AssertFalse(identityStorage.doesIdentityExist(identityName));
		}
	
#if false // Skip ECDSA for now.
		public void testEcdsaIdentity() {
			Name identityName = new Name("/TestSqlIdentityStorage/KeyType/ECDSA");
			Name keyName = identityManager
					.generateEcdsaKeyPairAsDefault(identityName);
			IdentityCertificate cert = identityManager.selfSign(keyName);
			identityManager.addCertificateAsIdentityDefault(cert);
	
			// Check the self-signature.
			VerifyCounter counter = new VerifyCounter();
			keyChain.verifyData(cert, counter, counter);
			Assert.AssertEquals("Verification callback was not used.", 1,
					counter.onVerifiedCallCount_);
	
			keyChain.deleteIdentity(identityName);
			Assert.AssertFalse(identityStorage.doesKeyExist(keyName));
		}
#endif
	
		private FileInfo databaseFilePath;
		private IdentityStorage identityStorage;
		private IdentityManager identityManager;
		private PolicyManager policyManager;
		private KeyChain keyChain;
	}
}
