using System;
using System.Collections.Generic;
using Illumina.Networking;
using Illumina.Security;
using UnityEngine;

namespace Illumina.Model {

    public class Field {
        public string name;
        protected string value;
        protected string _value;
        protected bool protected_field;

        public Field(string name) {
            this.name = name;
            value = "novalue";
            _value = value;
        }
        public Field(string value, bool protected_field) {
            this.value = value;
            this._value = value;
            this.protected_field = protected_field;
            if (protected_field) {
                this.value = IlluminaHash.GetHash(value);
            }
        }
        public static implicit operator Field(string value) => new Field(value, false);

        public static implicit operator string(Field me) {
            return me.value;
        }
    }

    public class ProtectedField : Field {

        public ProtectedField(string name) : base(name) {
            this.name = name;
            value = "novalue";
            _value = value;
        }

        public ProtectedField(string value, bool protected_field) : base(value, protected_field) {

        }
        public static implicit operator ProtectedField(string value) => new ProtectedField(value, true);

        public static implicit operator string(ProtectedField me) {
            return me.value;
        }
    }

    public abstract class Model {

        protected Dictionary<string, Field> fields = new Dictionary<string, Field>();
        public Request request = new Request();
        protected string create_url;
        protected string delete_url;
        protected string update_url;

        public delegate void ModelCreatedEventHandler(object source, Response response);

        public delegate void ModelDeletedEventHandler(object source, Response response);

        public delegate void ModelUpdatedEventHandler(object source, Response response);

        public event ModelCreatedEventHandler ModelCreated;
        public event ModelDeletedEventHandler ModelDeleted;
        public event ModelUpdatedEventHandler ModelUpdated;

        public WWWForm ToForm() {
            WWWForm data = new WWWForm();
            foreach (var item in fields) {
                data.AddField(item.Key, item.Value);
            }
            return data;
        }

        public override string ToString() {
            string data = "?";
            int i = 0;
            foreach (var item in fields) {
                data += item.Key + "=" + item.Value;
                if (i < fields.Count - 1) {
                    data += "&";
                }
                i++;
            }

            return data;
        }

        public bool SetValues(Request request) {
            this.request = request;

            Dictionary<string, Field> temp = new Dictionary<string, Field>();
            foreach (var item in request.requests) {
                if (request.requests.ContainsKey(item.Key)) {
                    temp.Add(item.Key, request[item.Key]);
                } else {
                    return false;
                }
            }
            fields = temp;
            return true;
        }
        public abstract bool OnBeforeCreate(Request request);
        public virtual bool OnBeforeUpdate(Request request) {
            return true;
        }
        public virtual bool OnBeforeDelete(Request request) {
            return true;
        }
        public abstract void OnCreated(Response response);
        public virtual void OnUpdated(Response response) {
            Debug.Log("Update complete");
        }
        public virtual void OnDeleted(Response response) {
            Debug.Log("Delete method complete");
        }
        public void CallAfterEvents(Response response) {
            if (request.model_method == "CREATE") {
                OnCreated(response);
                if (ModelCreated != null) {

                    ModelCreated(this, response);
                }
            }
            if (request.model_method == "DELETE") {
                OnDeleted(response);
                if (ModelCreated != null) {

                    ModelDeleted(this, response);
                }
            }
            if (request.model_method == "UPDATE") {
                OnUpdated(response);
                if (ModelCreated != null) {

                    ModelUpdated(this, response);
                }
            }

        }

        public string this [string key] {
            get {
                return fields[key];
            }
        }
        public void Create() {
            if (OnBeforeCreate(this.request)) {
                LaravelWebRequestHandler.CreatePost(create_url, this);
            }

        }

        public void Create(Request request) {
            this.SetValues(request);
            this.Create();
        }
        public void Update() {
            if (OnBeforeUpdate(this.request)) {
                LaravelWebRequestHandler.CreatePost(update_url, this);
            }

        }

        public void Update(Request request) {
            this.SetValues(request);
            this.Create();
        }
        public void Delete() {
            if (OnBeforeUpdate(this.request)) {
                LaravelWebRequestHandler.CreatePost(delete_url, this);
            }

        }

        public void Delete(Request request) {
            this.SetValues(request);
            this.Create();
        }

    }
}