﻿using System;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Mapping
{
    public class OneToManyPart<TChild> : ToManyBase<OneToManyPart<TChild>, TChild, OneToManyMapping>, IOneToManyPart, IAccessStrategy<OneToManyPart<TChild>> 
    {
        private readonly Type entity;
        private readonly ColumnNameCollection<OneToManyPart<TChild>> columns;
        private readonly CollectionCascadeExpression<IOneToManyPart> cascade;
        private readonly NotFoundExpression<OneToManyPart<TChild>> notFound;

        public OneToManyPart(Type entity, PropertyInfo property)
            : this(entity, property, property.PropertyType)
        {}

        public OneToManyPart(Type entity, MethodInfo method)
            : this(entity, method, method.ReturnType)
        {}

        protected OneToManyPart(Type entity, MemberInfo member, Type collectionType)
            : base(entity, member, collectionType)
        {
            this.entity = entity;
            columns = new ColumnNameCollection<OneToManyPart<TChild>>(this);
            cascade = new CollectionCascadeExpression<IOneToManyPart>(this, value => collectionAttributes.Set(x => x.Cascade, value));
            notFound = new NotFoundExpression<OneToManyPart<TChild>>(this, value => relationshipAttributes.Set(x => x.NotFound, value));

            collectionAttributes.SetDefault(x => x.Name, member.Name);
        }

        public override ICollectionMapping GetCollectionMapping()
        {
            var collection = base.GetCollectionMapping();

            if (columns.List().Count == 0)
                collection.Key.AddDefaultColumn(new ColumnMapping { Name = entity.Name + "_id" });

            foreach (var column in columns.List())
                collection.Key.AddColumn(new ColumnMapping { Name = column });

            return collection;
        }

        protected override ICollectionRelationshipMapping GetRelationship()
        {
            return new OneToManyMapping(relationshipAttributes.CloneInner())
            {
                ContainingEntityType = entity
            };
        }

        public IOneToManyPart KeyColumn(string columnName)
        {
            KeyColumns.Clear();
            KeyColumns.Add(columnName);
            return this;
        }

        public ColumnNameCollection<OneToManyPart<TChild>> KeyColumns
        {
            get { return columns; }
        }

        public IOneToManyPart ForeignKeyConstraintName(string foreignKeyName)
        {
            keyAttributes.Set(x => x.ForeignKey, foreignKeyName);
            return this;
        }

        #region Explicit IOneToManyPart Implementation

        CollectionCascadeExpression<IOneToManyPart> IOneToManyPart.Cascade
        {
            get { return cascade; }
        }

        IOneToManyPart IOneToManyPart.Inverse()
        {
            return Inverse();
        }

        OptimisticLockBuilder<IOneToManyPart> IOneToManyPart.OptimisticLock
        {
            get { return new OptimisticLockBuilder<IOneToManyPart>(this, value => collectionAttributes.Set(x => x.OptimisticLock, value)); }
        }

        FetchTypeExpression<IOneToManyPart> IOneToManyPart.Fetch
        {
            get { return new FetchTypeExpression<IOneToManyPart>(this, value => collectionAttributes.Set(x => x.Fetch, value)); }
        }

        IOneToManyPart IOneToManyPart.Schema(string schema)
        {
            return Schema(schema);
        }

        IOneToManyPart IOneToManyPart.Persister<T>()
        {
            return Persister<T>();
        }

        OuterJoinBuilder<IOneToManyPart> IOneToManyPart.OuterJoin
        {
            get { return new OuterJoinBuilder<IOneToManyPart>(this, value => collectionAttributes.Set(x => x.OuterJoin, value)); }
        }

        public new CollectionCascadeExpression<IOneToManyPart> Cascade
        {
            get { return cascade; }
        }

        IOneToManyPart IOneToManyPart.LazyLoad()
        {
            return LazyLoad();
        }

        IOneToManyPart IOneToManyPart.Not
        {
            get { return Not; }
        }

        IOneToManyPart IOneToManyPart.Check(string checkSql)
        {
            return Check(checkSql);
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        IOneToManyPart IOneToManyPart.CollectionType<TCollection>()
        {
            return CollectionType<TCollection>();
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        IOneToManyPart IOneToManyPart.CollectionType(Type type)
        {
            return CollectionType(type);
        }

        IOneToManyPart IOneToManyPart.Generic()
        {
            return Generic();
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        IOneToManyPart IOneToManyPart.CollectionType(string type)
        {
            return CollectionType(type);
        }

        IColumnNameCollection IOneToManyPart.KeyColumns
        {
            get { return KeyColumns; }
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }

        public NotFoundExpression<OneToManyPart<TChild>> NotFound
        {
            get { return notFound; }
        }

        INotFoundExpression IOneToManyPart.NotFound
        {
            get { return NotFound; }
        }

        #endregion
    }
}
