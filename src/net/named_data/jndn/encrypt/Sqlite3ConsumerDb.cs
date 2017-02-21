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
namespace net.named_data.jndn.encrypt {
	
	using ILOG.J2CsMapping.Util.Logging;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data.SqlClient;
	using System.IO;
	using System.Runtime.CompilerServices;
	using net.named_data.jndn;
	using net.named_data.jndn.util;
	
	/// <summary>
	/// Sqlite3ConsumerDb extends ConsumerDb to implement the storage of decryption
	/// keys for the consumer using SQLite3.
	/// </summary>
	///
	/// @note This class is an experimental feature. The API may change.
	public class Sqlite3ConsumerDb : Sqlite3ConsumerDbBase {
		/// <summary>
		/// Create an Sqlite3ConsumerDb to use the given SQLite3 file.
		/// </summary>
		///
		/// <param name="databaseFilePath">The path of the SQLite file.</param>
		/// <exception cref="ConsumerDb.Error">for a database error.</exception>
		public Sqlite3ConsumerDb(String databaseFilePath) {
			this.database_ = null;
			try {
				ILOG.J2CsMapping.Reflect.Helper.GetNativeType("org.sqlite.JDBC");
			} catch (TypeLoadException ex) {
				// We don't expect this to happen.
				ILOG.J2CsMapping.Util.Logging.Logger.getLogger(typeof(Sqlite3ConsumerDb).FullName).log(
						ILOG.J2CsMapping.Util.Logging.Level.SEVERE, null, ex);
				return;
			}
	
			try {
				database_ = System.Data.SqlClient.DriverManager.getConnection("jdbc:sqlite:"
						+ databaseFilePath);
	
				Statement statement = database_.CreateCommand();
				// Use "try/finally instead of "try-with-resources" or "using" which are
				// not supported before Java 7.
				try {
					// Initialize database specific tables.
					statement.executeUpdate(net.named_data.jndn.encrypt.Sqlite3ConsumerDbBase.INITIALIZATION1);
					statement.executeUpdate(net.named_data.jndn.encrypt.Sqlite3ConsumerDbBase.INITIALIZATION2);
				} finally {
					statement.close();
				}
			} catch (SQLException exception) {
				throw new ConsumerDb.Error("Sqlite3ConsumerDb: SQLite error: "
						+ exception);
			}
		}
	
		/// <summary>
		/// Get the key with keyName from the database.
		/// </summary>
		///
		/// <param name="keyName">The key name.</param>
		/// <returns>A Blob with the encoded key, or an isNull Blob if cannot find the
		/// key with keyName.</returns>
		/// <exception cref="ConsumerDb.Error">for a database error.</exception>
		public override Blob getKey(Name keyName) {
			try {
				PreparedStatement statement = database_
						.prepareStatement(net.named_data.jndn.encrypt.Sqlite3ConsumerDbBase.SELECT_getKey);
				statement.setBytes(1, keyName.wireEncode(net.named_data.jndn.encoding.TlvWireFormat.get())
						.getImmutableArray());
	
				Blob key = new Blob();
				try {
					SqlDataReader result = statement.executeQuery();
	
					if (result.NextResult())
						key = new Blob(result.getBytes(1), false);
				} finally {
					statement.close();
				}
	
				return key;
			} catch (SQLException exception) {
				throw new ConsumerDb.Error(
						"Sqlite3ConsumerDb.getKey: SQLite error: " + exception);
			}
		}
	
		/// <summary>
		/// Add the key with keyName and keyBlob to the database.
		/// </summary>
		///
		/// <param name="keyName">The key name.</param>
		/// <param name="keyBlob">The encoded key.</param>
		/// <exception cref="ConsumerDb.Error">if a key with the same keyName already exists inthe database, or other database error.</exception>
		public override void addKey(Name keyName, Blob keyBlob) {
			try {
				PreparedStatement statement = database_
						.prepareStatement(net.named_data.jndn.encrypt.Sqlite3ConsumerDbBase.INSERT_addKey);
				statement.setBytes(1, keyName.wireEncode(net.named_data.jndn.encoding.TlvWireFormat.get())
						.getImmutableArray());
				statement.setBytes(2, keyBlob.getImmutableArray());
	
				try {
					statement.executeUpdate();
				} finally {
					statement.close();
				}
			} catch (SQLException exception) {
				throw new ConsumerDb.Error(
						"Sqlite3ConsumerDb.addKey: SQLite error: " + exception);
			}
		}
	
		/// <summary>
		/// Delete the key with keyName from the database. If there is no key with
		/// keyName, do nothing.
		/// </summary>
		///
		/// <param name="keyName">The key name.</param>
		/// <exception cref="ConsumerDb.Error">for a database error.</exception>
		public override void deleteKey(Name keyName) {
			try {
				PreparedStatement statement = database_
						.prepareStatement(net.named_data.jndn.encrypt.Sqlite3ConsumerDbBase.DELETE_deleteKey);
				statement.setBytes(1, keyName.wireEncode(net.named_data.jndn.encoding.TlvWireFormat.get())
						.getImmutableArray());
	
				try {
					statement.executeUpdate();
				} finally {
					statement.close();
				}
			} catch (SQLException exception) {
				throw new ConsumerDb.Error(
						"Sqlite3ConsumerDb.deleteKey: SQLite error: " + exception);
			}
		}
	
		internal SqlConnection database_;
	}
}
