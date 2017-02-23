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
	
	using ILOG.J2CsMapping.Threading;
	using ILOG.J2CsMapping.Util.Logging;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.security;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// Test that registration callbacks work as expected; optionally can use a
	/// non-localhost NFD (run with -Dnfd.hostname=...) but will use localhost by
	/// default.
	/// </summary>
	///
	public class TestRegistrationCallbacks {
	
		private static readonly Logger logger = ILOG.J2CsMapping.Util.Logging.Logger
				.getLogger(typeof(TestRegistrationCallbacks).FullName);
		private const long MAX_TEST_DURATION_MS = 10000;
		private const long PROCESS_EVENTS_INTERVAL_MS = 50;
		protected internal Face face;
	
		public void setUp() {
			// retrieve NFD hostname to use
			String hostname = System.Environment.GetEnvironmentVariable("nfd.hostname");
			if (hostname == null) {
				hostname = "localhost";
			}
	
			// build face
			face = net.named_data.jndn.tests.integration_tests.IntegrationTestsCommon.buildFaceWithKeyChain(hostname);
		}
	
		public void testRegistrationCallbacks() {
			long startTime = DateTime.Now.Millisecond;
			TestRegistrationCallbacks.Counter  counter = new TestRegistrationCallbacks.Counter ();
	
			// register the prefix and count when it registers successfully
			face.registerPrefix(new Name("/test/register/callbacks"),
					(OnInterestCallback) null, new TestRegistrationCallbacks.Anonymous_C1 (startTime), new TestRegistrationCallbacks.Anonymous_C0 (startTime, counter));
	
			// wait until complete or the test times out
			long endTime = startTime + MAX_TEST_DURATION_MS;
			while (counter.count < 1 && DateTime.Now.Millisecond < endTime) {
				face.processEvents();
				ILOG.J2CsMapping.Threading.ThreadWrapper.sleep(PROCESS_EVENTS_INTERVAL_MS);
			}
	
			Assert.AssertEquals(1, counter.count);
		}
	
		public sealed class Anonymous_C1 : OnRegisterFailed {
			private readonly long startTime;
	
			public Anonymous_C1(long startTime_0) {
				this.startTime = startTime_0;
			}
	
			public void onRegisterFailed(Name prefix) {
				long endTime = DateTime.Now.Millisecond;
				net.named_data.jndn.tests.integration_tests.TestRegistrationCallbacks.logger.log(ILOG.J2CsMapping.Util.Logging.Level.INFO, "Registration failed in (ms): "
						+ (endTime - startTime));
			}
		}
	
		public sealed class Anonymous_C0 : OnRegisterSuccess {
			private readonly long startTime;
			private readonly TestRegistrationCallbacks.Counter  counter;
	
			public Anonymous_C0(long startTime_0, TestRegistrationCallbacks.Counter  counter_1) {
				this.startTime = startTime_0;
				this.counter = counter_1;
			}
	
			public void onRegisterSuccess(Name prefix,
					long registeredPrefixId) {
				long endTime = DateTime.Now.Millisecond;
				counter.count++;
				net.named_data.jndn.tests.integration_tests.TestRegistrationCallbacks.logger.log(ILOG.J2CsMapping.Util.Logging.Level.INFO,
						"Registration succeeded in (ms): "
								+ (endTime - startTime));
			}
		}
	
		/// <summary>
		/// Helper class for enclosing a final reference int the callbacks
		/// </summary>
		///
		public class Counter {
	
			public Counter() {
				this.count = 0;
			}
	
			public int count;
		}
	
		// This is to force an import of net.named_data.jndn.util.
		private static Common dummyCommon_ = new Common();
	}
}
