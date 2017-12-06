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
	using net.named_data.jndn.encoding;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// A CertificateCacheV2 holds other user's verified certificates in security v2
	/// format CertificateV2. A certificate is removed no later than its NotAfter
	/// time, or maxLifetime after it has been added to the cache.
	/// </summary>
	///
	public class CertificateCacheV2 {
		/// <summary>
		/// Create a CertificateCacheV2.
		/// </summary>
		///
		/// <param name="maxLifetimeMilliseconds"></param>
		public CertificateCacheV2(double maxLifetimeMilliseconds) {
			this.certificatesByName_ = new SortedList();
			this.nextRefreshTime_ = System.Double.MaxValue;
			this.nowOffsetMilliseconds_ = 0;
			maxLifetimeMilliseconds_ = maxLifetimeMilliseconds;
		}
	
		/// <summary>
		/// Create a CertificateCacheV2. Set the maximum time that certificates can
		/// live inside the cache to getDefaultLifetime().
		/// </summary>
		///
		public CertificateCacheV2() {
			this.certificatesByName_ = new SortedList();
			this.nextRefreshTime_ = System.Double.MaxValue;
			this.nowOffsetMilliseconds_ = 0;
			maxLifetimeMilliseconds_ = getDefaultLifetime();
		}
	
		/// <summary>
		/// Insert the certificate into the cache. The inserted certificate will be
		/// removed no later than its NotAfter time, or maxLifetimeMilliseconds given
		/// to the constructor.
		/// </summary>
		///
		/// <param name="certificate">The certificate object, which is copied.</param>
		public void insert(CertificateV2 certificate) {
			double notAfterTime = certificate.getValidityPeriod().getNotAfter();
			// nowOffsetMilliseconds_ is only used for testing.
			double now = net.named_data.jndn.util.Common.getNowMilliseconds() + nowOffsetMilliseconds_;
			if (notAfterTime < now) {
				logger_.log(
						ILOG.J2CsMapping.Util.Logging.Level.FINE,
						"Not adding {0}: already expired at {1}",
						new Object[] { certificate.getName().toUri(),
								net.named_data.jndn.encrypt.Schedule.toIsoString(notAfterTime) });
				return;
			}
	
			double removalTime = Math.Min(notAfterTime,now
							+ maxLifetimeMilliseconds_);
			if (removalTime < nextRefreshTime_)
				// We need to run refresh() sooner.)
				nextRefreshTime_ = removalTime;
	
			double removalHours = (removalTime - now) / (3600 * 1000.0d);
			logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE, "Adding {0}, will remove in {1} hours",
					new Object[] { certificate.getName().toUri(), removalHours });
			CertificateV2 certificateCopy = new CertificateV2(certificate);
			ILOG.J2CsMapping.Collections.Collections.Put(certificatesByName_,certificateCopy.getName(),new CertificateCacheV2.Entry (
							certificateCopy, removalTime));
		}
	
		/// <summary>
		/// Find the certificate by the given key name.
		/// </summary>
		///
		/// <param name="certificatePrefix"></param>
		/// <returns>The found certificate, or null if not found. You must not modify
		/// the returned object. If you need to modify it, then make a copy.</returns>
		public CertificateV2 find(Name certificatePrefix) {
			if (certificatePrefix.size() > 0
					&& certificatePrefix.get(-1).isImplicitSha256Digest())
				logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE,
						"Certificate search using a name with an implicit digest is not yet supported");
	
			refresh();
	
			Name entryKey = (Name) certificatesByName_
					.ceilingKey(certificatePrefix);
			if (entryKey == null)
				return null;
	
			CertificateV2 certificate = ((CertificateCacheV2.Entry ) certificatesByName_[entryKey]).certificate_;
			if (!certificatePrefix.isPrefixOf(certificate.getName()))
				return null;
			return certificate;
		}
	
		/// <summary>
		/// Find the certificate by the given interest.
		/// </summary>
		///
		/// <param name="interest">The input interest object.</param>
		/// <returns>The found certificate which matches the interest, or null if not
		/// found. You must not modify the returned object. If you need to modify it,
		/// then make a copy.</returns>
		/// @note ChildSelector is not supported.
		public CertificateV2 find(Interest interest) {
			if (interest.getChildSelector() >= 0)
				logger_.log(
						ILOG.J2CsMapping.Util.Logging.Level.FINE,
						"Certificate search using a ChildSelector is not supported. Searching as if this selector not specified");
	
			if (interest.getName().size() > 0
					&& interest.getName().get(-1).isImplicitSha256Digest())
				logger_.log(ILOG.J2CsMapping.Util.Logging.Level.FINE,
						"Certificate search using a name with an implicit digest is not yet supported");
	
			refresh();
	
			Name firstKey = (Name) certificatesByName_.ceilingKey(interest
					.getName());
			if (firstKey == null)
				return null;
	
			/* foreach */
			foreach (Object key  in  certificatesByName_.navigableKeySet().tailSet(
					firstKey)) {
				CertificateV2 certificate = ((CertificateCacheV2.Entry ) certificatesByName_[(Name) key]).certificate_;
				if (!interest.getName().isPrefixOf(certificate.getName()))
					break;
	
				try {
					if (interest.matchesData(certificate))
						return certificate;
				} catch (EncodingException ex) {
					// We don't expect this. Promote to Error.
					throw new Exception("Error in Interest.matchesData: " + ex);
				}
			}
	
			return null;
		}
	
		/// <summary>
		/// Remove the certificate whose name equals the given name. If no such
		/// certificate is in the cache, do nothing.
		/// </summary>
		///
		/// <param name="certificateName">The name of the certificate.</param>
		public void deleteCertificate(Name certificateName) {
			ILOG.J2CsMapping.Collections.Collections.Remove(certificatesByName_,certificateName);
			// This may be the certificate to be removed at nextRefreshTime_ by refresh(),
			// but just allow refresh() to run instead of update nextRefreshTime_ now.
		}
	
		/// <summary>
		/// Clear all certificates from the cache.
		/// </summary>
		///
		public void clear() {
			certificatesByName_.clear();
			nextRefreshTime_ = System.Double.MaxValue;
		}
	
		/// <summary>
		/// Get the default maximum lifetime (1 hour).
		/// </summary>
		///
		/// <returns>The lifetime in milliseconds.</returns>
		public static double getDefaultLifetime() {
			return 3600.0d * 1000;
		}
	
		/// <summary>
		/// Set the offset when insert() and refresh() get the current time, which
		/// should only be used for testing.
		/// </summary>
		///
		/// <param name="nowOffsetMilliseconds">The offset in milliseconds.</param>
		public void setNowOffsetMilliseconds_(double nowOffsetMilliseconds) {
			nowOffsetMilliseconds_ = nowOffsetMilliseconds;
		}
	
		/// <summary>
		/// CertificateCacheV2.Entry is the value of the certificatesByName_ map.
		/// </summary>
		///
		private class Entry {
			/// <summary>
			/// Create a new CertificateCacheV2.Entry with the given values.
			/// </summary>
			///
			/// <param name="certificate">The certificate.</param>
			/// <param name="removalTime"></param>
			public Entry(CertificateV2 certificate, double removalTime) {
				certificate_ = certificate;
				removalTime_ = removalTime;
			}
	
			public readonly CertificateV2 certificate_;
			public readonly double removalTime_;
		} 
	
		/// <summary>
		/// Remove all outdated certificate entries.
		/// </summary>
		///
		private void refresh() {
			// nowOffsetMilliseconds_ is only used for testing.
			double now = net.named_data.jndn.util.Common.getNowMilliseconds() + nowOffsetMilliseconds_;
			if (now < nextRefreshTime_)
				return;
	
			// We recompute nextRefreshTime_.
			double nextRefreshTime = System.Double.MaxValue;
			// Keep a separate list of entries to erase since we can't erase while iterating.
			ArrayList<Name> namesToErase = new ArrayList<Name>();
			/* foreach */
			foreach (Object key  in  new ILOG.J2CsMapping.Collections.ListSet(certificatesByName_.Keys)) {
				Name name = (Name) key;
				CertificateCacheV2.Entry  entry = (CertificateCacheV2.Entry ) certificatesByName_[name];
	
				if (entry.removalTime_ <= now)
					ILOG.J2CsMapping.Collections.Collections.Add(namesToErase,name);
				else
					nextRefreshTime = Math.Min(nextRefreshTime,entry.removalTime_);
			}
	
			nextRefreshTime_ = nextRefreshTime;
			// Now actually erase.
			for (int i = 0; i < namesToErase.Count; ++i)
				ILOG.J2CsMapping.Collections.Collections.Remove(certificatesByName_,namesToErase[i]);
		}
	
		// Name => CertificateCacheV2.Entry..
		private readonly SortedList certificatesByName_;
		private double nextRefreshTime_;
		private readonly double maxLifetimeMilliseconds_;
		private static readonly Logger logger_ = ILOG.J2CsMapping.Util.Logging.Logger
				.getLogger(typeof(CertificateCacheV2).FullName);
		private double nowOffsetMilliseconds_;
	
		// This is to force an import of net.named_data.jndn.util.
		private static Common dummyCommon_ = new Common();
	}
}
