// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : DocumentDbExtension.cs
// Created          : 2016-12-19  10:13
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-01-10  16:57
// ***********************************************************************
// <copyright file="DocumentDbExtension.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Moe.Lib.Jinyinmao;

namespace Jinyinmao.AuthManager.Libraries.Extension
{
    public static class DocumentDbExtension
    {
        private static string DocmentDatabase
        {
            get
            {
                try
                {
                    return App.Configurations.GetConfig<AuthSiloConfig>().DocumentDatabase;
                }
                catch
                {
                    return App.Configurations.GetConfig<AuthApiConfig>().DocumentDatabase;
                }
            }
        }

        /// <summary>
        ///     创建文档数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentClient">The document client.</param>
        /// <param name="collectionId">The collection identifier.</param>
        /// <param name="item">The item.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.Data.DataException">${collectionId}对应Document已存在</exception>
        public static async Task<T> CreateDocumentItemAsync<T>(this DocumentClient documentClient, string collectionId, T item) where T : DocumentBase
        {
            try
            {
                T tResult = await documentClient.GetDocumentFirstOrDefaultItemAsync<T>(collectionId, p => p.id == item.id);
                if (tResult != null)
                {
                    throw new DataException($"{collectionId}对应Document已存在");
                }

                ResourceResponse<Document> response = await documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DocmentDatabase, collectionId), item);
                return (T)(dynamic)response.Resource;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     获取文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentClient">The document client.</param>
        /// <param name="collectionId">The collection identifier.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static async Task<T> GetDocumentFirstOrDefaultItemAsync<T>(this DocumentClient documentClient, string collectionId, Expression<Func<T, bool>> predicate) where T : DocumentBase
        {
            try
            {
                IEnumerable<T> ts = await documentClient.GetDocumentItemsAsync(collectionId, predicate);
                return ts.FirstOrDefault();
            }
            catch (DocumentClientException)
            {
                //ToDo 记录日志或者其他操作
                return null;
            }
        }

        /// <summary>
        ///     获取文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentClient">The document client.</param>
        /// <param name="collectionId">The collection identifier.</param>
        /// <param name="pId">程序生成的唯一ID</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static async Task<T> GetDocumentItemAsync<T>(this DocumentClient documentClient, string collectionId, string pId) where T : DocumentBase
        {
            try
            {
                T response = await documentClient.GetDocumentFirstOrDefaultItemAsync<T>(collectionId, p => p.id == pId);
                return response;
            }
            catch (DocumentClientException)
            {
                //ToDo 记录日志或者其他操作
                return null;
            }
        }

        /// <summary>
        ///     获取文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentClient">The document client.</param>
        /// <param name="collectionId">The collection identifier.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static async Task<IEnumerable<T>> GetDocumentItemsAsync<T>(this DocumentClient documentClient, string collectionId, Expression<Func<T, bool>> predicate) where T : DocumentBase
        {
            IDocumentQuery<T> query = documentClient.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(DocmentDatabase, collectionId)).Where(predicate).AsDocumentQuery();
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }
            return results;
        }

        /// <summary>
        ///     更新文档数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentClient">The document client.</param>
        /// <param name="collectionId">The collection identifier.</param>
        /// <param name="item">The item.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        /// <exception cref="System.Data.Entity.Core.ObjectNotFoundException">$未找到{collectionId}对应DocumentCollection</exception>
        public static async Task<T> UpdateDocumentItemAsync<T>(this DocumentClient documentClient, string collectionId, T item) where T : DocumentBase
        {
            try
            {
                T findResult = await documentClient.GetDocumentItemAsync<T>(collectionId, item.id);
                if (findResult == null)
                {
                    throw new ObjectNotFoundException(nameof(item.id));
                }
                ResourceResponse<Document> doc = await documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DocmentDatabase, collectionId, item.id), item);
                return (T)(dynamic)doc.Resource;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ObjectNotFoundException(nameof(item.id));
                }
                //ToDo 记录日志或者其他操作
                // ReSharper disable once PossibleIntendedRethrow
                throw e;
            }
        }

        /// <summary>
        ///     不存在则新增、否则为更新文档数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentClient">The document client.</param>
        /// <param name="collectionId">The collection identifier.</param>
        /// <param name="item">The item.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static async Task<T> UpsetDocumentItemAsync<T>(this DocumentClient documentClient, string collectionId, T item) where T : DocumentBase
        {
            try
            {
                ResourceResponse<Document> response = await documentClient.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DocmentDatabase, collectionId), item);
                return (T)(dynamic)response.Resource;
            }
            catch (DocumentClientException)
            {
                //ToDo 记录日志或者其他操作
                return null;
            }
        }
    }
}