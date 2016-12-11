using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

namespace Clinic.Infrastructure.Helpers
{
    public static class DbContextExtensions
    {
        public static IDictionary<Type, String> GetTables(this DbContext ctx)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            IEnumerable<EntityType> entities = octx.MetadataWorkspace.GetItemCollection(DataSpace.OSpace).GetItems<EntityType>().ToList();

            return (entities.ToDictionary(x => Type.GetType(x.FullName), x => GetTableName(ctx, Type.GetType(x.FullName))));
        }

        public static String GetTableName(this DbContext ctx, Type entityType)
        {
            var tableName = "";          
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntitySetBase et = octx.MetadataWorkspace.GetItemCollection(DataSpace.SSpace)
                .GetItems<EntityContainer>()
                .Single()
                .BaseEntitySets
                .Where(x => x.Name == entityType.Name || x.Name == entityType.Name + "s")
                .SingleOrDefault();

            if (et != null && et.MetadataProperties["Table"].Value != null)
                tableName = string.Concat(et.MetadataProperties["Schema"].Value, ".", et.MetadataProperties["Table"].Value);

            return tableName;
        }

        public static IEnumerable<PropertyInfo> OneToMany(this DbContext ctx, Type entityType)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntityType et = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();

            return (et.NavigationProperties.Where(x => x.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.One && x.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many).Select(x => entityType.GetProperty(x.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty)).ToList());
        }

        public static IEnumerable<PropertyInfo> OneToOne(this DbContext ctx, Type entityType)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntityType et = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();

            return (et.NavigationProperties.Where(x => (x.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.One || x.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne) && (x.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.One || x.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne)).Select(x => entityType.GetProperty(x.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty)).ToList());
        }

        public static IEnumerable<PropertyInfo> ManyToOne(this DbContext ctx, Type entityType)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntityType et = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();

            return (et.NavigationProperties.Where(x => x.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many && x.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.One).Select(x => entityType.GetProperty(x.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty)).ToList());
        }

        public static IEnumerable<PropertyInfo> ManyToMany(this DbContext ctx, Type entityType)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntityType et = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();

            return (et.NavigationProperties.Where(x => x.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many && x.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many).Select(x => entityType.GetProperty(x.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty)).ToList());
        }

        public static IEnumerable<PropertyInfo> GetIdProperties(this DbContext ctx, Type entityType)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntityType et = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();

            return (et.KeyMembers.Select(x => entityType.GetProperty(x.Name)).ToList());
        }

        public static IEnumerable<PropertyInfo> GetNavigationProperties(this DbContext ctx, Type entityType)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntityType et = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();

            return (et.NavigationProperties.Select(x => entityType.GetProperty(x.Name)).ToList());
        }

        //public static IEnumerable<PropertyInfo> GetIdProperties(this DbContext ctx, Type entityType)
        //{
        //    ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
        //    EntityType et = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();

        //    return (et.KeyMembers.Select(x => entityType.GetProperty(x.Name)).ToList());
        //}

        public static IDictionary<String, PropertyInfo> GetTableKeyColumns(this DbContext ctx, Type entityType)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntityType storageEntityType = octx.MetadataWorkspace.GetItems(DataSpace.SSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();
            EntityType objectEntityType = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();
            return (storageEntityType.KeyMembers.Select((elm, index) => new { elm.Name, Property = entityType.GetProperty((objectEntityType.MetadataProperties["Members"].Value as IEnumerable<EdmMember>).ElementAt(index).Name) }).ToDictionary(x => x.Name, x => x.Property));
        }

        public static IDictionary<String, PropertyInfo> GetTableColumns(this DbContext ctx, Type entityType)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntityType storageEntityType = octx.MetadataWorkspace.GetItems(DataSpace.SSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();
            EntityType objectEntityType = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();
            return (storageEntityType.Properties.Select((elm, index) => new { elm.Name, Property = entityType.GetProperty(objectEntityType.Members[index].Name) }).ToDictionary(x => x.Name, x => x.Property));
        }

        public static IDictionary<String, PropertyInfo> GetTableNavigationColumns(this DbContext ctx, Type entityType)
        {
            ObjectContext octx = (ctx as IObjectContextAdapter).ObjectContext;
            EntityType storageEntityType = octx.MetadataWorkspace.GetItems(DataSpace.SSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();
            EntityType objectEntityType = octx.MetadataWorkspace.GetItems(DataSpace.OSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType).OfType<EntityType>().Where(x => x.Name == entityType.Name).Single();
            return (storageEntityType.NavigationProperties.Select((elm, index) => new { elm.Name, Property = entityType.GetProperty(objectEntityType.Members[index].Name) }).ToDictionary(x => x.Name, x => x.Property));
        }
    }

}
