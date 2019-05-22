// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.7.7.5
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Elevate.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	public partial class User : ISpecificRecord
	{
		public static Schema _SCHEMA = Schema.Parse("{\"type\":\"record\",\"name\":\"User\",\"namespace\":\"Elevate.Accounts\",\"fields\":[{\"name\":\"" +
				"id\",\"type\":\"string\"},{\"name\":\"first_name\",\"type\":\"string\"},{\"name\":\"last_name\",\"" +
				"type\":\"string\"},{\"name\":\"email_name\",\"type\":\"string\"}]}");
		private string _id;
		private string _first_name;
		private string _last_name;
		private string _email_name;
		public virtual Schema Schema
		{
			get
			{
				return User._SCHEMA;
			}
		}
		public string id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}
		public string first_name
		{
			get
			{
				return this._first_name;
			}
			set
			{
				this._first_name = value;
			}
		}
		public string last_name
		{
			get
			{
				return this._last_name;
			}
			set
			{
				this._last_name = value;
			}
		}
		public string email_name
		{
			get
			{
				return this._email_name;
			}
			set
			{
				this._email_name = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.id;
			case 1: return this.first_name;
			case 2: return this.last_name;
			case 3: return this.email_name;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.id = (System.String)fieldValue; break;
			case 1: this.first_name = (System.String)fieldValue; break;
			case 2: this.last_name = (System.String)fieldValue; break;
			case 3: this.email_name = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
