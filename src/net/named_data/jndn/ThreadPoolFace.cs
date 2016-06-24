// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2015-2016 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn {
	
	using ILOG.J2CsMapping.Util;
	using ILOG.J2CsMapping.Util.Logging;
	using System;
	using System.Collections;
	using System.Collections.concurrent;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn.encoding;
	using net.named_data.jndn.transport;
	
	/// <summary>
	/// ThreadPoolFace extends Face to provide the main methods for NDN communication
	/// by submitting to a given ScheduledExecutorService thread pool. This also
	/// uses the thread pool to schedule the interest timeouts.
	/// </summary>
	///
	public class ThreadPoolFace : Face {
		public sealed class Anonymous_C14 : OnData {
				public sealed class Anonymous_C24 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C14  outer_Anonymous_C14;
								private readonly Interest localInterest;
								private readonly Data data;
					
								public Anonymous_C24(ThreadPoolFace.Anonymous_C14  paramouter_Anonymous_C14,
										Interest localInterest_0, Data data_1) {
									this.localInterest = localInterest_0;
									this.data = data_1;
									this.outer_Anonymous_C14 = paramouter_Anonymous_C14;
								}
					
								// Call the passed-in onData.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C14.finalOnData.onData(localInterest, data);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, "Error in onData", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnData finalOnData;
		
				public Anonymous_C14(ThreadPoolFace paramouter_ThreadPoolFace,
						OnData finalOnData_0) {
					this.finalOnData = finalOnData_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onData(Interest localInterest_0, Data data_1) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C14.Anonymous_C24 (this, localInterest_0, data_1));
				}
			}
		public sealed class Anonymous_C13 : OnTimeout {
				public sealed class Anonymous_C23 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C13  outer_Anonymous_C13;
								private readonly Interest localInterest;
					
								public Anonymous_C23(ThreadPoolFace.Anonymous_C13  paramouter_Anonymous_C13,
										Interest localInterest_0) {
									this.localInterest = localInterest_0;
									this.outer_Anonymous_C13 = paramouter_Anonymous_C13;
								}
					
								// Call the passed-in onTimeout.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C13.finalOnTimeout.onTimeout(localInterest);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE,
												"Error in onTimeout", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnTimeout finalOnTimeout;
		
				public Anonymous_C13(ThreadPoolFace paramouter_ThreadPoolFace,
						OnTimeout finalOnTimeout_0) {
					this.finalOnTimeout = finalOnTimeout_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onTimeout(Interest localInterest_0) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C13.Anonymous_C23 (this, localInterest_0));
				}
			}
		public sealed class Anonymous_C12 : OnNetworkNack {
				public sealed class Anonymous_C22 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C12  outer_Anonymous_C12;
								private readonly Interest localInterest;
								private readonly NetworkNack networkNack;
					
								public Anonymous_C22(ThreadPoolFace.Anonymous_C12  paramouter_Anonymous_C12,
										Interest localInterest_0, NetworkNack networkNack_1) {
									this.localInterest = localInterest_0;
									this.networkNack = networkNack_1;
									this.outer_Anonymous_C12 = paramouter_Anonymous_C12;
								}
					
								// Call the passed-in onData.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C12.finalOnNetworkNack.onNetworkNack(
												localInterest, networkNack);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE,
												"Error in onNetworkNack", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnNetworkNack finalOnNetworkNack;
		
				public Anonymous_C12(ThreadPoolFace paramouter_ThreadPoolFace,
						OnNetworkNack finalOnNetworkNack_0) {
					this.finalOnNetworkNack = finalOnNetworkNack_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onNetworkNack(Interest localInterest_0,
						NetworkNack networkNack_1) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C12.Anonymous_C22 (this, localInterest_0, networkNack_1));
				}
			}
		public sealed class Anonymous_C11 : IRunnable {
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				private readonly long pendingInterestId;
				private readonly WireFormat wireFormat;
				private readonly OnData onDataSubmit;
				private readonly Interest interestCopy;
				private readonly OnTimeout onTimeoutSubmit;
				private readonly OnNetworkNack onNetworkNackSubmit;
		
				public Anonymous_C11(ThreadPoolFace paramouter_ThreadPoolFace,
						long pendingInterestId_0, WireFormat wireFormat_1,
						OnData onDataSubmit_2, Interest interestCopy_3,
						OnTimeout onTimeoutSubmit_4, OnNetworkNack onNetworkNackSubmit_5) {
					this.pendingInterestId = pendingInterestId_0;
					this.wireFormat = wireFormat_1;
					this.onDataSubmit = onDataSubmit_2;
					this.interestCopy = interestCopy_3;
					this.onTimeoutSubmit = onTimeoutSubmit_4;
					this.onNetworkNackSubmit = onNetworkNackSubmit_5;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void run() {
					// Need to catch and log exceptions at this async entry point.
					try {
						outer_ThreadPoolFace.node_.expressInterest(pendingInterestId, interestCopy,
								onDataSubmit, onTimeoutSubmit, onNetworkNackSubmit,
								wireFormat, outer_ThreadPoolFace);
					} catch (Exception ex) {
						net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, null, ex);
					}
				}
			}
		public sealed class Anonymous_C10 : OnData {
				public sealed class Anonymous_C21 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C10  outer_Anonymous_C10;
								private readonly Interest localInterest;
								private readonly Data data;
					
								public Anonymous_C21(ThreadPoolFace.Anonymous_C10  paramouter_Anonymous_C10,
										Interest localInterest_0, Data data_1) {
									this.localInterest = localInterest_0;
									this.data = data_1;
									this.outer_Anonymous_C10 = paramouter_Anonymous_C10;
								}
					
								// Call the passed-in onData.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C10.finalOnData.onData(localInterest, data);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, "Error in onData", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnData finalOnData;
		
				public Anonymous_C10(ThreadPoolFace paramouter_ThreadPoolFace,
						OnData finalOnData_0) {
					this.finalOnData = finalOnData_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onData(Interest localInterest_0, Data data_1) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C10.Anonymous_C21 (this, localInterest_0, data_1));
				}
			}
		public sealed class Anonymous_C9 : OnTimeout {
				public sealed class Anonymous_C20 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C9  outer_Anonymous_C9;
								private readonly Interest localInterest;
					
								public Anonymous_C20(ThreadPoolFace.Anonymous_C9  paramouter_Anonymous_C9,
										Interest localInterest_0) {
									this.localInterest = localInterest_0;
									this.outer_Anonymous_C9 = paramouter_Anonymous_C9;
								}
					
								// Call the passed-in onTimeout.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C9.finalOnTimeout.onTimeout(localInterest);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE,
												"Error in onTimeout", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnTimeout finalOnTimeout;
		
				public Anonymous_C9(ThreadPoolFace paramouter_ThreadPoolFace,
						OnTimeout finalOnTimeout_0) {
					this.finalOnTimeout = finalOnTimeout_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onTimeout(Interest localInterest_0) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C9.Anonymous_C20 (this, localInterest_0));
				}
			}
		public sealed class Anonymous_C8 : OnNetworkNack {
				public sealed class Anonymous_C19 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C8  outer_Anonymous_C8;
								private readonly NetworkNack networkNack;
								private readonly Interest localInterest;
					
								public Anonymous_C19(ThreadPoolFace.Anonymous_C8  paramouter_Anonymous_C8,
										NetworkNack networkNack_0, Interest localInterest_1) {
									this.networkNack = networkNack_0;
									this.localInterest = localInterest_1;
									this.outer_Anonymous_C8 = paramouter_Anonymous_C8;
								}
					
								// Call the passed-in onData.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C8.finalOnNetworkNack.onNetworkNack(
												localInterest, networkNack);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE,
												"Error in onNetworkNack", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnNetworkNack finalOnNetworkNack;
		
				public Anonymous_C8(ThreadPoolFace paramouter_ThreadPoolFace,
						OnNetworkNack finalOnNetworkNack_0) {
					this.finalOnNetworkNack = finalOnNetworkNack_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onNetworkNack(Interest localInterest_0,
						NetworkNack networkNack_1) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C8.Anonymous_C19 (this, networkNack_1, localInterest_0));
				}
			}
		public sealed class Anonymous_C7 : IRunnable {
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				private readonly Interest interestCopy;
				private readonly WireFormat wireFormat;
				private readonly long pendingInterestId;
				private readonly OnNetworkNack onNetworkNackSubmit;
				private readonly OnTimeout onTimeoutSubmit;
				private readonly OnData onDataSubmit;
		
				public Anonymous_C7(ThreadPoolFace paramouter_ThreadPoolFace,
						Interest interestCopy_0, WireFormat wireFormat_1,
						long pendingInterestId_2, OnNetworkNack onNetworkNackSubmit_3,
						OnTimeout onTimeoutSubmit_4, OnData onDataSubmit_5) {
					this.interestCopy = interestCopy_0;
					this.wireFormat = wireFormat_1;
					this.pendingInterestId = pendingInterestId_2;
					this.onNetworkNackSubmit = onNetworkNackSubmit_3;
					this.onTimeoutSubmit = onTimeoutSubmit_4;
					this.onDataSubmit = onDataSubmit_5;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void run() {
					// Need to catch and log exceptions at this async entry point.
					try {
						outer_ThreadPoolFace.node_.expressInterest(pendingInterestId, interestCopy,
								onDataSubmit, onTimeoutSubmit, onNetworkNackSubmit,
								wireFormat, outer_ThreadPoolFace);
					} catch (Exception ex) {
						net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, null, ex);
					}
				}
			}
		public sealed class Anonymous_C6 : OnInterestCallback {
				public sealed class Anonymous_C18 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C6  outer_Anonymous_C6;
								private readonly Name localPrefix;
								private readonly Interest interest;
								private readonly Face face;
								private readonly InterestFilter filter;
								private readonly long interestFilterId;
					
								public Anonymous_C18(ThreadPoolFace.Anonymous_C6  paramouter_Anonymous_C6,
										Name localPrefix_0, Interest interest_1, Face face_2,
										InterestFilter filter_3, long interestFilterId_4) {
									this.localPrefix = localPrefix_0;
									this.interest = interest_1;
									this.face = face_2;
									this.filter = filter_3;
									this.interestFilterId = interestFilterId_4;
									this.outer_Anonymous_C6 = paramouter_Anonymous_C6;
								}
					
								// Call the passed-in onInterest.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C6.finalOnInterest.onInterest(localPrefix,
												interest, face, interestFilterId,
												filter);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE,
												"Error in onInterest", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnInterestCallback finalOnInterest;
		
				public Anonymous_C6(ThreadPoolFace paramouter_ThreadPoolFace,
						OnInterestCallback finalOnInterest_0) {
					this.finalOnInterest = finalOnInterest_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onInterest(Name localPrefix_0,
						Interest interest_1, Face face_2,
						long interestFilterId_3,
						InterestFilter filter_4) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C6.Anonymous_C18 (this, localPrefix_0, interest_1, face_2, filter_4,
							interestFilterId_3));
				}
			}
		public sealed class Anonymous_C5 : OnRegisterFailed {
				public sealed class Anonymous_C17 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C5  outer_Anonymous_C5;
								private readonly Name localPrefix;
					
								public Anonymous_C17(ThreadPoolFace.Anonymous_C5  paramouter_Anonymous_C5,
										Name localPrefix_0) {
									this.localPrefix = localPrefix_0;
									this.outer_Anonymous_C5 = paramouter_Anonymous_C5;
								}
					
								// Call the passed-in onRegisterFailed.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C5.finalOnRegisterFailed.onRegisterFailed(localPrefix);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE,
												"Error in onRegisterFailed", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnRegisterFailed finalOnRegisterFailed;
		
				public Anonymous_C5(ThreadPoolFace paramouter_ThreadPoolFace,
						OnRegisterFailed finalOnRegisterFailed_0) {
					this.finalOnRegisterFailed = finalOnRegisterFailed_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onRegisterFailed(Name localPrefix_0) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C5.Anonymous_C17 (this, localPrefix_0));
				}
			}
		public sealed class Anonymous_C4 : OnRegisterSuccess {
				public sealed class Anonymous_C16 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C4  outer_Anonymous_C4;
								private readonly Name localPrefix;
								private readonly long localRegisteredPrefixId;
					
								public Anonymous_C16(ThreadPoolFace.Anonymous_C4  paramouter_Anonymous_C4,
										Name localPrefix_0, long localRegisteredPrefixId_1) {
									this.localPrefix = localPrefix_0;
									this.localRegisteredPrefixId = localRegisteredPrefixId_1;
									this.outer_Anonymous_C4 = paramouter_Anonymous_C4;
								}
					
								// Call the passed-in onRegisterSuccess.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C4.finalOnRegisterSuccess.onRegisterSuccess(
												localPrefix,
												localRegisteredPrefixId);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE,
												"Error in onRegisterSuccess", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnRegisterSuccess finalOnRegisterSuccess;
		
				public Anonymous_C4(ThreadPoolFace paramouter_ThreadPoolFace,
						OnRegisterSuccess finalOnRegisterSuccess_0) {
					this.finalOnRegisterSuccess = finalOnRegisterSuccess_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onRegisterSuccess(Name localPrefix_0,
						long localRegisteredPrefixId_1) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C4.Anonymous_C16 (this, localPrefix_0, localRegisteredPrefixId_1));
				}
			}
		public sealed class Anonymous_C3 : IRunnable {
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				private readonly OnRegisterSuccess onRegisterSuccessSubmit;
				private readonly long registeredPrefixId;
				private readonly WireFormat wireFormat;
				private readonly Name prefix;
				private readonly OnInterestCallback onInterestSubmit;
				private readonly OnRegisterFailed onRegisterFailedSubmit;
				private readonly ForwardingFlags flags;
		
				public Anonymous_C3(ThreadPoolFace paramouter_ThreadPoolFace,
						OnRegisterSuccess onRegisterSuccessSubmit_0,
						long registeredPrefixId_1, WireFormat wireFormat_2, Name prefix_3,
						OnInterestCallback onInterestSubmit_4,
						OnRegisterFailed onRegisterFailedSubmit_5, ForwardingFlags flags_6) {
					this.onRegisterSuccessSubmit = onRegisterSuccessSubmit_0;
					this.registeredPrefixId = registeredPrefixId_1;
					this.wireFormat = wireFormat_2;
					this.prefix = prefix_3;
					this.onInterestSubmit = onInterestSubmit_4;
					this.onRegisterFailedSubmit = onRegisterFailedSubmit_5;
					this.flags = flags_6;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void run() {
					// Need to catch and log exceptions at this async entry point.
					try {
						outer_ThreadPoolFace.node_.registerPrefix(registeredPrefixId, prefix,
								onInterestSubmit, onRegisterFailedSubmit,
								onRegisterSuccessSubmit, flags, wireFormat,
								outer_ThreadPoolFace.commandKeyChain_, outer_ThreadPoolFace.commandCertificateName_,
								outer_ThreadPoolFace);
					} catch (Exception ex) {
						net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, null, ex);
					}
				}
			}
		public sealed class Anonymous_C2 : OnInterestCallback {
				public sealed class Anonymous_C15 : IRunnable {
								private readonly ThreadPoolFace.Anonymous_C2  outer_Anonymous_C2;
								private readonly long interestFilterId;
								private readonly Name prefix;
								private readonly Face face;
								private readonly Interest interest;
								private readonly InterestFilter filter;
					
								public Anonymous_C15(ThreadPoolFace.Anonymous_C2  paramouter_Anonymous_C2,
										long interestFilterId_0, Name prefix_1, Face face_2,
										Interest interest_3, InterestFilter filter_4) {
									this.interestFilterId = interestFilterId_0;
									this.prefix = prefix_1;
									this.face = face_2;
									this.interest = interest_3;
									this.filter = filter_4;
									this.outer_Anonymous_C2 = paramouter_Anonymous_C2;
								}
					
								// Call the passed-in onInterest.
								public void run() {
									// Need to catch and log exceptions at this async entry point.
									try {
										outer_Anonymous_C2.finalOnInterest.onInterest(prefix, interest, face,
												interestFilterId, filter);
									} catch (Exception ex) {
										net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, "Error in onInterest", ex);
									}
								}
							}
		
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				internal readonly OnInterestCallback finalOnInterest;
		
				public Anonymous_C2(ThreadPoolFace paramouter_ThreadPoolFace,
						OnInterestCallback finalOnInterest_0) {
					this.finalOnInterest = finalOnInterest_0;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void onInterest(Name prefix_0, Interest interest_1,
						Face face_2, long interestFilterId_3,
						InterestFilter filter_4) {
					outer_ThreadPoolFace.threadPool_.submit(new net.named_data.jndn.ThreadPoolFace.Anonymous_C2.Anonymous_C15 (this, interestFilterId_3, prefix_0, face_2, interest_1,
							filter_4));
				}
			}
		public sealed class Anonymous_C1 : IRunnable {
				private readonly ThreadPoolFace outer_ThreadPoolFace;
				private readonly long interestFilterId;
				private readonly OnInterestCallback onInterestSubmit;
				private readonly InterestFilter filter;
		
				public Anonymous_C1(ThreadPoolFace paramouter_ThreadPoolFace,
						long interestFilterId_0, OnInterestCallback onInterestSubmit_1,
						InterestFilter filter_2) {
					this.interestFilterId = interestFilterId_0;
					this.onInterestSubmit = onInterestSubmit_1;
					this.filter = filter_2;
					this.outer_ThreadPoolFace = paramouter_ThreadPoolFace;
				}
		
				public void run() {
					// Need to catch and log exceptions at this async entry point.
					try {
						outer_ThreadPoolFace.node_.setInterestFilter(interestFilterId, filter,
								onInterestSubmit, outer_ThreadPoolFace);
					} catch (Exception ex) {
						net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, null, ex);
					}
				}
			}
		public sealed class Anonymous_C0 : IRunnable {
			private readonly IRunnable callback;
	
			public Anonymous_C0(IRunnable callback_0) {
				this.callback = callback_0;
			}
	
			public void run() {
				// Need to catch and log exceptions at this async entry point.
				try {
					callback.run();
				} catch (Exception ex) {
					net.named_data.jndn.ThreadPoolFace.logger_.log(ILOG.J2CsMapping.Util.Logging.Level.SEVERE, null, ex);
				}
			}
		}
		/// <summary>
		/// Create a new ThreadPoolFace for communication with an NDN hub with the given
		/// Transport object and connectionInfo.
		/// </summary>
		///
		/// <param name="threadPool">is also used to schedule the interest timeouts.</param>
		/// <param name="transport">like AsyncTcpTransport, in which case the transport should use the same ioService.</param>
		/// <param name="connectionInfo"></param>
		public ThreadPoolFace(ScheduledExecutorService threadPool,
				Transport transport, Transport.ConnectionInfo connectionInfo) : base(transport, connectionInfo) {
			threadPool_ = threadPool;
		}
	
		/// <summary>
		/// Override to submit a task to use the thread pool given to the constructor.
		/// Also wrap the supplied onData, onTimeout and onNetworkNack callbacks in an
		/// outer callback which submits a task to the thread pool to call the supplied
		/// callback. See Face.expressInterest for calling details.
		/// </summary>
		///
		public override long expressInterest(Interest interest_0, OnData onData,
				OnTimeout onTimeout, OnNetworkNack onNetworkNack,
				WireFormat wireFormat_1) {
			long pendingInterestId_2 = node_.getNextEntryId();
	
			// Wrap callbacks to submit to the thread pool.
			OnData finalOnData_3 = onData;
			OnData onDataSubmit_4 = new ThreadPoolFace.Anonymous_C14 (this, finalOnData_3);
	
			OnTimeout finalOnTimeout_5 = onTimeout;
			OnTimeout onTimeoutSubmit_6 = (onTimeout == null) ? null
					: new ThreadPoolFace.Anonymous_C13 (this, finalOnTimeout_5);
	
			OnNetworkNack finalOnNetworkNack_7 = onNetworkNack;
			OnNetworkNack onNetworkNackSubmit_8 = (onNetworkNack == null) ? null
					: new ThreadPoolFace.Anonymous_C12 (this, finalOnNetworkNack_7);
	
			// Make an interest copy as required by Node.expressInterest.
			Interest interestCopy_9 = new Interest(interest_0);
			threadPool_.submit(new ThreadPoolFace.Anonymous_C11 (this, pendingInterestId_2, wireFormat_1, onDataSubmit_4,
					interestCopy_9, onTimeoutSubmit_6, onNetworkNackSubmit_8));
	
			return pendingInterestId_2;
		}
	
		/// <summary>
		/// Override to submit a task to use the thread pool given to the constructor.
		/// Also wrap the supplied onData, onTimeout and onNetworkNack callbacks in an
		/// outer callback which submits a task to the thread pool to call the supplied
		/// callback. See Face.expressInterest for calling details. We make a separate
		/// expressInterest overload for supplying a Name vs. Interest to avoid making
		/// multiple copies of the Interest.
		/// </summary>
		///
		public override long expressInterest(Name name, Interest interestTemplate,
				OnData onData, OnTimeout onTimeout,
				OnNetworkNack onNetworkNack, WireFormat wireFormat_0) {
			long pendingInterestId_1 = node_.getNextEntryId();
	
			// Wrap callbacks to submit to the thread pool.
			OnData finalOnData_2 = onData;
			OnData onDataSubmit_3 = new ThreadPoolFace.Anonymous_C10 (this, finalOnData_2);
	
			OnTimeout finalOnTimeout_4 = onTimeout;
			OnTimeout onTimeoutSubmit_5 = (onTimeout == null) ? null
					: new ThreadPoolFace.Anonymous_C9 (this, finalOnTimeout_4);
	
			OnNetworkNack finalOnNetworkNack_6 = onNetworkNack;
			OnNetworkNack onNetworkNackSubmit_7 = (onNetworkNack == null) ? null
					: new ThreadPoolFace.Anonymous_C8 (this, finalOnNetworkNack_6);
	
			// Make an interest copy as required by Node.expressInterest.
			Interest interestCopy_8 = net.named_data.jndn.Face.getInterestCopy(name, interestTemplate);
			threadPool_.submit(new ThreadPoolFace.Anonymous_C7 (this, interestCopy_8, wireFormat_0, pendingInterestId_1,
					onNetworkNackSubmit_7, onTimeoutSubmit_5, onDataSubmit_3));
	
			return pendingInterestId_1;
		}
	
		/// <summary>
		/// Submit a task to the thread pool to register prefix with the connected
		/// forwarder and call onInterest when a matching interest is received. To
		/// register a prefix with NFD, you must first call setCommandSigningInfo.
		/// </summary>
		///
		/// <param name="prefix_0">A Name for the prefix to register. This copies the Name.</param>
		/// <param name="onInterest">onInterest.onInterest(prefix, interest, face, interestFilterId, filter). The onInterest callback should supply the Data with face.putData(). NOTE: You must not change the prefix or filter objects - if you need to change them then make a copy. If onInterest is null, it is ignored and you must call setInterestFilter. NOTE: The library will log any exceptions thrown by this callback, but for better error handling the callback should catch and properly handle any exceptions.</param>
		/// <param name="onRegisterFailed">NOTE: The library will log any exceptions thrown by this callback, but for better error handling the callback should catch and properly handle any exceptions.</param>
		/// <param name="onRegisterSuccess">receives a success message from the forwarder. If onRegisterSuccess is null, this does not use it. (The onRegisterSuccess parameter comes after onRegisterFailed because it can be null or omitted, unlike onRegisterFailed.) NOTE: The library will log any exceptions thrown by this callback, but for better error handling the callback should catch and properly handle any exceptions.</param>
		/// <param name="flags_1"></param>
		/// <param name="wireFormat_2">A WireFormat object used to encode the message.</param>
		/// <returns>The registered prefix ID which can be used with
		/// removeRegisteredPrefix.</returns>
		public override long registerPrefix(Name prefix_0,
				OnInterestCallback onInterest, OnRegisterFailed onRegisterFailed,
				OnRegisterSuccess onRegisterSuccess, ForwardingFlags flags_1,
				WireFormat wireFormat_2) {
			long registeredPrefixId_3 = node_.getNextEntryId();
	
			// Wrap callbacks to submit to the thread pool.
			OnInterestCallback finalOnInterest_4 = onInterest;
			OnInterestCallback onInterestSubmit_5 = (onInterest == null) ? null
					: new ThreadPoolFace.Anonymous_C6 (this, finalOnInterest_4);
	
			OnRegisterFailed finalOnRegisterFailed_6 = onRegisterFailed;
			OnRegisterFailed onRegisterFailedSubmit_7 = new ThreadPoolFace.Anonymous_C5 (this, finalOnRegisterFailed_6);
	
			// Wrap callbacks to submit to the thread pool.
			OnRegisterSuccess finalOnRegisterSuccess_8 = onRegisterSuccess;
			OnRegisterSuccess onRegisterSuccessSubmit_9 = (onRegisterSuccess == null) ? null
					: new ThreadPoolFace.Anonymous_C4 (this, finalOnRegisterSuccess_8);
	
			threadPool_.submit(new ThreadPoolFace.Anonymous_C3 (this, onRegisterSuccessSubmit_9, registeredPrefixId_3,
					wireFormat_2, prefix_0, onInterestSubmit_5, onRegisterFailedSubmit_7,
					flags_1));
	
			return registeredPrefixId_3;
		}
	
		/// <summary>
		/// Submit a task to the thread pool to add an entry to the local interest
		/// filter table to call the onInterest callback for a matching incoming
		/// Interest. This method only modifies the library's local callback table and
		/// does not register the prefix with the forwarder. It will always succeed.
		/// To register a prefix with the forwarder, use registerPrefix.
		/// </summary>
		///
		/// <param name="filter_0"></param>
		/// <param name="onInterest">onInterest.onInterest(prefix, interest, face, interestFilterId, filter). NOTE: The library will log any exceptions thrown by this callback, but for better error handling the callback should catch and properly handle any exceptions.</param>
		/// <returns>The interest filter ID which can be used with unsetInterestFilter.</returns>
		public override long setInterestFilter(InterestFilter filter_0,
				OnInterestCallback onInterest) {
			long interestFilterId_1 = node_.getNextEntryId();
	
			// Wrap callbacks to submit to the thread pool.
			OnInterestCallback finalOnInterest_2 = onInterest;
			OnInterestCallback onInterestSubmit_3 = new ThreadPoolFace.Anonymous_C2 (this, finalOnInterest_2);
	
			threadPool_.submit(new ThreadPoolFace.Anonymous_C1 (this, interestFilterId_1, onInterestSubmit_3, filter_0));
	
			return interestFilterId_1;
		}
	
		/// <summary>
		/// Override to schedule in the thread pool to call callback.run() after the
		/// given delay. Even though this is public, it is not part of the public API
		/// of Face.
		/// </summary>
		///
		/// <param name="delayMilliseconds">The delay in milliseconds.</param>
		/// <param name="callback_0">This calls callback.run() after the delay.</param>
		public override void callLater(double delayMilliseconds, IRunnable callback_0) {
			threadPool_.schedule(new ThreadPoolFace.Anonymous_C0 (callback_0), (long) delayMilliseconds, java.util.concurrent.TimeUnit.MILLISECONDS);
		}
	
		internal readonly ScheduledExecutorService threadPool_;
		static internal readonly Logger logger_ = ILOG.J2CsMapping.Util.Logging.Logger.getLogger(typeof(ThreadPoolFace).FullName);
	}
}
