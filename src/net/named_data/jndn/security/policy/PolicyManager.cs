// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2013-2016 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.security.policy {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.spec;
	using net.named_data.jndn;
	using net.named_data.jndn.encoding;
	using net.named_data.jndn.security;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// A PolicyManager is an abstract base class to represent the policy for
	/// verifying data packets.
	/// You must create an object of a subclass.
	/// </summary>
	///
	public abstract class PolicyManager {
		/// <summary>
		/// Check if the received data packet can escape from verification and be
		/// trusted as valid.
		/// </summary>
		///
		/// <param name="data">The received data packet.</param>
		/// <returns>true if the data does not need to be verified to be trusted as
		/// valid, otherwise false.</returns>
		public abstract bool skipVerifyAndTrust(Data data);
	
		/// <summary>
		/// Check if the received signed interest can escape from verification and be
		/// trusted as valid.
		/// </summary>
		///
		/// <param name="interest">The received interest.</param>
		/// <returns>true if the interest does not need to be verified to be trusted as
		/// valid, otherwise false.</returns>
		public abstract bool skipVerifyAndTrust(Interest interest);
	
		/// <summary>
		/// Check if this PolicyManager has a verification rule for the received data.
		/// </summary>
		///
		/// <param name="data">The received data packet.</param>
		/// <returns>true if the data must be verified, otherwise false.</returns>
		public abstract bool requireVerify(Data data);
	
		/// <summary>
		/// Check if this PolicyManager has a verification rule for the received interest.
		/// </summary>
		///
		/// <param name="interest">The received interest.</param>
		/// <returns>true if the interest must be verified, otherwise false.</returns>
		public abstract bool requireVerify(Interest interest);
	
		/// <summary>
		/// Check whether the received data packet complies with the verification
		/// policy, and get the indication of the next verification step.
		/// </summary>
		///
		/// <param name="data">The Data object with the signature to check.</param>
		/// <param name="stepCount"></param>
		/// <param name="onVerified">NOTE: The library will log any exceptions thrown by this callback, but for better error handling the callback should catch and properly handle any exceptions.</param>
		/// <param name="onValidationFailed">NOTE: The library will log any exceptions thrown by this callback, but for better error handling the callback should catch and properly handle any exceptions.</param>
		/// <returns>the indication of next verification step, null if there is no
		/// further step.</returns>
		public abstract ValidationRequest checkVerificationPolicy(Data data,
				int stepCount, OnVerified onVerified,
				OnDataValidationFailed onValidationFailed);
	
		/// <summary>
		/// Check whether the received signed interest complies with the verification
		/// policy, and get the indication of the next verification step.
		/// </summary>
		///
		/// <param name="interest">The interest with the signature to check.</param>
		/// <param name="stepCount"></param>
		/// <param name="onVerified">NOTE: The library will log any exceptions thrown by this callback, but for better error handling the callback should catch and properly handle any exceptions.</param>
		/// <param name="onValidationFailed">NOTE: The library will log any exceptions thrown by this callback, but for better error handling the callback should catch and properly handle any exceptions.</param>
		/// <returns>the indication of next verification step, null if there is no
		/// further step.</returns>
		public abstract ValidationRequest checkVerificationPolicy(
				Interest interest, int stepCount, OnVerifiedInterest onVerified,
				OnInterestValidationFailed onValidationFailed, WireFormat wireFormat);
	
		public ValidationRequest checkVerificationPolicy(Interest interest,
				int stepCount, OnVerifiedInterest onVerified,
				OnInterestValidationFailed onValidationFailed) {
			return checkVerificationPolicy(interest, stepCount, onVerified,
					onValidationFailed, net.named_data.jndn.encoding.WireFormat.getDefaultWireFormat());
		}
	
		/// <summary>
		/// Check if the signing certificate name and data name satisfy the signing
		/// policy.
		/// </summary>
		///
		/// <param name="dataName">The name of data to be signed.</param>
		/// <param name="certificateName">The name of signing certificate.</param>
		/// <returns>true if the signing certificate can be used to sign the data,
		/// otherwise false.</returns>
		public abstract bool checkSigningPolicy(Name dataName,
				Name certificateName);
	
		/// <summary>
		/// Infer the signing identity name according to the policy. If the signing
		/// identity cannot be inferred, return an empty name.
		/// </summary>
		///
		/// <param name="dataName">The name of data to be signed.</param>
		/// <returns>The signing identity or an empty name if cannot infer.</returns>
		public abstract Name inferSigningIdentity(Name dataName);
	
		/// <summary>
		/// Check the type of signature and use the publicKeyDer to verify the
		/// signedBlob using the appropriate signature algorithm.
		/// </summary>
		///
		/// <param name="signature"></param>
		/// <param name="signedBlob">the SignedBlob with the signed portion to verify.</param>
		/// <param name="publicKeyDer"></param>
		/// <returns>True if the signature is verified, false if failed.</returns>
		/// <exception cref="System.Security.SecurityException">if the signature type is not recognized or ifpublicKeyDer can't be decoded.</exception>
		protected static internal bool verifySignature(
				net.named_data.jndn.Signature signature, SignedBlob signedBlob,
				Blob publicKeyDer) {
			if (signature  is  Sha256WithRsaSignature) {
				if (publicKeyDer.isNull())
					return false;
				return verifySha256WithRsaSignature(signature.getSignature(),
						signedBlob, publicKeyDer);
			} else if (signature  is  Sha256WithEcdsaSignature) {
				if (publicKeyDer.isNull())
					return false;
				return verifySha256WithEcdsaSignature(signature.getSignature(),
						signedBlob, publicKeyDer);
			} else if (signature  is  DigestSha256Signature)
				return verifyDigestSha256Signature(signature.getSignature(),
						signedBlob);
			else
				// We don't expect this to happen.
				throw new SecurityException(
						"PolicyManager.verify: Signature type is unknown");
		}
	
		/// <summary>
		/// Verify the RSA signature on the SignedBlob using the given public key.
		/// </summary>
		///
		/// <param name="signature">The signature bits.</param>
		/// <param name="signedBlob">the SignedBlob with the signed portion to verify.</param>
		/// <param name="publicKeyDer">The DER-encoded public key used to verify the signature.</param>
		/// <returns>true if the signature verifies, false if not.</returns>
		protected static internal bool verifySha256WithRsaSignature(Blob signature,
				SignedBlob signedBlob, Blob publicKeyDer) {
			KeyFactory keyFactory = null;
			try {
				keyFactory = System.KeyFactory.getInstance("RSA");
			} catch (Exception exception) {
				// Don't expect this to happen.
				throw new SecurityException("RSA is not supported: "
						+ exception.Message);
			}
	
			System.SecurityPublicKey publicKey = null;
			try {
				publicKey = keyFactory.generatePublic(new X509EncodedKeySpec(
						publicKeyDer.getImmutableArray()));
			} catch (InvalidKeySpecException exception_0) {
				// Don't expect this to happen.
				throw new SecurityException("X509EncodedKeySpec is not supported: "
						+ exception_0.Message);
			}
	
			System.SecuritySignature rsaSignature = null;
			try {
				rsaSignature = System.SecuritySignature.getInstance("SHA256withRSA");
			} catch (Exception e) {
				// Don't expect this to happen.
				throw new SecurityException(
						"SHA256withRSA algorithm is not supported");
			}
	
			try {
				rsaSignature.initVerify(publicKey);
			} catch (InvalidKeyException exception_1) {
				throw new SecurityException("InvalidKeyException: "
						+ exception_1.Message);
			}
			try {
				rsaSignature.update(signedBlob.signedBuf());
				return rsaSignature.verify(signature.getImmutableArray());
			} catch (SignatureException exception_2) {
				throw new SecurityException("SignatureException: "
						+ exception_2.Message);
			}
		}
	
		/// <summary>
		/// Verify the ECDSA signature on the SignedBlob using the given public key.
		/// </summary>
		///
		/// <param name="signature">The signature bits.</param>
		/// <param name="signedBlob">the SignedBlob with the signed portion to verify.</param>
		/// <param name="publicKeyDer">The DER-encoded public key used to verify the signature.</param>
		/// <returns>true if the signature verifies, false if not.</returns>
		protected static internal bool verifySha256WithEcdsaSignature(Blob signature,
				SignedBlob signedBlob, Blob publicKeyDer) {
			KeyFactory keyFactory = null;
			try {
				keyFactory = System.KeyFactory.getInstance("EC");
			} catch (Exception exception) {
				// Don't expect this to happen.
				throw new SecurityException("EC is not supported: "
						+ exception.Message);
			}
	
			System.SecurityPublicKey publicKey = null;
			try {
				publicKey = keyFactory.generatePublic(new X509EncodedKeySpec(
						publicKeyDer.getImmutableArray()));
			} catch (InvalidKeySpecException exception_0) {
				// Don't expect this to happen.
				throw new SecurityException("X509EncodedKeySpec is not supported: "
						+ exception_0.Message);
			}
	
			System.SecuritySignature ecSignature = null;
			try {
				ecSignature = System.SecuritySignature
						.getInstance("SHA256withECDSA");
			} catch (Exception e) {
				// Don't expect this to happen.
				throw new SecurityException(
						"SHA256withECDSA algorithm is not supported");
			}
	
			try {
				ecSignature.initVerify(publicKey);
			} catch (InvalidKeyException exception_1) {
				throw new SecurityException("InvalidKeyException: "
						+ exception_1.Message);
			}
			try {
				ecSignature.update(signedBlob.signedBuf());
				return ecSignature.verify(signature.getImmutableArray());
			} catch (SignatureException exception_2) {
				throw new SecurityException("SignatureException: "
						+ exception_2.Message);
			}
		}
	
		/// <summary>
		/// Verify the DigestSha256 signature on the SignedBlob by verifying that the
		/// digest of SignedBlob equals the signature.
		/// </summary>
		///
		/// <param name="signature">The signature bits.</param>
		/// <param name="signedBlob">the SignedBlob with the signed portion to verify.</param>
		/// <returns>true if the signature verifies, false if not.</returns>
		protected static internal bool verifyDigestSha256Signature(Blob signature,
				SignedBlob signedBlob) {
			// Set signedPortionDigest to the digest of the signed portion of the signedBlob.
			byte[] signedPortionDigest = net.named_data.jndn.util.Common
					.digestSha256(signedBlob.signedBuf());
	
			return ILOG.J2CsMapping.Collections.Arrays.Equals(signedPortionDigest,signature.getImmutableArray());
		}
	}
}
