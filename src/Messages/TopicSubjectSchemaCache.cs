using System.Collections.Generic;
using Avro;
using Elevate.Accounts;
using Elevate.Recruit;

namespace Messages
{
    public class TopicSubjectSchemaCache
    {
        readonly Dictionary<string, Schema> _subjectsToSchemas = new Dictionary<string, Schema>();
        readonly Dictionary<string, Schema> _nameToSchemas = new Dictionary<string, Schema>();
           
        public void Init(string topic)
        {
            Add(SubjectFactory.ValueSubjectNameFrom<Application>(topic), Application._SCHEMA);
            Add(Application._SCHEMA);
            
            Add(SubjectFactory.ValueSubjectNameFrom<User>(topic), User._SCHEMA);
            Add(User._SCHEMA);
        }

        void Add(string subject, Schema schema)
        {
            _subjectsToSchemas.Add(subject, schema);
        }

        void Add(Schema schema)
        {
            _nameToSchemas.Add(schema.Name, schema);
        }
        
        public Schema GetValue<T>(string topic)
        {
            return _subjectsToSchemas[SubjectFactory.ValueSubjectNameFrom<T>(topic)];
        }
        
        public Schema GetSchema(string name)
        {
            return _nameToSchemas[name];
        }
        
        public Schema GetSchema(Schema schema)
        {
            return _nameToSchemas[schema.Name];
        }
    }
}