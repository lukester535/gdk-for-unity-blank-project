using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.QueryBasedInterest;
using UnityEngine;
using SpatialosGame;

namespace BlankProject.Scripts.Config
{
    public static class EntityTemplates
    {
        public static EntityTemplate CreatePlayerEntityTemplate(EntityId entityId, string workerId, byte[] serializedArguments)
        {
            var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);
            var serverAttribute = UnityGameLogicConnector.WorkerType;

            var template = new EntityTemplate();
            template.AddComponent(new SpatialosGame.PlayerTransform.Snapshot(), clientAttribute);
            template.AddComponent(new SpatialosGame.ServerTransform.Snapshot(), serverAttribute);
            template.AddComponent(new Position.Snapshot(), serverAttribute);
            template.AddComponent(new Metadata.Snapshot("Player"), clientAttribute);

            PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, workerId, serverAttribute);

            const int serverRadius = 500;
            var clientRadius = workerId.Contains(MobileClientWorkerConnector.WorkerType) ? 100 : 500;

            var serverQuery = InterestQuery.Query(Constraint.RelativeCylinder(serverRadius));
            var clientQuery = InterestQuery.Query(Constraint.RelativeCylinder(clientRadius));

            var interest = InterestTemplate.Create()
                .AddQueries<Metadata.Component>(serverQuery)
                .AddQueries<Position.Component>(clientQuery);
            template.AddComponent(interest.ToSnapshot(), serverAttribute);

            template.SetReadAccess(UnityClientConnector.WorkerType, serverAttribute);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            return template;
        }

        public static EntityTemplate CreateBasicObject(string ID, Vector3 Vec)
        {
            var serverAttribute = UnityGameLogicConnector.WorkerType;
            var clientAttribute = UnityClientConnector.WorkerType;

            var template = new EntityTemplate();

            template.AddComponent(new Position.Snapshot(), serverAttribute);
            template.AddComponent(new SpatialosGame.BasicObjectClient.Snapshot(), serverAttribute);
            template.AddComponent(new SpatialosGame.BasicObjectAccess.Snapshot(), clientAttribute);
            template.AddComponent(new SpatialosGame.BasicObjectServer.Snapshot(Vec3ToVec3f(Vec), new Improbable.Quaternionf(), false), serverAttribute);
            template.AddComponent(new Metadata.Snapshot(ID), serverAttribute);

            template.SetReadAccess(UnityClientConnector.WorkerType, serverAttribute);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            return template;
        }

        public static Improbable.Vector3f Vec3ToVec3f(Vector3 Vec)
        {
            return new Vector3f(Vec.x, Vec.y, Vec.z);
        }

        /*
        public static EntityTemplate PlayerCreator(EntityId entityId, string workerId, byte[] serializedArguments, Vector3 position, uint healthValue)
        {
            // Create a HealthPickup component snapshot which is initially active and grants "heathValue" on pickup.
            var entityTemplate = new EntityTemplate();
            var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);
            var serverAttribute = UnityGameLogicConnector.WorkerType;

            //entityTemplate.AddComponent(new SpatialosGame.PlayerTransform.Snapshot(), clientAttribute);
            entityTemplate.AddComponent(new Metadata.Snapshot("PlayerController"), serverAttribute);

            return entityTemplate;
        }
        */
    }
}
