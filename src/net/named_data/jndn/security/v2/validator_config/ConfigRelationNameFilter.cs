// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2017-2018 Regents of the University of California.
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
	
	/// <summary>
	/// ConfigRelationNameFilter extends ConfigFilter to check that the name is in
	/// the given relation to the packet name.
	/// The configuration
	/// "filter
	/// {
	/// type name
	/// name /example
	/// relation is-prefix-of
	/// }"
	/// creates ConfigRelationNameFilter("/example",
	/// ConfigNameRelation.Relation.IS_PREFIX_OF) .
	/// </summary>
	///
	public class ConfigRelationNameFilter : ConfigFilter {
		/// <summary>
		/// Create a ConfigRelationNameFilter for the given values.
		/// </summary>
		///
		/// <param name="name">The relation name, which is copied.</param>
		/// <param name="relation">The relation type as a ConfigNameRelation.Relation enum.</param>
		public ConfigRelationNameFilter(Name name,
				ConfigNameRelation.Relation relation) {
			// Copy the Name.
			name_ = new Name(name);
			relation_ = relation;
		}
	
		/// <summary>
		/// Implementation of the check for match.
		/// </summary>
		///
		/// <param name="packetName"></param>
		/// <returns>True for a match.</returns>
		protected internal override bool matchName(Name packetName) {
			return net.named_data.jndn.security.v2.validator_config.ConfigNameRelation.checkNameRelation(relation_, name_,
					packetName);
		}
	
		private readonly Name name_;
		private readonly ConfigNameRelation.Relation relation_;
	}
}
