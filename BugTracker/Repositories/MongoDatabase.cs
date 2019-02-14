using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugTracker.Repositories.Models;
using MongoDB.Driver;

namespace BugTracker.Repositories
{
    public class MongoDatabase<T> : IDatabase<T> where T : IModel 
    {
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<T> _collection;

        public MongoDatabase(IMongoDatabase mongoDatabase)
        {
            this._mongoDatabase = mongoDatabase;
            _collection = this._mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public async Task<bool> Create(T document)
        {
            bool isCreated = true;

            try
            {
                await _collection.InsertOneAsync(document);
            }
            catch(MongoWriteConcernException)
            {
                // TODO: Log
                throw; 
            }

            return isCreated;
        }

        public async Task<bool> Update(T document)
        {
            bool isUpdated = false;

            try
            {
                var result = await _collection.ReplaceOneAsync(t => t.Id == document.Id, document);
                isUpdated = result.ModifiedCount == 1 ? true : false;
            }
            catch(MongoWriteConcernException)
            {
                // TODO: Log
                throw;
            }

            return isUpdated; 
        }

        public async Task<T> Find(string id)
        {
            T result = default(T);

            try
            {
                var all = await _collection.FindAsync<T>(t => t.Id == id);
                result = all.FirstOrDefault<T>();
            }
            catch(Exception)
            {
                // TODO: Log 
                throw;
            }
            return result;
        }

        public async Task<List<T>> FindAll()
        {
            var all = await FindAll(_ => true);

            return all;
        }

        public async Task<List<T>> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            List<T> result = null;

            try
            {
                var all = await _collection.FindAsync<T>(filter);
                result = all.ToList<T>();
            }
            catch (Exception)
            {
                // TODO: Log 
                throw;
            }
            return result;
        }

        public async Task<bool> Delete(string id)
        {
            bool isDeleted = false;

            try
            {
                var result = await _collection.DeleteOneAsync<T>(t => t.Id == id);
                isDeleted = result.DeletedCount == 1 ? true : false;
            }
            catch(Exception)
            {
                // TODO: Log 
                throw;
            }

            return isDeleted;
        }
    }
}
