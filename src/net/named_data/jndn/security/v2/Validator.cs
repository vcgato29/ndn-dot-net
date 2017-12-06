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
namespace net.named_data.jndn.security.v2 {
	
	using ILOG.J2CsMapping.Util.Logging;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.security;
	
	/// <summary>
	/// The Validator class provides an interface for validating data and interest
	/// packets.
	/// Every time a validation process is initiated, it creates a ValidationState
	/// that exists until the validation finishes with either success or failure.
	/// This state serves several purposes:
	/// to record the Interest or Data packet being validated,
	/// to record the failure callback,
	/// to record certificates in the certification chain for the Interest or Data
	/// packet being validated,
	/// to record the names of the requested certificates in order to detect loops in
	/// the certificate chain,
	/// and to keep track of the validation chain size (also known as the validation
	/// "depth").
	/// During validation, the policy and/or key fetcher can augment the validation
	/// state with policy- and fetcher-specific information using tags.
	/// A Validator has a trust anchor cache to save static and dynamic trust
	/// anchors, a verified certificate cache for saving certificates that are
	/// already verified, and an unverified certificate cache for saving pre-fetched
	/// but not yet verified certificates.
	/// </summary>
	///
	public class Validator : CertificateStorage {
		public sealed class Anonymous_C3 : 				ValidationPolicy.ValidationContinuation {
				private readonly Validator outer_Validator;
		
				public Anonymous_C3(Validator paramouter_Validator) {
					this.outer_Validator = paramouter_Validator;
				}
		
				public void continueValidation(
						CertificateRequest certificateRequest,
						ValidationState state) {
					if (certificateRequest == null)
						state.bypassValidation_();
					else
						// We need to fetch the key and validate it.
						outer_Validator.requestCertificate(certificateRequest, state);
				}
			}
		public sealed class Anonymous_C2 : 				ValidationPolicy.ValidationContinuation {
				private readonly Validator outer_Validator;
		
				public Anonymous_C2(Validator paramouter_Validator) {
					this.outer_Validator = paramouter_Validator;
				}
		
				public void continueValidation(
						CertificateRequest certificateRequest,
						ValidationState state) {
					if (certificateRequest == null)
						state.bypassValidation_();
					else
						// We need to fetch the key and validate it.
						outer_Validator.requestCertificate(certificateRequest, state);
				}
			}
		public sealed class Anonymous_C1 : 				ValidationPolicy.ValidationContinuation {
				private readonly Validator outer_Validator;
				private readonly CertificateV2 certificate;
		
				public Anonymous_C1(Validator paramouter_Validator,
						CertificateV2 certificate_0) {
					this.certificate = certificate_0;
					this.outer_Validator = paramouter_Validator;
				}
		
				public void continueValidation(
						CertificateRequest certificateRequest,
						ValidationState state) {
					if (certificateRequest == null)
						state.fail(new ValidationError(
								net.named_data.jndn.security.v2.ValidationError.POLICY_ERROR,
								"Validation policy is not allowed to designate `"
										+ certificate.getName().toUri()
										+ "` as a trust anchor"));
					else {
						// We need to fetch the key and validate it.
						state.addCertificate(certificate);
						outer_Validator.requestCertificate(certificateRequest, state);
					}
				}
			}
		public sealed class Anonymous_C0 : 				CertificateFetcher.ValidationContinuation {
				private readonly Validator outer_Validator;
		
				public Anonymous_C0(Validator paramouter_Validator) {
					this.outer_Validator = paramouter_Validator;
				}
		
				public void continueValidation(CertificateV2 certificate_0,
						ValidationState state) {
					outer_Validator.validateCertificate(certificate_0, state);
				}
			}
		/// <summary>
		/// Create a Validator with the policy and fetcher.
		/// </summary>
		///
		/// <param name="policy">The validation policy to be associated with this validator.</param>
		/// <param name="certificateFetcher">The certificate fetcher implementation.</param>
		public Validator(ValidationPolicy policy,
				CertificateFetcher certificateFetcher) {
			policy_ = policy;
			certificateFetcher_ = certificateFetcher;
			maxDepth_ = 25;
	
			if (policy_ == null)
				throw new ArgumentException("The policy is null");
			if (certificateFetcher_ == null)
				throw new ArgumentException("The certificateFetcher is null");
	
			policy_.setValidator(this);
			certificateFetcher_.setCertificateStorage(this);
		}
	
		/// <summary>
		/// Create a Validator with the policy. Use a CertificateFetcherOffline
		/// (assuming that the validation policy doesn't need to fetch certificates).
		/// </summary>
		///
		/// <param name="policy">The validation policy to be associated with this validator.</param>
		public Validator(ValidationPolicy policy) {
			policy_ = policy;
			certificateFetcher_ = new CertificateFetcherOffline();
			maxDepth_ = 25;
	
			if (policy_ == null)
				throw new ArgumentException("The policy is null");
	
			policy_.setValidator(this);
			certificateFetcher_.setCertificateStorage(this);
		}
	
		/// <summary>
		/// Get the ValidationPolicy given to the constructor.
		/// </summary>
		///
		/// <returns>The ValidationPolicy.</returns>
		public ValidationPolicy getPolicy() {
			return policy_;
		}
	
		/// <summary>
		/// Get the CertificateFetcher given to the constructor.
		/// </summary>
		///
		/// <returns>The CertificateFetcher.</returns>
		public CertificateFetcher getFetcher() {
			return certificateFetcher_;
		}
	
		/// <summary>
		/// Set the maximum depth of the certificate chain.
		/// </summary>
		///
		/// <param name="maxDepth">The maximum depth.</param>
		public void setMaxDepth(int maxDepth) {
			maxDepth_ = maxDepth;
		}
	
		
		/// <returns>The maximum depth of the certificate chain</returns>
		public int getMaxDepth() {
			return maxDepth_;
		}
	
		/// <summary>
		/// Asynchronously validate the Data packet.
		/// </summary>
		///
		/// <param name="data">The Data packet to validate, which is copied.</param>
		/// <param name="successCallback"></param>
		/// <param name="failureCallback">ValidationError.</param>
		public void validate(Data data,
				DataValidationSuccessCallback successCallback,
				DataValidationFailureCallback failureCallback) {
			DataValidationState state = new DataValidationState(data,
					successCallback, failureCallback);
			logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE, "Start validating data {0}", data.getName()
					.toUri());
	
			policy_.checkPolicy(data, state,
					new Validator.Anonymous_C3 (this));
		}
	
		/// <summary>
		/// Asynchronously validate the Interest.
		/// </summary>
		///
		/// <param name="interest">The Interest to validate, which is copied.</param>
		/// <param name="successCallback"></param>
		/// <param name="failureCallback">ValidationError.</param>
		public void validate(Interest interest,
				InterestValidationSuccessCallback successCallback,
				InterestValidationFailureCallback failureCallback) {
			InterestValidationState state = new InterestValidationState(interest,
					successCallback, failureCallback);
			logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE, "Start validating interest {0}", interest
					.getName().toUri());
	
			policy_.checkPolicy(interest, state,
					new Validator.Anonymous_C2 (this));
		}
	
		/// <summary>
		/// Recursively validate the certificates in the certification chain.
		/// </summary>
		///
		/// <param name="certificate_0">The certificate to check.</param>
		/// <param name="state">The current validation state.</param>
		internal void validateCertificate(CertificateV2 certificate_0,
				ValidationState state) {
			logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE, "Start validating certificate {0}", certificate_0
					.getName().toUri());
	
			if (!certificate_0.isValid()) {
				state.fail(new ValidationError(net.named_data.jndn.security.v2.ValidationError.EXPIRED_CERTIFICATE,
						"Retrieved certificate is not yet valid or expired `"
								+ certificate_0.getName().toUri() + "`"));
				return;
			}
	
			policy_.checkCertificatePolicy(certificate_0, state,
					new Validator.Anonymous_C1 (this, certificate_0));
		}
	
		/// <summary>
		/// Request a certificate for further validation.
		/// </summary>
		///
		/// <param name="certificateRequest">The certificate request.</param>
		/// <param name="state">The current validation state.</param>
		internal void requestCertificate(CertificateRequest certificateRequest,
				ValidationState state) {
			if (state.getDepth() >= maxDepth_) {
				state.fail(new ValidationError(
						net.named_data.jndn.security.v2.ValidationError.EXCEEDED_DEPTH_LIMIT,
						"Exceeded validation depth limit"));
				return;
			}
	
			if (state
					.hasSeenCertificateName(certificateRequest.interest_.getName())) {
				state.fail(new ValidationError(net.named_data.jndn.security.v2.ValidationError.LOOP_DETECTED,
						"Validation loop detected for certificate `"
								+ certificateRequest.interest_.getName().toUri()
								+ "`"));
				return;
			}
	
			logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE, "Retrieving {0}", certificateRequest.interest_
					.getName().toUri());
	
			CertificateV2 certificate_0 = findTrustedCertificate(certificateRequest.interest_);
			if (certificate_0 != null) {
				logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE, "Found trusted certificate "
						+ certificate_0.getName().toUri());
	
				certificate_0 = state.verifyCertificateChain_(certificate_0);
				if (certificate_0 != null)
					state.verifyOriginalPacket_(certificate_0);
	
				for (int i = 0; i < state.getCertificateChain_().Count; ++i)
					cacheVerifiedCertificate(state.getCertificateChain_()[i]);
	
				return;
			}
	
			certificateFetcher_.fetch(certificateRequest, state,
					new Validator.Anonymous_C0 (this));
		}
	
		private readonly ValidationPolicy policy_;
		private readonly CertificateFetcher certificateFetcher_;
		private int maxDepth_;
		private static readonly Logger logger_ = ILOG.J2CsMapping.Util.Logging.Logger.getLogger(typeof(Validator).FullName);
	}
}
