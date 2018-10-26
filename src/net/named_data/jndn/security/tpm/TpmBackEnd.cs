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
namespace net.named_data.jndn.security.tpm {
	
	using ILOG.J2CsMapping.NIO;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.security;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// TpmBackEnd is an abstract base class for a TPM backend implementation which
	/// provides a TpmKeyHandle to the TPM front end. This class defines the
	/// interface that an actual TPM backend implementation should provide, for
	/// example TpmBackEndMemory.
	/// </summary>
	///
	public abstract class TpmBackEnd {
		/// <summary>
		/// A TpmBackEnd.Error extends Exception and represents a non-semantic
		/// error in backend TPM processing.
		/// Note that even though this is called "Error" to be consistent with the
		/// other libraries, it extends the Java Exception class, not Error.
		/// </summary>
		///
		[Serializable]
		public class Error : Exception {
			public Error(String message) : base(message) {
			}
		}
	
		/// <summary>
		/// Check if the key with name keyName exists in the TPM.
		/// </summary>
		///
		/// <param name="keyName">The name of the key.</param>
		/// <returns>True if the key exists.</returns>
		public bool hasKey(Name keyName) {
			return doHasKey(keyName);
		}
	
		/// <summary>
		/// Get the handle of the key with name keyName.
		/// Calling getKeyHandle multiple times with the same keyName will return
		/// different TpmKeyHandle objects that all refer to the same key.
		/// </summary>
		///
		/// <param name="keyName">The name of the key.</param>
		/// <returns>The handle of the key, or null if the key does not exist.</returns>
		public TpmKeyHandle getKeyHandle(Name keyName) {
			return doGetKeyHandle(keyName);
		}
	
		/// <summary>
		/// Create a key for the identityName according to params.
		/// </summary>
		///
		/// <param name="identityName">The name if the identity.</param>
		/// <param name="params">The KeyParams for creating the key.</param>
		/// <returns>The handle of the created key.</returns>
		/// <exception cref="Tpm.Error">if params is invalid.</exception>
		/// <exception cref="TpmBackEnd.Error">if the key cannot be created.</exception>
		public TpmKeyHandle createKey(Name identityName, KeyParams paras) {
			// Do key name checking.
			if (paras.getKeyIdType() == net.named_data.jndn.security.KeyIdType.USER_SPECIFIED) {
				// The keyId is pre-set.
				Name keyName = net.named_data.jndn.security.pib.PibKey.constructKeyName(identityName,
						paras.getKeyId());
				if (hasKey(keyName))
					throw new Tpm.Error("Key `" + keyName.toUri()
							+ "` already exists");
			} else if (paras.getKeyIdType() == net.named_data.jndn.security.KeyIdType.SHA256) {
				// The key name will be assigned in setKeyName after the key is generated.
			} else if (paras.getKeyIdType() == net.named_data.jndn.security.KeyIdType.RANDOM) {
				Name keyName_0;
				Name.Component keyId;
				ByteBuffer random = ILOG.J2CsMapping.NIO.ByteBuffer.allocate(8);
				do {
					net.named_data.jndn.util.Common.getRandom().nextBytes(random.array());
					keyId = new Name.Component(new Blob(random, false));
					keyName_0 = net.named_data.jndn.security.pib.PibKey.constructKeyName(identityName, keyId);
				} while (hasKey(keyName_0));
	
				paras.setKeyId(keyId);
			} else
				throw new Tpm.Error("Unsupported key id type");
	
			return doCreateKey(identityName, paras);
		}
	
		/// <summary>
		/// Delete the key with name keyName. If the key doesn't exist, do nothing.
		/// Note: Continuing to use existing Key handles on a deleted key results in
		/// undefined behavior.
		/// </summary>
		///
		/// <param name="keyName">The name of the key to delete.</param>
		/// <exception cref="TpmBackEnd.Error">if the deletion fails.</exception>
		public void deleteKey(Name keyName) {
			doDeleteKey(keyName);
		}
	
		/// <summary>
		/// Get the encoded private key with name keyName in PKCS #8 format, possibly
		/// password-encrypted.
		/// </summary>
		///
		/// <param name="keyName">The name of the key in the TPM.</param>
		/// <param name="password">it to return a PKCS #8 EncryptedPrivateKeyInfo. If the password is null, return an unencrypted PKCS #8 PrivateKeyInfo.</param>
		/// <returns>The encoded private key.</returns>
		/// <exception cref="TpmBackEnd.Error">if the key does not exist or if the key cannot beexported, e.g., insufficient privileges.</exception>
		public Blob exportKey(Name keyName, ByteBuffer password) {
			if (!hasKey(keyName))
				throw new TpmBackEnd.Error ("Key `" + keyName.toUri() + "` does not exist");
	
			return doExportKey(keyName, password);
		}
	
		/// <summary>
		/// Import an encoded private key with name keyName in PKCS #8 format, possibly
		/// password-encrypted.
		/// </summary>
		///
		/// <param name="keyName">The name of the key to use in the TPM.</param>
		/// <param name="pkcs8">unencrypted PKCS #8 PrivateKeyInfo.</param>
		/// <param name="password">it to decrypt the PKCS #8 EncryptedPrivateKeyInfo. If the password is null, import an unencrypted PKCS #8 PrivateKeyInfo.</param>
		/// <exception cref="TpmBackEnd.Error">if a key with name keyName already exists, or foran error importing the key.</exception>
		public void importKey(Name keyName, ByteBuffer pkcs8,
				ByteBuffer password) {
			if (hasKey(keyName))
				throw new TpmBackEnd.Error ("Key `" + keyName.toUri() + "` already exists");
	
			doImportKey(keyName, pkcs8, password);
		}
	
		/// <summary>
		/// Check if the TPM is in terminal mode. The default implementation always
		/// returns true.
		/// </summary>
		///
		/// <returns>True if in terminal mode.</returns>
		public bool isTerminalMode() {
			return true;
		}
	
		/// <summary>
		/// Set the terminal mode of the TPM. In terminal mode, the TPM will not ask
		/// for a password from the GUI. The default implementation does nothing.
		/// </summary>
		///
		/// <param name="isTerminal">True to enable terminal mode.</param>
		public void setTerminalMode(bool isTerminal) {
		}
	
		/// <summary>
		/// Check if the TPM is locked. The default implementation returns false.
		/// </summary>
		///
		/// <returns>True if the TPM is locked, otherwise false.</returns>
		public bool isTpmLocked() {
			return false;
		}
	
		/// <summary>
		/// Unlock the TPM. If !isTerminalMode(), prompt for a password from the GUI.
		/// The default implementation does nothing and returns !isTpmLocked().
		/// </summary>
		///
		/// <param name="password">The password to unlock TPM.</param>
		/// <returns>True if the TPM was unlocked.</returns>
		public bool unlockTpm(ByteBuffer password) {
			return !isTpmLocked();
		}
	
		/// <summary>
		/// Set the key name in keyHandle according to identityName and params.
		/// </summary>
		///
		protected static internal void setKeyName(TpmKeyHandle keyHandle, Name identityName,
				KeyParams paras) {
			Name.Component keyId;
			if (paras.getKeyIdType() == net.named_data.jndn.security.KeyIdType.USER_SPECIFIED)
				keyId = paras.getKeyId();
			else if (paras.getKeyIdType() == net.named_data.jndn.security.KeyIdType.SHA256) {
				byte[] digest = net.named_data.jndn.util.Common.digestSha256(keyHandle.derivePublicKey()
						.buf());
				keyId = new Name.Component(digest);
			} else if (paras.getKeyIdType() == net.named_data.jndn.security.KeyIdType.RANDOM) {
				if (paras.getKeyId().getValue().size() == 0)
					throw new TpmBackEnd.Error (
							"setKeyName: The keyId is empty for type RANDOM");
				keyId = paras.getKeyId();
			} else
				throw new TpmBackEnd.Error ("setKeyName: unrecognized params.getKeyIdType()");
	
			keyHandle.setKeyName(net.named_data.jndn.security.pib.PibKey.constructKeyName(identityName, keyId));
		}
	
		/// <summary>
		/// Check if the key with name keyName exists in the TPM.
		/// </summary>
		///
		/// <param name="keyName">The name of the key.</param>
		/// <returns>True if the key exists.</returns>
		protected abstract internal bool doHasKey(Name keyName);
	
		/// <summary>
		/// Get the handle of the key with name keyName.
		/// </summary>
		///
		/// <param name="keyName">The name of the key.</param>
		/// <returns>The handle of the key, or null if the key does not exist.</returns>
		protected abstract internal TpmKeyHandle doGetKeyHandle(Name keyName);
	
		/// <summary>
		/// Create a key for identityName according to params. The created key is
		/// named as: /{identityName}/[keyId]/KEY . The key name is set in the returned
		/// TpmKeyHandle.
		/// </summary>
		///
		/// <param name="identityName">The name if the identity.</param>
		/// <param name="params">The KeyParams for creating the key.</param>
		/// <returns>The handle of the created key.</returns>
		/// <exception cref="TpmBackEnd.Error">if the key cannot be created.</exception>
		protected abstract internal TpmKeyHandle doCreateKey(Name identityName,
				KeyParams paras);
	
		/// <summary>
		/// Delete the key with name keyName. If the key doesn't exist, do nothing.
		/// </summary>
		///
		/// <param name="keyName">The name of the key to delete.</param>
		/// <exception cref="TpmBackEnd.Error">if the deletion fails.</exception>
		protected abstract internal void doDeleteKey(Name keyName);
	
		/// <summary>
		/// Get the encoded private key with name keyName in PKCS #8 format, possibly
		/// password-encrypted.
		/// </summary>
		///
		/// <param name="keyName">The name of the key in the TPM.</param>
		/// <param name="password">it to return a PKCS #8 EncryptedPrivateKeyInfo. If the password is null, return an unencrypted PKCS #8 PrivateKeyInfo.</param>
		/// <returns>The encoded private key.</returns>
		/// <exception cref="TpmBackEnd.Error">if the key does not exist or if the key cannot beexported, e.g., insufficient privileges.</exception>
		protected internal virtual Blob doExportKey(Name keyName, ByteBuffer password) {
			throw new TpmBackEnd.Error ("TpmBackEnd doExportKey is not implemented");
		}
	
		/// <summary>
		/// Import an encoded private key with name keyName in PKCS #8 format, possibly
		/// password-encrypted.
		/// </summary>
		///
		/// <param name="keyName">The name of the key to use in the TPM.</param>
		/// <param name="pkcs8">unencrypted PKCS #8 PrivateKeyInfo.</param>
		/// <param name="password">it to decrypt the PKCS #8 EncryptedPrivateKeyInfo. If the password is null, import an unencrypted PKCS #8 PrivateKeyInfo.</param>
		/// <exception cref="TpmBackEnd.Error">for an error importing the key.</exception>
		protected internal virtual void doImportKey(Name keyName, ByteBuffer pkcs8,
				ByteBuffer password) {
			throw new TpmBackEnd.Error ("TpmBackEnd doImportKey is not implemented");
		}
	}
}
