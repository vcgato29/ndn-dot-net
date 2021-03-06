// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2018-2019 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.security.v2 {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.security;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// ValidationPolicyCommandInterest extends ValidationPolicy as a policy for
	/// stop-and-wait command Interests. See:
	/// https://redmine.named-data.net/projects/ndn-cxx/wiki/CommandInterest
	/// This policy checks the timestamp field of a stop-and-wait command Interest.
	/// Signed Interest validation and Data validation requests are delegated to an
	/// inner policy.
	/// </summary>
	///
	public class ValidationPolicyCommandInterest : ValidationPolicy {
		public sealed class Anonymous_C0 : 				InterestValidationSuccessCallback {
				private readonly ValidationPolicyCommandInterest outer_ValidationPolicyCommandInterest;
				private readonly double timestamp;
				private readonly Name keyName;
		
				public Anonymous_C0(
						ValidationPolicyCommandInterest paramouter_ValidationPolicyCommandInterest,
						double timestamp_0, Name keyName_1) {
					this.timestamp = timestamp_0;
					this.keyName = keyName_1;
					this.outer_ValidationPolicyCommandInterest = paramouter_ValidationPolicyCommandInterest;
				}
		
				public void successCallback(Interest interest) {
					outer_ValidationPolicyCommandInterest.insertNewRecord(interest, keyName, timestamp);
				}
			}
		public class Options {
			/// <summary>
			/// Create a ValidationPolicyCommandInterest.Options with the values.
			/// </summary>
			///
			/// <param name="gracePeriod">See below for description.</param>
			/// <param name="maxRecords">See below for description.</param>
			/// <param name="recordLifetime">See below for description.</param>
			public Options(double gracePeriod, int maxRecords, double recordLifetime) {
				gracePeriod_ = gracePeriod;
				maxRecords_ = maxRecords;
				recordLifetime_ = recordLifetime;
			}
	
			/// <summary>
			/// Create a ValidationPolicyCommandInterest.Options with the values and
			/// where recordLifetime is 1 hour.
			/// </summary>
			///
			/// <param name="gracePeriod">See below for description.</param>
			/// <param name="maxRecords">See below for description.</param>
			public Options(double gracePeriod, int maxRecords) {
				gracePeriod_ = gracePeriod;
				maxRecords_ = maxRecords;
				recordLifetime_ = 3600 * 1000.0d;
			}
	
			/// <summary>
			/// Create a ValidationPolicyCommandInterest.Options with the gracePeriod and
			/// where maxRecords is 1000 and recordLifetime is 1 hour.
			/// </summary>
			///
			/// <param name="gracePeriod">See below for description.</param>
			public Options(double gracePeriod) {
				gracePeriod_ = gracePeriod;
				maxRecords_ = 1000;
				recordLifetime_ = 3600 * 1000.0d;
			}
	
			/// <summary>
			/// Create a ValidationPolicyCommandInterest.Options where gracePeriod is 2
			/// minutes, maxRecords is 1000 and recordLifetime is 1 hour.
			/// </summary>
			///
			public Options() {
				gracePeriod_ = 2 * 60 * 1000.0d;
				maxRecords_ = 1000;
				recordLifetime_ = 3600 * 1000.0d;
			}
	
			/// <summary>
			/// Create a ValidationPolicyCommandInterest.Options from the given options.
			/// </summary>
			///
			/// <param name="options"></param>
			public Options(ValidationPolicyCommandInterest.Options  options) {
				gracePeriod_ = options.gracePeriod_;
				maxRecords_ = options.maxRecords_;
				recordLifetime_ = options.recordLifetime_;
			}
	
			/// <summary>
			/// gracePeriod is the tolerance of the initial timestamp in milliseconds.
			/// A stop-and-wait command Interest is considered "initial" if the validator
			/// has not recorded the last timestamp from the same public key, or when
			/// such knowledge has been erased. For an initial command Interest, its
			/// timestamp is compared to the current system clock, and the command
			/// Interest is rejected if the absolute difference is greater than the grace
			/// interval.
			/// This should be positive. Setting this option to 0 or negative causes the
			/// validator to require exactly the same timestamp as the system clock,
			/// which most likely rejects all command Interests.
			/// </summary>
			///
			public double gracePeriod_;
	
			/// <summary>
			/// maxRecords is the maximum number of distinct public keys of which to
			/// record the last timestamp.
			/// The validator records the last timestamps for every public key. For a
			/// subsequent command Interest using the same public key, its timestamp is
			/// compared to the last timestamp from that public key, and the command
			/// Interest is rejected if its timestamp is less than or equal to the
			/// recorded timestamp.
			/// This option limits the number of distinct public keys being tracked. If
			/// the limit is exceeded, then the oldest record is deleted.
			/// Setting this option to -1 allows tracking unlimited public keys. Setting
			/// this option to 0 disables using last timestamp records and causes every
			/// command Interest to be processed as initial.
			/// </summary>
			///
			public int maxRecords_;
	
			/// <summary>
			/// recordLifetime is the maximum lifetime of a last timestamp record in
			/// milliseconds.
			/// A last timestamp record expires and can be deleted if it has not been
			/// refreshed within this duration. Setting this option to 0 or negative
			/// makes last timestamp records expire immediately and causes every command
			/// Interest to be processed as initial.
			/// </summary>
			///
			public double recordLifetime_;
		}
	
		/// <summary>
		/// Create a ValidationPolicyCommandInterest.
		/// </summary>
		///
		/// <param name="innerPolicy"></param>
		/// <param name="options">The stop-and-wait command Interest validation options.</param>
		/// <exception cref="System.AssertionError">if innerPolicy is null.</exception>
		public ValidationPolicyCommandInterest(ValidationPolicy innerPolicy,
				ValidationPolicyCommandInterest.Options  options) {
			this.container_ = new ArrayList<LastTimestampRecord>();
					this.nowOffsetMilliseconds_ = 0;
			// Copy the Options.
			options_ = new ValidationPolicyCommandInterest.Options (options);
	
			if (innerPolicy == null)
				throw new AssertionError("inner policy is missing");
	
			setInnerPolicy(innerPolicy);
	
			if (options_.gracePeriod_ < 0.0d)
				options_.gracePeriod_ = 0.0d;
		}
	
		/// <summary>
		/// Create a ValidationPolicyCommandInterest with default Options.
		/// </summary>
		///
		/// <param name="innerPolicy"></param>
		/// <exception cref="System.AssertionError">if innerPolicy is null.</exception>
		public ValidationPolicyCommandInterest(ValidationPolicy innerPolicy) {
			this.container_ = new ArrayList<LastTimestampRecord>();
			this.nowOffsetMilliseconds_ = 0;
			options_ = new ValidationPolicyCommandInterest.Options ();
	
			if (innerPolicy == null)
				throw new AssertionError("inner policy is missing");
	
			setInnerPolicy(innerPolicy);
		}
	
		public override void checkPolicy(Data data, ValidationState state,
				ValidationPolicy.ValidationContinuation  continueValidation) {
			getInnerPolicy().checkPolicy(data, state, continueValidation);
		}
	
		public override void checkPolicy(Interest interest, ValidationState state,
				ValidationPolicy.ValidationContinuation  continueValidation) {
			Name[] keyName_0 = new Name[1];
			double[] timestamp_1 = new double[1];
			if (!parseCommandInterest(interest, state, keyName_0, timestamp_1))
				return;
	
			if (!checkTimestamp(state, keyName_0[0], timestamp_1[0]))
				return;
	
			getInnerPolicy().checkPolicy(interest, state, continueValidation);
		}
	
		/// <summary>
		/// Set the offset when insertNewRecord() and cleanUp() get the current time,
		/// which should only be used for testing.
		/// </summary>
		///
		/// <param name="nowOffsetMilliseconds">The offset in milliseconds.</param>
		public void setNowOffsetMilliseconds_(double nowOffsetMilliseconds) {
			nowOffsetMilliseconds_ = nowOffsetMilliseconds;
		}
	
		private class LastTimestampRecord {
			public LastTimestampRecord(Name keyName_0, double timestamp_1,
					double lastRefreshed) {
				// Copy the Name.
				keyName_ = new Name(keyName_0);
				timestamp_ = timestamp_1;
				lastRefreshed_ = lastRefreshed;
			}
	
			public Name keyName_;
			public double timestamp_;
			public double lastRefreshed_;
		} 
	
		private void cleanUp() {
			// nowOffsetMilliseconds_ is only used for testing.
			double now = net.named_data.jndn.util.Common.getNowMilliseconds() + nowOffsetMilliseconds_;
			double expiring = now - options_.recordLifetime_;
	
			while ((container_.Count > 0 && container_[0].lastRefreshed_ <= expiring)
					|| (options_.maxRecords_ >= 0 && container_.Count > options_.maxRecords_))
				ILOG.J2CsMapping.Collections.Collections.RemoveAt(container_,0);
		}
	
		/// <summary>
		/// Get the keyLocatorName and timestamp from the command interest.
		/// </summary>
		///
		/// <param name="interest">The Interest to parse.</param>
		/// <param name="state">On error, this calls state.fail and returns false.</param>
		/// <param name="keyLocatorName">Set keyLocatorName[0] to the KeyLocator name.</param>
		/// <param name="timestamp_0"></param>
		/// <returns>On success, return true. On error, call state.fail and return false.</returns>
		private static bool parseCommandInterest(Interest interest,
				ValidationState state, Name[] keyLocatorName, double[] timestamp_0) {
			keyLocatorName[0] = new Name();
			timestamp_0[0] = 0;
	
			Name name = interest.getName();
			if (name.size() < net.named_data.jndn.security.CommandInterestSigner.MINIMUM_SIZE) {
				state.fail(new ValidationError(net.named_data.jndn.security.v2.ValidationError.POLICY_ERROR,
						"Command interest name `" + interest.getName().toUri()
								+ "` is too short"));
				return false;
			}
	
			timestamp_0[0] = name.get(net.named_data.jndn.security.CommandInterestSigner.POS_TIMESTAMP).toNumber();
	
			keyLocatorName[0] = net.named_data.jndn.security.v2.ValidationPolicy.getKeyLocatorName(interest, state);
			if (state.isOutcomeFailed())
				// Already failed.
				return false;
	
			return true;
		}
	
		
		/// <param name="state">On error, this calls state.fail and returns false.</param>
		/// <param name="keyName_0">The key name.</param>
		/// <param name="timestamp_1">The timestamp as milliseconds since Jan 1, 1970 UTC.</param>
		/// <returns>On success, return true. On error, call state.fail and return false.</returns>
		private bool checkTimestamp(ValidationState state, Name keyName_0,
				double timestamp_1) {
			cleanUp();
	
			// nowOffsetMilliseconds_ is only used for testing.
			double now = net.named_data.jndn.util.Common.getNowMilliseconds() + nowOffsetMilliseconds_;
			if (timestamp_1 < now - options_.gracePeriod_
					|| timestamp_1 > now + options_.gracePeriod_) {
				state.fail(new ValidationError(net.named_data.jndn.security.v2.ValidationError.POLICY_ERROR,
						"Timestamp is outside the grace period for key "
								+ keyName_0.toUri()));
				return false;
			}
	
			int index = findByKeyName(keyName_0);
			if (index >= 0) {
				if (timestamp_1 <= container_[index].timestamp_) {
					state.fail(new ValidationError(net.named_data.jndn.security.v2.ValidationError.POLICY_ERROR,
							"Timestamp is reordered for key " + keyName_0.toUri()));
					return false;
				}
			}
	
			InterestValidationState interestState = (InterestValidationState) state;
			interestState
					.addSuccessCallback(new ValidationPolicyCommandInterest.Anonymous_C0 (this, timestamp_1, keyName_0));
	
			return true;
		}
	
		internal void insertNewRecord(Interest interest, Name keyName_0,
				double timestamp_1) {
			// nowOffsetMilliseconds_ is only used for testing.
			double now = net.named_data.jndn.util.Common.getNowMilliseconds() + nowOffsetMilliseconds_;
			ValidationPolicyCommandInterest.LastTimestampRecord  newRecord = new ValidationPolicyCommandInterest.LastTimestampRecord (keyName_0,
					timestamp_1, now);
	
			int index = findByKeyName(keyName_0);
			if (index >= 0)
				// Remove the existing record so we can move it to the end.
				ILOG.J2CsMapping.Collections.Collections.RemoveAt(container_,index);
	
			ILOG.J2CsMapping.Collections.Collections.Add(container_,newRecord);
		}
	
		/// <summary>
		/// Find the record in container_ which has the keyName.
		/// </summary>
		///
		/// <param name="keyName_0">The key name to search for.</param>
		/// <returns>The index in container_ of the record, or -1 if not found.</returns>
		internal int findByKeyName(Name keyName_0) {
			for (int i = 0; i < container_.Count; ++i) {
				if (container_[i].keyName_.equals(keyName_0))
					return i;
			}
	
			return -1;
		}
	
		private readonly ValidationPolicyCommandInterest.Options  options_;
		private readonly ArrayList<LastTimestampRecord> container_;
		private double nowOffsetMilliseconds_;
		// This is to force an import of net.named_data.jndn.util.
		private static Common dummyCommon_ = new Common();
	}
}
