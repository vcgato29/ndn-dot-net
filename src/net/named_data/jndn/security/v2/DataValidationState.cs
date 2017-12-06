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
	
	/// <summary>
	/// The DataValidationState class extends ValidationState to hold the validation
	/// state for a Data packet.
	/// </summary>
	///
	public class DataValidationState : ValidationState {
		/// <summary>
		/// Create a DataValidationState for the Data packet.
		/// The caller must ensure that state instance is valid until validation
		/// finishes (i.e., until validateCertificateChain() and
		/// validateOriginalPacket() have been called).
		/// </summary>
		///
		/// <param name="data">The Date packet being validated, which is copied.</param>
		/// <param name="successCallback"></param>
		/// <param name="failureCallback"></param>
		public DataValidationState(Data data,
				DataValidationSuccessCallback successCallback,
				DataValidationFailureCallback failureCallback) {
			data_ = data;
			successCallback_ = successCallback;
			failureCallback_ = failureCallback;
	
			if (successCallback_ == null)
				throw new ArgumentException("The successCallback is null");
			if (failureCallback_ == null)
				throw new ArgumentException("The failureCallback is null");
		}
	
		public override void fail(ValidationError error) {
			logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE, "" + error);
			try {
				failureCallback_.failureCallback(data_, error);
			} catch (Exception exception) {
				logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, "Error in failureCallback", exception);
			}
			setOutcome(false);
		}
	
		/// <summary>
		/// Get the original Data packet being validated which was given to the
		/// constructor.
		/// </summary>
		///
		/// <returns>The original Data packet.</returns>
		public Data getOriginalData() {
			return data_;
		}
	
		public override void verifyOriginalPacket_(CertificateV2 trustedCertificate) {
			if (net.named_data.jndn.security.VerificationHelpers.verifyDataSignature(data_, trustedCertificate)) {
				logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE, "OK signature for data `{0}`", data_
						.getName().toUri());
				try {
					successCallback_.successCallback(data_);
				} catch (Exception exception) {
					logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, "Error in successCallback", exception);
				}
				setOutcome(true);
			} else
				fail(new ValidationError(net.named_data.jndn.security.v2.ValidationError.INVALID_SIGNATURE,
						"Invalid signature of data `" + data_.getName().toUri()
								+ "`"));
		}
	
		public override void bypassValidation_() {
			logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE,
					"Signature verification bypassed for data `{0}`", data_
							.getName().toUri());
			try {
				successCallback_.successCallback(data_);
			} catch (Exception exception) {
				logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, "Error in successCallback", exception);
			}
			setOutcome(true);
		}
	
		private readonly Data data_;
		private readonly DataValidationSuccessCallback successCallback_;
		private readonly DataValidationFailureCallback failureCallback_;
		private static readonly Logger logger_ = ILOG.J2CsMapping.Util.Logging.Logger
				.getLogger(typeof(DataValidationState).FullName);
	}
}
