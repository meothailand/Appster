using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwinkleStars.Infrastructure.Utils;

namespace AppsterBackendAdmin.Models.Business
{
    public abstract class BaseBusinessModel
    {
        /// <summary>
        /// copy data fields from this business model to entity model
        /// </summary>
        /// <typeparam name="T">the type of entity model</typeparam>
        /// <returns>entity model</returns>
        public T ToEntity<T>(T entity) where T : class
        {
            ModelObjectHelper.CopyObject(this, entity);
            return entity;
        }
    }
}