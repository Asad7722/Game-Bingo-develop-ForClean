﻿using System;
using Random = UnityEngine.Random;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	/// <summary>
	/// Use it instead of regular <c>char</c> for any cheating-sensitive variables.
	/// </summary>
	/// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong>
	[Serializable]
	public struct ObscuredChar : IEquatable<ObscuredChar>
	{
		private static char cryptoKey = '\x2014';

#if UNITY_EDITOR
		// For internal Editor usage only (may be useful for drawers).
		public static char cryptoKeyEditor = cryptoKey;
#endif

		private char currentCryptoKey;
		private char hiddenValue;
		private char fakeValue;
		private bool inited;

		private ObscuredChar(char value)
		{
			currentCryptoKey = cryptoKey;
			hiddenValue = value;
			fakeValue = '\0';
			inited = true;
		}

		/// <summary>
		/// Allows to change default crypto key of this type instances. All new instances will use specified key.<br/>
		/// All current instances will use previous key unless you call ApplyNewCryptoKey() on them explicitly.
		/// </summary>
		public static void SetNewCryptoKey(char newKey)
		{
			cryptoKey = newKey;
		}

		/// <summary>
		/// Simple symmetric encryption, uses default crypto key.
		/// </summary>
		/// <returns>Encrypted or decrypted <c>char</c> (depending on what <c>char</c> was passed to the function)</returns>
		public static char EncryptDecrypt(char value)
		{
			return EncryptDecrypt(value, '\0');
		}

		/// <summary>
		/// Simple symmetric encryption, uses passed crypto key.
		/// </summary>
		/// <returns>Encrypted or decrypted <c>char</c> (depending on what <c>char</c> was passed to the function)</returns>
		public static char EncryptDecrypt(char value, char key)
		{
			if (key == '\0')
			{
				return (char)(value ^ cryptoKey);
			}
			return (char)(value ^ key);
		}

		/// <summary>
		/// Use it after SetNewCryptoKey() to re-encrypt current instance using new crypto key.
		/// </summary>
		public void ApplyNewCryptoKey()
		{
			if (currentCryptoKey != cryptoKey)
			{
				hiddenValue = EncryptDecrypt(InternalDecrypt(), cryptoKey);
				currentCryptoKey = cryptoKey;
			}
		}

		/// <summary>
		/// Allows to change current crypto key to the new random value and re-encrypt variable using it.
		/// Use it for extra protection against 'unknown value' search.
		/// Just call it sometimes when your variable doesn't change to fool the cheater.
		/// </summary>
		public void RandomizeCryptoKey()
		{
			char decrypted = InternalDecrypt();

			// here we just use first 8 bits of the integer
			currentCryptoKey = (char)(Random.seed >> 24);
			hiddenValue = EncryptDecrypt(decrypted, currentCryptoKey);
		}

		/// <summary>
		/// Allows to pick current obscured value as is.
		/// </summary>
		/// Use it in conjunction with SetEncrypted().<br/>
		/// Useful for saving data in obscured state.
		public char GetEncrypted()
		{
			ApplyNewCryptoKey();
			return hiddenValue;
		}

		/// <summary>
		/// Allows to explicitly set current obscured value.
		/// </summary>
		/// Use it in conjunction with GetEncrypted().<br/>
		/// Useful for loading data stored in obscured state.
		public void SetEncrypted(char encrypted)
		{
			inited = true;
			hiddenValue = encrypted;
			if (Detectors.ObscuredCheatingDetector.IsRunning)
			{
				fakeValue = InternalDecrypt();
			}
		}

		private char InternalDecrypt()
		{
			if (!inited)
			{
				currentCryptoKey = cryptoKey;
				hiddenValue = EncryptDecrypt('\0');
				fakeValue = '\0';
				inited = true;
			}

			char decrypted = EncryptDecrypt(hiddenValue, currentCryptoKey);

			if (Detectors.ObscuredCheatingDetector.IsRunning && fakeValue != '\0' && decrypted != fakeValue)
			{
				Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}

			return decrypted;
		}

		#region operators, overrides, interface implementations
		//! @cond
		public static implicit operator ObscuredChar(char value)
		{
			ObscuredChar obscured = new ObscuredChar(EncryptDecrypt(value));
			if (Detectors.ObscuredCheatingDetector.IsRunning)
			{
				obscured.fakeValue = value;
			}
			return obscured;
		}

		public static implicit operator char(ObscuredChar value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredChar operator ++(ObscuredChar input)
		{
			char decrypted = (char)(input.InternalDecrypt() + 1);
			input.hiddenValue = EncryptDecrypt(decrypted, input.currentCryptoKey);

			if (Detectors.ObscuredCheatingDetector.IsRunning)
			{
				input.fakeValue = decrypted;
			}
			return input;
		}

		public static ObscuredChar operator --(ObscuredChar input)
		{
			char decrypted = (char)(input.InternalDecrypt() - 1);
			input.hiddenValue = EncryptDecrypt(decrypted, input.currentCryptoKey);

			if (Detectors.ObscuredCheatingDetector.IsRunning)
			{
				input.fakeValue = decrypted;
			}
			return input;
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		/// </summary>
		/// 
		/// <returns>
		/// true if <paramref name="obj"/> is an instance of ObscuredChar and equals the value of this instance; otherwise, false.
		/// </returns>
		/// <param name="obj">An object to compare with this instance, or null. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (!(obj is ObscuredChar))
				return false;
			return Equals((ObscuredChar)obj);
		}

		/// <summary>
		/// Returns a value indicating whether this instance and a specified ObscuredChar object represent the same value.
		/// </summary>
		/// 
		/// <returns>
		/// true if <paramref name="obj"/> is equal to this instance; otherwise, false.
		/// </returns>
		/// <param name="obj">An ObscuredChar object to compare to this instance.</param><filterpriority>2</filterpriority>
		public bool Equals(ObscuredChar obj)
		{
			if (currentCryptoKey == obj.currentCryptoKey)
			{
				return hiddenValue == obj.hiddenValue;
			}

			return EncryptDecrypt(hiddenValue, currentCryptoKey) == EncryptDecrypt(obj.hiddenValue, obj.currentCryptoKey);
		}

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation.
		/// </summary>
		/// 
		/// <returns>
		/// The string representation of the value of this instance, consisting of a sequence of digits ranging from 0 to 9, without a sign or leading zeros.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			return InternalDecrypt().ToString();
		}

#if !UNITY_WINRT
		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.
		/// </summary>
		/// 
		/// <returns>
		/// The string representation of the value of this instance , which consists of a sequence of digits ranging from 0 to 9, without a sign or leading zeros.
		/// </returns>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param><filterpriority>1</filterpriority>
		public string ToString(IFormatProvider provider)
		{
			return InternalDecrypt().ToString(provider);
		}
#endif
		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// 
		/// <returns>
		/// A hash code for the current ObscuredChar.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return InternalDecrypt().GetHashCode();
		}

		//! @endcond
		#endregion
	}
}
