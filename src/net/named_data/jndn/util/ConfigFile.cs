// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110331_01     
//
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2016 Regents of the University of California.
/// </summary>
///
namespace net.named_data.jndn.util {
	
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// A ConfigFile locates, opens, and parses a library configuration file, and
	/// holds the values for the application to get.
	/// </summary>
	///
	public class ConfigFile {
		/// <summary>
		/// Locate, open, and parse a library configuration file.
		/// </summary>
		///
		public ConfigFile() {
			this.config_ = new Hashtable<String, String>();
			path_ = findConfigFile();
	
			if (!path_.equals(""))
				parse();
		}
	
		/// <summary>
		/// Get the value for the key, or a default value if not found.
		/// </summary>
		///
		/// <param name="key">The key to search for.</param>
		/// <param name="defaultValue">The default value if the key is not found.</param>
		/// <returns>The value, or defaultValue if the key is not found.</returns>
		public String get(String key, String defaultValue) {
			if (config_.Contains(key))
				return ILOG.J2CsMapping.Collections.Collections.Get(config_,key);
			else
				return defaultValue;
		}
	
		/// <summary>
		/// Get the path of the configuration file.
		/// </summary>
		///
		/// <returns>The path or an empty string if not found.</returns>
		public String getPath() {
			return path_;
		}
	
		/// <summary>
		/// Get the configuration key/value pairs.
		/// </summary>
		///
		/// <returns>A map of key/value pairs.</returns>
		public IDictionary<String, String> getParsedConfiguration() {
			return config_;
		}
	
		/// <summary>
		/// Look for the configuration file in these well-known locations:
		/// 1. $HOME/.ndn/client.conf
		/// 2. /etc/ndn/client.conf
		/// We don't support the C++ #define value @SYSCONFDIR@.
		/// </summary>
		///
		/// <returns>The path of the config file or an empty string if not found.</returns>
		private static String findConfigFile() {
			// NOTE: Use File because java.nio.file.Path is not available before Java 7.
			FileInfo filePath = new FileInfo(System.IO.Path.Combine(new FileInfo(System.Environment.GetEnvironmentVariable("user.home")+".ndn").FullName,"client.conf"));
			if (filePath.Exists)
				return System.IO.Path.GetFullPath(filePath.Name);
	
			// Ignore the C++ SYSCONFDIR.
	
			filePath = new FileInfo("/etc/ndn/client.conf");
			if (filePath.Exists)
				return System.IO.Path.GetFullPath(filePath.Name);
	
			return "";
		}
	
		/// <summary>
		/// Open path_, parse the configuration file and set config_.
		/// </summary>
		///
		private void parse() {
			if (path_.equals(""))
				throw new Exception(
						"ConfigFile::parse: Failed to locate the configuration file for parsing");
	
			TextReader input;
			try {
				input = new FileReader(path_);
			} catch (FileNotFoundException ex) {
				// We don't expect this to happen since we just checked for the file.
				throw new Exception(ex.Message);
			}
	
			// Use "try/finally instead of "try-with-resources" or "using"
			// which are not supported before Java 7.
			try {
				String line;
				while ((line = input.readLine()) != null) {
					line = line.trim();
					if (line.equals("") || line[0] == ';')
						// Skip empty lines and comments.
						continue;
	
					int iSeparator = line.indexOf('=');
					if (iSeparator < 0)
						continue;
	
					String key = line.Substring(0,(iSeparator)-(0)).trim();
					String value_ren = line.Substring(iSeparator + 1).trim();
	
					ILOG.J2CsMapping.Collections.Collections.Put(config_,key,value_ren);
				}
			} finally {
				input.close();
			}
		}
	
		private String path_;
		private readonly Hashtable<String, String> config_;
	}
}
