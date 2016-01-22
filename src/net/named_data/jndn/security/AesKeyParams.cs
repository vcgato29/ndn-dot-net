// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
// 1/22/16 11:38 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2015-2016 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.security {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public class AesKeyParams : KeyParams {
		public AesKeyParams(int size) : base(getType()) {
			size_ = size;
		}
	
		public AesKeyParams() : base(getType()) {
			size_ = getDefaultSize();
		}
	
		public int getKeySize() {
			return size_;
		}
	
		private static int getDefaultSize() {
			return 64;
		}
	
		private static KeyType getType() {
			return net.named_data.jndn.security.KeyType.AES;
		}
	
		private readonly int size_;
	}
}
