// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2017-2019 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.security.v2.validator_config {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.security;
	using net.named_data.jndn.security.v2;
	
	public class ConfigNameRelationChecker : ConfigChecker {
		public ConfigNameRelationChecker(Name name,
				ConfigNameRelation.Relation relation) {
			name_ = name;
			relation_ = relation;
		}
	
		protected internal override bool checkNames(Name packetName, Name keyLocatorName,
				ValidationState state) {
			// packetName is not used in this check.
	
			Name identity = net.named_data.jndn.security.pib.PibKey.extractIdentityFromKeyName(keyLocatorName);
			bool result = net.named_data.jndn.security.v2.validator_config.ConfigNameRelation.checkNameRelation(relation_, name_,
					identity);
			if (!result)
				state.fail(new ValidationError(net.named_data.jndn.security.v2.ValidationError.POLICY_ERROR,
						"KeyLocator check failed: name relation " + name_.toUri()
								+ " " + net.named_data.jndn.security.v2.validator_config.ConfigNameRelation.toString(relation_)
								+ " for packet " + packetName.toUri()
								+ " is invalid (KeyLocator="
								+ keyLocatorName.toUri() + ", identity="
								+ identity.toUri() + ")"));
	
			return result;
		}
	
		private readonly Name name_;
		private readonly ConfigNameRelation.Relation relation_;
	}
}
